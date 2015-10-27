namespace AssetPackage
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using AssetPackage;

    /// <summary>
    /// An asset settings.
    /// 
    /// BaseSettings contains the (de-)serialization methods.
    /// </summary>
    public class AssetSettings : BaseSettings
    {
        /// <summary>
        /// Initializes a new instance of the AssetPackage.AssetSettings class.
        /// </summary>
        public AssetSettings()
            : base()
        {
            //
        }

        /// <summary>
        /// Gets or sets the test property.
        /// </summary>

        /// <summary>
        /// Gets or sets the test property.
        /// </summary>
        ///
        /// <value>
        /// The test property.
        /// </value>
        [DefaultValue("Hello Default World")]
        [XmlElement()]
        public String TestProperty
        {
            get;
            set;
        }

        // Not Portable to Xamarin.
        ///// <summary>
        ///// Gets or sets the color of the test.
        ///// </summary>
        /////
        ///// <value>
        ///// The color of the test.
        ///// </value>
        //[DefaultValue(typeof(Color), "Red")]
        //public Color TestColor
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Gets the string[].
        /// </summary>
        ///
        /// <value>
        /// .
        /// </value>
        [XmlArray()]
        [XmlArrayItem("ListItem")]
        [DefaultValue(new String[] { "Hello", "List", "World" })]
        public String[] TestList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the test read only.
        /// </summary>
        ///
        /// <value>
        /// true if test read only, false if not.
        /// </value>
        public Boolean TestReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}
