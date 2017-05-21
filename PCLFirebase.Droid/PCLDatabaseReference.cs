﻿using System;
using System.Reactive.Linq;
using Firebase.Database;
using GoogleGson;
using Java.Lang;
using Java.Util;
using Newtonsoft.Json;
using PCLFirebase.Contracts;

namespace PCLFirebase.Droid
{
    public class PCLDatabaseReference : IDatabaseReference
    {
        private readonly DatabaseReference _databaseReference;
        private readonly ReactiveValueEventListener _valueEventListener;
        private readonly Gson _gson;

        public PCLDatabaseReference(DatabaseReference databaseReference)
        {
            _databaseReference = databaseReference;
            _valueEventListener = new ReactiveValueEventListener();
            _gson = new GsonBuilder().Create();
            _databaseReference.AddValueEventListener(_valueEventListener);
        }

        public IDatabaseReference GetChild(string path)
        {
            return new PCLDatabaseReference(_databaseReference.Child(path));
        }

        public void SetValue<T>(T item)
        {
            _databaseReference
                .SetValue(_gson.FromJson(JsonConvert.SerializeObject(item), Class.FromType(typeof(HashMap))));
        }

        public IObservable<T> ObserveValueChanged<T>()
        {
            return _valueEventListener
                .DataChange
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        private T GetValueFromSnapshot<T>(DataSnapshot x)
        {
            return JsonConvert.DeserializeObject<T>(_gson.ToJson(x.Value));
        }
    }
}
