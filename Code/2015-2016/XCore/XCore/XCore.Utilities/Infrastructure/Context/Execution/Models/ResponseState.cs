﻿namespace XCore.Utilities.Infrastructure.Context.Execution.Models
{
    public enum ResponseState
    {
        Success = 100,
        Error = 101,
        ValidationError = 102,
        NotFound = 103,
        AccessDenied = 104,
        AuthenticationError = 105,
        ProcessError = 106,
        InvalidInput = 107,
        Redirect = 108,
        LoginDenied = 109,
        Locked = 110,
    }
}
