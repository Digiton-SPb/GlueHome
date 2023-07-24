using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace TT.Deliveries.Common.Services;

/// <summary>
/// Base abstract class implementing crontab powered scheduling service
/// </summary>
public abstract class BaseSchedulingService : IHostedService, IDisposable
{
    #region Private Fields
    private readonly string _crontabExpression;
    private Timer _timer;
    #endregion

    #region Protected Fields
    protected readonly ILogger _logger;
    protected Func<Task<bool>> _checkFunc = null;
    #endregion
    
    #region Properties
    public string CrontabExpression => _crontabExpression;
    #endregion

    #region Ctor
    protected BaseSchedulingService(string crontabExpression, ILogger logger)
    {
        _crontabExpression = crontabExpression;
        _logger = logger;
    }
    #endregion

    #region Public Virtual Methods
    public virtual async Task StartAsync(CancellationToken cancelToken)
    {
        cancelToken.Register(() => _logger.LogDebug("Task cancelling"));
        _logger.LogInformation($"Starting scheduled service");
        await ScheduleTask(cancelToken);
    }

    public virtual async Task StopAsync(CancellationToken cancelToken)
    {
        cancelToken.Register(() => _logger.LogDebug("Task cancelling"));
        _logger.LogDebug($"Stopping scheduled service");
        _timer?.Stop();
        await Task.CompletedTask;
    }

    public void SetCustomCheckFunction(Func<Task<bool>> func)
    {
        _checkFunc = func;
    }

    public virtual void Dispose()
    {
        _timer?.Dispose();
    }
    #endregion

    #region Protected Virtual Methods
    protected virtual async Task ScheduleTask(CancellationToken cancelToken)
    {
        string message = string.Empty;
        if (string.IsNullOrWhiteSpace(_crontabExpression))
        {
            message = $"HostedService: {GetType().Name}. This instance of service not appeared to run scheduled tasks.";
            _logger.LogInformation(message);
            await Task.CompletedTask;
            return;
        }
        else
        {
            message = $"HostedService: {GetType().Name}, crontab: {_crontabExpression}";
            _logger.LogInformation(message);
        }
        
        var schedule = CrontabSchedule.Parse(_crontabExpression);
        var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Utc);
        var nextOccurrence = schedule.GetNextOccurrence(now);
        var delay = nextOccurrence - now;
        _logger.LogInformation($"HostedService: {GetType().Name}. Scheduling task, next occurrence in: {nextOccurrence}, delay time: {delay}");

        _timer = new Timer(delay.TotalMilliseconds);
        _timer.Elapsed += async (sender, args) =>
        {
            _timer.Stop();

            if (_checkFunc != null && !await _checkFunc()) 
                return;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                var startTime = DateTime.UtcNow;
                message = $"HostedService: {GetType().Name}. Task started.";
                _logger.LogInformation(message);
                Console.WriteLine(message);
                
                await ExecuteAsync(cancelToken);
                
                stopwatch.Stop();
                var elapsedSec = Math.Ceiling(stopwatch.ElapsedMilliseconds / 1000.0);
                message =
                    $"HostedService: {GetType().Name}. Task completed. Start time: {startTime:yyyy-MM-dd HH:mm:ss}, elapsed: {elapsedSec} sec";
                _logger.LogInformation(message);
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HostedService: {GetType().Name}. Error occured during executing task.");
                throw;
            }
            finally
            {
                await ScheduleTask(cancelToken);
                stopwatch.Stop();
            }                
        };
        _timer.Start();

        await Task.CompletedTask;
    }

    protected virtual async Task ExecuteAsync(CancellationToken cancelToken)
    {
        await Task.CompletedTask;
    }
    #endregion
}
