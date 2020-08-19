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
//    public static class JopTitleRepository : DetailCodeRepository
//    {
//        #region fields

//        // private static List<DetailCode> jopTitles;

//        #endregion
        
//        #region props

//        //private static List<DetailCode> JopTitles
//        //{
//        //    get
//        //    {
//        //        if (jopTitles == null) Reload();
//        //        return jopTitles;
//        //    }
//        //    set
//        //    {
//        //        jopTitles = value;
//        //    }
//        //} 

//        #endregion
        
//        #region Helpers

//        //private static List<DetailCode> GetJopTitles()
//        //{
//        //    return Broker.DetailCodeHandler.GetDetailCodesByMasterCode(TamamConstants.MasterCodes.JobTitle);
//        //}

//        #endregion
        
//        #region publics

//        //public static int Translate(string code)
//        //{
//        //    var jopTitle = JopTitles.SingleOrDefault(j => j.Code == code);
//        //    return jopTitle == null ? default(int) : jopTitle.Id;
//        //}

//        //public static void Reload()
//        //{
//        //    JopTitles = GetJopTitles();
//        //}

//        #endregion

//        #region Overrides

//        protected static override string MasterKey
//        {
//            get
//            {
//                return TamamConstants.MasterCodes.JobTitle;
//            }
//        }

//        #endregion


//    }
//}
