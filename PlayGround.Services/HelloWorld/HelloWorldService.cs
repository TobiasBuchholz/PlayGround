using System;
using System.Reactive.Interfaces;
using System.Reactive.Linq;
using PlayGround.Contracts.Services.HelloWorld;
using PlayGround.Models;

namespace PlayGround.Services.HelloWorld
{
	public class HelloWorldService : IHelloWorldService
	{
        private readonly IObservable<HelloWorldModel> _counter;

        public HelloWorldService()
        {
            _counter = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Select(tick => new HelloWorldModel(tick))
                .Publish()
                .PermaRef();
        }

		public IObservable<HelloWorldModel> GetHelloWorld()
		{
            return _counter;
		}
	}
}
