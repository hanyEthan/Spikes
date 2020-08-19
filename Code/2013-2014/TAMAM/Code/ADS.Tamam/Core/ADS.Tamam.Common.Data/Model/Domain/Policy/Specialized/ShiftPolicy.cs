using ADS.Tamam.Common.Data.Model.Enums;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class ShiftPolicy : AbstractSpecialPolicy
    {
        public int EarlyComeGrace { get; private set; }
        public bool EarlyComeAsOvertime { get; private set; }
        public bool EarlyLeaveDeductFromOvertime { get; private set; }
        public int LateComeGrace { get; private set; }
        public int LateComeAbsentThreshold { get; private set; }
        public bool LateComeDeductFromOvertime { get; private set; }
        public int ShiftStartMargin { get; private set; }
        public int ShiftEndMargin { get; private set; }
        public int OutOffsetForOvertime { get; private set; }
        public int EarlyLeaveGrace { get; private set; }
        public bool EarlyLeaveOffsetRelatedToGraceTime { get; private set; }
        public bool LateComeOffsetRelatedToGraceTime { get; private set; }
        public bool SubtractBreaksDuration { get; private set; }

        public ShiftPolicy(Policy policy) : base(policy)
        {
            EarlyComeGrace = GetInt(PolicyFields.ShiftPolicy.EarlyComeGrace) ?? 1440;
            EarlyComeAsOvertime = GetBool(PolicyFields.ShiftPolicy.EarlyComeAsOvertime) ?? false;
            EarlyLeaveDeductFromOvertime = GetBool(PolicyFields.ShiftPolicy.EarlyLeaveDeductFromOvertime) ?? false;
            LateComeGrace = GetInt(PolicyFields.ShiftPolicy.LateGrace) ?? 0;
            LateComeAbsentThreshold = GetInt(PolicyFields.ShiftPolicy.LateComeAbsentThreshold) ?? 0;
            LateComeDeductFromOvertime = GetBool(PolicyFields.ShiftPolicy.LateComeDeductFromOvertime) ?? false;
            ShiftStartMargin = GetInt( PolicyFields.ShiftPolicy.ShiftStartMargin ) ?? 1440;
            ShiftEndMargin = GetInt( PolicyFields.ShiftPolicy.ShiftEndMargin ) ?? 1440;
            OutOffsetForOvertime = GetInt(PolicyFields.ShiftPolicy.OutOffsetForOvertime) ?? 0;
            EarlyLeaveGrace = GetInt(PolicyFields.ShiftPolicy.EarlyLeaveGrace) ?? 0;
            EarlyLeaveOffsetRelatedToGraceTime = GetBool(PolicyFields.ShiftPolicy.EarlyLeaveOffsetRelatedToGraceTime) ?? false;
            LateComeOffsetRelatedToGraceTime = GetBool(PolicyFields.ShiftPolicy.LateComeOffsetRelatedToGraceTime) ?? false;
            SubtractBreaksDuration = GetBool(PolicyFields.ShiftPolicy.SubtractBreaksDuration) ?? true;
        }
    }
}
