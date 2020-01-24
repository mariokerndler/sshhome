using System;

public struct SSHResponse 
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Exception Exception { get; set; }
    public SSHResponse(bool success, string message, Exception exception = null)
    {
        Success = success;
        Message = message;
        Exception = exception;
    }
}
