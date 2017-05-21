﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;
using Java.Util;
using PCLFirebase.Contracts;

namespace PCLFirebase.Droid
{
    public class PCLDatabaseReference : IDatabaseReference
    {
        private readonly DatabaseReference _databaseReference;
        private readonly ReactiveValueEventListener _valueEventListener;

        public PCLDatabaseReference(DatabaseReference databaseReference)
        {
            _databaseReference = databaseReference;
            _valueEventListener = new ReactiveValueEventListener();
            _databaseReference.AddValueEventListener(_valueEventListener);
        }

        public IDatabaseReference GetChild(string path)
        {
            return new PCLDatabaseReference(_databaseReference.Child(path));
        }

        public void SetValue(IPCLFirebaseObject firebaseObject)
        {
            _databaseReference.SetValue(firebaseObject.ToDictionary() as HashMap);
        }

        public IObservable<T> ObserveValueChanged<T>(Func<object, T> factory) where T : IPCLFirebaseObject
        {
            return _valueEventListener
                .DataChange
                .Select(x => factory(x.Value));
        }

    }
}
