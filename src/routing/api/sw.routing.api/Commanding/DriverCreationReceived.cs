﻿using System;

namespace sw.routing.api.Commanding;

/// <summary>
/// Class : DriverCreationReceived
/// </summary>
public class DriverCreationReceived
{
    public Guid CorrelationId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Gender { get; set; }
    public Guid MemberId { get; set; }
}