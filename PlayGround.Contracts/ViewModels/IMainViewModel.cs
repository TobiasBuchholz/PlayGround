using System;

namespace PlayGround.Contracts.ViewModels
{
	public interface IMainViewModel : IDisposable
	{
		string HelloWorldText { get; }
	}
}
