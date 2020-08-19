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
//    public static class LeaveTypesRepository
//    {
//        #region fields

//        private static List<DetailCode> leaveTypes;

//        #endregion

//        #region props

//        private static List<DetailCode> LeaveTypes
//        {
//            get
//            {
//                if (leaveTypes == null) Reload();
//                return leaveTypes;
//            }
//            set
//            {
//                leaveTypes = value;
//            }
//        }

//        #endregion

//        #region helpers

//        private static List<DetailCode> GetLeaveTypes()
//        {
//            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.LeaveType);
//        }

//        #endregion

//        #region publics

//        public static int Translate(string code)
//        {
//            var leaveType = LeaveTypes.SingleOrDefault(lt => lt.Code == code);
//            return leaveType == null ? default(int) : leaveType.Id;
//        }

//        public static void Reload()
//        {
//            LeaveTypes = GetLeaveTypes();
//        }

//        #endregion
//    }
//}
