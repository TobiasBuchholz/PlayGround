﻿using System;
namespace PCLFirebase.Contracts
{
    public interface IPCLFirebaseService
    {
        IDatabaseReference RootNode {get; }
    }
}
