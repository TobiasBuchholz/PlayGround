using ReactiveUI;

namespace PlayGround.Contracts.ViewModels
{
	public interface ICoversViewModel
	{
		IReactiveDerivedList<ICoverViewModel> CoverViewModels { get; }
	}
}
