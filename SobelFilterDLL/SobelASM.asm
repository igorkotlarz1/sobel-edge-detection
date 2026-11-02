; Sobel edge detection, x64 MASM with SSE2/SSSE3, 8 pixels per iteration.
;
; void SobelASM(SobelParams* params)            ; Windows x64 ABI: RCX = params
;
; SobelParams layout (see InterfaceDll.cpp):
;   [RCX + 0]  input   (8 B)   8-bit grayscale image
;   [RCX + 8]  output  (8 B)   8-bit buffer, same stride
;   [RCX + 16] width   (4 B)
;   [RCX + 20] height  (4 B)   rows 1 .. height-2 are computed
;   [RCX + 24] stride  (4 B)   bytes per row, including padding
;
; Gx = [-1 0 1; -2 0 2; -1 0 1]   Gy = [1 2 1; 0 0 0; -1 -2 -1]
; magnitude = min(|Gx| + |Gy|, 255), the same as the C++ version.
;
; Each iteration loads 8 neighbouring bytes per kernel tap, widens them to
; 16-bit lanes (PUNPCKLBW), combines the taps with adds and shifts, takes
; PABSW, and packs back to bytes with unsigned saturation (PACKUSWB), which
; also does the clamp to 255.

.code

PUBLIC SobelASM

SobelASM PROC
    ; RSI, RDI, R12, R13 and XMM6-XMM15 are callee-saved on Windows x64.
    push rsi
    push rdi
    push r12
    push r13
    sub rsp, 56                         ; keeps RSP 16-byte aligned for MOVDQA
    movdqa xmmword ptr [rsp], xmm6
    movdqa xmmword ptr [rsp + 16], xmm7
    movdqa xmmword ptr [rsp + 32], xmm15

    mov r12, [rcx]                      ; r12 = input pointer
    mov r13, [rcx + 8]                  ; r13 = output pointer
    mov r8d, [rcx + 16]                 ; r8  = width  (32-bit mov zero-extends)
    mov r9d, [rcx + 20]                 ; r9  = height
    mov r10d, [rcx + 24]                ; r10 = stride

    pxor xmm15, xmm15                   ; zero register used for widening

    cmp r8, 10                          ; a block needs columns x-1 .. x+8
    jl Done
    cmp r9, 3                           ; and a row above and below
    jl Done

    dec r9                              ; rows 1 .. height-2
    sub r8, 9                           ; r8 = last block start (width-9), whose
                                        ; outputs end at column width-2
    mov r11, 1                          ; y = 1
    add r12, r10                        ; point both buffers at row 1
    add r13, r10

RowLoop:
    cmp r11, r9
    jge Done

    mov rsi, r12
    sub rsi, r10                        ; rsi = row y-1
    mov rdi, r12
    add rdi, r10                        ; rdi = row y+1
    mov rax, 1                          ; x = 1

BlockLoop:
    ; ---- Gx: right column (x+1) minus left column (x-1), middle row x2 ----
    movq xmm0, qword ptr [rsi + rax + 1]    ; top right
    punpcklbw xmm0, xmm15
    movq xmm1, qword ptr [r12 + rax + 1]    ; middle right
    punpcklbw xmm1, xmm15
    psllw xmm1, 1                           ; x2
    movq xmm2, qword ptr [rdi + rax + 1]    ; bottom right
    punpcklbw xmm2, xmm15
    paddw xmm0, xmm1
    paddw xmm0, xmm2                        ; xmm0 = right column sum

    movq xmm3, qword ptr [rsi + rax - 1]    ; top left
    punpcklbw xmm3, xmm15
    movq xmm4, qword ptr [r12 + rax - 1]    ; middle left
    punpcklbw xmm4, xmm15
    psllw xmm4, 1                           ; x2
    movq xmm5, qword ptr [rdi + rax - 1]    ; bottom left
    punpcklbw xmm5, xmm15
    paddw xmm3, xmm4
    paddw xmm3, xmm5                        ; xmm3 = left column sum

    psubw xmm0, xmm3                        ; xmm0 = Gx

    ; ---- Gy: bottom row minus top row (sign is irrelevant after abs) ----
    movdqa xmm1, xmm5                       ; bottom left (reused)
    movq xmm6, qword ptr [rdi + rax]        ; bottom middle
    punpcklbw xmm6, xmm15
    psllw xmm6, 1                           ; x2
    paddw xmm1, xmm6
    paddw xmm1, xmm2                        ; + bottom right (reused)

    movq xmm4, qword ptr [rsi + rax - 1]    ; top left
    punpcklbw xmm4, xmm15
    movq xmm7, qword ptr [rsi + rax]        ; top middle
    punpcklbw xmm7, xmm15
    psllw xmm7, 1                           ; x2
    movq xmm5, qword ptr [rsi + rax + 1]    ; top right
    punpcklbw xmm5, xmm15
    paddw xmm4, xmm7
    paddw xmm4, xmm5                        ; xmm4 = top row sum

    psubw xmm1, xmm4                        ; xmm1 = Gy

    ; ---- magnitude = |Gx| + |Gy|, saturated to 0..255 ----
    pabsw xmm0, xmm0
    pabsw xmm1, xmm1
    paddw xmm0, xmm1
    packuswb xmm0, xmm0
    movq qword ptr [r13 + rax], xmm0        ; store 8 output pixels

    ; ---- next block; the last one is clamped to start at width-9 ----
    cmp rax, r8
    je NextRow                              ; that was the final block
    add rax, 8
    cmp rax, r8
    jle BlockLoop
    mov rax, r8                             ; overlap the previous block slightly
    jmp BlockLoop                           ; instead of skipping the right edge

NextRow:
    inc r11
    add r12, r10
    add r13, r10
    jmp RowLoop

Done:
    movdqa xmm6, xmmword ptr [rsp]
    movdqa xmm7, xmmword ptr [rsp + 16]
    movdqa xmm15, xmmword ptr [rsp + 32]
    add rsp, 56
    pop r13
    pop r12
    pop rdi
    pop rsi
    ret

SobelASM ENDP
END
