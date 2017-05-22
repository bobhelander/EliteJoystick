using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides the methods to manage the Strategic Commander configuration.
    /// </summary>
    /// <seealso cref="ServiceFactory"/>
    public class ModelService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelService"/> class.
        /// </summary>
        /// <seealso cref="ServiceFactory"/>
        internal ModelService()
        {
        }

        /// <summary>
        /// Retrieves all the managed controller models.
        /// </summary>
        /// <returns>The models that can be managed by the service.</returns>
        public ICollection<Model> RetrieveModels()
        {
            // TODO: convert from logical singleton (called one time) to technical singleton (init one time)
            Model[] models = new Model[]
            { 
                new LazyModel() { Name = "Strategic" },
                new LazyModel() { Name = "Dummy" },
            };

            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button1 });
            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button2 });
            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button3 });
            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button4 });
            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button5 });
            models[0].DefaultMappings.Add(new Mapping() { Button = SwscButton.Button6 });

            return models;
        }

        /// <summary>
        /// Saves a specified profile.
        /// </summary>
        /// <param name="profile">The profile to be saved.</param>
        /// <remarks>
        /// If the property <see cref="Profile.FilePath"/> is <c>null</c>, the profile is saved
        /// as a new profile and the property is updated.
        /// </remarks>
        public void SaveProfile(Profile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }
            else if (profile.Model == null)
            {
                throw new ArgumentException("The profile should be associated to a model", "profile");
            }

            if (string.IsNullOrEmpty(profile.FilePath))
            {
                string name = RemoveChars(profile.Name, Path.GetInvalidFileNameChars());
                profile.FilePath = Path.Combine(Environment.CurrentDirectory, profile.Model.Name, name + ".profile");
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Profile));
            using (Stream stream = File.Open(profile.FilePath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(stream, profile);
            }
        }

        /// <summary>
        /// Removes the specified characters from a string.
        /// </summary>
        /// <param name="text">The string to update.</param>
        /// <param name="characters">Th characters to remove.</param>
        /// <returns>The updated string.</returns>
        private string RemoveChars(string text, char[] characters)
        {
            StringBuilder builder = new StringBuilder();
            int prev = 0;
            int index = text.IndexOfAny(characters);

            while (index != -1)
            {
                builder.Append(text, prev, index - prev);
                prev = index;
                index = text.IndexOfAny(characters, index + 1);
            }

            builder.Append(text, prev, text.Length - prev);

            return builder.ToString();
        }

        /// <summary>
        /// Loads the profiles associated with a specific model.
        /// </summary>
        /// <param name="model">The model for which the profiles should be extracted.</param>
        /// <returns>The profiles associated with the <paramref name="model"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="model"/> parameter is <c>null</c>.
        /// </exception>
        internal static ICollection<Profile> RetrieveProfiles(Model model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            ICollection<Profile> result = new LinkedList<Profile>();

            // TODO: extract the name from the file content.
            string directory = Path.Combine(Environment.CurrentDirectory, model.Name);
            if (!Directory.Exists(directory))
            {
                return result;
            }

            var files = Directory.GetFiles(directory)
                .Where(f => string.Equals(Path.GetExtension(f), ".profile", StringComparison.OrdinalIgnoreCase));

            foreach (string file in files)
            {
                result.Add(new LazyProfile(model) { Name = Path.GetFileNameWithoutExtension(file), FilePath = file });
            }

            return result;
        }

        /// <summary>
        /// Loads the mappings associated with a specific profile.
        /// </summary>
        /// <param name="profile">The profile for which the mappings should be extracted.</param>
        /// <returns>The mappings associated with the <paramref name="profile"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// The <paramref name="profile"/> parameter is <c>null</c>.
        /// </exception>
        internal static ICollection<Mapping> RetrieveMappings(Profile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            string file = Path.Combine(Environment.CurrentDirectory, profile.Model.Name, profile.Name + ".profile");
            XmlSerializer serializer = new XmlSerializer(typeof(Profile));

            ICollection<Mapping> defaultMappings = profile.Model.DefaultMappings;
            ICollection<Mapping> extractedMappings;
            try
            {
                using (Stream stream = File.OpenRead(file))
                {
                    Profile extracted = (Profile)serializer.Deserialize(stream);
                    extractedMappings = extracted.Mappings;
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new ArgumentException("Not a valid profile", "profile", exception);
            }

            return extractedMappings.Union(defaultMappings, MappingComparer.Instance).ToArray();
        }
    }
}
