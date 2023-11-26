using System.Reactive.Subjects;

namespace KarcagS.Http;

public class HttpRefreshService
{
    public readonly AsyncSubject<RefreshState> RefreshInProgressSubject = new();

    public RefreshState Current() => RefreshInProgressSubject.GetResult();

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