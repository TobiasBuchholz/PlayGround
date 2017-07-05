using System;
using ReactiveUI;
namespace PlayGround.Contracts.ViewModels
{
	public interface ICoverViewModel : IReactiveObject, IDisposable
	{
		string Title { get; }
        string ImageUrl { get; }

        string Property1 { get; }
        string Property2 { get; }
        string Property3 { get; }
        string Property4 { get; }
        string Property5 { get; }
        string Property6 { get; }
        string Property7 { get; }
	}
}
