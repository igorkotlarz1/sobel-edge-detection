.code

PUBLIC SobelASM

; ==============================================================================
; Funkcja: SobelASM
; Argument: RCX = Wskaznik do struktury SobelParams
;
; Uklad struktury w pamieci:
; [RCX + 0]  = Input Ptr (8B)
; [RCX + 8]  = Output Ptr (8B)
; [RCX + 16] = Width (4B)
; [RCX + 20] = Height (4B)
; [RCX + 24] = Stride (4B)
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
    ;Ekstrakcja arguemntow wywolania funkcji ze structa przekazanego w C++
    mov r12, [rcx]        ; r12 = Input Ptr - adres obrazu wejsciowego
    mov r13, [rcx + 8]    ; r13 = Output Ptr - adres bufora wyjsciowego
    
    xor r8, r8            ; zerowanie R8
    mov r8d, [rcx + 16]   ; r8 = Width - szerokosc obrazu
    
    xor r9, r9
    mov r9d, [rcx + 20]   ; r9 = Height - wysokosc obrazu
    
    xor r10, r10
    mov r10d, [rcx + 24]  ; r10 = Stride - szerokosc obrazu uwzgledniajaca padding

    pxor xmm15, xmm15     ; Zerowanie XMM15

    ;Zabezpieczenie szerokoœci
    cmp r8, 9 
    jl EndLoop ; jezeli obraz wezszy niz 9px, pomin obliczenia

    ; Zabezpieczenie petli: Height = Height - 1, pomijamy ostatni wiersz
    dec r9                

    mov r11, 1            ; Y = 1
    sub r8, 8             ; Limit X = Width - 8, zostawiamy margines o szerokoœci <= 8px 

    ; Pomijamy wiersz 0, bo sobel potrzebuje 9 pikseli naraz do przetworzenia 
    ; Przesuwamy wskazniki na obraz wejsciowy i wyjsciowy na wiersz 1
    add r12, r10          ; Input += Stride
    add r13, r10          ; Output += Stride

; Petla zewnetrzna po wierszach
; R11 = licznik petli Y
LoopY:
    cmp r11, r9           
    jge EndLoop           ; Jezeli Y >= Height-1, Opuszczamy petle 

    mov rax, 1            ; Zawsze zaczynamy od kolumny 1, X = 1

