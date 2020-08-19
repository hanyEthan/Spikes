using System;

namespace ADS.Tamam.Common.Data.Model.Enums
{
    public class AttendanceCodes
    {
        public class ScheduleEventStatus
        {
            public static readonly Guid CurrentlyIn = Guid.Parse ( "beeb39bc-e900-4e34-afac-d0dfb4fd92a5" );
            public static readonly Guid MissedOutPunch = Guid.Parse ( "e6a1f0d6-c43a-42a9-8f7b-78dee61f2cf7" );
            public static readonly Guid MissedInPunch = Guid.Parse ( "13b27644-249f-4747-810b-4e8e40b39c80" );
            public static readonly Guid Attended = Guid.Parse ( "e7f63043-3592-4fcf-949c-66aadb99ef99" );
            public static readonly Guid Absent = Guid.Parse ( "b1f8716b-f243-4c88-9493-d1141251e2b8" );
        }

        public class AttendanceEventStatus
        {
            public static readonly Guid CameEarly = Guid.Parse ( "79ee37f1-cac4-4721-bb58-0e5698a78159" );
            public static readonly Guid CameOnTime = Guid.Parse ( "73a269eb-c5fc-4003-a1b9-799f021e6caa" );
            public static readonly Guid CameOnGrace = Guid.Parse ( "c7df9a4d-d481-4d33-aa5d-ff8a48931ecd" );
            public static readonly Guid CameLate = Guid.Parse ( "19ba3331-8c9e-438e-b1c8-adf5cbbdc220" );
            public static readonly Guid LateAbsent = Guid.Parse ( "4bfda1c0-e940-42ad-9491-bb3b3fa7774c" );

            public static readonly Guid LeftOnGrace = Guid.Parse ( "f2b10cc8-e01d-4b34-bf38-c5020d0d0642" );
            public static readonly Guid LeftEarly = Guid.Parse ( "7a8e70b4-9264-4b41-af8a-3ee50a10760d" );
            public static readonly Guid LeftLate = Guid.Parse ( "b89a89a5-4d5b-46b9-bfdf-82a72b3e431f" );
            public static readonly Guid LeftOnTime = Guid.Parse ( "fff786ce-ec56-490f-9bd3-c6e636ed3d08" );

            public static readonly Guid ExitDuringWork = Guid.Parse ( "f916dfce-642b-422e-98bd-fa6a4edf9c91" );
        }
        public class PayCodeStatus
        {
            public static readonly Guid NormalDay = Guid.Parse ( "6c6cad82-4830-45fe-935c-c8ecf32a2bd9" );
            public static readonly Guid WorkOnHoliday = Guid.Parse ( "d25e2660-b7e4-4a6e-8f96-17219a6436a4" );
            public static readonly Guid WorkOnLeave = Guid.Parse ( "2d401bb9-ff95-4feb-9183-cec27833d168" );
            public static readonly Guid WorkOnWeekend = Guid.Parse ( "7746a5d4-1b2c-4584-a5a2-6200e88aeff2" );
            public static readonly Guid WorkOnUnScheduled = Guid.Parse ( "2bea318b-9e0a-47c3-b6b7-8d6a7e82dea9" );

            public static readonly Guid OnWeekend = Guid.Parse ( "cdfa82e8-92a3-41f6-8b98-acc0aa855e16" );
            public static readonly Guid OnLeave = Guid.Parse ( "8d841ade-48b8-4d21-afc7-3d99fe091bb6" );
            public static readonly Guid OnAway = Guid.Parse( "c207e89e-a1e2-4f4f-92ad-f6b4b5f18f60" );
            public static readonly Guid OnHoliday = Guid.Parse ( "e551a144-7d0c-48f9-8687-5c8388eb03aa" );
        }
        public class HourStatus
        {
            public static readonly Guid WorkLess = Guid.Parse("F71C5173-7B89-4B79-8C64-2EAF74EC902C");
            public static readonly Guid WorkMore = Guid.Parse("FCB17309-F555-464A-91EF-A26E467E97E5");
        }
        
    }
}
