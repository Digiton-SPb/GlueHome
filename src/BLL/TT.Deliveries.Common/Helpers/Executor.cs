using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TT.Deliveries.Common.Helpers {

    /// <summary>
    /// Executes method with logging of performance and exceptions.
    /// Exceptions are logged and thrown to the calling method.
    /// </summary>
    public class Executor
    {
        #region Private Fields
        private readonly ILogger _logger;
        #endregion

        #region Public Properties
        /// <summary>
        /// Get or set performance logging level. To disable performance logging should be set to LogLevel.None
        /// </summary>
        /// <value></value>
        public LogLevel PerformanceLogLevel { get; set; } = LogLevel.None;
        #endregion

        #region Ctor
        public Executor(ILogger logger) => _logger = logger;
        #endregion

        #region Public Methods
        /// <summary>
        /// Invoke method asynchronously with returning result from it
        /// </summary>
        /// <param name="func">Method to execute</param>
        /// <param name="callerMethodName"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public async Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> func, [CallerMemberName] string callerMethodName = "")
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var result = await func();
                sw.Stop();
                _logger?.Log(PerformanceLogLevel, "Method {callerMethodName} execution time: {elapsed} ms", callerMethodName, sw.ElapsedMilliseconds);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Method {callerMethodName} execution error. {errorMessage}", callerMethodName, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Invoke method asynchronously with no result
        /// </summary>
        /// <param name="func">Method to execute</param>
        /// <param name="callerMethodName"></param>
        /// <returns></returns>
        public async Task InvokeAsync(Func<Task> func, [CallerMemberName] string callerMethodName = "")
        {
            try
            {
                var sw = Stopwatch.StartNew();
                await func();
                sw.Stop();
                _logger?.Log(PerformanceLogLevel, "Method {callerMethodName} execution time: {elapsed} ms", callerMethodName, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Method {callerMethodName} execution error. {errorMessage}", callerMethodName, ex.Message);
                throw;
            }
        }
        #endregion
    }
}