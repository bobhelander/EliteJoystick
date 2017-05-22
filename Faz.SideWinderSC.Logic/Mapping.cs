using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Represents a mapping between a Strategic Commander button and a keyboard combination.
    /// </summary>
    public class Mapping : IXmlSerializable
    {
        private readonly ICollection<ConsoleKey> keys;

        public Mapping()
        {
            this.keys = new LinkedList<ConsoleKey>();
        }

        /// <summary>
        /// Get or sets the name of the mapping.
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the associated Strategic Commander button.
        /// </summary>
        [XmlAttribute]
        public SwscButton Button { get; set; }

        /// <summary>
        /// Gets or sets the associated keyboard keys.
        /// </summary>
        [XmlElement]
        public ICollection<ConsoleKey> Keys
        {
            get { return this.keys; }
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
                subReader.Read();

                // Try to retrieve attributes
                string button = subReader.GetAttribute("Button");
                if (!string.IsNullOrEmpty(button))
                {
                    this.Button = (SwscButton)Enum.Parse(typeof(SwscButton), button);
                }

                // Try to retrieve next node
                subReader.Read();
                while (subReader.IsStartElement())
                {
                    if (subReader.Name == "Name")
                    {
                        this.Name = subReader.ReadElementContentAsString();
                        subReader.MoveToContent();
                    }

                    if (subReader.Name == "Key")
                    {
                        var key = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), subReader.ReadElementContentAsString());
                        this.Keys.Add(key);
                        subReader.MoveToContent();
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

            // a value indicating whether an encapsulated element is already present
            bool withElement = writer.WriteState == WriteState.Element;

            if (!withElement)
            {
                writer.WriteStartElement("Mapping");
            }

            if (this.Button != 0)
            {
                writer.WriteStartAttribute("Button");
                writer.WriteValue(this.Button.ToString());
                writer.WriteEndAttribute();
            }

            if (this.Name != null)
            {
                writer.WriteStartElement("Name");
                writer.WriteValue(this.Name);
                writer.WriteEndElement();
            }

            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("Key");
                writer.WriteValue(key.ToString());
                writer.WriteEndElement();
            }

            if (!withElement)
            {
                writer.WriteEndElement();
            }
        }
    }
}
