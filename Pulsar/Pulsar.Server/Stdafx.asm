﻿; Listing generated by Microsoft (R) Optimizing Compiler Version 16.00.40219.01 

; Generated by VC++ for Common Language Runtime
.file "D:\Projects11\Pulsar\Pulsar.Server\Stdafx.cpp"
.global	___@@_PchSym_@00@UkilqvxghBBUkfohziUkfohziOhvieviUwvyftUhgwzucOlyq@
	.bss
.local	___@@_PchSym_@00@UkilqvxghBBUkfohziUkfohziOhvieviUwvyftUhgwzucOlyq@,4
.global	?_logLevel@Logger@Server@Pulsar@@3EA		; Pulsar::Server::Logger::_logLevel
.global	
.local	$T9631,0
.local	?_logLevel@Logger@Server@Pulsar@@3EA,1		; Pulsar::Server::Logger::_logLevel
	.align	2
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMEXZ
.global	?get@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMEXZ	; Pulsar::Server::Logger::LogLevel::get
?get@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMEXZ:	; Pulsar::Server::Logger::LogLevel::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 9 bytes
; local varsig tk = 0x11000002 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x50AB05).class (token:0x50AB07)
;	.local.u1 0,"$T9629" SIG: u1

;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\logger.h
; Line 29
	ldarg.0				; _this$
	ldfld		?_logLevel@Logger@Server@Pulsar@@3EA
	stloc.0				; $T9629
	ldloc.0				; $T9629
	ret		
 .end ?get@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMEXZ	; Pulsar::Server::Logger::LogLevel::get
;	.proc.end.u1
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMXE@Z
.global	?set@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMXE@Z ; Pulsar::Server::Logger::LogLevel::set
?set@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMXE@Z:	; Pulsar::Server::Logger::LogLevel::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 2
; function size = 8 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 1,"_level$" SIG: u1
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x50AB05).class (token:0x50AB07)

;	.proc.beg
; Line 30
	ldarg.0				; _this$
	ldarg.1				; _level$
	stfld		?_logLevel@Logger@Server@Pulsar@@3EA
	ret		
 .end ?set@LogLevel@Logger@Server@Pulsar@@$$FQ$AAMXE@Z	; Pulsar::Server::Logger::LogLevel::set
;	.proc.end.void
.global	?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA ; Pulsar::Server::Logger::<backing_store>LogToConsole
	.bss
.local	$T9638,0
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
; local varsig tk = 0x11000004 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x50AB05).class (token:0x50AB07)
;	.local.u1 0,"$T9637" SIG: bool

;	.proc.beg
; Line 35
	ldarg.0				; _this$
	ldfld		?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA
	stloc.0				; $T9637
	ldloc.0				; $T9637
	ret		
 .end ?get@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAM_NXZ ; Pulsar::Server::Logger::LogToConsole::get
;	.proc.end.u1
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAMX_N@Z
.global	?set@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAMX_N@Z ; Pulsar::Server::Logger::LogToConsole::set
?set@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAMX_N@Z:	; Pulsar::Server::Logger::LogToConsole::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 2
; function size = 8 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 1,"___set_formal$" SIG: bool
;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x50AB05).class (token:0x50AB07)

;	.proc.beg
; Line 35
	ldarg.0				; _this$
	ldarg.1				; ___set_formal$
	stfld		?<backing_store>LogToConsole@Logger@Server@Pulsar@@3_NA
	ret		
 .end ?set@LogToConsole@Logger@Server@Pulsar@@$$FQ$AAMX_N@Z ; Pulsar::Server::Logger::LogToConsole::set
