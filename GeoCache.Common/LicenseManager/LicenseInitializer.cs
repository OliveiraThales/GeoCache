using System;
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace GeoCache.Common.LicenseManager
{
    /// <summary>
    /// License manager
    /// </summary>
    public class LicenseInitializer
    {
        #region Constants

        /// <summary>
        /// Message: No licenses were requested
        /// </summary>
        private const string MessageNoLicensesRequested = "Product: No licenses were requested";

        /// <summary>
        /// Message: Product available
        /// </summary>
        private const string MessageProductAvailable = "Product: {0}: Available";

        /// <summary>
        /// Message: Product Not Licensed
        /// </summary>
        private const string MessageProductNotLicensed = "Product: {0}: Not Licensed";

        /// <summary>
        /// Message:  Extension available
        /// </summary>
        private const string MessageExtensionAvailable = " Extension: {0}: Available";

        /// <summary>
        /// Message: Extension Not Licensed
        /// </summary>
        private const string MessageExtensionNotLicensed = " Extension: {0}: Not Licensed";

        /// <summary>
        /// Message: Failed
        /// </summary>
        private const string MessageExtensionFailed = " Extension: {0}: Failed";

        /// <summary>
        /// Message: Extension unavailable
        /// </summary>
        private const string MessageExtensionUnavailable = " Extension: {0}: Unavailable";

        #endregion

        #region Private members

        /// <summary>
        /// Inicializer.
        /// </summary>
        private IAoInitialize _aoInit;

        /// <summary>
        /// Extensions Status 
        /// </summary>
        private readonly Dictionary<esriLicenseExtensionCode, esriLicenseStatus> _extensionStatus = new Dictionary<esriLicenseExtensionCode, esriLicenseStatus>();

        /// <summary>
        /// Product Status
        /// </summary>
        private readonly Dictionary<esriLicenseProductCode, esriLicenseStatus> _productStatus = new Dictionary<esriLicenseProductCode, esriLicenseStatus>();

        /// <summary>
        /// Product initialized flag
        /// </summary>
        private bool _hasInitializeProduct;

        /// <summary>
        /// Released license flad
        /// </summary>
        private bool _hasShutDown;

        /// <summary>
        /// Checkout order flag
        /// </summary>
        private bool _productCheckOrdering = true;

        /// <summary>
        /// Extension list
        /// </summary>
        private List<esriLicenseExtensionCode> _requestedExtensions;

        /// <summary>
        /// Products required.
        /// </summary>
        private List<int> _requestedProducts;

        #endregion

        #region Properties

        /// <summary>
        /// Get/Set the ordering of product code checking. If true, check from lowest to 
        /// highest license. True by default.
        /// </summary>
        public bool InitializeLowerProductFirst
        {
            get { return _productCheckOrdering; }
            set { _productCheckOrdering = value; }
        }

        /// <summary>
        /// Retrieves the product code initialized in the ArcObjects application
        /// </summary>
        public esriLicenseProductCode InitializedProduct
        {
            get
            {
                try
                {
                    return _aoInit.InitializedProduct();
                }
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize application license
        /// </summary>
        /// <param name="productCodes">Products codes</param>
        /// <param name="extensionLics">Extensions codes.</param>
        public bool InitializeApplication(esriLicenseProductCode[] productCodes, esriLicenseExtensionCode[] extensionLics)
        {
            if (!RuntimeManager.Bind(ProductCode.EngineOrDesktop))
            {
                return false;
            }

            _aoInit = new AoInitializeClass();

            // Cache product codes by enum int so can be sorted without custom sorter
            _requestedProducts = new List<int>();
            foreach (var requestCodeNum in productCodes.Select(code => Convert.ToInt32(code)).Where(requestCodeNum => !_requestedProducts.Contains(requestCodeNum)))
            {
                _requestedProducts.Add(requestCodeNum);
            }

            AddExtensions(extensionLics);
            return Initialize();
        }

        /// <summary>
        /// A summary of the status of product and extensions initialization.
        /// </summary>
        public string LicenseMessage()
        {
            var prodStatus = string.Empty;
            if (_productStatus == null || _productStatus.Count == 0)
            {
                prodStatus = MessageNoLicensesRequested + Environment.NewLine;
            }
            else if (_productStatus.ContainsValue(esriLicenseStatus.esriLicenseAlreadyInitialized) || _productStatus.ContainsValue(esriLicenseStatus.esriLicenseCheckedOut))
            {
                prodStatus = ReportInformation(_aoInit as ILicenseInformation, _aoInit.InitializedProduct(), esriLicenseStatus.esriLicenseCheckedOut) + Environment.NewLine;
            }
            else
            {
                prodStatus = _productStatus.Aggregate(prodStatus, (current, item) => current + (ReportInformation(_aoInit as ILicenseInformation, item.Key, item.Value) + Environment.NewLine));
            }

            var extStatus = _extensionStatus.Select(item => ReportInformation(_aoInit as ILicenseInformation, item.Key, item.Value)).Where(info => !string.IsNullOrEmpty(info)).Aggregate(string.Empty, (current, info) => current + (info + Environment.NewLine));
            var status = prodStatus + extStatus;
            return status.Trim();
        }

        /// <summary>
        /// Shuts down AoInitialize object and check back in extensions to ensure
        /// any ESRI libraries that have been used are unloaded in the correct order.
        /// </summary>
        /// <remarks>Once Shutdown has been called, you cannot re-initialize the product license
        /// and should not make any ArcObjects call.</remarks>
        public void ShutdownApplication()
        {
            if (_hasShutDown)
            {
                return;
            }

            // Check back in extensions
            foreach (var item in _extensionStatus.Where(item => item.Value == esriLicenseStatus.esriLicenseCheckedOut))
            {
                _aoInit.CheckInExtension(item.Key);
            }

            _requestedProducts.Clear();
            _requestedExtensions.Clear();
            _extensionStatus.Clear();
            _productStatus.Clear();
            _aoInit.Shutdown();
            _hasShutDown = true;
        }

        /// <summary>
        /// Indicates if the extension is currently checked out.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        public bool IsExtensionCheckedOut(esriLicenseExtensionCode code)
        {
            return _aoInit.IsExtensionCheckedOut(code);
        }

        /// <summary>
        /// Set the extension(s) to be checked out for your ArcObjects code. 
        /// </summary>
        /// <param name="requestCodes">
        /// The request Codes.
        /// </param>
        public bool AddExtensions(params esriLicenseExtensionCode[] requestCodes)
        {
            if (_requestedExtensions == null)
            {
                _requestedExtensions = new List<esriLicenseExtensionCode>();
            }

            foreach (var code in requestCodes.Where(code => !_requestedExtensions.Contains(code)))
            {
                _requestedExtensions.Add(code);
            }

            return _hasInitializeProduct && CheckOutLicenses(InitializedProduct);
        }

        /// <summary>
        /// Check in extension(s) when it is no longer needed.
        /// </summary>
        /// <param name="requestCodes">
        /// The request Codes.
        /// </param>
        public void RemoveExtensions(params esriLicenseExtensionCode[] requestCodes)
        {
            if (_extensionStatus == null || _extensionStatus.Count == 0)
            {
                return;
            }

            foreach (var code in requestCodes.Where(code => _extensionStatus.ContainsKey(code)).Where(code => _aoInit.CheckInExtension(code) == esriLicenseStatus.esriLicenseCheckedIn))
            {
                _extensionStatus[code] = esriLicenseStatus.esriLicenseCheckedIn;
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Initilize
        /// </summary>
        private bool Initialize()
        {
            if (RuntimeManager.ActiveRuntime == null)
            {
                return false;
            }

            if (_requestedProducts == null || _requestedProducts.Count == 0)
            {
                return false;
            }

            var currentProduct = new esriLicenseProductCode();
            var productInitialized = false;
            _requestedProducts.Sort();
            if (!InitializeLowerProductFirst)
            {
                _requestedProducts.Reverse();
            }

            foreach (var prodNumber in _requestedProducts)
            {
                var prod = (esriLicenseProductCode)System.Enum.ToObject(typeof(esriLicenseProductCode), prodNumber);
                var status = _aoInit.IsProductCodeAvailable(prod);
                if (status == esriLicenseStatus.esriLicenseAvailable)
                {
                    status = _aoInit.Initialize(prod);
                    if (status == esriLicenseStatus.esriLicenseAlreadyInitialized || status == esriLicenseStatus.esriLicenseCheckedOut)
                    {
                        productInitialized = true;
                        currentProduct = _aoInit.InitializedProduct();
                    }
                }

                _productStatus.Add(prod, status);
                if (productInitialized)
                {
                    break;
                }
            }

            _hasInitializeProduct = productInitialized;
            _requestedProducts.Clear();

            // No product is initialized after trying all requested licenses, quit
            return productInitialized && CheckOutLicenses(currentProduct);
        }

        /// <summary>
        /// Checkout licenses
        /// </summary>
        /// <param name="currentProduct">producto code</param>
        private bool CheckOutLicenses(esriLicenseProductCode currentProduct)
        {
            var allSuccessful = true;

            // Request extensions
            if (_requestedExtensions != null && currentProduct != 0)
            {
                foreach (var ext in _requestedExtensions)
                {
                    var licenseStatus = _aoInit.IsExtensionCodeAvailable(currentProduct, ext);
                    if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
                    {
                        licenseStatus = _aoInit.CheckOutExtension(ext);
                    }

                    allSuccessful = allSuccessful && licenseStatus == esriLicenseStatus.esriLicenseCheckedOut;
                    if (_extensionStatus.ContainsKey(ext))
                    {
                        _extensionStatus[ext] = licenseStatus;
                    }
                    else
                    {
                        _extensionStatus.Add(ext, licenseStatus);
                    }
                }

                _requestedExtensions.Clear();
            }

            return allSuccessful;
        }

        /// <summary>
        /// Get license information
        /// </summary>
        /// <param name="licInfo">License info</param>
        /// <param name="code">Product code</param>
        /// <param name="status">License status</param>
        private static string ReportInformation(ILicenseInformation licInfo, esriLicenseProductCode code, esriLicenseStatus status)
        {
            string prodName;
            try
            {
                prodName = licInfo.GetLicenseProductName(code);
            }
            catch
            {
                prodName = code.ToString();
            }

            string statusInfo;
            switch (status)
            {
                case esriLicenseStatus.esriLicenseAlreadyInitialized:
                case esriLicenseStatus.esriLicenseCheckedOut:
                    statusInfo = string.Format(MessageProductAvailable, prodName);
                    break;
                default:
                    statusInfo = string.Format(MessageProductNotLicensed, prodName);
                    break;
            }

            return statusInfo;
        }

        /// <summary>
        /// Get license information
        /// </summary>
        /// <param name="licInfo">License info</param>
        /// <param name="code">Extension code</param>
        /// <param name="status">License status</param>
        private static string ReportInformation(ILicenseInformation licInfo, esriLicenseExtensionCode code, esriLicenseStatus status)
        {
            string extensionName;
            try
            {
                extensionName = licInfo.GetLicenseExtensionName(code);
            }
            catch
            {
                extensionName = code.ToString();
            }

            var statusInfo = string.Empty;

            switch (status)
            {
                case esriLicenseStatus.esriLicenseAlreadyInitialized:
                case esriLicenseStatus.esriLicenseCheckedOut:
                    statusInfo = string.Format(MessageExtensionAvailable, extensionName);
                    break;
                case esriLicenseStatus.esriLicenseCheckedIn:
                    break;
                case esriLicenseStatus.esriLicenseUnavailable:
                    statusInfo = string.Format(MessageExtensionUnavailable, extensionName);
                    break;
                case esriLicenseStatus.esriLicenseFailure:
                    statusInfo = string.Format(MessageExtensionFailed, extensionName);
                    break;
                default:
                    statusInfo = string.Format(MessageExtensionNotLicensed, extensionName);
                    break;
            }

            return statusInfo;
        }

        #endregion
    }
}