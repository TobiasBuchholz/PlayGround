using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;

namespace PCLFirebase.Droid
{
    public class ReactiveValueEventListener : Java.Lang.Object, IValueEventListener
    {
        private readonly ISubject<DataSnapshot> _dataChanged;

        public ReactiveValueEventListener()
        {
            _dataChanged = new Subject<DataSnapshot>();
        }

        public void OnCancelled(DatabaseError error)
        {
            _dataChanged.OnError(new Exception(error.Message));
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            _dataChanged.OnNext(snapshot);
        }

        public IObservable<DataSnapshot> DataChange => _dataChanged.AsObservable();
    }
}
