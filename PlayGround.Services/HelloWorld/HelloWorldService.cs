using System;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;

namespace PlayGround.Services.HelloWorld
{
	public class HelloWorldService : IHelloWorldService
	{
		public IObservable<string> GetHelloWorld()
		{
			return Observable.Return("Hello world!");
		}
	}
}
