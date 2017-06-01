using System;
using ReactiveUI;

namespace PlayGround.Contracts.ViewModels
{
	public interface ICoversViewModel : IDisposable
	{
		ReactiveList<ICoverViewModel> CoverViewModels { get; }
	}
}
