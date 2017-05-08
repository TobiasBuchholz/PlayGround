using ReactiveUI;
namespace PlayGround.Contracts.ViewModels
{
	public interface ICoverViewModel : IReactiveObject
	{
		string Title { get; }
	}
}
