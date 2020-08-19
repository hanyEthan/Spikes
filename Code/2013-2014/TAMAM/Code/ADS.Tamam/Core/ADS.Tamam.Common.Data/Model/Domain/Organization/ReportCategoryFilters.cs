using System;

namespace ADS.Tamam.Common.Data.Model.Domain.Organization
{
    public class ReportCategoryFilters
    {
        public Guid? CategoryId { get; set; }
        public bool IsQuick { get; set; }
        public bool DisableAuthorizationFilter { get; set; }
        public ReportCategoryFilters(Guid? categoryId, bool isQuick, bool disableAuthorizationFilter)
        {
            CategoryId = categoryId;
            IsQuick = isQuick;
            DisableAuthorizationFilter = disableAuthorizationFilter;
        }

        public override string ToString()
        {
            return string.Format ( "{0}_{1}" , CategoryId.ToString () , IsQuick.ToString () );
        }
    }
}
