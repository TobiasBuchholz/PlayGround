using ReactiveUI;

namespace PlayGround.Contracts.ViewModels
{
	public interface ICoversViewModel
	{
		IReactiveList<ICoverViewModel> CoverViewModels { get; }
	}
}
