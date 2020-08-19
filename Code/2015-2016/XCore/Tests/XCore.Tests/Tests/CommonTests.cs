namespace XCore.Tests.Tests
{
    public static class CommonTests
    {
        #region ...

        public static void Start()
        {
            Test();
        }

        #endregion

        private static void Test()
        {
            var x = SampleUnity.SampleService1.SampleAction( 2 , 3 );
            var y = SampleUnity.SampleService2.SampleAction( 2 , 3 );
            var z = SampleUnity.SampleService3.SampleAction( 2 , 3 );
        }
    }
}
