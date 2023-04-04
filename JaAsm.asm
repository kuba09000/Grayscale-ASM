.data

const dd 3

.code

MyProc1 proc

;czytanie do rejestru aktualnego pixela byte->dword
PMOVZXBD xmm0,[rcx+rdx]

;czytanie kolejnych kolorow i przesuwanie bitów 
;sumowanie kolorów rgb w jednym rejestrze
MOVD eax,xmm0

MOV r10,rax

PSRLDQ xmm0,4

MOVD eax,xmm0

ADD r10,rax

PSRLDQ xmm0,4

MOVD eax,xmm0

ADD r10,rax

MOVD xmm1,r10

CVTDQ2PS xmm1,xmm1

MOVD xmm2, [const]

CVTDQ2PS xmm2,xmm2
;dzielenie otrzymanego wyniku przez sta³¹ 3 aby zrealizowaæ algorytm (r+g+b)/3
DIVPS xmm1,xmm2

CVTSS2SI r11,xmm1

MOV rax,r11
;zwrócenie wyniku
ret

MyProc1 endp
end