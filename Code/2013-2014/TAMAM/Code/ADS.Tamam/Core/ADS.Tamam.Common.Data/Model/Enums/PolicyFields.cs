using System;

namespace ADS.Tamam.Common.Data.Model.Enums
{
    public class PolicyFields
    {
        #region AccrualPolicy
        public class AccrualPolicy
        {
            public static readonly Guid AnnualAccrualType = Guid.Parse( "ca6e554f-98af-4c9f-9c2b-3937dbb1cf5a" );
            public static readonly Guid YearStart = Guid.Parse( "9b7fb782-9bec-458e-a666-e68182c4059f" );
        }
        #endregion

        #region ApprovalsPolicy
        
        public class ApprovalsPolicy
        {
            public static readonly Guid EnableIterations = Guid.Parse( "4dfeac4f-18f3-40d4-8ef0-f5ac3803d174" );
            public static readonly Guid MaxIterationsCount = Guid.Parse( "bc2c7d25-5d62-434c-8e02-5e988b13503a" );
            public static readonly Guid IncludePassedStepsPerIteration = Guid.Parse( "13e91d33-5c5e-4d1b-bafe-b6f2c2669b83" );

            public static readonly Guid HR = Guid.Parse( "59002052-1afc-4278-992a-073b53a2e2bd" );
            public static readonly Guid SuperManagerSequance = Guid.Parse( "7c7d4087-db36-4c14-9d86-2cc718c6596f" );
            public static readonly Guid SuperManagerCondition = Guid.Parse( "55ff085f-62a2-4aff-af98-39154279a906" );
            public static readonly Guid DirectManagerSequance = Guid.Parse( "95b76653-b110-42b9-8190-3f16a44bda41" );
            public static readonly Guid SuperManager = Guid.Parse( "6091de22-e6ff-4e69-9667-6f8993eb1b00" );
            public static readonly Guid DirectManager = Guid.Parse( "7499bff1-8d56-4d31-80c7-bfec5631a06c" );
            public static readonly Guid HRCondition = Guid.Parse( "f26b764e-a95b-47e0-9bb7-c1b310bf690e" );
            public static readonly Guid HRSequance = Guid.Parse( "d0d56509-51a6-44aa-bab6-d4ca820a80a3" );
            public static readonly Guid DirectManagerCondition = Guid.Parse( "b76cad6e-2caf-4fbf-bbdb-dcc6c181be1d" );
        }

        #endregion

        #region HRPolicy
        public class HRPolicy
        {
            public static readonly Guid HRDepartment = Guid.Parse( "ffb3cc5e-d7b1-4947-9448-6ef47d9a9bf9" );
            public static readonly Guid HRRole = Guid.Parse( "3f6f5ab6-7964-4098-b870-3c8d5155d2fd" );
            public static readonly Guid CCs = Guid.Parse( "9322E5A1-BB0B-433F-AF9A-9CBDA44E8C49" );
        }
        #endregion

        #region HolidayPolicy
        public class HolidayPolicy
        {
            public static readonly Guid DateTo = Guid.Parse( "5f352350-a7e8-4054-885e-dba635e4c982" );
            public static readonly Guid DateFrom = Guid.Parse( "9aae97b7-01d6-4076-b18e-f6ba46289986" );
        }
        #endregion

