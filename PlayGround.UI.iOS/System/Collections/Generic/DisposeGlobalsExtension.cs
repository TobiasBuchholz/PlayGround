using System.Linq;
using System.Reactive.Concurrency;
using PlayGround.UI.iOS.Utility;
using ReactiveUI;

namespace System.Collections.Generic
{
    public static class DisposeGlobalsExtension
    {
        public static void DisposeGlobals(this IEnumerable<object> enumerable)
        {
            enumerable
                .Where(x => x is ICanDisposeGlobals)
                .Cast<ICanDisposeGlobals>()
                .ToList()
                .ForEach(x => {
                RxApp.MainThreadScheduler.Schedule(() => x.DisposeGlobalsOnMainThread());
                RxApp.TaskpoolScheduler.Schedule(() => x.DisposeGlobalsOnBackgroundThread());
            });
        }
    }
}
