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
    public static class ExcuseTypesRepository
    {
        #region fields

        private static List<DetailCode> excuseTypes;

        #endregion

        #region props

        private static List<DetailCode> ExcuseTypes
        {
            get
            {
                if (excuseTypes == null) Reload();
                return excuseTypes;
            }
            set
            {
                excuseTypes = value;
            }
        }

        #endregion

        #region helpers

        private static List<DetailCode> GetExcuseTypes()
        {
            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.ExcuseType);
        }

        #endregion

        #region publics

        public static int Translate(string code)
        {
            var excuseType = ExcuseTypes.SingleOrDefault(lt => lt.Code == code);
            return excuseType == null ? default(int) : excuseType.Id;
        }

        public static void Reload()
        {
            ExcuseTypes = GetExcuseTypes();
        }

        #endregion
    }
}
