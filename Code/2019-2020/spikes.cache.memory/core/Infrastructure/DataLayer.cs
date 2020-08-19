using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Models.Domain;

namespace core.Infrastructure
{
    public class DataLayer
    {
        #region props.

        private List<MyModel> _myModels { get; set; } = new List<MyModel>();

        #endregion
        #region cst.

        public DataLayer()
        {

        }

        #endregion
        #region data.

        public async Task Create(MyModel instance)
        {
            this._myModels.Add(instance);
        }
        public async Task Update(MyModel instance)
        {
            if (instance == null || instance.Id == 0) throw new Exception("invalid input.");
            var existing = this._myModels.Where(x => x.Id == instance.Id).FirstOrDefault();
            if(existing == null) throw new Exception("not found.");

            this._myModels.Remove(existing);
            this._myModels.Add(instance);
        }
        public async Task Delete(MyModel instance)
        {
            if (instance == null || instance.Id == 0) throw new Exception("invalid input.");
            var existing = this._myModels.Where(x => x.Id == instance.Id).FirstOrDefault();
            if (existing == null) throw new Exception("not found.");

            this._myModels.Remove(existing);
        }
        public async Task<MyModel> Get(int id)
        {
            if (id == 0) throw new Exception("invalid input.");
            var existing = this._myModels.Where(x => x.Id == id).FirstOrDefault();

            return existing;
        }
        public async Task<List<MyModel>> List()
        {
            return this._myModels;
        }

        #endregion
    }
}
