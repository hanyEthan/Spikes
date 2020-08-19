using System.Collections.Generic;

namespace ADS.Tamam.Common.Data.Model.Enums
{
    public class PolicyTypes
    {
        public const string LeavePolicyType = "1195be90-16da-43af-b35e-424369f52365";
        public const string HolidayPolicyType = "29eb56c3-e98d-47de-b704-172693205cd1";
        public const string AccrualPolicyType = "4d02b653-191a-4f5a-b6e0-fca55c6b3e8e";
        public const string ExcusesPolicyType = "2ca172be-fd85-4275-81ea-54e93df89e9b";
        public const string ShiftPolicyType = "982ba8fe-1f32-4a19-9948-c207616e6719";
        public const string OvertimePolicyType = "3ebf8eab-3ee6-47d1-a582-f08937426ce6";
        public const string HRPolicyType = "24e60e5c-df1f-4f2f-832b-bd46482aaa91";
        public const string ApprovalPolicyType = "56065693-c874-455c-ae9b-b1678d8d7f2b";
        public const string AttendancePolicyType = "13c8a962-de16-43fe-95f0-7419e5beadec";
        public const string NotificationsPolicyType = "2e97ad64-3706-40d7-a1b6-8f89b3ef0fbf";
        public const string IslamicPolicyType = "91b78f51-1ae7-4532-bdc2-b2a9a4ba2c21";
        public const string PersonCustomFields = "2b62e7e4-d9d1-46aa-b827-a8a1306240f3";

        public static Dictionary<string , string> Names = new Dictionary<string , string>()
        {
            { "1195be90-16da-43af-b35e-424369f52365" , "LeavePolicyType" } ,
            { "29eb56c3-e98d-47de-b704-172693205cd1" , "HolidayPolicyType" } ,
            { "4d02b653-191a-4f5a-b6e0-fca55c6b3e8e" , "AccrualPolicyType" } ,
            { "2ca172be-fd85-4275-81ea-54e93df89e9b" , "ExcusesPolicyType" } ,
            { "982ba8fe-1f32-4a19-9948-c207616e6719" , "ShiftPolicyType" } ,
            { "3ebf8eab-3ee6-47d1-a582-f08937426ce6" , "OvertimePolicyType" } ,
            { "24e60e5c-df1f-4f2f-832b-bd46482aaa91" , "HRPolicyType" } ,
            { "56065693-c874-455c-ae9b-b1678d8d7f2b" , "ApprovalPolicyType" } ,
            { "13c8a962-de16-43fe-95f0-7419e5beadec" , "AttendancePolicyType" } ,
            { "2e97ad64-3706-40d7-a1b6-8f89b3ef0fbf" , "NotificationsPolicyType" } ,
            { "91b78f51-1ae7-4532-bdc2-b2a9a4ba2c21" , "IslamicPolicyType" } ,
            { "2b62e7e4-d9d1-46aa-b827-a8a1306240f3" , "PersonCustomFields" } ,
        };
    }
}