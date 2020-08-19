using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Repositories
{
    public class DetailCodeRepository
    {
        public DetailCodeRepository(string masterKey)
        {
            this.MasterKey = masterKey;
            Reload();
        }

        #region fields

        private List<DetailCode> items;

        private MasterCode masterCode;

        #endregion

        #region props

        private List<DetailCode> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }

        private string MasterKey { get; set; }

        public MasterCode MasterCode
        {
            get
            {
                return masterCode ?? (masterCode = Broker.MasterCodeHandler.GetMasterCode(MasterKey));
            }
        }

        #endregion

        #region Helpers

        private List<DetailCode> GetItems()
        {
            return Broker.DetailCodeHandler.GetDetailCodesByMasterCode( MasterKey, true );
        }

        #endregion

        #region publics

        public DetailCode GetDetailCode(string code)
        {
            var detailCode = Items.SingleOrDefault(i => i.Code == code);
            return detailCode;
        }

        public int Translate(string code)
        {
            var item = Items.SingleOrDefault(i => i.Code == code);
            return item == null ? default(int) : item.Id;
        }

        public void Reload()
        {
            Items = GetItems();
        }

        #endregion
    }
}
