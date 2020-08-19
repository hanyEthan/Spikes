using System;

namespace XCore.Framework.Utilities
{
    public static class XMath
    {
        public static int Round( decimal value )
        {
            var round = Math.Round( value , MidpointRounding.AwayFromZero );
            return ( int ) round;
        }
        public static int Round( double value )
        {
            var round = Math.Round( value , MidpointRounding.AwayFromZero );
            return ( int ) round;
        }
        public static int Round( float value )
        {
            var round = Math.Round( value , MidpointRounding.AwayFromZero );
            return ( int ) round;
        }

        public static void DeductBalance( ref int value , ref int deductee )
        {
            if ( deductee > value )
            {
                Deduct( ref deductee , ref value );
            }
            else
            {
                Deduct( ref value , ref deductee );
            }
        }
        public static void Deduct( ref int source , ref int amount )
        {
            source -= amount;
            amount -= amount;
        }
    }
}
