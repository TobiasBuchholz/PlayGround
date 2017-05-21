﻿using System;
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
            _databaseReference.ObserveEvent(DataEventType.Value, _dataChange.OnNext);
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

        public IObservable<T> ObserveValueChanged<T>()
        {
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
    }
}
