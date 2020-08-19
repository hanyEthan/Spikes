using XCore.Framework.Framework.Unity.Contracts;
using XCore.Framework.Framework.Unity.Handlers;

namespace XCore.Tests.Tests
{
    #region SampleUnity

    public static class SampleUnity
    {
        #region props.

        public static bool Initialized { get { return unity?.Initialized ?? false; } }
        private static XUnity unity { get; set; }

        #endregion
        #region services.

        #region SampleService1
        private static ISampleService _SampleService1;
        public static ISampleService SampleService1
        {
            get
            {
                return _SampleService1 = !Initialized ? null : _SampleService1 ?? unity.Resolve<ISampleService>( "System" , "K1" );
            }
        }
        #endregion
        #region SampleService2
        private static ISampleService _SampleService2;
        public static ISampleService SampleService2
        {
            get
            {
                return _SampleService2 = !Initialized ? null : _SampleService2 ?? unity.Resolve<ISampleService>( "System" , "K2" );
            }
        }
        #endregion
        #region SampleService3
        private static ISampleService _SampleService3;
        public static ISampleService SampleService3
        {
            get
            {
                return _SampleService3 = !Initialized ? null : _SampleService3 ?? unity.Resolve<ISampleService>( "System" , "K3" );
            }
        }
        #endregion

        #endregion

        #region cst.

        static SampleUnity()
        {
            Initialize();
        }

        #endregion
        #region helpers.

        private static bool Initialize()
        {
            unity = new XUnity();
            return Initialized;
        }

        #endregion
    }

    #endregion
    #region sample contracts

    public interface ISampleService : IUnityService
    {
        int SampleAction(int x , int y);
    }

    #endregion
    #region sample service

    class SampleService1 : ISampleService
    {
        public string domain { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public SampleService1( string domain , string username , string password )
        {
            this.domain = domain;
            this.username = username;
            this.password = password;
        }

        public int SampleAction( int x , int y )
        {
            return ( x + y );
        }
    }
    class SampleService2 : ISampleService
    {
        public string domain { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public SampleService2( string domain , string username , string password )
        {
            this.domain = domain;
            this.username = username;
            this.password = password;
        }

        public int SampleAction( int x , int y )
        {
            return ( x + y );
        }
    }

    #endregion
}
