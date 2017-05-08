using ReactiveUI;

namespace PlayGround.Contracts.ViewModels
{
	public interface ICoversViewModel
	{
		ReactiveList<ICoverViewModel> CoverViewModels { get; }
	}
}
