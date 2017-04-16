using System;
namespace PlayGround.Contracts.Services.HelloWorld
{
	public interface IHelloWorldService
	{
		IObservable<string> GetHelloWorld();
	}
}
