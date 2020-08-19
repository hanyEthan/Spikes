using System;
using ADS.Tamam.Common.Data.Model.Enums;
using PolicyModel = ADS.Tamam.Common.Data.Model.Domain.Policy.Policy;

namespace ADS.Tamam.Common.Data.Model.Domain.Policy.Specialized
{
    public class AccrualPolicy : AbstractSpecialPolicy
    {
        #region props ...
        public AnnualAccrualType? AnnualAccrualType
        {
            get;
            private set;
        }

        public int YearStart
        {

             get;
            private set;
        }
        #endregion
        #region cst ...

        public AccrualPolicy( PolicyModel policy ) : base( policy )
        {
            AnnualAccrualType = ( AnnualAccrualType? ) GetInt ( PolicyFields.AccrualPolicy.AnnualAccrualType ) ?? ( AnnualAccrualType? ) null;
            int? value = GetInt ( PolicyFields.AccrualPolicy.YearStart );
            YearStart = value.HasValue ? value.Value - 491 : -1;//491 is here to compensate for the difference between system code ids and month numbers.
        }

        #endregion
        #region Helpers

        public DateTime GetAccrualPolicyStartDate( DateTime hireDate )
        {
            DateTime effectiveYear = DateTime.Now;

            if ( AnnualAccrualType.Value == Enums.AnnualAccrualType.Yearly )
            {
                var month = YearStart > 0 ? YearStart : 1;

                if ( month > DateTime.Today.Month )
                {
                    effectiveYear = new DateTime( DateTime.Today.Year - 1 , month , 1 );
                }
                else if ( month < DateTime.Today.Month )
                {
                    effectiveYear = new DateTime( DateTime.Today.Year , month , 1 );
                }
                else
                {
                    effectiveYear = new DateTime( DateTime.Today.Year , month , 1 );
                }
            }
            else
            {
                if ( hireDate.Month > DateTime.Today.Month )
                {
                    effectiveYear = new DateTime( DateTime.Today.Year - 1 , hireDate.Month , hireDate.Day );
                }
                else if ( hireDate.Month < DateTime.Today.Month )
                {
                    effectiveYear = new DateTime( DateTime.Today.Year , hireDate.Month , hireDate.Day );
                }
                else
                {
                    if ( hireDate.Day <= DateTime.Today.Day )
                    {
                        effectiveYear = new DateTime( DateTime.Today.Year , hireDate.Month , hireDate.Day );
                    }
                    else
                    {
                        effectiveYear = new DateTime( DateTime.Today.Year - 1 , hireDate.Month , hireDate.Day );
                    }
                }
            }

            return effectiveYear;
        }
        
        #endregion
    }
}
