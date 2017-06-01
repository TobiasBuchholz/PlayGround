using System;
using ReactiveUI;
namespace PlayGround.Contracts.ViewModels
{
	public interface ICoverViewModel : IReactiveObject, IDisposable
	{
		string Title { get; }
        string ImageUrl { get; }
	}
}
