﻿; Listing generated by Microsoft (R) Optimizing Compiler Version 16.00.40219.01 

; Generated by VC++ for Common Language Runtime
.file "D:\Projects11\Pulsar\Pulsar.Server\Logger.cpp"
.global	?_logLevel@Logger@Server@Pulsar@@3EA		; Pulsar::Server::Logger::_logLevel
.global	
	.bss
.local	?_logLevel@Logger@Server@Pulsar@@3EA,1		; Pulsar::Server::Logger::_logLevel
	.align	2
.local	,4
; Function compile flags: /Odtp
	.text
.global	??0Logger@Server@Pulsar@@$$FQ$AAM@XZ		; Pulsar::Server::Logger::Logger
??0Logger@Server@Pulsar@@$$FQ$AAM@XZ:			; Pulsar::Server::Logger::Logger
;	.proc.def	D:V()

; Function Header:
; max stack depth = 2
; function size = 14 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)

;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\logger.cpp
; Line 6
	ldarg.0				; _this$
	call		??0Object@System@@$$FQ$AAM@XZ
; Line 7
	ldarg.0				; _this$
	ldc.i.3		3		; u8 0x3
	stfld		?_logLevel@Logger@Server@Pulsar@@3EA
; Line 8
	ret		
 .end ??0Logger@Server@Pulsar@@$$FQ$AAM@XZ		; Pulsar::Server::Logger::Logger
;	.proc.end.void
.global	?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA ; Pulsar::Server::Logger::<backing_store>LogToConsole
	.bss
.local	$T10973,0
.local	?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA,1 ; Pulsar::Server::Logger::<backing_store>LogToConsole
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ
.global	?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ ; Pulsar::Server::Logger::LogToConsole::get
?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ:	; Pulsar::Server::Logger::LogToConsole::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 9 bytes
; local varsig tk = 0x11000002 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)
;	.local.u1 0,"$T10972" SIG: bool

;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\logger.h
; Line 35
	ldarg.0				; _this$
	ldfld		?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA
	stloc.0				; $T10972
	ldloc.0				; $T10972
	ret		
 .end ?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ ; Pulsar::Server::Logger::LogToConsole::get
;	.proc.end.u1
.global	
.global	
.global	
.global	
	.bss
	.align	2
.local	$StringLiteralTok$??_C@_1O@FODMHNCE@?$AAP?$AAu?$AAl?$AAs?$AAa?$AAr?$AA?$AA@,4
.local	,4
.local	,4
.local	,4
.local	,4
; Function compile flags: /Odtp
	.text
.global	?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z ; Pulsar::Server::Logger::Log
?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z: ; Pulsar::Server::Logger::Log
;	.proc.def	D:V(IPIP)

; Function Header:
; max stack depth = 2
; function size = 71 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 3,"_breakLine$" SIG: bool
;	.formal.mptr 2,"_message$" SIG: string
;	.formal.mptr 4,"_args$" SIG: szarray (token:0x5113D5)
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)
;	.formal.u1 1,"_level$" SIG: u1

;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\logger.cpp
; Line 27
	ldarg.1				; _level$
	ldarg.0				; _this$
	ldfld		?_logLevel@Logger@Server@Pulsar@@3EA
	ble.s		$LN5@Log
; Line 28
	br.s		$LN6@Log
$LN5@Log:
; Line 29
	ldarg.s		4		; _args$
	ldlen		
	ldc.i.0		0		; i32 0x0
	ble.s		$LN4@Log
; Line 30
	ldarg.2				; _message$
	ldarg.s		4		; _args$
	call		?Format@String@System@@$$FSMP$AAV12@P$AAV12@P$01AP$AAVObject@2@@Z
	starg.s		2		; _message$
$LN4@Log:
; Line 31
	ldarg.3				; _breakLine$
	brfalse.s	$LN3@Log
; Line 32
	ldarg.2				; _message$
	call		?get@NewLine@Environment@System@@$$FSMP$AAVString@3@XZ
	call		?Concat@String@System@@$$FSMP$AAV12@P$AAV12@0@Z
	starg.s		2		; _message$
$LN3@Log:
; Line 33
	ldarg.0				; _this$
	call		?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ
	brfalse.s	$LN2@Log
; Line 34
	ldarg.2				; _message$
	call		?Write@Console@System@@$$FSMXP$AAVString@2@@Z
; Line 35
	br.s		$LN6@Log
$LN2@Log:
; Line 36
	ldarg.2				; _message$
	ldstr		$StringLiteralTok$??_C@_1O@FODMHNCE@?$AAP?$AAu?$AAl?$AAs?$AAa?$AAr?$AA?$AA@
	call		?Write@Debug@Diagnostics@System@@$$FSMXP$AAVString@3@0@Z
