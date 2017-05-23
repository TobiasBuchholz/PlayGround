﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;
using Foundation;
using Newtonsoft.Json;
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
        }

        public IDatabaseReference GetChild(string path)
        {
            return new PCLDatabaseReference(_databaseReference.GetChild(path));
        }

        public void SetValue<T>(T item)
        {
            var data = NSData.FromString(JsonConvert.SerializeObject(item));
            _databaseReference.SetValue(NSJsonSerialization.Deserialize(data, NSJsonReadingOptions.MutableContainers, out NSError error));
        }

        public IDatabaseReference CreateChildWithAutoId()
        {
            return new PCLDatabaseReference(_databaseReference.GetChildByAutoId());
        }

        public IObservable<T> ObserveValueChanged<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.Value, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        private T GetValueFromSnapshot<T>(DataSnapshot x)
        {

            return JsonConvert
                .DeserializeObject<T>(
                    NSJsonSerialization.Serialize(x.GetValue(), 
                                                  NSJsonWritingOptions.PrettyPrinted, 
                                                  out NSError error).ToString());
        }

        public IObservable<IList<T>> ObserveValuesChanged<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.Value, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValuesFromSnapshot<T>(x));
        }

        private List<T> GetValuesFromSnapshot<T>(DataSnapshot x)
        {
            var values = new List<T>();
            var iterator = x.Children;
            NSObject child;
            while ((child = iterator.NextObject()) != null) {
                values.Add(GetValueFromSnapshot<T>(child as DataSnapshot));
            }
            return values;
        }

        public IObservable<T> ObserveChildAdded<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.ChildAdded, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildChanged<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.ChildChanged, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildMoved<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.ChildMoved, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildRemoved<T>()
        {
            _databaseReference.ObserveEvent(DataEventType.ChildRemoved, _dataChange.OnNext);
            return _dataChange
                .AsObservable()
                .Select(x => GetValueFromSnapshot<T>(x));
        }
    }
}
