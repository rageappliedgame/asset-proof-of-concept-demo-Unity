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

        ///// <summary>
        ///// Loads this object.
        ///// </summary>
        //internal void Load()
        //{
        //    throw (new NotImplementedException());
        //}

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

        #endregion Methods
    }
}