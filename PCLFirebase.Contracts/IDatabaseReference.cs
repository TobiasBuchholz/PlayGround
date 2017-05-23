﻿using System;
using System.Collections.Generic;

namespace PCLFirebase.Contracts
{
    public interface IDatabaseReference
    {
        IDatabaseReference GetChild(string path);
        void SetValue<T>(T item);
		IDatabaseReference CreateChildWithAutoId();
        IObservable<T> ObserveValueChanged<T>();
		IObservable<IList<T>> ObserveValuesChanged<T>();
        IObservable<T> ObserveChildAdded<T>();
        IObservable<T> ObserveChildChanged<T>();
        IObservable<T> ObserveChildMoved<T>();
        IObservable<T> ObserveChildRemoved<T>();
    }
}
