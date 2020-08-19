﻿namespace XCore.Services.Configurations.Models
{
    public class AppSearchCriteriaDTO
    {
        #region criteria.        

        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; } = true;
        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int OrderByCultureMode { get; set; }

        #endregion
    }
}
