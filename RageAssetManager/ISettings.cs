namespace asset_proof_of_concept_demo_CSharp
{
    using System;

    /// <summary>
    /// Interface for settings.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Initializes this ISettings object from the given from XML.
        /// </summary>
        ///
        /// <param name="xml"> The XML. </param>
        void FromXml(String xml);

        /// <summary>
        /// Converts this ISettings object to an XML.
        /// </summary>
        ///
        /// <returns>
        /// This object as a String.
        /// </returns>
        String ToXml();
    }
}
