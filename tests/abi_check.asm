; Test helper: loads marker values into callee-saved registers, calls SobelASM
; through the Windows x64 calling convention and checks the markers survived.
section .text
global check_abi
extern SobelASM_ms
; int check_abi(void* params)  (SysV: rdi) -> returns 1 if xmm6/7/15 and rsi/rdi/r12/r13 survived
check_abi:
    push rbx
    push r12
    push r13
    push r14
    push r15
    sub rsp, 32
    mov rcx, rdi
    mov rax, 0x1111111111111111
    movq xmm6, rax
    mov rax, 0x7777777777777777
    movq xmm7, rax
    mov rax, 0x5555555555555555
    movq xmm15, rax
    mov r12, 0x1212
    mov r13, 0x1313
    mov r14, rdi
    call SobelASM_ms
    xor eax, eax
    movq rdx, xmm6
    mov r8, 0x1111111111111111
    cmp rdx, r8
    jne .bad
    movq rdx, xmm7
    mov r8, 0x7777777777777777
    cmp rdx, r8
    jne .bad
    movq rdx, xmm15
    mov r8, 0x5555555555555555
    cmp rdx, r8
    jne .bad
    cmp r12, 0x1212
    jne .bad
    cmp r13, 0x1313
    jne .bad
    mov eax, 1
.bad:
    add rsp, 32
    pop r15
    pop r14
    pop r13
    pop r12
    pop rbx
    ret

section .note.GNU-stack noalloc noexec nowrite progbits
