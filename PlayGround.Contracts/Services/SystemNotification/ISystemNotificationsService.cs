namespace PlayGround.Contracts.Services.SystemNotifications
{
	using System;
	using System.Reactive;

	public interface ISystemNotificationsService
    {
        IObservable<Unit> DynamicTypeChanged
        {
            get;
        }
    }
}