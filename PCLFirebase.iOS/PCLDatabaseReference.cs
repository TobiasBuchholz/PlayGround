﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;
using Foundation;
using PCLFirebase.Contracts;

namespace PCLFirebase.iOS
{
    public class PCLDatabaseReference : IDatabaseReference
    {
        private readonly DatabaseReference _databaseReference;
        private readonly ISubject<DataSnapshot> _dataChange;

        public PCLDatabaseReference(DatabaseReference databaseReference)
        {
            _databaseReference = databaseReference;
            _dataChange = new Subject<DataSnapshot>();
            _databaseReference.ObserveEvent(DataEventType.Value, _dataChange.OnNext);
        }

        public IDatabaseReference GetChild(string path)
        {
            return new PCLDatabaseReference(_databaseReference.GetChild(path));
        }

        public void SetValue(IPCLFirebaseObject firebaseObject)
        {
            _databaseReference.SetValue(firebaseObject.ToDictionary() as NSDictionary);
        }

        public IObservable<T> ObserveValueChanged<T>(Func<object, T> factory) where T : IPCLFirebaseObject
        {
            return _dataChange
                .AsObservable()
                .Select(x => factory(x.GetValue()));
        }
    }
}
