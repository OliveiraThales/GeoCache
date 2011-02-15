namespace GeoCache.Common.Repository
{
    using System;
    using System.Collections.Generic;

    using ESRI.ArcGIS.Geodatabase;

    using GeoCache.Contracts;

    using ESRI.ArcGIS.DataSourcesGDB;
    using ESRI.ArcGIS.esriSystem;

    using GeoCache.Common.LicenseManager;
    using GeoCache.Common.Geometry;

    using FeatureCursor = GeoCache.Common.FeatureCursor;

    /// <summary>
    /// Geo Repository to retrive data from sde
    /// </summary>
    public class SdeRepository : IRepository
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
        public SdeRepository(string server, string instance, string user, string password, string version)
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
            var feature = this.OpenFeatureClass(featureClassName);
            var filter = new QueryFilterClass();
            return new FeatureCursor(feature.Search(filter, false), feature.FeatureCount(filter));
        }

        /// <summary>
        /// Get all geometries inside the envelope from a featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <param name="envelope">Envelope</param>
        /// <returns>Enumerable of geometries</returns>
        public IEnumerable<IGeometry> GetByEnvelope(string featureClassName, IEnvelope envelope)
        {
            var feature = this.OpenFeatureClass(featureClassName);
            var filter = new SpatialFilterClass
                {
                    Geometry = new ESRI.ArcGIS.Geometry.EnvelopeClass
                        {
                            XMax = envelope.MaxX, XMin = envelope.MinX, YMax = envelope.MaxY, YMin = envelope.MinY
                        },
                    SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
                    
                };
            return new FeatureCursor(feature.Search(filter, false), feature.FeatureCount(filter));
        }

        /// <summary>
        /// Get full extent of featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <returns>Envelope</returns>
        public IEnvelope GetFullExtent(string featureClassName)
        {
            var dataset = (IGeoDataset)this.OpenFeatureClass(featureClassName);
            var extent = dataset.Extent;

            return new Envelope(extent.XMax, extent.XMin, extent.YMax, extent.YMin);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Open a featureClass
        /// </summary>
        /// <param name="featureClass">FeatureClass name</param>
        private IFeatureClass OpenFeatureClass(string featureClass)
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
