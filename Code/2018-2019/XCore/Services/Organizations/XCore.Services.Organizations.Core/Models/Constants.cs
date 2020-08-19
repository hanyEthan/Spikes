namespace XCore.Services.Organizations.Core.Models
{
    public class Constants
    {
        #region DB.

        public static class DB
        {
            public const string Schema = "Org";
            public const string TableContactInformation = "ContactInfo";
            public const string TableContactPersonnel = "ContactPersonnel";
            public const string TableDepartment = "Departments";
            public const string TableOrganization = "Organizations";
            public const string TableOrganizationDelegation = "OrganizationDelegation";
            public const string TableSettings = "Settings";
        }

        #endregion

        public static string DateFormatt1 = "dd/MM/yyyy";
        public const string DBConnectionString = "XCore.Organization";
        public const string DefaultConnectionString = "Data Source = 10.0.0.50\\sql2016;Initial Catalog=XCore.Dev.Organization;Persist Security Info=True;User ID=sa;Password=P@ssw0rd";
    }
}
