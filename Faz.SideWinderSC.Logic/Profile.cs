using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents a mapping profile associated with the Strategic Commander.
    /// </summary>
    [XmlRoot]
    public class Profile : IXmlSerializable
    {
        /// <summary>
        /// The parent model.
        /// </summary>
        private readonly Model model;

        /// <summary>
        /// The associated mappings.
        /// </summary>
        private readonly List<Mapping> mappings;

        /// <summary>
        /// Provides a default constructor for xml serialization.
        /// </summary>
        private Profile()
        {
            this.mappings = new List<Mapping>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        /// <param name="model">The parent model.</param>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="model"/> parameter is <c>null</c>.
        /// </exception>
        public Profile(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            this.model = model;
            this.mappings = new List<Mapping>();
        }

        /// <summary>
        /// Gets the parent model.
        /// </summary>
        [XmlIgnore]
        public Model Model
        {
            get { return this.model; }
        }

        /// <summary>
        /// The name of the profile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The path to the file containing the profile.
        /// </summary>
        /// <remarks>
        /// Could be <c>null</c> when a new file should be created.
        /// </remarks>
        [XmlIgnore]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the associated mappings.
        /// </summary>
        [XmlElement(ElementName = "Mapping")]
        public virtual ICollection<Mapping> Mappings
        {
            get { return this.mappings; }
        }

        /// <summary>
        /// This method is reserved and should return <c>null</c>.
        /// </summary>
        /// <returns>Always <c>null</c>.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">
        /// The reader from which the object is deserialized.
        /// </param>
        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            // a value indicating whether an encapsulated element is already present
            bool withElement = reader.ReadState != ReadState.Initial;
            if (!withElement)
            {
                reader.MoveToContent();
            }

            using (XmlReader subReader = reader.ReadSubtree())
            {
                while (subReader.Read())
                {
                    if (subReader.IsStartElement())
                    {
                        if (subReader.Name == "Name")
                        {
                            this.Name = subReader.ReadElementContentAsString();
                            subReader.MoveToContent();
                        }

                        if (subReader.Name == "Mapping")
                        {
                            var mapping = new Mapping();
                            mapping.ReadXml(subReader);
                            this.Mappings.Add(mapping);
                            subReader.MoveToContent();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        /// The writer to which the object is serialized.
        /// </param>
        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            // a value indicating whether an encapsulzted element is already present
            bool withElement = writer.WriteState == WriteState.Element;

            if (!withElement)
            {
                writer.WriteStartElement("Profile");
            }

            if (this.Name != null)
            {
                writer.WriteStartElement("Name");
                writer.WriteValue(this.Name);
                writer.WriteEndElement();
            }

            foreach (var mapping in this.mappings)
            {
                writer.WriteStartElement("Mapping");
                mapping.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (!withElement)
            {
                writer.WriteEndElement();
            }
        }
    }
}
