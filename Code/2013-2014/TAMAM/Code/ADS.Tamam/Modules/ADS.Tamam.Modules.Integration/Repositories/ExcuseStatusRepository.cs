using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Repositories
{
    public static class ExcuseStatusRepository
    {
        #region fields

        private static List<DetailCode> excuseStatuses;

        #endregion

        #region props

        private static List<DetailCode> ExcuseStatuses
        {
            get
            {
                if (excuseStatuses == null) Reload();
                return excuseStatuses;
            }
            set
            {
                excuseStatuses = value;
            }
        }

        #endregion

        #region helpers

        private static List<DetailCode> GetExcuseStatuses()
        {
            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.ExcuseStatus);
        }

        #endregion

        #region publics

        public static int Translate(string code)
        {
            var excuseStatus = ExcuseStatuses.SingleOrDefault(ls => ls.Code == code);
            return excuseStatus == null ? default(int) : excuseStatus.Id;
        }

        public static void Reload()
        {
            ExcuseStatuses = GetExcuseStatuses();
        }

        #endregion
    }
}
