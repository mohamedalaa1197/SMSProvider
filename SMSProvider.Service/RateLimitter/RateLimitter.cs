namespace SMSProvider.Service.RateLimitter;

public class RateLimitter
{
    private readonly Dictionary<string, (int count, DateTime timestamp)> _attempts = new();
    private readonly int _maxAttempts;
    private readonly TimeSpan _timeWindow;


    public RateLimitter(int maxAttempts, TimeSpan timeWindow)
    {
        _maxAttempts = maxAttempts;
        _timeWindow = timeWindow;
    }

    public bool IsAllowed(string recipientPhone)
    {
        if (_attempts.ContainsKey(recipientPhone))
        {
            var (count, timestamp) = _attempts[recipientPhone];
            if (DateTime.UtcNow - timestamp < _timeWindow)
            {
                if (count >= _maxAttempts)
                {
                    return false;
                }
            }
            else
            {
                _attempts[recipientPhone] = (0, DateTime.UtcNow);
            }
        }

        _attempts[recipientPhone] = (_attempts.ContainsKey(recipientPhone) ? _attempts[recipientPhone].count + 1 : 1, DateTime.UtcNow);
        return true;
    }
}