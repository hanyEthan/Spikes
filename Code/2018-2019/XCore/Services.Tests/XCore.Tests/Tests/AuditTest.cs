using System;
using XCore.Framework.Utilities;
using XCore.Tests.Mocks;

namespace XCore.Tests.Tests
{
    public static class AuditTest
    {
        #region bootstrap

        public static void Start()
        {
            Test02();
        }

        #endregion
        #region privates.

        private static void Test02()
        {
            var xx = XSerialize.JSON.Serialize(AuditMocks.Audit_01);



            Console.ReadLine();
        }

        #endregion
    }
}