        #region LeavePolicy
        public class LeavePolicy
        {
            public static readonly Guid LeaveType = Guid.Parse( "03d3390e-4b4c-4568-a914-5528e29bd3e1" );
            public static readonly Guid AllowedAmount = Guid.Parse( "70af6239-2c70-4dba-abef-86a68727ec76" );
            public static readonly Guid CarryOver = Guid.Parse( "fc4cd4d1-ffe4-4ffd-8e32-b0c0714f5fe6" );
            public static readonly Guid ApprovalPolicy = Guid.Parse( "dac403c5-0fe4-4f8d-8c1d-b43515df0118" );
            public static readonly Guid DaysBeforeRequest = Guid.Parse( "0c05c3db-1eee-4293-a2c6-bb5f8c51502c" );
            public static readonly Guid AllowRequests = Guid.Parse( "10b438d5-8074-46b1-8ada-c225781b0436" );
            public static readonly Guid MaxCarryOverDays = Guid.Parse( "353c9234-9461-4807-848a-dead28d3bd48" );
            public static readonly Guid AllowHalfDays = Guid.Parse( "79184acb-631b-4e29-b3c0-c82d848a0157" );
            public static readonly Guid RequireAttachments = Guid.Parse( "E293ECDC-5405-4FB1-A318-88149641055B" );
            public static readonly Guid IncludeWeekEndsAndHolidays = Guid.Parse("57c1cf7a-1d58-4293-872e-301a58ce5e81");
            public static readonly Guid DaysLimitForOldLeaves = Guid.Parse( "ec8ae5a7-de4e-402f-ae7d-9e4c8a192720" );
            public static readonly Guid MaxDaysPerRequest = Guid.Parse("4E5CF38A-23A3-4AEC-BA0B-CCCC5AD9B0F0");
            public static readonly Guid EssentialCredit = Guid.Parse("FE71477B-C5F9-4F98-9F1D-AC1EA775AEE7");
            public static readonly Guid DisablePlannedLeaves = Guid.Parse("8EDA273C-06BC-4EEA-AB4A-62C151BBB28D");
            public static readonly Guid UnlimitedCredit = Guid.Parse("8942CE32-AFB0-408A-9717-EC2BB02DA940");
            public static readonly Guid ExceedsProgressiveCredit = Guid.Parse( "81563F32-6BCD-481F-968C-8650AB2A17EB" );
        }
        #endregion

        # region ExcusePolicy
        
        public class ExcusePolicy
        {
            public static readonly Guid MaxExcusesPerDay = Guid.Parse( "2f606ebd-4bd7-48b8-b4d5-dba9c7017382" );
            public static readonly Guid MaxExcusesPerMonth = Guid.Parse( "7c748e24-d04f-408d-ba14-56bc40e643bd" );
            public static readonly Guid AllowedHoursPerDay = Guid.Parse( "9c043f42-33da-4c16-a19b-cee897ef5e88" );
            public static readonly Guid AllowedHoursPerMonth = Guid.Parse( "2e036613-96b7-4ab1-bd1e-64b5882cb919" );
            public static readonly Guid MinExcuseDuration = Guid.Parse( "dedd09d9-1467-4aab-8726-9aa5de16ed6d" );
            // public static readonly Guid HasCredit = Guid.Parse("52694eb0-b74d-4334-bea2-8938f2e1e881");
            public static readonly Guid ApprovalPolicy = Guid.Parse( "18c1a939-33e5-4b71-906a-f83aa0ead14f" );
            public static readonly Guid ExcuseType = Guid.Parse( "3d671f48-c4bc-48d8-a5f4-17e5aa1d98cc" );
        }

        # endregion

        #region OvertimePolicy
        public class OvertimePolicy
        {
            public static readonly Guid MaxOvertime = Guid.Parse( "151336f0-e61d-4792-ac76-1e3c9aa6e5f8" );
            public static readonly Guid WeekendOvertimeRate = Guid.Parse( "1131ce71-d4ac-42b8-977d-3adeff4acecd" );
            public static readonly Guid HolidayOvertimeRate = Guid.Parse( "31cd9bad-05db-4a3d-b7c5-4353f9207527" );
            public static readonly Guid OvertimeRate = Guid.Parse( "8b508443-8a3b-403a-895b-43dda0c14bce" );
            public static readonly Guid OvertimeRelatedToWorkingHours = Guid.Parse( "8adfb9b9-ab2b-4d42-b7e1-677d41646f22" );
            public static readonly Guid MaxWeekendOvertime = Guid.Parse( "831bfec9-d458-4eb2-9f8e-8f3b9de8abff" );
            public static readonly Guid MaxHolidayOvertime = Guid.Parse( "916389dc-18b5-4db6-87f3-9f21ec2f33d3" );
            public static readonly Guid MaxLeaveOvertime = Guid.Parse( "e1deb019-e279-4159-8abe-adb66f3407fa" );
            public static readonly Guid LeaveOvertimeRate = Guid.Parse( "d6d80eae-d894-44db-9c62-dc3323fb2991" );
        }
        #endregion

        # region AttendancePolicy

