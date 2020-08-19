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
//    public static class GenderRepository
//    {
//        #region fields

//        private static List<DetailCode> genders;

//        #endregion

//        #region props

//        private static List<DetailCode> Genders
//        {
//            get
//            {
//                if (genders == null) Reload();
//                return genders;
//            }
//            set
//            {
//                genders = value;
//            }
//        }

//        #endregion

//        #region helpers

//        private static List<DetailCode> GetGenders()
//        {
//            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.Gender);
//        }

//        #endregion

//        #region publics

//        public static int Translate(string code)
//        {
//            var gender = Genders.SingleOrDefault(g => g.Code == code);
//            return gender == null ? default(int) : gender.Id;
//        }

//        public static void Reload()
//        {
//            Genders = GetGenders();
//        }

//        #endregion
//    }
//}
