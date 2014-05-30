namespace King.Service.Azure
{
    using System;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;

    /// <summary>
    /// Azure Storage
    /// </summary>
    public class AzureStorage
    {
        #region Members
        /// <summary>
        /// Account
        /// </summary>
        protected readonly CloudStorageAccount account;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionStringKey">Storage Account Key</param>
        public AzureStorage(string connectionStringKey)
        {
            if (string.IsNullOrEmpty(connectionStringKey))
            {
                throw new ArgumentException("connectionStringKey");
            }

            this.account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(connectionStringKey));
        }
        #endregion
    }
}