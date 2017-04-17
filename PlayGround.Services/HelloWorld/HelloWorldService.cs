using System;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Models;

namespace PlayGround.Services.HelloWorld
{
	public class HelloWorldService : IHelloWorldService
	{
		public IObservable<HelloWorldModel> GetHelloWorld()
		{
			return Observable.Return(new HelloWorldModel());
		}
	}
}
