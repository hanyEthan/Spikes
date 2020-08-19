using System.Text.RegularExpressions;

namespace XCore.Framework.Infrastructure.Entities.Validation.Handlers
{
    public class CommonValidators
    {
        #region Publics.

        public static bool IsValidEmail( string Email )
        {
            if ( string.IsNullOrEmpty( Email ) ) return false;
            return Regex.IsMatch( Email , @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase );
        }
        public static bool IsValidCoordinates(string Value)
        {
            if (string.IsNullOrEmpty(Value)) return true;
            return Regex.IsMatch(Value, @"^(\-?\d+(\.\d+)?).\s*(\-?\d+(\.\d+)?)$", RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
