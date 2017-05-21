﻿using System;
using Firebase.Analytics;
using Firebase.Database;
using Foundation;
using PCLFirebase.Contracts;

namespace PCLFirebase.iOS
{
    public class PCLFirebaseService : IPCLFirebaseService
    {
        public PCLFirebaseService()
        {
            App.Configure();

            Database.DefaultInstance.PersistenceEnabled = true;
            RootNode = new PCLDatabaseReference(Database.DefaultInstance.GetRootReference());
        }

        public IDatabaseReference RootNode { get; }
    }
}
