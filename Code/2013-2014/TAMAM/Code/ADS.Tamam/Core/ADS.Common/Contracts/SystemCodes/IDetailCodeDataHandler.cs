using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Common.Models.DTO;

namespace ADS.Common.Contracts.SystemCodes
{
    public interface IDetailCodeDataHandler : IBaseHandler
    {
        List<DetailCode> GetDetailCodes();
        DetailCode GetDetailCode(int id);
        DetailCode GetDetailCode(int id, bool underlyingCollections);
        DetailCode GetDetailCode(string code);
        DetailCode GetDetailCode(string code, bool underlyingCollections);
        DetailCode CreateDetailCode(DetailCode detailCode);
        DetailCode UpdateDetailCode(DetailCode detailCode);
        bool DeleteDetailCode(int id);

        List<DetailCode> GetViewableDetailCodes();
        bool IsDetailCode_CodeUnique(string code);
        bool IsDetailCode_NameUnique(string name);
        bool IsDetailCode_NameCultureVariantUnique(string nameCultureVariant);

        bool CheckDetailCodeExistance( int id );

        List<DetailCodeDTO> GetDetailCodeParentsList(int id);
        List<DetailCode> SearchDetailCodes(DetailCodeCriteria criteria);

        List<DetailCode> GetDetailCodesByMasterCode( string masterCode, bool allDetailCodes );
        List<DetailCode> GetDetailCodesByMasterCode ( string masterCode );
        List<DetailCode> GetDetailCodesByMasterCode ( int masterCode );
    }
}
