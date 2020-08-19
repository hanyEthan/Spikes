using System;
using System.Collections.Generic;
using System.Text;
using XCore.Framework.Utilities;

namespace XCore.Tests.Tests
{
    public static class LogTest
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
            XLogger.Verbose("hello world");
            XLogger.Trace("hello world");
            XLogger.Info("hello world");
            XLogger.Warning("hello world");
            XLogger.Error("hello world");
            XLogger.Fatal("hello world");
        }

        #endregion
    }
}
