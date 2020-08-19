namespace ADS.Tamam.Modules.Integration.Helpers
{
    class IntegrationConstants
    {
        public static int BatchSize = -1;
        public static int Retries = 10;

        public static string DateFormat = "dd-MM-yyyy";
        public static string TimeFormat = "hh:mm:ss tt";
        public static string DateTimeFormat = DateFormat + " " + TimeFormat;

        public static string DefaultSysApprovalPolicy = "c99c4ec9-0841-493c-a34f-55a5b74c91b8";
        public static string DefaultSysSecurityRole = "00ec5cde-d2bb-4666-9c3f-ff7396fc657c";
    }
}