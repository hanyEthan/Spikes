using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NUlid;

namespace Mcs.Invoicing.Core.Framework.Utilities
{
    public static class StringUtilities
    {
        #region props.

        private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        #endregion
        #region statics.

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static string NewGuidReversed()
        {
            return Reverse(NewGuid());
        }

        public static string NewUlid()
        {
            return Ulid.NewUlid().ToString();
        }
        public static string NewUlidReversed()
        {
            return NewUlid().Reverse();
        }

        public static string Reverse(this string data)
        {
            if (data == null) throw new ArgumentNullException("data");

            #region [method 01]

            char[] array = data.ToCharArray();
            Array.Reverse(array);
            return new string(array);

            #endregion
            #region [method 02]

            //char[] array = new char[data.Length];
            //int forward = 0;
            //for (int i = data.Length - 1; i >= 0; i--)
            //{
            //    array[forward++] = data[i];
            //}
            //return new string(array);

            #endregion
        }
        public static string Random(int length)
        {
            return Random(length, StringUtilities.chars);
        }
        public static string Random(int length, char[] allowedChars)
        {
            if (length <= 0) throw new ArgumentNullException("length");
            if (allowedChars == null || allowedChars.Length == 0) throw new ArgumentNullException("allowedChars");

            byte[] data = new byte[4 * length];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % allowedChars.Length;
                result.Append(allowedChars[idx]);
            }

            return result.ToString();
        }

        public static Stream ToStream(this string data)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(data ?? ""));
        }
        public static bool MatchPattern(this string data, string pattern)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pattern == null) throw new ArgumentNullException("pattern");

            Regex reg = new Regex(pattern);
            Match match = reg.Match(data.Trim());

            return match.Success && match.Value == data;
        }

        #endregion
    }
}