;	.proc.end.void
.global	?_conStr@ServerParams@Server@Pulsar@@0P$AAVString@System@@$AA ; Pulsar::Server::ServerParams::_conStr
.global	?_sendBufferSize@ServerParams@Server@Pulsar@@0HA ; Pulsar::Server::ServerParams::_sendBufferSize
.global	?_receiveBufferSize@ServerParams@Server@Pulsar@@0HA ; Pulsar::Server::ServerParams::_receiveBufferSize
.global	?_sendTimeout@ServerParams@Server@Pulsar@@0HA	; Pulsar::Server::ServerParams::_sendTimeout
.global	?_receiveTimeout@ServerParams@Server@Pulsar@@0HA ; Pulsar::Server::ServerParams::_receiveTimeout
.global	?_port@ServerParams@Server@Pulsar@@0HA		; Pulsar::Server::ServerParams::_port
.global	?_isSqlStorage@ServerParams@Server@Pulsar@@0_NA	; Pulsar::Server::ServerParams::_isSqlStorage
.global	?_isInit@ServerParams@Server@Pulsar@@0_NA	; Pulsar::Server::ServerParams::_isInit
	.bss
	.align	2
.local	?_conStr@ServerParams@Server@Pulsar@@0P$AAVString@System@@$AA,4 ; Pulsar::Server::ServerParams::_conStr
.local	$StringLiteralTok$??_C@_1IK@PNOKDAHN@?$AAA?$AAp?$AAp?$AAl?$AAi?$AAc?$AAa?$AAt?$AAi?$AAo?$AAn?$AA?5?$AAN?$AAa?$AAm?$AAe?$AA?$DN?$AAP?$AAu?$AAl?$AAs?$AAa?$AAr?$AA?$DL?$AAP?$AAo?$AAo?$AAl?$AAi?$AAn?$AAg?$AA?$DN@,4
.local	$StringLiteralTok$??_C@_1FO@CDJHFBMB@?$AAI?$AAn?$AAt?$AAe?$AAg?$AAr?$AAa?$AAt?$AAe?$AAd?$AA?5?$AAS?$AAe?$AAc?$AAu?$AAr?$AAi?$AAt?$AAy?$AA?$DN?$AAT?$AAr?$AAu?$AAe?$AA?$DL?$AAC?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt@,4
.local	$StringLiteralTok$??_C@_1FM@NKJIAOJE@?$AAD?$AAa?$AAt?$AAa?$AA?5?$AAS?$AAo?$AAu?$AAr?$AAc?$AAe?$AA?$DN?$AA1?$AA2?$AA7?$AA?4?$AA0?$AA?4?$AA0?$AA?4?$AA1?$AA?$DL?$AAI?$AAn?$AAi?$AAt?$AAi?$AAa?$AAl?$AA?5?$AAC?$AAa@,4
.local	?_sendBufferSize@ServerParams@Server@Pulsar@@0HA,4 ; Pulsar::Server::ServerParams::_sendBufferSize
.local	?_receiveBufferSize@ServerParams@Server@Pulsar@@0HA,4 ; Pulsar::Server::ServerParams::_receiveBufferSize
.local	?_sendTimeout@ServerParams@Server@Pulsar@@0HA,4	; Pulsar::Server::ServerParams::_sendTimeout
.local	?_receiveTimeout@ServerParams@Server@Pulsar@@0HA,4 ; Pulsar::Server::ServerParams::_receiveTimeout
.local	?_port@ServerParams@Server@Pulsar@@0HA,4	; Pulsar::Server::ServerParams::_port
.local	?_isSqlStorage@ServerParams@Server@Pulsar@@0_NA,1 ; Pulsar::Server::ServerParams::_isSqlStorage
	.align	2
.local	?_isInit@ServerParams@Server@Pulsar@@0_NA,1	; Pulsar::Server::ServerParams::_isInit
; Function compile flags: /Odtp
	.text
	.comdat	any, ?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ
.global	?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ	; ?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ
?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ:		; ?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ
;	.proc.def	D:V()

; Function Header:
; max stack depth = 2
; function size = 93 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers


;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\serverparams.h
; Line 28
	ldc.i.0		0		; u8 0x0
	stsfld		?_isInit@ServerParams@Server@Pulsar@@0_NA
; Line 29
	ldc.i.1		1		; u8 0x1
	stsfld		?_isSqlStorage@ServerParams@Server@Pulsar@@0_NA
; Line 30
	ldc.i4		5021		; i32 0x139d
	stsfld		?_port@ServerParams@Server@Pulsar@@0HA
