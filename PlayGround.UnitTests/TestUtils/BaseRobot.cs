using System;
using System.Linq.Expressions;
using Genesis.Ensure;
using Genesis.TestUtil;
using Microsoft.Reactive.Testing;
using PCLMock;
using PlayGround.Contracts.Repositories;
using PlayGround.Contracts.Services.ServerApi;
using PlayGround.UnitTests.Repositories.Mock;
using PlayGround.UnitTests.Services.ServerApi.Mocks;

namespace PlayGround.UnitTests.TestUtils
{
	public abstract class BaseRobot<TRobot, TResult> : IBuilder 
		where TRobot : BaseRobot<TRobot, TResult>
		where TResult : BaseRobotResult
	{
		protected TestScheduler _scheduler;
		protected IApiServiceFactory _apiServiceFactory;
		protected ICoversRepository _coversRepository;

		protected BaseRobot()
		{
			_scheduler = new TestScheduler();
		}

		public TRobot WithScheduler(TestScheduler scheduler) => 
			With(ref _scheduler, scheduler);

		public TRobot WithApiServiceFactory(IApiServiceFactory factory) => 
			With(ref _apiServiceFactory, factory);

		public TRobot WithCoversRepository(ICoversRepository coversRepository) => 
			With(ref _coversRepository, coversRepository);

		public TRobot With<TField>(ref TField field, TField value)
		{
			field = value;
			return (TRobot) this;
		}

		public virtual TRobot Build()
		{
			_apiServiceFactory = _apiServiceFactory ?? CreateApiServiceFactoryMock();
			_coversRepository = _coversRepository ?? CreateCoversRepositoryMock();
			return (TRobot) (this);
		}

		protected virtual ApiServiceFactoryMock CreateApiServiceFactoryMock() => new ApiServiceFactoryMock();
		protected virtual CoversRepositoryMock CreateCoversRepositoryMock() => new CoversRepositoryMock();

		public TResult AdvanceUntilEmpty()
		{
			_scheduler.AdvanceUntilEmpty();
			return CreateResultWithServices();
		}

		private TResult CreateResultWithServices()
		{
			var result = CreateResult();
			result.ApiServiceFactory = _apiServiceFactory;
			result.CoversRepository = _coversRepository;
			return result;
		}

		protected abstract TResult CreateResult();

		public TResult AdvanceTo(long time)
		{
			_scheduler.AdvanceTo(time);
			return CreateResultWithServices();
		}
	}

	public abstract class BaseRobotResult
	{
		public IApiServiceFactory ApiServiceFactory { get; set; }
		public ICoversRepository CoversRepository { get; set; }

		public VerifyContinuation VerifyApiServiceFactoryMock(Expression<Action<IApiServiceFactory>> selector)
		{
			Ensure.Condition(ApiServiceFactory is ApiServiceFactoryMock, () => new ArgumentException("ApiServiceFactory is not a mock"));

			var mock = (ApiServiceFactoryMock) ApiServiceFactory;
			return mock.Verify(selector);
		}

		public VerifyContinuation VerifyCoversRepositoryMock(Expression<Action<ICoversRepository>> selector)
		{
			Ensure.Condition(CoversRepository is CoversRepositoryMock, () => new ArgumentException("CoversRepository is not a mock"));

			var mock = (CoversRepositoryMock) CoversRepository;
			return mock.Verify(selector);
		}
	}
}
