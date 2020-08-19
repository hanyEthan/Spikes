using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;

namespace ADS.Common.Contracts.SystemCodes
{
    public interface IMasterCodeDataHandler : IBaseHandler
    {
        List<MasterCode> GetMasterCodes();
        MasterCode GetMasterCode(int id);
        MasterCode GetMasterCode(int id, bool underlyingCollections);
        MasterCode GetMasterCode(string code);
        MasterCode GetMasterCode(string code, bool underlyingCollections);
        MasterCode CreateMasterCode(MasterCode masterCode);
        MasterCode UpdateMasterCode(MasterCode masterCode);
        bool DeleteMasterCode(int id);

        List<MasterCode> GetViewableMasterCodes();
        bool IsMasterCode_CodeUnique(string code);
        bool IsMasterCode_NameUnique(string name);
        bool IsMasterCode_NameCultureVariantUnique(string nameCultureVariant);
        bool IsMasterCode_CodeEditUnique(int id, string code);
        bool IsMasterCode_NameEditUnique(int id, string name);
        bool IsMasterCode_NameCultureVariantEditUnique(int id, string nameCultureVariant);
        bool CheckMasterCodeExistance( int id );

        List<MasterCodeDTO> GetMasterCodeParentsList(int id);
        List<MasterCode> SearchByName(string name);
    }
}
