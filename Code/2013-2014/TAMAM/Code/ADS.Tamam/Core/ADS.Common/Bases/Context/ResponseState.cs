namespace ADS.Common.Context
{
    public enum ResponseState
    {
        Success = 100,
        Failure = 101 ,
        Info = 102 ,
        SystemError = 103 ,
        ValidationError = 104 ,
        AccessDenied = 105 ,
        NotFound = 106 ,
        InvalidInput = 107 ,
        ProcessError = 108 ,
        AuthenticationError = 109 ,
        LicenseError = 110 ,
    };
}