        public class AttendancePolicy
        {
            public static readonly Guid EnableManualEditApprovals = Guid.Parse( "121912E2-DCDA-42F2-AD0E-A65B7BDC4741" );
            public static readonly Guid ManualEditApprovalPolicy = Guid.Parse( "27FBF739-3135-4D9D-8E3B-0FE6B3D88A5B" );
            public static readonly Guid ViolationsApprovalPolicy = Guid.Parse( "841a94e7-fa4b-4c9f-adee-6dcfb8f7ac0b" );
            public static readonly Guid ConsiderEarlyLeaveAsViolation  = Guid.Parse("5D074C61-B466-4366-9080-E289036B4FCF");
            public static readonly Guid ConsiderAbsentAsViolation      = Guid.Parse("83117EF4-06D3-4E2D-AF06-87370E8D2025");           
            public static readonly Guid ConsiderInLateAsViolation      = Guid.Parse("5A39E334-54FF-40E2-B154-82CDD51A229A");
            public static readonly Guid ConsiderLateAbsentAsViolation  = Guid.Parse("4103FE7D-30F5-476B-94A5-2584D9B9B69D");
            public static readonly Guid ConsiderMissedPunchAsViolation = Guid.Parse("1DF8DD1E-30BD-43A5-971D-53915EB57A00");
            public static readonly Guid ConsiderWorkingLessAsViolation = Guid.Parse("4ED52DD7-3798-4916-9B3B-F8A06D0B0129");
        }

        # endregion

        #region NotificationsPolicy

        public class NotificationsPolicy
        {
            public static readonly Guid EnableNotifications = Guid.Parse( "5F56895E-A747-4569-A602-754BC71E7178" );

            public static readonly Guid InLate_Tamam = Guid.Parse( "663bd350-060d-40ea-a4db-0d695c6cfb34" );
            public static readonly Guid InLate_Email = Guid.Parse( "e0cd4e06-7af7-455e-82a7-2cc7524b8c14" );
            public static readonly Guid InLate_SMS = Guid.Parse( "86018de2-ebc3-4264-ba7d-678f333be5d1" );
            public static readonly Guid InLate_Delay = Guid.Parse( "1d60e63b-72ba-406e-ab73-3b34c1d7f39e" );

            public static readonly Guid LateAbsent_Tamam = Guid.Parse( "12f6629a-a192-4409-ba77-37cacb61eea3" );
            public static readonly Guid LateAbsent_Email = Guid.Parse( "ade73d80-8dcc-40e9-bf3d-e5ed96adf024" );
            public static readonly Guid LateAbsent_SMS = Guid.Parse( "0cf7f641-6a97-4035-9f7d-a79888d028c8" );
            public static readonly Guid LateAbsent_Delay = Guid.Parse( "5ef09471-7947-4eda-a146-dadc70bfb505" );

            public static readonly Guid Absent_Tamam = Guid.Parse( "a5461863-4853-43c3-8826-551c7304d677" );
            public static readonly Guid Absent_Email = Guid.Parse( "701c4898-07c2-4f8f-9f47-ebb7c6bfb020" );
            public static readonly Guid Absent_SMS = Guid.Parse( "1d9611d8-2bd5-42e8-a35b-7875eee2194c" );
            public static readonly Guid Absent_Delay = Guid.Parse( "ef4f95f8-7855-43c2-aad3-4d8dc0023fe1" );

            public static readonly Guid EarlyLeave_Tamam = Guid.Parse( "540ecaad-4fe8-4ffa-acd6-f9f6b55cf47d" );
            public static readonly Guid EarlyLeave_Email = Guid.Parse( "06f69e4f-d94b-4a33-af16-c46857b2b0c2" );
            public static readonly Guid EarlyLeave_SMS = Guid.Parse( "e2fd2034-2663-4723-a65a-93b356d561f8" );
            public static readonly Guid EarlyLeave_Delay = Guid.Parse( "81f8373c-d4cc-4a8d-a173-0d0340c2fd5e" );

            public static readonly Guid LeaveOnGrace_Tamam = Guid.Parse( "F110A0EA-88E5-45FC-A3AC-07DFE729241B" );
            public static readonly Guid LeaveOnGrace_Email = Guid.Parse( "19EE7BB9-18B3-4D2A-9559-9848810107E8" );
            public static readonly Guid LeaveOnGrace_SMS = Guid.Parse( "7ADCD85C-D785-4FA0-B518-0227BB3C9669" );
            public static readonly Guid LeaveOnGrace_Delay = Guid.Parse( "5C5CA494-D34C-4060-8532-06210E58539F" );

            public static readonly Guid MissedPunches_Tamam = Guid.Parse( "04e8c31f-91db-4e60-9d18-8bdf21a2f870" );
            public static readonly Guid MissedPunches_Email = Guid.Parse( "0659c55c-e5b4-427c-a9d5-a2b6a57c2383" );
            public static readonly Guid MissedPunches_SMS = Guid.Parse( "ad9f983a-08c1-4f9b-a2d5-9abf2c632977" );
            public static readonly Guid MissedPunches_Delay = Guid.Parse( "41a1484c-a2c8-4141-b710-5f9c88d75a24" );

