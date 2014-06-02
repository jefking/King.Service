namespace King.Service.Data.Azure
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
        /// <param name="connectionString">Storage Account</param>
        public AzureStorage(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("connectionString");
            }

            this.account = CloudStorageAccount.Parse(connectionString);
        }
        #endregion
    }
}