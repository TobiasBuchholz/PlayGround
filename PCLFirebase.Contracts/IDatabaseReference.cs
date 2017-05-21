﻿using System;
namespace PCLFirebase.Contracts
{
    public interface IDatabaseReference
    {
        IDatabaseReference GetChild(string path);
        void SetValue<T>(T item);
        IObservable<T> ObserveValueChanged<T>();
    }
}
