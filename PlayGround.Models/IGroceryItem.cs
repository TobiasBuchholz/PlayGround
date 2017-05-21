using System;
using PCLFirebase.Contracts;

namespace PlayGround.Models
{
    public interface IGroceryItem : IPCLFirebaseObject
    {
        string Name { get; set; }
        int Amount { get; set; }
    }
}
