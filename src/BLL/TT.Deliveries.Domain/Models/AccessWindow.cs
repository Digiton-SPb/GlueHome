using System;

namespace TT.Deliveries.Domain.Models;

/// <summary>
/// Date access window that has start and end time bounds
/// </summary>
public class AccessWindow
{
    public AccessWindow(DateTime startTime, DateTime endTime)
    {
        this.StartTime = startTime;
        this.EndTime = endTime;
    }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
