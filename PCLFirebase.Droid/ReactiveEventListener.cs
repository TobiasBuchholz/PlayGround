using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Firebase.Database;

namespace PCLFirebase.Droid
{
    public class ReactiveEventListener : Java.Lang.Object, IValueEventListener, IChildEventListener
    {
        private readonly ISubject<DataSnapshot> _valueChanged;
        private readonly ISubject<DataSnapshot> _childAdded;
        private readonly ISubject<DataSnapshot> _childChanged;
        private readonly ISubject<DataSnapshot> _childMoved;
        private readonly ISubject<DataSnapshot> _childRemoved;

        public ReactiveEventListener()
        {
            _valueChanged = new Subject<DataSnapshot>();
            _childAdded = new Subject<DataSnapshot>();
            _childChanged = new Subject<DataSnapshot>();
            _childMoved = new Subject<DataSnapshot>();
            _childRemoved = new Subject<DataSnapshot>();
        }

        public void OnCancelled(DatabaseError error)
        {
            _valueChanged.OnError(new Exception(error.Message));
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            _valueChanged.OnNext(snapshot);
        }

        public void OnChildAdded(DataSnapshot snapshot, string previousChildName)
        {
            _childAdded.OnNext(snapshot);
        }

        public void OnChildChanged(DataSnapshot snapshot, string previousChildName)
        {
            _childChanged.OnNext(snapshot);
        }

        public void OnChildMoved(DataSnapshot snapshot, string previousChildName)
        {
            _childMoved.OnNext(snapshot);
        }

        public void OnChildRemoved(DataSnapshot snapshot)
        {
            _childRemoved.OnNext(snapshot);
        }

        public IObservable<DataSnapshot> ValueChanged => _valueChanged.AsObservable();
        public IObservable<DataSnapshot> ChildAdded => _childAdded.AsObservable();
        public IObservable<DataSnapshot> ChildChanged => _childChanged.AsObservable();
        public IObservable<DataSnapshot> ChildMoved => _childMoved.AsObservable();
        public IObservable<DataSnapshot> ChildRemoved => _childRemoved.AsObservable();
    }
}
