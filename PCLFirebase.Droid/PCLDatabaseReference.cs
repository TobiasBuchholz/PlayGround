﻿using System;
using System.Collections.Generic;
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
        private readonly Gson _gson;

        public PCLDatabaseReference(DatabaseReference databaseReference)
        {
            _databaseReference = databaseReference;
            _gson = new GsonBuilder().Create();
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

        public IDatabaseReference CreateChildWithAutoId()
        {
            return new PCLDatabaseReference(_databaseReference.Push());
        }

        public IObservable<T> ObserveValueChanged<T>()
        {
			var listener = new ReactiveEventListener();
            _databaseReference.AddValueEventListener(listener);
            return listener
                .ValueChanged
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        private T GetValueFromSnapshot<T>(DataSnapshot x)
        {
            return JsonConvert.DeserializeObject<T>(_gson.ToJson(x.Value));
        }

        public IObservable<IList<T>> ObserveValuesChanged<T>()
        {
            var listener = new ReactiveEventListener();
            _databaseReference.AddValueEventListener(listener);
            return listener
                .ValueChanged
                .Select(x => GetValuesFromSnapshot<T>(x));
        }

        private List<T> GetValuesFromSnapshot<T>(DataSnapshot x)
        {
            var values = new List<T>();
            var iterator = x.Children.Iterator();
            while (iterator.HasNext) {
                values.Add(GetValueFromSnapshot<T>(iterator.Next() as DataSnapshot));
            }
            return values;
        }

        public IObservable<T> ObserveChildAdded<T>()
        {
            var listener = new ReactiveEventListener();
            _databaseReference.AddChildEventListener(listener);
            return listener
                .ChildAdded
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildChanged<T>()
        {
            var listener = new ReactiveEventListener();
            _databaseReference.AddChildEventListener(listener);
            return listener
                .ChildChanged
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildMoved<T>()
        {
            var listener = new ReactiveEventListener();
            _databaseReference.AddChildEventListener(listener);
            return listener
                .ChildMoved
                .Select(x => GetValueFromSnapshot<T>(x));
        }

        public IObservable<T> ObserveChildRemoved<T>()
        {
            var listener = new ReactiveEventListener();
            _databaseReference.AddChildEventListener(listener);
            return listener
                .ChildRemoved
                .Select(x => GetValueFromSnapshot<T>(x));
        }
    }
}
