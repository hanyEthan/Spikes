//using ADS.Common.Handlers;
//using ADS.Common.Models.Domain;
//using ADS.Tamam.Common.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ADS.Tamam.Modules.Integration.Repositories
//{
//    public static class LeaveStatusRepository
//    {
//        #region fields

//        private static List<DetailCode> leaveStatuses;

//        #endregion

//        #region props

//        private static List<DetailCode> LeaveStatuses
//        {
//            get
//            {
//                if (leaveStatuses == null) Reload();
//                return leaveStatuses;
//            }
//            set
//            {
//                leaveStatuses = value;
//            }
//        }

//        #endregion

//        #region helpers

//        private static List<DetailCode> GetLeaveStatuses()
//        {
//            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.LeaveStatus);
//        }

//        #endregion

//        #region publics

//        public static int Translate(string code)
//        {
//            var leaveStatus = LeaveStatuses.SingleOrDefault(ls => ls.Code == code);
//            return leaveStatus == null ? default(int) : leaveStatus.Id;
//        }

//        public static void Reload()
//        {
//            LeaveStatuses = GetLeaveStatuses();
//        }

//        #endregion
//    }
//}
