using System.Reactive.Subjects;

namespace KarcagS.Http;

public class HttpRefreshService
{
    public readonly BehaviorSubject<RefreshState> RefreshInProgressSubject = new(RefreshState.FinishState(true));

    public RefreshState Current() => RefreshInProgressSubject.Value;

    public record RefreshState(bool InProgress, bool LastSuccess)
    {
        public static RefreshState ProgressState(bool lastSuccess)
        {
            return new RefreshState(true, lastSuccess);
        }

        public static RefreshState FinishState(bool lastSuccess)
        {
            return new RefreshState(false, lastSuccess);
        }
    }
}