ReadMe for ArcGIS API for Silverlight/WPF server-side ASP.NET proxy
-----------------------------------------------------------

To use:

1) Add proxy.ashx and proxy.config to a Web site that hosts a Silverlight application.  Keep both files in the same location.  No additional configuration is required in the web.config, but you can add a handler reference to change how the proxy.ashx is exposed to the client.  Note, you can also create an ASP.NET Web application which only contains the proxy page (ashx and config).  

2) Modify the proxy.config to add Web sites that you want to permit clients to be able to access through the proxy. The Web sites host ArcGIS Server services.  In the the case of ArcGIS Silverlight API applications, REST endpoints for ArcGIS Server services will be used.  If a Web site is secured using tokens or Windows\HTTP, add the appropriate entries to the config file.  Use the examples provided as a guide.  

3) In the Silverlight client code, define the Proxy property on the ArcGIS Server layer to point to the proxy page.  For example, if the proxy page is available at the root of the Web site that hosts the Silverlight application, define the path relative to the site:

                <esri:ArcGISDynamicMapServiceLayer ID="USA" 
                    Url="http://net931/ArcGISToken/rest/services/USA_Data/MapServer"
                    ProxyURL="../proxy.ashx" 
                 /> 
                 
