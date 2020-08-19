using System;
using System.Collections.Generic;
using System.Text;

using ADS.Common.Models.Domain;

using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Tamam.Modules.Integration.Helpers;

using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Common.Utilities;
using ADS.Common.Handlers;
using ADS.Tamam.Modules.Integration.Models;
using RAWDetailCode = ADS.Tamam.Modules.Integration.Models.IDetailCodeSimilar;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class DetailCodeIntegrator
    {
        #region Props

        private IDetailCodeDataHandler DetailCodesDataHandler { get; set; }
        private DetailCodeRepository DetailCodesRepository { get; set; }

        #endregion
        #region Ctor

        public DetailCodeIntegrator( IDetailCodeDataHandler detailCodesDataHandler, DetailCodeRepository detailCodesRepository )
        {
            DetailCodesDataHandler = detailCodesDataHandler;
            DetailCodesRepository = detailCodesRepository;
        }

        #endregion

        #region publics

        public void Integrate()
        {
            var integrationDetailCodes = this.DetailCodesDataHandler.GetDetailCodes();
            if ( integrationDetailCodes.Count == 0 ) { return; }
            Integrate( integrationDetailCodes );
        }
        private void Integrate( List<RAWDetailCode> RAWDetailCodes )
        {
            foreach ( var integrationDetailCode in RAWDetailCodes )
            {
                var tamamDetailCode = DetailCodesRepository.GetDetailCode( integrationDetailCode.Code );
                if ( tamamDetailCode == null )
                {
                    // not exist ---> Insert
                    this.Create( integrationDetailCode );
                }
                else
                {
                    if ( this.IsChanged( tamamDetailCode, integrationDetailCode ) )
                    {
                        // exist and Changed ---> update
                        this.Edit( tamamDetailCode, integrationDetailCode );
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage( integrationDetailCode );
                        XLogger.Info( message );
                    }
                }
            }
        }

        public virtual ValidationResponse IsValidForEdit( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            return new ValidationResponse( true, string.Empty );
        }

        #endregion

        #region Helpers

        private bool IsChanged( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            if ( RAWDetailCode.Code != DetailCode.Code ) return true;
            if ( RAWDetailCode.Name != DetailCode.Name ) return true;
            if ( RAWDetailCode.NameVariant != DetailCode.NameCultureVariant ) return true;
            if ( RAWDetailCode.Activated == DetailCode.IsDeleted ) return true;

            return false;
        }

        private DetailCode Map( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            if ( string.IsNullOrWhiteSpace( RAWDetailCode.Code ) ) throw new ApplicationException( " [Code cannot be empty] " );
            if ( string.IsNullOrWhiteSpace( RAWDetailCode.Name ) ) throw new ApplicationException( " [Name cannot be empty] " );
            if ( string.IsNullOrWhiteSpace( RAWDetailCode.NameVariant ) ) throw new ApplicationException( " [NameCultureVariant cannot be empty] " );

            DetailCode.Code = RAWDetailCode.Code;
            DetailCode.Name = RAWDetailCode.Name;
            DetailCode.NameCultureVariant = RAWDetailCode.NameVariant;
            DetailCode.IsDeleted = !RAWDetailCode.Activated;

            return DetailCode;
        }

        private void Create( RAWDetailCode RAWDetailCode )
        {
            try
            {
                var emptyDetailCode = new DetailCode();
                emptyDetailCode.MasterCodeId = DetailCodesRepository.MasterCode.Id;
                emptyDetailCode.IsActive = true;
                //emptyDetailCode.IsDeleted = false;

                var newDetailCode = Map( emptyDetailCode, RAWDetailCode );
                var response = Broker.DetailCodeHandler.CreateDetailCode( newDetailCode );
                ResponseHelper.HandleResponse( response, RAWDetailCode, Actions.CreateDetailCode );
                if ( response.Id > 0 )
                {
                    DetailCodesDataHandler.UpdateAsSynced( RAWDetailCode );
                }
            }
            catch ( Exception ex )
            {
                XLogger.Error( Actions.CreateDetailCode + RAWDetailCode.GetLoggingData(), ex );
            }
        }

        private void Edit( DetailCode DetailCode, RAWDetailCode RAWDetailCode )
        {
            try
            {
                var validationResponse = IsValidForEdit( DetailCode, RAWDetailCode );
                if ( !validationResponse.IsValid )
                {
                    ResponseHelper.HandleResponse( null, RAWDetailCode, Actions.EditDetailCode, validationResponse.Message );
                    return;
                }

                var modifiedDetailCode = Map( DetailCode, RAWDetailCode );
                var response = Broker.DetailCodeHandler.UpdateDetailCode( modifiedDetailCode );
                ResponseHelper.HandleResponse( response, RAWDetailCode, Actions.EditDetailCode );
                if ( response.Id > 0 )
                {
                    DetailCodesDataHandler.UpdateAsSynced( RAWDetailCode );
                }
            }
            catch ( Exception ex )
            {
                XLogger.Error( Actions.EditDetailCode + RAWDetailCode, ex );
            }
        }

        #endregion

        # region inner

        static class Actions
        {
            public const string CreateDetailCode = "Create DetailCode Action";
            public const string EditDetailCode = "Edit DetailCode Action";
        }

        # endregion
    }
}