LoopX:
    cmp rax, r8           
    jge NextRow           ; Jezeli offset X >= Width-8, Przejdz do nastepnego wiersza

    ; Wskazniki pomocnicze do operatora Sobela 3x3
    ; RSI - wskaznik na wiersz Górny (Y-1)
    ; R12 - wskaznik na wiersz Obecny (Y)
    ; RDI - wskaznik na wiersz Dolny (Y+1)
    mov rsi, r12
    sub rsi, r10          ; Input - Stride == Y-1
    mov rdi, r12
    add rdi, r10          ; Output + Stride == Y+1

    ; Obliczanie kernela GX
    ; -1  0  1
    ; -2  0  2
    ; -1  0  1

    ; --- PRAWA KOLUMNA (X+1) ---
    ; Prawy GORNY pixel
    movq xmm0, qword ptr [rsi + rax + 1] ; Pobieramy 8 bajtów 
    punpcklbw xmm0, xmm15                ; Rozpakowujemy bajty do slow 16 bit
    
    ; Prawy SRODKOWY pixel
    movq xmm1, qword ptr [r12 + rax + 1] ; Pobieramy 8 bajtów 
    punpcklbw xmm1, xmm15                ; Rozpakowujemy bajty do slow 16 bit
    psllw xmm1, 1                        ; Przesuniecie bitowe w lewo o 1 == Mnozenie przez 2
    
    ; Prawy DOLNY pixel
    movq xmm2, qword ptr [rdi + rax + 1] ; Pobieramy 8 bajtów 
    punpcklbw xmm2, xmm15                ; Rozpakowujemy bajty do slow 16 bit

    ; Sumowanie, XMM0 = Suma prawej kolumny kernela 
    paddw xmm0, xmm1    ; Sumowanie gory i srodka
    paddw xmm0, xmm2    ; Sumowanie wyniuku z dolem  

    ; --- LEWA KOLUMNA (X-1) ---
    ; Lewy GORNY pixel
    movq xmm3, qword ptr [rsi + rax - 1] 
    punpcklbw xmm3, xmm15
    
    ; Lewy SRODKOWY pixel
    movq xmm4, qword ptr [r12 + rax - 1] 
    punpcklbw xmm4, xmm15
    psllw xmm4, 1       ; Przesuniecie bitowe w lewo o 1 == Mnozenie przez 2
    
    ; Lewy DOLNY pixel
    movq xmm5, qword ptr [rdi + rax - 1] 
    punpcklbw xmm5, xmm15

    ; Sumowanie, XMM3 = Suma lewej kolumny kernela 
    paddw xmm3, xmm4    ; Sumowanie gory i srodka
    paddw xmm3, xmm5    ; Sumowanie wyniuku z dolem  

    ; Finalna wartosc splotu GX = PRAWA kolumna - LEWA kolumna Kernela
    psubw xmm0, xmm3    ; XMM0 = GX

    ;-------------------------------
    ; Obliczanie kernela GY
    ;  1   2   1
    ;  0   0   0
    ; -1  -2  -1

    ; --- DOLNY WIERSZ (Y+1) ---
    ; LEWY dolny pixel
    movdqa xmm1, xmm5   ; Juz mielismy zaladowany w XMM5 wyliczajac GX
    
    ; SRODKOWY dolny pixel 
    movq xmm6, qword ptr [rdi + rax]
    punpcklbw xmm6, xmm15
    psllw xmm6, 1       ; Przesuniecie bitowe w lewo o 1 == Mnozenie przez 2
    
    ; Sumowanie, XMM1 = Suma DOLNEGO wiersza kernela GY
    ; Wartosc prawego dolu rowniez byl wczesniej zaladowany do XMM2 wyliczajac GX
    paddw xmm1, xmm6    ; Sumowanie gory i srodka
    paddw xmm1, xmm2    ; Symowanie wyniku z PRAWYM dolem

    ; --- GORNY WIERSZ (Y-1) ---
    ; LEWY gorny pixel
    movq xmm4, qword ptr [rsi + rax - 1] 
    punpcklbw xmm4, xmm15

    ; SRODKOWY gorny pixel
    movq xmm7, qword ptr [rsi + rax]     
    punpcklbw xmm7, xmm15
    psllw xmm7, 1       ; Przesuniecie bitowe w lewo o 1 == Mnozenie przez 2

    ; PRAWY gorny pixel
    movq xmm5, qword ptr [rsi + rax + 1] 
    punpcklbw xmm5, xmm15

    ; Sumowanie, XMM4 = Suma GORNEGO wiersza kernela GY
    paddw xmm4, xmm7    ; Sumowanie lewo + srodek
    paddw xmm4, xmm5    ; Sumowanie wyniku z prawym

    ; Finalna wartosc splotu GY = DOLNY wiersz - GORNY wiersz Kernela 
    psubw xmm1, xmm4    ; XMM1 = GY

    ; --- OBLCIZANIE MAGNITUDY ---
    ; M = |GX| + |GY|
    ; XMM0 = MAGNITUDA Kernela
    pabsw xmm0, xmm0    ; ABS GX 
    pabsw xmm1, xmm1    ; ABS GY 
    paddw xmm0, xmm1    ; ABS GX + ABS GY   

    ; -------------------------------
    ; Pakowanie Unsigned Saturate Word do bajtow
    packuswb xmm0, xmm0 ; jezeli M > 255 to M = 255
    movq qword ptr [r13 + rax], xmm0 ; Zapis 8 pixeli do bufora wyjsciowego

    add rax, 8          ; Przesuwamy offset petli X += 8
    jmp LoopX           ; Skok bezwarunkowy do kolejnej iteracji petli wewnetrznej

NextRow:
    inc r11             ; Inkrementujemy licznik petli zewnetrznej, Y++
    add r12, r10        ; Przesuwamy wskaznik Input o Stride (caly wiersz)
    add r13, r10        ; Przesuwamy wskaznik Output o Stride
    jmp LoopY           ; Powrot do petli zewnetrznej

; Przywracanie rejestrow po zakonczeniu przetwarzania
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