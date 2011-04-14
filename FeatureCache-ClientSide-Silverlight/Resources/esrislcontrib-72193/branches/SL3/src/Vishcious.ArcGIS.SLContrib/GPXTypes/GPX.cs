using System;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Vishcious.GPX
{

    public partial class gpxType
    {
        public gpxType()
        {
            this.version = "1.1";
        }
        /// <summary>
        /// The private member referenced by the
        /// <see cref="metadata" /> property.
        /// </summary>
        private metadataType @__metadata;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="wpt" /> property.
        /// </summary>
        private wptTypeCollection @__wpt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="rte" /> property.
        /// </summary>
        private rteTypeCollection @__rte;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="trk" /> property.
        /// </summary>
        private trkTypeCollection @__trk;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="version" /> property.
        /// </summary>
        private string @__version;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="creator" /> property.
        /// </summary>
        private string @__creator;
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__metadata" /> value of the
        /// <see cref="gpxType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public metadataType _metadata
        {
            get
            {
                return this.@__metadata;
            }
            set
            {
                this.@__metadata = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__metadata" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public metadataType metadata
        {
            get
            {
                if( ( this.@__metadata == null ) )
                {
                    this.@__metadata = new metadataType();
                }
                return this.@__metadata;
            }
            set
            {
                this.@__metadata = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__wpt" /> value of the
        /// <see cref="gpxType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public wptType[] _wpt
        {
            get
            {
                return @__wpt.ToArray();
            }
            set
            {
                this.@__wpt = new wptTypeCollection();
                @__wpt.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__wpt" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public wptTypeCollection wpt
        {
            get
            {
                if( ( this.@__wpt == null ) )
                {
                    this.@__wpt = new wptTypeCollection();
                }
                return this.@__wpt;
            }
            set
            {
                this.@__wpt = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__rte" /> value of the
        /// <see cref="gpxType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public rteType[] _rte
        {
            get
            {
                return this.@__rte.ToArray();
            }
            set
            {
                this.@__rte = new rteTypeCollection();
                @__rte.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__rte" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public rteTypeCollection rte
        {
            get
            {
                if( ( this.@__rte == null ) )
                {
                    this.@__rte = new rteTypeCollection();
                }
                return this.@__rte;
            }
            set
            {
                this.@__rte = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__trk" /> value of the
        /// <see cref="gpxType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public trkType[] _trk
        {
            get
            {
                return @__trk.ToArray();
            }
            set
            {
                this.@__trk = new trkTypeCollection();
                @__trk.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__trk" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public trkTypeCollection trk
        {
            get
            {
                if( ( this.@__trk == null ) )
                {
                    this.@__trk = new trkTypeCollection();
                }
                return this.@__trk;
            }
            set
            {
                this.@__trk = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="gpxType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__version" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.@__version;
            }
            set
            {
                this.@__version = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__creator" />
        /// value of the <see cref="gpxType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string creator
        {
            get
            {
                return this.@__creator;
            }
            set
            {
                this.@__creator = value;
            }
        }

        /// <summary>
        /// Gets the schema location as a URI string.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The location is generated from the source location
        /// specified for the schema.
        /// </para>
        /// <para>
        /// Note that this property must be generated with a set accessor in order
        /// that the <b>xsi:schemaLocation</b> attribute is correctly generated by
        /// the <see cref="XmlSerializer" /> object.
        /// </para>
        /// </remarks>
        public virtual string schemaLocation
        {
            get
            {
                return "http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
            }
            set
            {
            }
        }
    }
    
    public partial class metadataType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="name" /> property.
        /// </summary>
        private string @__name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="desc" /> property.
        /// </summary>
        private string @__desc;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="author" /> property.
        /// </summary>
        private personType @__author;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="copyright" /> property.
        /// </summary>
        private copyrightType @__copyright;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="link" /> property.
        /// </summary>
        private linkTypeCollection @__link;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="time" /> property.
        /// </summary>
        private System.DateTime @__time;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="timeSpecified" /> property.
        /// </summary>
        private bool @__timeSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="keywords" /> property.
        /// </summary>
        private string @__keywords;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="bounds" /> property.
        /// </summary>
        private boundsType @__bounds;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// Sets or gets the <see cref="__name" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        public string name
        {
            get
            {
                return this.@__name;
            }
            set
            {
                this.@__name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__desc" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        public string desc
        {
            get
            {
                return this.@__desc;
            }
            set
            {
                this.@__desc = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__author" /> value of the
        /// <see cref="metadataType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public personType _author
        {
            get
            {
                return this.@__author;
            }
            set
            {
                this.@__author = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__author" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public personType author
        {
            get
            {
                if( ( this.@__author == null ) )
                {
                    this.@__author = new personType();
                }
                return this.@__author;
            }
            set
            {
                this.@__author = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__copyright" /> value of the
        /// <see cref="metadataType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public copyrightType _copyright
        {
            get
            {
                return this.@__copyright;
            }
            set
            {
                this.@__copyright = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__copyright" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public copyrightType copyright
        {
            get
            {
                if( ( this.@__copyright == null ) )
                {
                    this.@__copyright = new copyrightType();
                }
                return this.@__copyright;
            }
            set
            {
                this.@__copyright = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__link" /> value of the
        /// <see cref="metadataType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public linkType[] _link
        {
            get
            {
                return @__link.ToArray();
            }
            set
            {
                this.@__link = new linkTypeCollection();
                @__link.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__link" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public linkTypeCollection link
        {
            get
            {
                if( ( this.@__link == null ) )
                {
                    this.@__link = new linkTypeCollection();
                }
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__time" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        public System.DateTime time
        {
            get
            {
                return this.@__time;
            }
            set
            {
                this.@__time = value;
                this.timeSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__timeSpecified" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.@__timeSpecified;
            }
            set
            {
                this.@__timeSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__keywords" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        public string keywords
        {
            get
            {
                return this.@__keywords;
            }
            set
            {
                this.@__keywords = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__bounds" /> value of the
        /// <see cref="metadataType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public boundsType _bounds
        {
            get
            {
                return this.@__bounds;
            }
            set
            {
                this.@__bounds = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__bounds" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public boundsType bounds
        {
            get
            {
                if( ( this.@__bounds == null ) )
                {
                    this.@__bounds = new boundsType();
                }
                return this.@__bounds;
            }
            set
            {
                this.@__bounds = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="metadataType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="metadataType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }

    }
    
    public partial class wptType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="ele" /> property.
        /// </summary>
        private decimal @__ele;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="eleSpecified" /> property.
        /// </summary>
        private bool @__eleSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="time" /> property.
        /// </summary>
        private System.DateTime @__time;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="timeSpecified" /> property.
        /// </summary>
        private bool @__timeSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="magvar" /> property.
        /// </summary>
        private decimal @__magvar;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="magvarSpecified" /> property.
        /// </summary>
        private bool @__magvarSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="geoidheight" /> property.
        /// </summary>
        private decimal @__geoidheight;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="geoidheightSpecified" /> property.
        /// </summary>
        private bool @__geoidheightSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="name" /> property.
        /// </summary>
        private string @__name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="cmt" /> property.
        /// </summary>
        private string @__cmt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="desc" /> property.
        /// </summary>
        private string @__desc;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="src" /> property.
        /// </summary>
        private string @__src;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="link" /> property.
        /// </summary>
        private linkTypeCollection @__link;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="sym" /> property.
        /// </summary>
        private string @__sym;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="type" /> property.
        /// </summary>
        private string @__type;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="fix" /> property.
        /// </summary>
        private fixType @__fix;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="fixSpecified" /> property.
        /// </summary>
        private bool @__fixSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="sat" /> property.
        /// </summary>
        private string @__sat;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="hdop" /> property.
        /// </summary>
        private decimal @__hdop;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="hdopSpecified" /> property.
        /// </summary>
        private bool @__hdopSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="vdop" /> property.
        /// </summary>
        private decimal @__vdop;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="vdopSpecified" /> property.
        /// </summary>
        private bool @__vdopSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="pdop" /> property.
        /// </summary>
        private decimal @__pdop;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="pdopSpecified" /> property.
        /// </summary>
        private bool @__pdopSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="ageofdgpsdata" /> property.
        /// </summary>
        private decimal @__ageofdgpsdata;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="ageofdgpsdataSpecified" /> property.
        /// </summary>
        private bool @__ageofdgpsdataSpecified;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="dgpsid" /> property.
        /// </summary>
        private string @__dgpsid;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="lat" /> property.
        /// </summary>
        private decimal @__lat;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="lon" /> property.
        /// </summary>
        private decimal @__lon;
        /// <summary>
        /// Sets or gets the <see cref="__ele" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal ele
        {
            get
            {
                return this.@__ele;
            }
            set
            {
                this.@__ele = value;
                this.eleSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__eleSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool eleSpecified
        {
            get
            {
                return this.@__eleSpecified;
            }
            set
            {
                this.@__eleSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__time" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public System.DateTime time
        {
            get
            {
                return this.@__time;
            }
            set
            {
                this.@__time = value;
                this.timeSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__timeSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.@__timeSpecified;
            }
            set
            {
                this.@__timeSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__magvar" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal magvar
        {
            get
            {
                return this.@__magvar;
            }
            set
            {
                this.@__magvar = value;
                this.magvarSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__magvarSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool magvarSpecified
        {
            get
            {
                return this.@__magvarSpecified;
            }
            set
            {
                this.@__magvarSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__geoidheight" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal geoidheight
        {
            get
            {
                return this.@__geoidheight;
            }
            set
            {
                this.@__geoidheight = value;
                this.geoidheightSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__geoidheightSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool geoidheightSpecified
        {
            get
            {
                return this.@__geoidheightSpecified;
            }
            set
            {
                this.@__geoidheightSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__name" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string name
        {
            get
            {
                return this.@__name;
            }
            set
            {
                this.@__name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__cmt" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string cmt
        {
            get
            {
                return this.@__cmt;
            }
            set
            {
                this.@__cmt = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__desc" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string desc
        {
            get
            {
                return this.@__desc;
            }
            set
            {
                this.@__desc = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__src" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string src
        {
            get
            {
                return this.@__src;
            }
            set
            {
                this.@__src = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__link" /> value of the
        /// <see cref="wptType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public linkType[] _link
        {
            get
            {
                return @__link.ToArray();
            }
            set
            {
                this.@__link = new linkTypeCollection();
                @__link.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__link" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public linkTypeCollection link
        {
            get
            {
                if( ( this.@__link == null ) )
                {
                    this.@__link = new linkTypeCollection();
                }
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__sym" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string sym
        {
            get
            {
                return this.@__sym;
            }
            set
            {
                this.@__sym = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__type" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string type
        {
            get
            {
                return this.@__type;
            }
            set
            {
                this.@__type = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__fix" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public fixType fix
        {
            get
            {
                return this.@__fix;
            }
            set
            {
                this.@__fix = value;
                this.fixSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__fixSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fixSpecified
        {
            get
            {
                return this.@__fixSpecified;
            }
            set
            {
                this.@__fixSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__sat" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string sat
        {
            get
            {
                return this.@__sat;
            }
            set
            {
                this.@__sat = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__hdop" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal hdop
        {
            get
            {
                return this.@__hdop;
            }
            set
            {
                this.@__hdop = value;
                this.hdopSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__hdopSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hdopSpecified
        {
            get
            {
                return this.@__hdopSpecified;
            }
            set
            {
                this.@__hdopSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__vdop" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal vdop
        {
            get
            {
                return this.@__vdop;
            }
            set
            {
                this.@__vdop = value;
                this.vdopSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__vdopSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool vdopSpecified
        {
            get
            {
                return this.@__vdopSpecified;
            }
            set
            {
                this.@__vdopSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__pdop" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal pdop
        {
            get
            {
                return this.@__pdop;
            }
            set
            {
                this.@__pdop = value;
                this.pdopSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__pdopSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pdopSpecified
        {
            get
            {
                return this.@__pdopSpecified;
            }
            set
            {
                this.@__pdopSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__ageofdgpsdata" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public decimal ageofdgpsdata
        {
            get
            {
                return this.@__ageofdgpsdata;
            }
            set
            {
                this.@__ageofdgpsdata = value;
                this.ageofdgpsdataSpecified = true;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__ageofdgpsdataSpecified" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ageofdgpsdataSpecified
        {
            get
            {
                return this.@__ageofdgpsdataSpecified;
            }
            set
            {
                this.@__ageofdgpsdataSpecified = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__dgpsid" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        public string dgpsid
        {
            get
            {
                return this.@__dgpsid;
            }
            set
            {
                this.@__dgpsid = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="wptType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__lat" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lat
        {
            get
            {
                return this.@__lat;
            }
            set
            {
                this.@__lat = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__lon" />
        /// value of the <see cref="wptType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lon
        {
            get
            {
                return this.@__lon;
            }
            set
            {
                this.@__lon = value;
            }
        }

    }
    
    public partial class rteType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="name" /> property.
        /// </summary>
        private string @__name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="cmt" /> property.
        /// </summary>
        private string @__cmt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="desc" /> property.
        /// </summary>
        private string @__desc;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="src" /> property.
        /// </summary>
        private string @__src;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="link" /> property.
        /// </summary>
        private linkTypeCollection @__link;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="number" /> property.
        /// </summary>
        private string @__number;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="type" /> property.
        /// </summary>
        private string @__type;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="rtept" /> property.
        /// </summary>
        private wptTypeCollection @__rtept;
        /// <summary>
        /// Sets or gets the <see cref="__name" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string name
        {
            get
            {
                return this.@__name;
            }
            set
            {
                this.@__name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__cmt" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string cmt
        {
            get
            {
                return this.@__cmt;
            }
            set
            {
                this.@__cmt = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__desc" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string desc
        {
            get
            {
                return this.@__desc;
            }
            set
            {
                this.@__desc = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__src" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string src
        {
            get
            {
                return this.@__src;
            }
            set
            {
                this.@__src = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__link" /> value of the
        /// <see cref="rteType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public linkType[] _link
        {
            get
            {
                return @__link.ToArray();
            }
            set
            {
                this.@__link = new linkTypeCollection();
                @__link.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__link" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public linkTypeCollection link
        {
            get
            {
                if( ( this.@__link == null ) )
                {
                    this.@__link = new linkTypeCollection();
                }
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__number" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string number
        {
            get
            {
                return this.@__number;
            }
            set
            {
                this.@__number = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__type" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        public string type
        {
            get
            {
                return this.@__type;
            }
            set
            {
                this.@__type = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="rteType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__rtept" /> value of the
        /// <see cref="rteType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public wptType[] _rtept
        {
            get
            {
                return @__rtept.ToArray();
            }
            set
            {
                this.@__rtept = new wptTypeCollection();
                @__rtept.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__rtept" />
        /// value of the <see cref="rteType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public wptTypeCollection rtept
        {
            get
            {
                if( ( this.@__rtept == null ) )
                {
                    this.@__rtept = new wptTypeCollection();
                }
                return this.@__rtept;
            }
            set
            {
                this.@__rtept = value;
            }
        }

    }

    public partial class trkType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="name" /> property.
        /// </summary>
        private string @__name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="cmt" /> property.
        /// </summary>
        private string @__cmt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="desc" /> property.
        /// </summary>
        private string @__desc;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="src" /> property.
        /// </summary>
        private string @__src;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="link" /> property.
        /// </summary>
        private linkTypeCollection @__link;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="number" /> property.
        /// </summary>
        private string @__number;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="type" /> property.
        /// </summary>
        private string @__type;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="trkseg" /> property.
        /// </summary>
        private trksegTypeCollection @__trkseg;
        /// <summary>
        /// Sets or gets the <see cref="__name" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string name
        {
            get
            {
                return this.@__name;
            }
            set
            {
                this.@__name = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__cmt" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string cmt
        {
            get
            {
                return this.@__cmt;
            }
            set
            {
                this.@__cmt = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__desc" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string desc
        {
            get
            {
                return this.@__desc;
            }
            set
            {
                this.@__desc = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__src" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string src
        {
            get
            {
                return this.@__src;
            }
            set
            {
                this.@__src = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__link" /> value of the
        /// <see cref="trkType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public linkType[] _link
        {
            get
            {
                return @__link.ToArray();
            }
            set
            {
                this.@__link = new linkTypeCollection();
                @__link.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__link" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public linkTypeCollection link
        {
            get
            {
                if( ( this.@__link == null ) )
                {
                    this.@__link = new linkTypeCollection();
                }
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__number" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string number
        {
            get
            {
                return this.@__number;
            }
            set
            {
                this.@__number = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__type" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        public string type
        {
            get
            {
                return this.@__type;
            }
            set
            {
                this.@__type = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="trkType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__trkseg" /> value of the
        /// <see cref="trkType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public trksegType[] _trkseg
        {
            get
            {
                return @__trkseg.ToArray();
            }
            set
            {
                this.@__trkseg = new trksegTypeCollection();
                @__trkseg.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__trkseg" />
        /// value of the <see cref="trkType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public trksegTypeCollection trkseg
        {
            get
            {
                if( ( this.@__trkseg == null ) )
                {
                    this.@__trkseg = new trksegTypeCollection();
                }
                return this.@__trkseg;
            }
            set
            {
                this.@__trkseg = value;
            }
        }

    }

    public partial class extensionsType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="Any" /> property.
        /// </summary>
        private XmlElementCollection @__Any;
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__Any" /> value of the
        /// <see cref="extensionsType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        [System.Xml.Serialization.XmlAnyElementAttribute()]
        public XElement[] _Any
        {
            get
            {
                return @__Any.ToArray();
            }
            set
            {
                this.@__Any = new XmlElementCollection();
                @__Any.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__Any" />
        /// value of the <see cref="extensionsType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public XmlElementCollection Any
        {
            get
            {
                if( ( this.@__Any == null ) )
                {
                    this.@__Any = new XmlElementCollection();
                }
                return this.@__Any;
            }
            set
            {
                this.@__Any = value;
            }
        }

    }

    public partial class trksegType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="trkpt" /> property.
        /// </summary>
        private wptTypeCollection @__trkpt;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="extensions" /> property.
        /// </summary>
        private extensionsType @__extensions;
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__trkpt" /> value of the
        /// <see cref="trksegType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public wptType[] _trkpt
        {
            get
            {
                return @__trkpt.ToArray();
            }
            set
            {
                this.@__trkpt = new wptTypeCollection();
                @__trkpt.AddRange( value );
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__trkpt" />
        /// value of the <see cref="trksegType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public wptTypeCollection trkpt
        {
            get
            {
                if( ( this.@__trkpt == null ) )
                {
                    this.@__trkpt = new wptTypeCollection();
                }
                return this.@__trkpt;
            }
            set
            {
                this.@__trkpt = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__extensions" /> value of the
        /// <see cref="trksegType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public extensionsType _extensions
        {
            get
            {
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__extensions" />
        /// value of the <see cref="trksegType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public extensionsType extensions
        {
            get
            {
                if( ( this.@__extensions == null ) )
                {
                    this.@__extensions = new extensionsType();
                }
                return this.@__extensions;
            }
            set
            {
                this.@__extensions = value;
            }
        }

    }

    public partial class copyrightType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="year" /> property.
        /// </summary>
        private string @__year;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="license" /> property.
        /// </summary>
        private string @__license;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="author" /> property.
        /// </summary>
        private string @__author;
        /// <summary>
        /// Sets or gets the <see cref="__year" />
        /// value of the <see cref="copyrightType" />
        /// object class.
        /// </summary>
        public string year
        {
            get
            {
                return this.@__year;
            }
            set
            {
                this.@__year = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__license" />
        /// value of the <see cref="copyrightType" />
        /// object class.
        /// </summary>
        public string license
        {
            get
            {
                return this.@__license;
            }
            set
            {
                this.@__license = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__author" />
        /// value of the <see cref="copyrightType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string author
        {
            get
            {
                return this.@__author;
            }
            set
            {
                this.@__author = value;
            }
        }

    }

    public partial class linkType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="text" /> property.
        /// </summary>
        private string @__text;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="type" /> property.
        /// </summary>
        private string @__type;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="href" /> property.
        /// </summary>
        private string @__href;
        /// <summary>
        /// Sets or gets the <see cref="__text" />
        /// value of the <see cref="linkType" />
        /// object class.
        /// </summary>
        public string text
        {
            get
            {
                return this.@__text;
            }
            set
            {
                this.@__text = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__type" />
        /// value of the <see cref="linkType" />
        /// object class.
        /// </summary>
        public string type
        {
            get
            {
                return this.@__type;
            }
            set
            {
                this.@__type = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__href" />
        /// value of the <see cref="linkType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute( DataType = "anyURI" )]
        public string href
        {
            get
            {
                return this.@__href;
            }
            set
            {
                this.@__href = value;
            }
        }

    }

    public partial class emailType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="id" /> property.
        /// </summary>
        private string @__id;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="domain" /> property.
        /// </summary>
        private string @__domain;
        /// <summary>
        /// Sets or gets the <see cref="__id" />
        /// value of the <see cref="emailType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.@__id;
            }
            set
            {
                this.@__id = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__domain" />
        /// value of the <see cref="emailType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string domain
        {
            get
            {
                return this.@__domain;
            }
            set
            {
                this.@__domain = value;
            }
        }

    }

    public partial class personType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="name" /> property.
        /// </summary>
        private string @__name;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="email" /> property.
        /// </summary>
        private emailType @__email;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="link" /> property.
        /// </summary>
        private linkType @__link;
        /// <summary>
        /// Sets or gets the <see cref="__name" />
        /// value of the <see cref="personType" />
        /// object class.
        /// </summary>
        public string name
        {
            get
            {
                return this.@__name;
            }
            set
            {
                this.@__name = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__email" /> value of the
        /// <see cref="personType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public emailType _email
        {
            get
            {
                return this.@__email;
            }
            set
            {
                this.@__email = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__email" />
        /// value of the <see cref="personType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public emailType email
        {
            get
            {
                if( ( this.@__email == null ) )
                {
                    this.@__email = new emailType();
                }
                return this.@__email;
            }
            set
            {
                this.@__email = value;
            }
        }
        /// <summary>
        /// Used for the serialization of the 
        /// <see cref="__link" /> value of the
        /// <see cref="personType" /> object class.
        /// </summary>
        /// <remarks>
        /// Used internally by the <see cref="XmlSerializer" /> class.
        /// </remarks>
        public linkType _link
        {
            get
            {
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__link" />
        /// value of the <see cref="personType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public linkType link
        {
            get
            {
                if( ( this.@__link == null ) )
                {
                    this.@__link = new linkType();
                }
                return this.@__link;
            }
            set
            {
                this.@__link = value;
            }
        }

    }

    public partial class boundsType
    {
        /// <summary>
        /// The private member referenced by the
        /// <see cref="minlat" /> property.
        /// </summary>
        private decimal @__minlat;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="minlon" /> property.
        /// </summary>
        private decimal @__minlon;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="maxlat" /> property.
        /// </summary>
        private decimal @__maxlat;
        /// <summary>
        /// The private member referenced by the
        /// <see cref="maxlon" /> property.
        /// </summary>
        private decimal @__maxlon;
        /// <summary>
        /// Sets or gets the <see cref="__minlat" />
        /// value of the <see cref="boundsType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal minlat
        {
            get
            {
                return this.@__minlat;
            }
            set
            {
                this.@__minlat = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__minlon" />
        /// value of the <see cref="boundsType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal minlon
        {
            get
            {
                return this.@__minlon;
            }
            set
            {
                this.@__minlon = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__maxlat" />
        /// value of the <see cref="boundsType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal maxlat
        {
            get
            {
                return this.@__maxlat;
            }
            set
            {
                this.@__maxlat = value;
            }
        }
        /// <summary>
        /// Sets or gets the <see cref="__maxlon" />
        /// value of the <see cref="boundsType" />
        /// object class.
        /// </summary>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal maxlon
        {
            get
            {
                return this.@__maxlon;
            }
            set
            {
                this.@__maxlon = value;
            }
        }

    }

    public enum fixType
    {
        /// <remarks/>
        none,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute( "2d" )]
        Item2d,
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute( "3d" )]
        Item3d,
        /// <remarks/>
        dgps,
        /// <remarks/>
        pps,
    }

    public class linkTypeCollection : List<linkType>
    {
    }

    public class rteTypeCollection : List<rteType>
    {
    }

    public class trksegTypeCollection : List<trksegType>
    {
    }

    public class trkTypeCollection : List<trkType>
    {
    }

    public class wptTypeCollection : List<wptType>
    {
    }

    public class XmlElementCollection : List<XElement>
    {
    }
}
