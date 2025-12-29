.code

PUBLIC SobelASM

; ==============================================================================
; Funkcja: SobelASM
; Argument: RCX = Wskaznik do struktury SobelParams
;
; Uklad struktury w pamieci:
; [RCX + 0]  = Input Ptr (8 bajtow)
; [RCX + 8]  = Output Ptr (8 bajtow)
; [RCX + 16] = Width (4 bajty)
; [RCX + 20] = Height (4 bajty)
; [RCX + 24] = Stride (4 bajty)
; ==============================================================================

SobelASM PROC
    push rbp
    push rbx
    push rsi
    push rdi
    push r12
    push r13
    push r14
    push r15

    ;U¿ywamy rdx, r8, r9 jako zmiennych lokalnych, bo dane sa w RCX
    
    mov r12, [rcx]        ; r12 = Input Ptr
    mov r13, [rcx + 8]    ; r13 = Output Ptr
    
    xor r8, r8
    mov r8d, [rcx + 16]   ; r8 = Width
    
    xor r9, r9
    mov r9d, [rcx + 20]   ; r9 = Height
    
    xor r10, r10
    mov r10d, [rcx + 24]  ; r10 = Stride

    pxor xmm15, xmm15     ; Zerowanie XMM15

    ; Zabezpieczenie szerokoœci
    cmp r8, 9
    jl EndLoop

    ; Zabezpieczenie petli Y (Height - 1)
    dec r9

    mov r11, 1            ; Y = 1
    sub r8, 8             ; Limit X = Width - 8

    ; Przesuwamy wskazniki na start (Wiersz 1)
    add r12, r10          ; Input += Stride
    add r13, r10          ; Output += Stride

LoopY:
    cmp r11, r9           ; if Y >= Height-1
    jge EndLoop

    mov rax, 1            ; X = 1

LoopX:
    cmp rax, r8           ; if X >= Width-8
    jge NextRow

    ; Wskazniki pomocnicze
    mov rsi, r12
    sub rsi, r10          ; Gora (Y-1)
    mov rdi, r12
    add rdi, r10          ; Dol (Y+1)

    ; --- PRAWA STRONA (X+1) ---
    movq xmm0, qword ptr [rsi + rax + 1] ; Top-Right
    punpcklbw xmm0, xmm15                
    
    movq xmm1, qword ptr [r12 + rax + 1] ; Mid-Right
    punpcklbw xmm1, xmm15
    psllw xmm1, 1                        
    
    movq xmm2, qword ptr [rdi + rax + 1] ; Bot-Right
    punpcklbw xmm2, xmm15

    paddw xmm0, xmm1
    paddw xmm0, xmm2    ; XMM0 = Prawa

    ; --- LEWA STRONA (X-1) ---
    movq xmm3, qword ptr [rsi + rax - 1] ; Top-Left
    punpcklbw xmm3, xmm15
    
    movq xmm4, qword ptr [r12 + rax - 1] ; Mid-Left
    punpcklbw xmm4, xmm15
    psllw xmm4, 1
    
    movq xmm5, qword ptr [rdi + rax - 1] ; Bot-Left
    punpcklbw xmm5, xmm15

    paddw xmm3, xmm4
    paddw xmm3, xmm5    ; XMM3 = Lewa

    psubw xmm0, xmm3    ; XMM0 = GX

    ; --- Dol (GY) ---
    movdqa xmm1, xmm5   ; Bot-Left
    
    movq xmm6, qword ptr [rdi + rax] ; Bot-Mid
    punpcklbw xmm6, xmm15
    psllw xmm6, 1
    
    paddw xmm1, xmm6
    paddw xmm1, xmm2    ; + Bot-Right
    ; XMM1 = Dol

    ; --- GoRA (GY) ---
    movq xmm4, qword ptr [rsi + rax - 1] ; Top-Left
    punpcklbw xmm4, xmm15

    movq xmm7, qword ptr [rsi + rax]     ; Top-Mid
    punpcklbw xmm7, xmm15
    psllw xmm7, 1

    movq xmm5, qword ptr [rsi + rax + 1] ; Top-Right
    punpcklbw xmm5, xmm15

    paddw xmm4, xmm7
    paddw xmm4, xmm5    ; XMM4 = Gora

    psubw xmm1, xmm4    ; XMM1 = GY

    ; --- MAGNITUDA ---
    pabsw xmm0, xmm0    ; ABS GX 
    pabsw xmm1, xmm1    ; ABS GY 
    paddw xmm0, xmm1    ; Suma

    packuswb xmm0, xmm0 ; Pakowanie do bajtow
    movq qword ptr [r13 + rax], xmm0 ; Zapis wyniku

    add rax, 8          ; X += 8
    jmp LoopX

NextRow:
    inc r11             ; Y++
    add r12, r10        ; Przesuniecie o Stride
    add r13, r10
    jmp LoopY

EndLoop:
    pop r15
    pop r14
    pop r13
    pop r12
    pop rdi
    pop rsi
    pop rbx
    pop rbp
    ret

SobelASM ENDP
END