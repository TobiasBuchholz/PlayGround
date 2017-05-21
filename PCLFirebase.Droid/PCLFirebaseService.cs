﻿using System;
using Android.Content;
using Firebase;
using Firebase.Database;
using PCLFirebase.Contracts;

namespace PCLFirebase.Droid
{
    public class PCLFirebaseService : IPCLFirebaseService
    {
        public PCLFirebaseService(
            Context context,
            string applicationId,
            string databaseUrl)
        {
            var options = new FirebaseOptions
                .Builder()
                .SetApplicationId(applicationId)
                .SetDatabaseUrl(databaseUrl)
                .Build();

            var firebaseApp = FirebaseApp.InitializeApp(context, options);
            var database = FirebaseDatabase.GetInstance(firebaseApp);
            database.SetPersistenceEnabled(true);
            RootNode = new PCLDatabaseReference(database.GetReference(""));
        }

        public IDatabaseReference RootNode { get; }
    }
}
