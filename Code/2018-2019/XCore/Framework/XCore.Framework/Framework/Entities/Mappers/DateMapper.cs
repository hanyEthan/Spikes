using System;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Utilities;

namespace XCore.Framework.Framework.Entities.Mappers
{
    public class DateMapper : IModelMapper<DateTime?, string>,
                              IModelMapper<string, DateTime?>
    {
        #region props.

        public static DateMapper Instance { get; } = new DateMapper();

        #endregion
        #region cst.

        static DateMapper()
        {
        }
        private DateMapper()
        {
        }

        #endregion
        #region IModelMapper

        public string Map(DateTime? from, object metadata = null)
        {
            if (from == null) return null;
            return XDate.Convert(from.Value, (metadata as string) ?? XCoreConstants.Formats.DateFormat, XDate.CalendarType.Gregorian);
        }
        public DateTime? Map(string from, object metadata = null)
        {
            if (from == null) return null;
            return XDate.Parse(from, (metadata as string) ?? XCoreConstants.Formats.DateFormat, XDate.CalendarType.Gregorian);
        }

        #endregion
    }
}
