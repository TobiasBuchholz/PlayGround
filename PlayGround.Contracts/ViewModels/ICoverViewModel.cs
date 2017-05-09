using ReactiveUI;
using Splat;
namespace PlayGround.Contracts.ViewModels
{
	public interface ICoverViewModel : IReactiveObject
	{
		string Title { get; }
		IBitmap Image { get; }
	}
}
