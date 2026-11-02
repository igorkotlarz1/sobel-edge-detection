#!/usr/bin/env bash
# Verifies the MASM routine against the C++ one on Linux (x86-64, g++, nasm).
#   tests/run.sh                         correctness + ABI check
#   tests/run.sh image.jpg               ... plus a benchmark on that image (needs python3 + Pillow)
#   CXXFLAGS="-O3 -march=native" tests/run.sh image.jpg   compare against auto-vectorised C++
set -euo pipefail
cd "$(dirname "$0")"
SRC=../SobelFilterDLL
BUILD=$(mktemp -d)
trap 'rm -rf "$BUILD"' EXIT

# MASM -> NASM: the instructions are identical, only directives and size keywords differ
sed -e 's/^\.code/section .text/' -e 's/^PUBLIC SobelASM/global SobelASM_ms/' \
    -e 's/^SobelASM PROC/SobelASM_ms:/' -e '/^SobelASM ENDP/d' -e '/^END$/d' \
    -e 's/qword ptr/qword/g' -e 's/xmmword ptr/oword/g' "$SRC/SobelASM.asm" > "$BUILD/sobel.asm"
printf 'section .note.GNU-stack noalloc noexec nowrite progbits\n' | tee -a "$BUILD/sobel.asm" > /dev/null
cp abi_check.asm "$BUILD/abi_check.asm"

nasm -f elf64 "$BUILD/sobel.asm" -o "$BUILD/sobel.o"
nasm -f elf64 "$BUILD/abi_check.asm" -o "$BUILD/abi_check.o"
g++ -std=c++17 ${CXXFLAGS:--O2} -pthread -I"$SRC" "$SRC"/Sobel.cpp "$SRC"/Gaussian.cpp "$SRC"/Grayscale.cpp \
    "$SRC"/InterfaceDll.cpp sobel_test.cpp "$BUILD/sobel.o" "$BUILD/abi_check.o" -o "$BUILD/sobel_test"

if [[ $# -ge 1 ]]; then
    read -r W H < <(python3 -c "
from PIL import Image; im = Image.open('$1').convert('L')
open('$BUILD/image.raw', 'wb').write(im.tobytes()); print(*im.size)")
    "$BUILD/sobel_test" "$BUILD/image.raw" "$W" "$H"
else
    "$BUILD/sobel_test"
fi
