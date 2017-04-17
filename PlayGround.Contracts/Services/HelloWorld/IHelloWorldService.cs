using System;
using PlayGround.Models;

namespace PlayGround.Contracts.Services.HelloWorld
{
	public interface IHelloWorldService
	{
		IObservable<HelloWorldModel> GetHelloWorld();
	}
}
