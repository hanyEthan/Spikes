using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace ADS.Common.Utilities
{
    public static class XCrypto
    {
        #region Properties

        private const string SECRET_KEY = ".??H????";
        private const string ENCRYPTION_KEY = "era";
        private readonly static byte[] SALT = Encoding.ASCII.GetBytes( ENCRYPTION_KEY.Length.ToString() );

        #endregion

        #region MD5

        public static string EncryptToMd5( byte[] value )
        {
            if ( value == null || value.Length == 0 ) return null;

            try
            {
                var cryptoProvider = new MD5CryptoServiceProvider();
                byte[] hashValue = cryptoProvider.ComputeHash( value );

                return ( BitConverter.ToString( hashValue ) ).Replace( "-" , String.Empty );
            }
            catch ( Exception )
            {
                return null;
            }
        }
        public static string EncryptToMd5( string value )
        {
            if ( string.IsNullOrEmpty( value ) ) return null;
            return EncryptToMd5( new ASCIIEncoding().GetBytes( value ) );
        }

        public static string EncryptToMd5Hash( string value )
        {
            var md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash( Encoding.ASCII.GetBytes( value ) );

            //get hash result after compute it
            byte[] result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach ( byte t in result )
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append( t.ToString( "X2" ) );
            }
            return strBuilder.ToString();
        }

        #endregion
        #region DES

        public static Stream EncryptToDES( Stream value )
        {
            try
            {
                var DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes( SECRET_KEY );
                DES.IV = ASCIIEncoding.ASCII.GetBytes( SECRET_KEY );
                var DESEncrypt = DES.CreateEncryptor();

                return new CryptoStream( value , DESEncrypt , CryptoStreamMode.Write );
            }
            catch ( Exception x )
            {
                XLogger.Error( "XUtilities.XCrypto.EncryptToDES ... Exception: " + x );
                return null;
            }
        }
        public static Stream DecryptFromDES( Stream value )
        {
            try
            {
                var DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes( SECRET_KEY );
                DES.IV = ASCIIEncoding.ASCII.GetBytes( SECRET_KEY );
                var DESDecrypt = DES.CreateDecryptor();

                return new CryptoStream( value , DESDecrypt , CryptoStreamMode.Read );
            }
            catch ( Exception x )
            {
                XLogger.Error( "XUtilities.XCrypto.DecryptFromDES ... Exception: " + x );
                return null;
            }
        }

        #endregion
        #region AES

        public static string EncryptToAES( string value )
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            var clearBytes = Encoding.Unicode.GetBytes( value );
            using ( var encryptor = Aes.Create() )
            {
                var pdb = new Rfc2898DeriveBytes( encryptionKey , new byte[] { 0x49 , 0x76 , 0x61 , 0x6e , 0x20 , 0x4d , 0x65 , 0x64 , 0x76 , 0x65 , 0x64 , 0x65 , 0x76 } );
                if ( encryptor == null ) return value;

                encryptor.Key = pdb.GetBytes( 32 );
                encryptor.IV = pdb.GetBytes( 16 );
                using ( var ms = new MemoryStream() )
                {
                    using ( var cs = new CryptoStream( ms , encryptor.CreateEncryptor() , CryptoStreamMode.Write ) )
                    {
                        cs.Write( clearBytes , 0 , clearBytes.Length );
                        cs.Close();
                    }
                    value = Convert.ToBase64String( ms.ToArray() );
                }
            }
            return value;
        }
        public static string DecryptFromAES( string value )
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            value = value.Replace( " " , "+" );
            byte[] cipherBytes = Convert.FromBase64String( value );
            using ( var encryptor = Aes.Create() )
            {
                var pdb = new Rfc2898DeriveBytes( encryptionKey , new byte[] { 0x49 , 0x76 , 0x61 , 0x6e , 0x20 , 0x4d , 0x65 , 0x64 , 0x76 , 0x65 , 0x64 , 0x65 , 0x76 } );
                if ( encryptor == null ) return value;

                encryptor.Key = pdb.GetBytes( 32 );
                encryptor.IV = pdb.GetBytes( 16 );
                using ( var ms = new MemoryStream() )
                {
                    using ( var cs = new CryptoStream( ms , encryptor.CreateDecryptor() , CryptoStreamMode.Write ) )
                    {
                        cs.Write( cipherBytes , 0 , cipherBytes.Length );
                        cs.Close();
                    }
                    value = Encoding.Unicode.GetString( ms.ToArray() );
                }
            }
            return value;
        }

        #endregion
        #region Rijndael

        public static string EncryptToRijndael( string value )
        {
            var cipher = new RijndaelManaged();
            var plainText = Encoding.Unicode.GetBytes( value );
            var secretKey = new PasswordDeriveBytes( ENCRYPTION_KEY , SALT );

            using ( var encryptor = cipher.CreateEncryptor( secretKey.GetBytes( 32 ) , secretKey.GetBytes( 16 ) ) )
            {
                using ( var memoryStream = new MemoryStream() )
                {
                    using ( var cryptoStream = new CryptoStream( memoryStream , encryptor , CryptoStreamMode.Write ) )
                    {
                        cryptoStream.Write( plainText , 0 , plainText.Length );
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String( memoryStream.ToArray() );
                    }
                }
            }
        }
        public static string DecryptFromRijndael( string value )
        {
            var cipher = new RijndaelManaged();
            var encryptedData = Convert.FromBase64String( value );
            var secretKey = new PasswordDeriveBytes( ENCRYPTION_KEY , SALT );

            using ( var decryptor = cipher.CreateDecryptor( secretKey.GetBytes( 32 ) , secretKey.GetBytes( 16 ) ) )
            {
                using ( var memoryStream = new MemoryStream( encryptedData ) )
                {
                    using ( var cryptoStream = new CryptoStream( memoryStream , decryptor , CryptoStreamMode.Read ) )
                    {
                        var plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read( plainText , 0 , plainText.Length );
                        return Encoding.Unicode.GetString( plainText , 0 , decryptedCount );
                    }
                }
            }
        }

        #endregion
    }
}