; Line 31
	ldc.i4		5000		; i32 0x1388
	stsfld		?_receiveTimeout@ServerParams@Server@Pulsar@@0HA
; Line 32
	ldc.i4		5000		; i32 0x1388
	stsfld		?_sendTimeout@ServerParams@Server@Pulsar@@0HA
; Line 33
	ldc.i4		8192		; i32 0x2000
	stsfld		?_receiveBufferSize@ServerParams@Server@Pulsar@@0HA
; Line 34
	ldc.i4		8192		; i32 0x2000
	stsfld		?_sendBufferSize@ServerParams@Server@Pulsar@@0HA
; Line 37
	ldstr		$StringLiteralTok$??_C@_1FM@NKJIAOJE@?$AAD?$AAa?$AAt?$AAa?$AA?5?$AAS?$AAo?$AAu?$AAr?$AAc?$AAe?$AA?$DN?$AA1?$AA2?$AA7?$AA?4?$AA0?$AA?4?$AA0?$AA?4?$AA1?$AA?$DL?$AAI?$AAn?$AAi?$AAt?$AAi?$AAa?$AAl?$AA?5?$AAC?$AAa@
	ldstr		$StringLiteralTok$??_C@_1FO@CDJHFBMB@?$AAI?$AAn?$AAt?$AAe?$AAg?$AAr?$AAa?$AAt?$AAe?$AAd?$AA?5?$AAS?$AAe?$AAc?$AAu?$AAr?$AAi?$AAt?$AAy?$AA?$DN?$AAT?$AAr?$AAu?$AAe?$AA?$DL?$AAC?$AAo?$AAn?$AAn?$AAe?$AAc?$AAt@
	call		?Concat@String@System@@$$FSMP$AAV12@P$AAV12@0@Z
	ldstr		$StringLiteralTok$??_C@_1IK@PNOKDAHN@?$AAA?$AAp?$AAp?$AAl?$AAi?$AAc?$AAa?$AAt?$AAi?$AAo?$AAn?$AA?5?$AAN?$AAa?$AAm?$AAe?$AA?$DN?$AAP?$AAu?$AAl?$AAs?$AAa?$AAr?$AA?$DL?$AAP?$AAo?$AAo?$AAl?$AAi?$AAn?$AAg?$AA?$DN@
	call		?Concat@String@System@@$$FSMP$AAV12@P$AAV12@0@Z
	stsfld		?_conStr@ServerParams@Server@Pulsar@@0P$AAVString@System@@$AA
; Line 38
	ret		
 .end ?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ	; ?.cctor@ServerParams@Server@Pulsar@@$$FSMXXZ
;	.proc.end.void
.global	?_logger@ServerParams@Server@Pulsar@@0P$AAVLogger@23@$AA ; Pulsar::Server::ServerParams::_logger
	.bss
	.align	2
.local	$T9646,0
.local	?_logger@ServerParams@Server@Pulsar@@0P$AAVLogger@23@$AA,4 ; Pulsar::Server::ServerParams::_logger
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@Logger@ServerParams@Server@Pulsar@@$$FSMP$AAV134@XZ
.global	?get@Logger@ServerParams@Server@Pulsar@@$$FSMP$AAV134@XZ ; Pulsar::Server::ServerParams::Logger::get
?get@Logger@ServerParams@Server@Pulsar@@$$FSMP$AAV134@XZ: ; Pulsar::Server::ServerParams::Logger::get
;	.proc.def	D:P()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000005 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9645" SIG: class (token:0x50894D)

;	.proc.beg
; Line 44
	ldsfld		?_logger@ServerParams@Server@Pulsar@@0P$AAVLogger@23@$AA
	stloc.0				; $T9645
	ldloc.0				; $T9645
	ret		
 .end ?get@Logger@ServerParams@Server@Pulsar@@$$FSMP$AAV134@XZ ; Pulsar::Server::ServerParams::Logger::get
