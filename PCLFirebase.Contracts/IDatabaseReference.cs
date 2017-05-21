﻿using System;
namespace PCLFirebase.Contracts
{
    public interface IDatabaseReference
    {
        IDatabaseReference GetChild(string path);
        void SetValue(IPCLFirebaseObject firebaseObject);
        IObservable<T> ObserveValueChanged<T>(Func<object, T> factory) where T : IPCLFirebaseObject;
    }
}
