﻿namespace application.Workflows;

public class Responder
{
    public Response Response(int statusCode)
    {
        return new Response { Status = statusCode };
    }

    public Response Response(int statusCode, string message)
    {
        return new Response { Status = statusCode, Message = message };
    }

    public Response Response(int statusCode, object objectData)
    {
        return new Response { Status = statusCode, ObjectData = objectData };
    }

    public Response Response(int statusCode, string message, object objectData)
    {
        return new Response { Status = statusCode, Message = message, ObjectData = objectData };
    }
}

public class Response
{
    private int _status;

    public int Status
    {
        get => _status;
        internal set
        {
            if (value >= 200 && value <= 399)
                IsSuccess = true;

            _status = value;
        }
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; internal set; } = string.Empty;
    public object? ObjectData { get; internal set; }
}