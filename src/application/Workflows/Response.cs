﻿namespace application.Workflows;

public class Response
{
    private int _status;

    public int Status
    {
        get { return _status; }
        internal set
        {
            if (value >= 200 && value <= 399)
                IsSuccess = true;

            _status = value;
        }
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; set; } = string.Empty;
    public object? ObjectData { get; internal set; }
}