;	.proc.end.mptr
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@Logger@ServerParams@Server@Pulsar@@$$FSMXP$AAV134@@Z
.global	?set@Logger@ServerParams@Server@Pulsar@@$$FSMXP$AAV134@@Z ; Pulsar::Server::ServerParams::Logger::set
?set@Logger@ServerParams@Server@Pulsar@@$$FSMXP$AAV134@@Z: ; Pulsar::Server::ServerParams::Logger::set
;	.proc.def	D:V(P)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_value$" SIG: class (token:0x4FFFF5)

;	.proc.beg
; Line 45
	ldarg.0				; _value$
	stsfld		?_logger@ServerParams@Server@Pulsar@@0P$AAVLogger@23@$AA
	ret		
 .end ?set@Logger@ServerParams@Server@Pulsar@@$$FSMXP$AAV134@@Z ; Pulsar::Server::ServerParams::Logger::set
;	.proc.end.void
	.bss
.local	$T9653,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@IsInit@ServerParams@Server@Pulsar@@$$FSM_NXZ
.global	?get@IsInit@ServerParams@Server@Pulsar@@$$FSM_NXZ ; Pulsar::Server::ServerParams::IsInit::get
?get@IsInit@ServerParams@Server@Pulsar@@$$FSM_NXZ:	; Pulsar::Server::ServerParams::IsInit::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000004 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.u1 0,"$T9652" SIG: bool

;	.proc.beg
; Line 52
	ldsfld		?_isInit@ServerParams@Server@Pulsar@@0_NA
	stloc.0				; $T9652
	ldloc.0				; $T9652
	ret		
 .end ?get@IsInit@ServerParams@Server@Pulsar@@$$FSM_NXZ	; Pulsar::Server::ServerParams::IsInit::get
;	.proc.end.u1
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@IsInit@ServerParams@Server@Pulsar@@$$FSMX_N@Z
.global	?set@IsInit@ServerParams@Server@Pulsar@@$$FSMX_N@Z ; Pulsar::Server::ServerParams::IsInit::set
?set@IsInit@ServerParams@Server@Pulsar@@$$FSMX_N@Z:	; Pulsar::Server::ServerParams::IsInit::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 0,"_value$" SIG: bool

;	.proc.beg
; Line 53
	ldarg.0				; _value$
	stsfld		?_isInit@ServerParams@Server@Pulsar@@0_NA
	ret		
 .end ?set@IsInit@ServerParams@Server@Pulsar@@$$FSMX_N@Z ; Pulsar::Server::ServerParams::IsInit::set
;	.proc.end.void
	.bss
.local	$T9660,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSM_NXZ
.global	?get@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSM_NXZ ; Pulsar::Server::ServerParams::IsSqlStorage::get
?get@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSM_NXZ: ; Pulsar::Server::ServerParams::IsSqlStorage::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000004 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.u1 0,"$T9659" SIG: bool

;	.proc.beg
; Line 60
	ldsfld		?_isSqlStorage@ServerParams@Server@Pulsar@@0_NA
	stloc.0				; $T9659
	ldloc.0				; $T9659
	ret		
 .end ?get@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSM_NXZ ; Pulsar::Server::ServerParams::IsSqlStorage::get
;	.proc.end.u1
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSMX_N@Z
.global	?set@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSMX_N@Z ; Pulsar::Server::ServerParams::IsSqlStorage::set
?set@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSMX_N@Z: ; Pulsar::Server::ServerParams::IsSqlStorage::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.u1 0,"_value$" SIG: bool

;	.proc.beg
; Line 61
	ldarg.0				; _value$
	stsfld		?_isSqlStorage@ServerParams@Server@Pulsar@@0_NA
	ret		
 .end ?set@IsSqlStorage@ServerParams@Server@Pulsar@@$$FSMX_N@Z ; Pulsar::Server::ServerParams::IsSqlStorage::set
;	.proc.end.void
	.bss
.local	$T9666,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@Port@ServerParams@Server@Pulsar@@$$FSMHXZ
.global	?get@Port@ServerParams@Server@Pulsar@@$$FSMHXZ	; Pulsar::Server::ServerParams::Port::get
?get@Port@ServerParams@Server@Pulsar@@$$FSMHXZ:		; Pulsar::Server::ServerParams::Port::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9665" SIG: i4

