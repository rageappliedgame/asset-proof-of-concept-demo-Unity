namespace asset_proof_of_concept_demo_CSharp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// A base settings.
    /// </summary>
    public class BaseSettings : ISettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Swiss.BaseSettings class.
        /// </summary>
        public BaseSettings()
        {
            //! Initialize Settings to their specified default values.
            UpdateDefaultValues();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Loads this object.
        /// </summary>
        internal void Load()
        {
            throw (new NotImplementedException());
        }

        public void FromXml(String xml)
        {
            throw (new NotImplementedException());
        }

        public String ToXml()
        {
            XmlSerializer ser = new XmlSerializer(GetType());

            using (StringWriterUtf8 textWriter = new StringWriterUtf8())
            {
                //! Use DataContractSerializer or DataContractJsonSerializer?
                // 
                ser.Serialize(textWriter, this);

                textWriter.Flush();

                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Set the value of (Public Instance) properties to the <see cref="DefaultValueAttribute"/>'s
        /// Value of that property.
        /// </summary>
        private void UpdateDefaultValues()
        {
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (pi.CanWrite)
                {
                    foreach (Object att in pi.GetCustomAttributes(typeof(DefaultValueAttribute), false))
                    {
                        if (att is DefaultValueAttribute)
                        {
                            Debug.Print("Updating {0}.{1} to {2}", GetType().Name, pi.Name, ((DefaultValueAttribute)att).Value);

                            pi.SetValue(this, ((DefaultValueAttribute)att).Value, new object[] { });

                            break;
                        }
                    }
                }
                else
                {
                    // Debug.Print("Cannot Update Default Value of Read-Only Property {0}", pi.Name);
                }

                Debug.Print("Error Updating Default Value of {0}", pi.Name);
            }
        }

        /// <summary>
        /// Empty settings.
        /// </summary>
        ///
        /// <param name="Class"> The class. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        public static String EmptySettings(String Class)
        {
            //! Return Empty Settings.
            // 
            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", null));

            XNamespace xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace xsd = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

            //XNamespace ns1 = "http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition";
            //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"
            doc.Add(
                new XElement(XNamespace.None + String.Format("{0}Settings", Class),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd))
                );

            using (StringWriterUtf8 sw = new StringWriterUtf8())
            {
                doc.Save(sw);

                return sw.ToString();
            }
        }

        #endregion Methods

        /// <summary>
        /// A string writer utf-8. Fix-up for XDocument Serialization defaulting to utf-16.
        /// </summary>
        internal class StringWriterUtf8 : StringWriter
        {
            //public StringWriterUtf8(StringBuilder sb)
            //    : base(sb)
            //{
            //}

            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}