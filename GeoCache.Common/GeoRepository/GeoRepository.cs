using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using GeoCache.Contracts;

namespace GeoCache.Common.GeoRepository
{
    using ESRI.ArcGIS.DataSourcesGDB;
    using ESRI.ArcGIS.esriSystem;

    using GeoCache.Common.LicenseManager;

    /// <summary>
    /// Geo Repository to retrive data from sde
    /// </summary>
    public class GeoRepository : IRepository
    {
        #region Fields

        /// <summary>
        /// Inidica se a Workspace está aberta.
        /// </summary>
        private bool isOpened;

        /// <summary>
        /// Workspace aberta.
        /// </summary>
        private IWorkspace workspace;

        /// <summary>
        /// Server address
        /// </summary>
        private readonly string _server;

        /// <summary>
        /// Server instance
        /// </summary>
        private readonly string _instance;

        /// <summary>
        /// Username
        /// </summary>
        private readonly string _user;

        /// <summary>
        /// Password
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Version
        /// </summary>
        private readonly string _version;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="server">Server address</param>
        /// <param name="instance">Server instance</param>
        /// <param name="user">username</param>
        /// <param name="password">password</param>
        /// <param name="version">version</param>
        public GeoRepository(string server, string instance, string user, string password, string version)
        {
            this._server = server;
            this._instance = instance;
            this._user = user;
            this._password = password;
            this._version = version;

            var lic = new LicenseInitializer();
            var initialized =
                lic.InitializeApplication(
                    new[] { esriLicenseProductCode.esriLicenseProductCodeEngine }, new esriLicenseExtensionCode[] { });

            this.OpenSdeWorkspace();
        }

        #endregion

        #region IRepository implementation

        /// <summary>
        /// Get all geometries from a featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <returns>Enumerable of geometries</returns>
        public IEnumerable<IGeometry> GetAll(string featureClassName)
        {
            return new FeatureCursor(this.OpenFeatureClass(featureClassName).Search(null, false));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Open a featureClass
        /// </summary>
        /// <param name="featureClass">FeatureClass name</param>
        public IFeatureClass OpenFeatureClass(string featureClass)
        {
            if (!isOpened)
            {
                throw new InvalidOperationException("Nenhuma workspace aberta.");
            }

            var featureWorkspace = (IFeatureWorkspace)workspace;
            try
            {
                return featureWorkspace.OpenFeatureClass(featureClass);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Feature Class não encontrada: {0}", featureClass), ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Conecta a uma workspace SDE
        /// </summary>
        private void OpenSdeWorkspace()
        {
            var factory = new SdeWorkspaceFactoryClass();

            IPropertySet set = new PropertySet();

            set.SetProperty("server", _server);
            set.SetProperty("instance", _instance);
            set.SetProperty("user", _user);
            set.SetProperty("version", _version);
            set.SetProperty("password", _password);

            try
            {
                workspace = factory.Open(set, 0);
                isOpened = true;
            }
            catch (Exception e)
            {
                isOpened = false;
                throw new InvalidOperationException(string.Format("Error to connected in SDE: {0}", e.Message), e);
            }
        }

        #endregion
    }
}