            public static readonly Guid Leaves_Tamam = Guid.Parse( "09669640-458e-4dfe-b09c-f13cbce7c7af" );
            public static readonly Guid Leaves_Email = Guid.Parse( "cd3932ed-adf4-4fc1-ab41-9e54779421d2" );
            public static readonly Guid Leaves_SMS = Guid.Parse( "a24ed7f6-2a7a-4873-9e0b-14368c5de955" );
            public static readonly Guid Leaves_Delay = Guid.Parse( "728cbe1b-9319-451e-ada3-a769ead48cc5" );

            public static readonly Guid Excuses_Tamam = Guid.Parse( "5f4d7934-76b4-4807-b0ea-04ba89a665ed" );
            public static readonly Guid Excuses_Email = Guid.Parse( "35f3077a-ce8b-4847-a034-daa86cbd83fb" );
            public static readonly Guid Excuses_SMS = Guid.Parse( "6cf53d23-f20e-44ea-b079-6dadbc10df47" );
            public static readonly Guid Excuses_Delay = Guid.Parse( "1857ffe3-77ba-49fc-b6dd-bb11af13fd4b" );

            public static readonly Guid AttendanceManualEdit_Tamam = Guid.Parse( "C2B83300-6203-425C-8F82-CBEA41BB13BB" );
            public static readonly Guid AttendanceManualEdit_Email = Guid.Parse( "3CBF883F-E4EC-4D07-BD3D-241B77E9DD42" );
            public static readonly Guid AttendanceManualEdit_SMS = Guid.Parse( "49B0C3E7-B7A1-4F85-A05F-6501615794E4" );
            public static readonly Guid AttendanceManualEdit_Delay = Guid.Parse( "DEF1DE6A-1CBC-4BE2-832B-5876EF4C3FE4" );
        }

        #endregion

        #region ShiftPolicy
        public class ShiftPolicy
        {
            public static readonly Guid LateComeAbsentThreshold = Guid.Parse( "11c93ea1-e7d5-404e-a2f9-57dd13874145" );
            public static readonly Guid LateComeDeductFromOvertime = Guid.Parse( "eea0e0d9-73b3-4d33-8931-5cb559e1fe39" );
            public static readonly Guid EarlyComeAsOvertime = Guid.Parse( "a1819485-7784-4a2a-8702-8962c2956866" );
            public static readonly Guid EarlyComeGrace = Guid.Parse( "9e69f30e-f1b7-4ea0-a2a8-ad4174748243" );
            public static readonly Guid EarlyLeaveDeductFromOvertime = Guid.Parse( "d437b9c0-a005-4558-bb15-c16ca98b9bee" );
            public static readonly Guid LateGrace = Guid.Parse( "d3f71763-90e7-4b10-bc0c-f771cfd940e0" );
            public static readonly Guid ShiftStartMargin = Guid.Parse( "8956d3a6-06b7-4863-a5d4-bfadbe97e4d6" );
            public static readonly Guid ShiftEndMargin = Guid.Parse( "7b6e704e-d186-4826-ae55-8462399bd30d" );
            public static readonly Guid OutOffsetForOvertime = Guid.Parse( "911b364f-f9b2-4c1f-8b56-9e2a0478b30a" );
            public static readonly Guid EarlyLeaveGrace = Guid.Parse( "AE3B2789-304E-48A4-B42A-2CC4A2C54670" );
            public static readonly Guid EarlyLeaveOffsetRelatedToGraceTime = Guid.Parse( "ABC3C345-D856-4757-9D14-316AD2F8208D" );
            public static readonly Guid LateComeOffsetRelatedToGraceTime = Guid.Parse( "0B5EECE6-34FB-4CBC-80EA-D3D72CFC67C5" );
            public static readonly Guid SubtractBreaksDuration = Guid.Parse("49F3B46D-6892-493C-B67A-6A3505B16DA1");
        }
        #endregion

        #region IslamicPolicy

        public class IslamicPolicy
        {
            public static readonly Guid RamadanShift = Guid.Parse( "5e33a847-1c9a-43b3-b753-9d8a8b4126c6" );
        }
        
        #endregion
    }
}