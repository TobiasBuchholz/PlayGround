using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using PlayGround.Services.HelloWorld;
using Xunit;
namespace PlayGround.UnitTests
{
	public class MustPassFixture
	{
		[Fact]
		public void must_pass()
		{
			var scheduler = new TestScheduler();
			var helloWorld = "";
			var sut = new HelloWorldService();

			scheduler.Schedule(TimeSpan.FromTicks(1),
			                   () => sut
			                   .GetHelloWorld()
			                   .Subscribe(text => helloWorld = text));

			scheduler.AdvanceUntilEmpty();
			Assert.Equal("Hello world!", helloWorld);
		}
	}
}
