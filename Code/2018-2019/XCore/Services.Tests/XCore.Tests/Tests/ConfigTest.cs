//using System;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;
//using XCore.Framework.Infrastructure.Context.Execution.Support;
//using XCore.Framework.Utilities;
//using XCore.Services.Config.Core.Unity;
//using XCore.Tests.Mocks;

//namespace XCore.Tests.Tests
//{
//    public static class ConfigTest
//    {
//        #region bootstrap

//        public static void Start()
//        {
//            Test02();
//        }

//        #endregion
//        #region privates.

//        private static void Test02()
//        {
//            var R01 = ConfigUnity.Configs.Create(ConfigMocks.App_01, SystemRequestContext.Instance);
//            //var R02 = ConfigUnity.Configs.Create(ConfigMocks.Module_01, SystemRequestContext.Instance);
//           // var R011 = ConfigUnity.Configs.Create(ConfigMocks.Config_01, SystemRequestContext.Instance);


//            //var R04 = ConfigUnity.Configs.Edit(ConfigMocks.App_01, SystemRequestContext.Instance);
//            // var R05 = ConfigUnity.Configs.Edit(ConfigMocks.Module_01, SystemRequestContext.Instance);
//            //   var R041 = ConfigUnity.Configs.Edit(ConfigMocks.Config_01, SystemRequestContext.Instance);


//            var R07 = ConfigUnity.Configs.DeleteApp(10, SystemRequestContext.Instance);
//            // var R02 = ConfigUnity.Configs.Delete(ConfigMocks.Module_01, SystemRequestContext.Instance);
//            // var R071 = ConfigUnity.Configs.Delete(ConfigMocks.Config_01, SystemRequestContext.Instance);Get


//            //  var R09 = ConfigUnity.Configs.Get(ConfigMocks.cr, SystemRequestContext.Instance);
//            //var R061 = ConfigUnity.Configs.Get(ConfigMocks.Criteria, SystemRequestContext.Instance);
//            //var R061 = ConfigUnity.Configs.Get(ConfigMocks.mo, SystemRequestContext.Instance);


//            //var R09 = ConfigUnity.Configs.ActivateApp(7, SystemRequestContext.Instance);
//            //var R0101 = ConfigUnity.Configs.ActivateModule(12, SystemRequestContext.Instance);
//            //var R012 = ConfigUnity.Configs.ActivateConfig(3, SystemRequestContext.Instance);


//            //var R09 = ConfigUnity.Configs.DeactivateApp(9, SystemRequestContext.Instance);
//            //var R0101 = ConfigUnity.Configs.DeactivateModule(9, SystemRequestContext.Instance);
//            //var R012 = ConfigUnity.Configs.DeactivateConfig(9, SystemRequestContext.Instance);

//           // var R0101 = ConfigUnity.Configs.Set(ConfigMocks.ky,"Test", SystemRequestContext.Instance);


//            #region ...

//            //var R03 = ConfigUnity.Configs.GetModules( SystemRequestContext.Instance);
//            //var R031 = ConfigUnity.Configs.GetModuleByID(ConfigMocks.Module_01, SystemRequestContext.Instance);


//            // var R04 = ConfigUnity.Configs.Edit(ConfigMocks.App_01, SystemRequestContext.Instance);
//            // var R06 = ConfigUnity.Configs.GetApps( SystemRequestContext.Instance);
//            //var R061 = ConfigUnity.Configs.GetAppByID(ConfigMocks.App_01,SystemRequestContext.Instance);
//            // var R07 = ConfigUnity.Configs.RemoveApp(ConfigMocks.App_01, SystemRequestContext.Instance);

//            //  var R041 = ConfigUnity.Configs.Edit(ConfigMocks.Config_01, SystemRequestContext.Instance);
//            //var R061 = ConfigUnity.Configs.GetConfigs(ConfigMocks.Criteria, SystemRequestContext.Instance);
//            //  var R0611 = ConfigUnity.Configs.GetConfigsById(ConfigMocks.Config_01,SystemRequestContext.Instance);
//            // var R071 = ConfigUnity.Configs.RemoveConfig(ConfigMocks.Config_01, SystemRequestContext.Instance);


//            //Console.WriteLine("State : " + R061?.State.ToString());
//            //Console.WriteLine("Error : "  + XSerialize.JSON.Serialize(R061?.DetailedMessages));
//            //Console.WriteLine("The Resule :  " + XSerialize.JSON.Serialize(R061?.Result));

//            #endregion
//        }

//        #endregion
//    }
//}
