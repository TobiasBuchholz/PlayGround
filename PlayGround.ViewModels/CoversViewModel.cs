using System.Reactive.Disposables;
using PlayGround.Contracts.ViewModels;
using ReactiveUI;
namespace PlayGround.ViewModels
{
	public class CoversViewModel : ReactiveObject, ICoversViewModel
	{
		private readonly CompositeDisposable disposables;

		public CoversViewModel()
		{
			disposables = new CompositeDisposable();
		}

		public void Dispose()
		{
			disposables.Dispose();
		}
	}
}
