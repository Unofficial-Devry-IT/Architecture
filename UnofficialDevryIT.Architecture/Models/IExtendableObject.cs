namespace UnofficialDevryIT.Architecture.Models
{
    /// <summary>
    /// Rather than modifying a database model
    /// Can leverage JSON for misc data that might be needed in the long run
    /// </summary>
    public interface IExtendableObject
    {
        /// <summary>
        /// JSON Formatted string to extend the containing object -- without modifying the database schema
        /// JSON data can contain properties with arbitrary values (like primitives or complext objects).
        /// Extension methods are available <see cref="ExtendableObjectExtensions""/> to manipulate this data
        /// </summary>
        string ExtensionData { get; set; }
    }
}