$LN6@Log:
; Line 38
	ret		
 .end ?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z ; Pulsar::Server::Logger::Log
;	.proc.end.void
.global	
	.bss
.local	$T10982,0
.local	$StringLiteralTok$??_C@_1BA@CMCHHHIG@?$AAE?$AAR?$AAR?$AAO?$AAR?$AA?3?$AA?5?$AA?$AA@,4
.local	,4
; Function compile flags: /Odtp
	.text
.global	?LogError@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVException@System@@@Z ; Pulsar::Server::Logger::LogError
?LogError@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVException@System@@@Z: ; Pulsar::Server::Logger::LogError
;	.proc.def	D:V(P)

; Function Header:
; max stack depth = 5
; function size = 41 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)
;	.formal.mptr 1,"_exc$" SIG: class (token:0x510FCD)
;	.local.mptr 0,"_s$" SIG: string

;	.proc.beg
; Line 41
	ldnull		0		; i32 0x0
	stloc.0				; _s$
; Line 42
	ldstr		$StringLiteralTok$??_C@_1BA@CMCHHHIG@?$AAE?$AAR?$AAR?$AAO?$AAR?$AA?3?$AA?5?$AA?$AA@
	ldarg.1				; _exc$
	callvirt	?get@Message@Exception@System@@$$FU$AAMP$AAVString@3@XZ
	call		?Concat@String@System@@$$FSMP$AAV12@P$AAV12@0@Z
	ldarg.1				; _exc$
	callvirt	?get@StackTrace@Exception@System@@$$FU$AAMP$AAVString@3@XZ
	call		?Concat@String@System@@$$FSMP$AAV12@P$AAV12@0@Z
	stloc.0				; _s$
; Line 43
	ldarg.0				; _this$
	ldc.i.0		0		; u8 0x0
	ldloc.0				; _s$
	ldc.i.1		1		; u8 0x1
	ldnull		0		; i32 0x0
	call		?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z
; Line 44
	ret		
 .end ?LogError@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVException@System@@@Z ; Pulsar::Server::Logger::LogError
;	.proc.end.void
; Function compile flags: /Odtp
.global ?Log@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVString@System@@@Z ; Pulsar::Server::Logger::Log
?Log@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVString@System@@@Z: ; Pulsar::Server::Logger::Log
;	.proc.def	D:V(P)

; Function Header:
; max stack depth = 5
; function size = 11 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 1,"_message$" SIG: string
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)

;	.proc.beg
; Line 12
	ldarg.0				; _this$
	ldc.i.3		3		; u8 0x3
	ldarg.1				; _message$
	ldc.i.1		1		; u8 0x1
	ldnull		0		; i32 0x0
	call		?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z
; Line 13
	ret		
 .end ?Log@Logger@Server@Pulsar@@$$FQ$AAMXP$AAVString@System@@@Z ; Pulsar::Server::Logger::Log
;	.proc.end.void
; Function compile flags: /Odtp
.global ?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@@Z ; Pulsar::Server::Logger::Log
?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@@Z: ; Pulsar::Server::Logger::Log
;	.proc.def	D:V(IP)

; Function Header:
; max stack depth = 5
; function size = 11 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 2,"_message$" SIG: string
;	.formal.u1 1,"_level$" SIG: u1
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)

;	.proc.beg
; Line 17
	ldarg.0				; _this$
	ldarg.1				; _level$
	ldarg.2				; _message$
	ldc.i.1		1		; u8 0x1
	ldnull		0		; i32 0x0
	call		?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z
; Line 18
	ret		
 .end ?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@@Z ; Pulsar::Server::Logger::Log
;	.proc.end.void
; Function compile flags: /Odtp
.global ?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_N@Z ; Pulsar::Server::Logger::Log
?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_N@Z: ; Pulsar::Server::Logger::Log
;	.proc.def	D:V(IPI)

; Function Header:
; max stack depth = 5
; function size = 11 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 3,"_breakLine$" SIG: bool
;	.formal.mptr 2,"_message$" SIG: string
;	.formal.u1 1,"_level$" SIG: u1
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x511E45).class (token:0x511E47)

;	.proc.beg
; Line 22
	ldarg.0				; _this$
	ldarg.1				; _level$
	ldarg.2				; _message$
	ldarg.3				; _breakLine$
	ldnull		0		; i32 0x0
	call		?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_NP$01AP$AAVObject@5@@Z
; Line 23
	ret		
 .end ?Log@Logger@Server@Pulsar@@$$FQ$AAMXEP$AAVString@System@@_N@Z ; Pulsar::Server::Logger::Log
;	.proc.end.void
