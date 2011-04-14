using System;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.Generic;

namespace Vishcious.ArcGIS.SLContrib
{
    public class LayerMetadata
    {
        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public string GeometryType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string DefinitionExpression
        {
            get;
            set;
        }

        public string CopyrightText
        {
            get;
            set;
        }

        public double MinimumScale
        {
            get;
            set;
        }

        public double MaximumScale
        {
            get;
            set;
        }

        public Envelope Extent
        {
            get;
            set;
        }

        public string DisplayField
        {
            get;
            set;
        }

        public List<FieldMetadata> Fields
        {
            get;
            set;
        }

        public KeyValuePair<int, string> ParentLayer
        {
            get;
            set;
        }

        public Dictionary<int, string> SubLayers
        {
            get;
            set;
        }
    }
}