;	.proc.beg
; Line 68
	ldsfld		?_port@ServerParams@Server@Pulsar@@0HA
	stloc.0				; $T9665
	ldloc.0				; $T9665
	ret		
 .end ?get@Port@ServerParams@Server@Pulsar@@$$FSMHXZ	; Pulsar::Server::ServerParams::Port::get
;	.proc.end.i4
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@Port@ServerParams@Server@Pulsar@@$$FSMXH@Z
.global	?set@Port@ServerParams@Server@Pulsar@@$$FSMXH@Z	; Pulsar::Server::ServerParams::Port::set
?set@Port@ServerParams@Server@Pulsar@@$$FSMXH@Z:	; Pulsar::Server::ServerParams::Port::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.i4 0,"_value$" SIG: i4

;	.proc.beg
; Line 69
	ldarg.0				; _value$
	stsfld		?_port@ServerParams@Server@Pulsar@@0HA
	ret		
 .end ?set@Port@ServerParams@Server@Pulsar@@$$FSMXH@Z	; Pulsar::Server::ServerParams::Port::set
;	.proc.end.void
	.bss
.local	$T9673,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ
.global	?get@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::ReceiveTimeout::get
?get@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ: ; Pulsar::Server::ServerParams::ReceiveTimeout::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9672" SIG: i4

;	.proc.beg
; Line 76
	ldsfld		?_receiveTimeout@ServerParams@Server@Pulsar@@0HA
	stloc.0				; $T9672
	ldloc.0				; $T9672
	ret		
 .end ?get@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::ReceiveTimeout::get
;	.proc.end.i4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z
.global	?set@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::ReceiveTimeout::set
?set@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z: ; Pulsar::Server::ServerParams::ReceiveTimeout::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.i4 0,"_value$" SIG: i4

;	.proc.beg
; Line 77
	ldarg.0				; _value$
	stsfld		?_receiveTimeout@ServerParams@Server@Pulsar@@0HA
	ret		
 .end ?set@ReceiveTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::ReceiveTimeout::set
;	.proc.end.void
	.bss
.local	$T9679,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@SendTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ
.global	?get@SendTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::SendTimeout::get
?get@SendTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ:	; Pulsar::Server::ServerParams::SendTimeout::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9678" SIG: i4

;	.proc.beg
; Line 84
	ldsfld		?_sendTimeout@ServerParams@Server@Pulsar@@0HA
	stloc.0				; $T9678
	ldloc.0				; $T9678
	ret		
 .end ?get@SendTimeout@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::SendTimeout::get
;	.proc.end.i4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@SendTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z
.global	?set@SendTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::SendTimeout::set
?set@SendTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z:	; Pulsar::Server::ServerParams::SendTimeout::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.i4 0,"_value$" SIG: i4

;	.proc.beg
; Line 85
	ldarg.0				; _value$
	stsfld		?_sendTimeout@ServerParams@Server@Pulsar@@0HA
	ret		
 .end ?set@SendTimeout@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::SendTimeout::set
;	.proc.end.void
	.bss
.local	$T9685,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ
.global	?get@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::ReceiveBufferSize::get
?get@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ: ; Pulsar::Server::ServerParams::ReceiveBufferSize::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9684" SIG: i4

;	.proc.beg
; Line 92
	ldsfld		?_receiveBufferSize@ServerParams@Server@Pulsar@@0HA
	stloc.0				; $T9684
	ldloc.0				; $T9684
	ret		
 .end ?get@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::ReceiveBufferSize::get
;	.proc.end.i4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z
.global	?set@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::ReceiveBufferSize::set
?set@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z: ; Pulsar::Server::ServerParams::ReceiveBufferSize::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.i4 0,"_value$" SIG: i4

;	.proc.beg
; Line 93
	ldarg.0				; _value$
	stsfld		?_receiveBufferSize@ServerParams@Server@Pulsar@@0HA
	ret		
 .end ?set@ReceiveBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::ReceiveBufferSize::set
;	.proc.end.void
	.bss
