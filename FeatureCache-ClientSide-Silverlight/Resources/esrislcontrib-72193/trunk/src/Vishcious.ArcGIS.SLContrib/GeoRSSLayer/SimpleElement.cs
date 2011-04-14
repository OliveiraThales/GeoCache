using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace Vishcious.ArcGIS.SLContrib
{
    public class SimpleElement
    {
        public string Name
        {
            get;
            set;
        }

        public string NamespaceName
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public ObservableCollection<SimpleElement> Attributes
        {
            get;
            set;
        }

        public SimpleElement()
        {
            Attributes = new ObservableCollection<SimpleElement>();
        }

        public static SimpleElement Create( XElement xElement )
        {
            if( xElement != null )
            {
                return new SimpleElement()
                {
                    Name = xElement.Name.LocalName,
                    NamespaceName = xElement.Name.NamespaceName,
                    Value = xElement.Value,
                    Attributes = GetAttributes(xElement)
                };
            }

            return null;
        }

        public static ObservableCollection<SimpleElement> GetAttributes( XElement xElement )
        {
            if( xElement != null )
            {
                ObservableCollection<SimpleElement> list = new ObservableCollection<SimpleElement>();
                foreach( XAttribute item in xElement.Attributes() )
                {
                    list.Add( new SimpleElement()
                        {
                            Name = item.Name.LocalName,
                            NamespaceName = item.Name.NamespaceName,
                            Value = item.Value
                        } );
                }

                return list;
            }

            return null;
        }
    }
}
