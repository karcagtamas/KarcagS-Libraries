using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace KarcagS.Shared.Utils;

public static class Extensions
{
    extension<T>(IObservable<T> source)
    {
        public IObservable<T> ThrottleMax(TimeSpan dueTime, TimeSpan maxTime) => source.ThrottleMax(dueTime, maxTime, Scheduler.Default);

        public IObservable<T> ThrottleMax(TimeSpan dueTime, TimeSpan maxTime, IScheduler scheduler)
        {
            return Observable.Create<T>(o =>
            {
                var hasValue = false;
                var value = default(T)!;

                var maxTimeDisposable = new SerialDisposable();
                var dueTimeDisposable = new SerialDisposable();

                var action = () =>
                {
                    if (hasValue)
                    {
                        maxTimeDisposable.Disposable = Disposable.Empty;
                        dueTimeDisposable.Disposable = Disposable.Empty;
                        o.OnNext(value);
                        hasValue = false;
                    }
                };

                return source.Subscribe(x =>
                {
                    if (!hasValue)
                    {
                        maxTimeDisposable.Disposable = scheduler.Schedule(maxTime, action);
                    }

                    hasValue = true;
                    value = x;
                    dueTimeDisposable.Disposable = scheduler.Schedule(dueTime, action);
                }, o.OnError, o.OnCompleted);
            });
        }
    }
}