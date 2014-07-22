// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once


#include <msclr\lock.h>
#include <msclr\auto_handle.h>

#define null nullptr

using namespace msclr;
using namespace System;
using namespace System::Collections::Generic;

#using "Pulsar.Atoms.dll"
#using "Pulsar.Bricks.dll"
#using "Pulsar.Clients.dll"
//#using "Pulsar.Serialization.dll"

#include "ThreadContext.h"
#include "ServerParams.h"
#include "Servant.h"
#include "Action2Invoker.h"
#include "MessageBusRecipient.h"
#include "ServerMessageBus.h"

#include "GlobalObjectsManager.h"

#include "AccessToken.h"
#include "ACE.h"
#include "SecurityGroup.h"
#include "PulsarSecurity.h"

#include "SchedulerServant.h"
#include "PulsarCore.h"

