using System.Collections.Generic;

namespace XCore.Services.Attachments.API.Models
{
    public class SearchResultsDTO<T> where T : class
    {
        #region props.

        public List<T> Results { get; set; }
      

        #endregion
    }
}