.local	$T9691,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ
.global	?get@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::SendBufferSize::get
?get@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ: ; Pulsar::Server::ServerParams::SendBufferSize::get
;	.proc.def	D:I()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x11000008 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9690" SIG: i4

;	.proc.beg
; Line 100
	ldsfld		?_sendBufferSize@ServerParams@Server@Pulsar@@0HA
	stloc.0				; $T9690
	ldloc.0				; $T9690
	ret		
 .end ?get@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMHXZ ; Pulsar::Server::ServerParams::SendBufferSize::get
;	.proc.end.i4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z
.global	?set@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::SendBufferSize::set
?set@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z: ; Pulsar::Server::ServerParams::SendBufferSize::set
;	.proc.def	D:V(I)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.i4 0,"_value$" SIG: i4

;	.proc.beg
; Line 101
	ldarg.0				; _value$
	stsfld		?_sendBufferSize@ServerParams@Server@Pulsar@@0HA
	ret		
 .end ?set@SendBufferSize@ServerParams@Server@Pulsar@@$$FSMXH@Z ; Pulsar::Server::ServerParams::SendBufferSize::set
;	.proc.end.void
	.bss
.local	$T9697,0
; Function compile flags: /Odtp
	.text
	.comdat	any, ?get@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMP$AAVString@System@@XZ
.global	?get@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMP$AAVString@System@@XZ ; Pulsar::Server::ServerParams::MainConnectionString::get
?get@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMP$AAVString@System@@XZ: ; Pulsar::Server::ServerParams::MainConnectionString::get
;	.proc.def	D:P()

; Function Header:
; max stack depth = 1
; function size = 8 bytes
; local varsig tk = 0x1100000A 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.local.i4 0,"$T9696" SIG: string

;	.proc.beg
; Line 108
	ldsfld		?_conStr@ServerParams@Server@Pulsar@@0P$AAVString@System@@$AA
	stloc.0				; $T9696
	ldloc.0				; $T9696
	ret		
 .end ?get@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMP$AAVString@System@@XZ ; Pulsar::Server::ServerParams::MainConnectionString::get
;	.proc.end.mptr
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ?set@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMXP$AAVString@System@@@Z
.global	?set@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMXP$AAVString@System@@@Z ; Pulsar::Server::ServerParams::MainConnectionString::set
?set@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMXP$AAVString@System@@@Z: ; Pulsar::Server::ServerParams::MainConnectionString::set
;	.proc.def	D:V(P)

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_value$" SIG: string

;	.proc.beg
; Line 109
	ldarg.0				; _value$
	stsfld		?_conStr@ServerParams@Server@Pulsar@@0P$AAVString@System@@$AA
	ret		
 .end ?set@MainConnectionString@ServerParams@Server@Pulsar@@$$FSMXP$AAVString@System@@@Z ; Pulsar::Server::ServerParams::MainConnectionString::set
;	.proc.end.void
.global	
	.bss
.local	,4
; Function compile flags: /Odtp
	.text
	.comdat	any, ??0Servant@Server@Pulsar@@$$FQ$AAM@XZ
.global	??0Servant@Server@Pulsar@@$$FQ$AAM@XZ		; Pulsar::Server::Servant::Servant
??0Servant@Server@Pulsar@@$$FQ$AAM@XZ:			; Pulsar::Server::Servant::Servant
;	.proc.def	D:V()

; Function Header:
; max stack depth = 1
; function size = 7 bytes
; local varsig tk = 0x0 
; Exception Information:
; 0 handlers, each consisting of filtered handlers

;	.formal.mptr 0,"_this$" SIG: Optional C Binding Modifier(token:0x50AD29).class (token:0x50AD2B)

;	.proc.beg
; File d:\projects11\pulsar\pulsar.server\servant.h
; Line 16
	ldarg.0				; _this$
	call		??0Object@System@@$$FQ$AAM@XZ
; Line 18
	ret		
 .end ??0Servant@Server@Pulsar@@$$FQ$AAM@XZ		; Pulsar::Server::Servant::Servant
;	.proc.end.void
