using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using PCLMock;
using PlayGround.Models;
using PlayGround.Repositories;
using PlayGround.UnitTests.Repositories.Mock;
using PlayGround.UnitTests.Services.ServerApi.Mocks;
using Realms;
using Xunit;

namespace PlayGround.UnitTests.Repositories
{
	public class CoversRepositoryFixture
	{
		[Fact]
		public async Task get_covers_returns_covers()
		{
			var sut = new CoversRepository(
				() => RealmExtensions.GetInstanceWithoutCapturingContext(),
				new ApiServiceFactoryMock());

			var covers = await sut
				.GetCovers()
				.Take(1)
				.ToTask();

			Assert.Equal(1, covers.Count());
		}

		[Fact]
		public async Task updates_covers()
		{
			var embedded = new CoversContainerEmbedded();
			embedded.Covers.Add(new Cover { Id = 1337 });
			var coversContainer = new CoversContainer { Embedded = embedded };

			var coversApiServiceMock = new CoversApiServiceMock();
			coversApiServiceMock
				.When(x => x.AuthorizedGet(It.IsAny<string>()))
				.ReturnsObservable(coversContainer);

			var apiServiceFactory = new ApiServiceFactoryMock();
			apiServiceFactory
				.When(x => x.CreateSimpleApiService<CoversContainer>(It.IsAny<string>()))
				.Return(coversApiServiceMock);

			var sut = new CoversRepository(
				() => RealmExtensions.GetInstanceWithoutCapturingContext(),
				apiServiceFactory);
			
			await sut.UpdateCovers().ToTask();

			var covers = RealmExtensions.GetInstanceWithoutCapturingContext().All<Cover>();
			Assert.Equal(1, covers.Count());
		}

		[Fact]
		public async Task get_covers_emits_items_when_covers_get_updated()
		{
			var embedded = new CoversContainerEmbedded();
			embedded.Covers.Add(new Cover { Id = 1337 });
			var coversContainer = new CoversContainer { Embedded = embedded };

			var coversApiServiceMock = new CoversApiServiceMock();
			coversApiServiceMock
				.When(x => x.AuthorizedGet(It.IsAny<string>()))
				.ReturnsObservable(coversContainer);

			var apiServiceFactory = new ApiServiceFactoryMock();
			apiServiceFactory
				.When(x => x.CreateSimpleApiService<CoversContainer>(It.IsAny<string>()))
				.Return(coversApiServiceMock);

			var sut = new CoversRepository(
				() => RealmExtensions.GetInstanceWithoutCapturingContext(),
				apiServiceFactory);

			var countTask = sut
				.GetCovers()
				.Take(2)
				.Count()
				.ToTask();

			await sut.UpdateCovers().ToTask();

			var count = await countTask;
			Assert.Equal(2, count);
		}
	}
}
