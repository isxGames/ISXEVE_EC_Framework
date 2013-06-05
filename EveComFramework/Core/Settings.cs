using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using EveCom;
using LavishScriptAPI;

namespace EveComFramework.Core
{
    public class Settings
    {
        private FileSystemWatcher watcher;
        public string ProfilePath { get; set; }
        public string ConfigDirectory { get; set; }
        
        
        static Settings()
        {
        }

        public Settings()
        {
            ConfigDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\configs\\";
            ProfilePath = ConfigDirectory + Config.Instance.DefaultProfile + ".xml";
            this.Load();

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            watcher = new FileSystemWatcher(ConfigDirectory, Config.Instance.DefaultProfile + ".xml");
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
        }

        public Settings(string profilename)
        {
            ConfigDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\configs\\global\\";
            ProfilePath = ConfigDirectory + profilename + ".xml";
            this.Load();

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
            }

            watcher = new FileSystemWatcher(ConfigDirectory, profilename + ".xml");
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
        }

        public string[] Profiles()
        {
            string dir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\configs\\";
            return Directory.GetFiles(dir).Select(Path.GetFileNameWithoutExtension).ToArray();
        }

        #region Events

        public delegate void NewAlert();
        public event NewAlert Updated;
        public void TriggerUpdate()
        {
            if (Updated != null)
            {
                Updated();
            }
        }

        #endregion

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
 	        this.Load();
            TriggerUpdate();
        }

        public void Save()
        {
            Config.Instance.InUse = true;
            try
            {
                XDocument settingsDoc;
                XElement trueRoot;
                XElement settingRoot;
                if (File.Exists(ProfilePath))
                {
                    settingsDoc = XDocument.Load(ProfilePath);
                    trueRoot = settingsDoc.Element("Settings");
                    if (trueRoot.Element(this.GetType().Name) != null)
                    {
                        settingRoot = trueRoot.Element(this.GetType().Name);
                    }
                    else
                    {
                        settingRoot = new XElement(this.GetType().Name);
                        trueRoot.Add(settingRoot);
                    }
                }
                else
                {
                    settingsDoc = new XDocument();
                    trueRoot = new XElement("Settings");
                    settingRoot = new XElement(this.GetType().Name);
                    trueRoot.Add(settingRoot);
                    settingsDoc.Add(trueRoot);
                }

                TypeConverter stringConverter = TypeDescriptor.GetConverter(typeof(string));
                foreach (FieldInfo field in this.GetType().GetFields())
                {
                    XElement fieldElement = null;
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(field.FieldType);
                    if (typeConverter.CanConvertFrom(typeof(string)))
                    {
                        fieldElement = new XElement(field.Name, field.GetValue(this).ToString());
                    }
                    else if (stringConverter.CanConvertTo(field.FieldType))
                    {
                        fieldElement = new XElement(field.Name, field.GetValue(this).ToString());
                    }
                    else
                    {
                        fieldElement = new XElement(field.Name, Serialize(field.GetValue(this)));
                    }
                    if (settingRoot.Element(fieldElement.Name) != null)
                    {
                        settingRoot.Element(fieldElement.Name).ReplaceWith(fieldElement);
                    }
                    else
                    {
                        settingRoot.Add(fieldElement);
                    }
                }
                settingsDoc.Save(ProfilePath);
            }
            catch
            {
            }
            Config.Instance.InUse = false;
        }

        public void Load()
        {
            if (Config.Instance.InUse) return;
            if (File.Exists(ProfilePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(new FileStream(ProfilePath, FileMode.Open, FileAccess.Read, FileShare.Read)))
                    {
                        XDocument settingsDoc = XDocument.Load(reader);
                        XElement trueRoot = settingsDoc.Root;
                        XElement settingRoot = trueRoot.Element(this.GetType().Name);
                        TypeConverter stringConverter = TypeDescriptor.GetConverter(typeof(string));
                        foreach (FieldInfo field in this.GetType().GetFields())
                        {
                            if (settingRoot.Element(field.Name) != null)
                            {
                                TypeConverter typeConverter = TypeDescriptor.GetConverter(field.FieldType);
                                if (typeConverter.CanConvertFrom(typeof(string)))
                                {
                                    field.SetValue(this, typeConverter.ConvertFrom(settingRoot.Element(field.Name).Value));
                                }
                                else if (stringConverter.CanConvertTo(field.FieldType))
                                {
                                    field.SetValue(this, typeConverter.ConvertTo(settingRoot.Element(field.Name).Value, field.FieldType));
                                }
                                else
                                {
                                    field.SetValue(this, Deserialize(settingRoot.Element(field.Name).Elements().First(), field.FieldType));
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Save();
                }
            }
        }

        static XElement Serialize(object source)
        {
            try
            {
                var serializer = GetSerializerFor(source.GetType());
                var xdoc = new XDocument();
                using (var writer = xdoc.CreateWriter())
                {
                    serializer.Serialize(writer, source, new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") }));
                }
                return (xdoc.Document != null) ? xdoc.Document.Root : new XElement("Error", "Document Missing");
            }
            catch (Exception x)
            {
                return new XElement("Error", x.ToString());
            }
        }

        static object Deserialize(XElement source, Type from)
        {
            try
            {
                var serializer = GetSerializerFor(from);

                return serializer.Deserialize(source.CreateReader());
            }
            catch //(Exception x)
            {
                return null;
            }
        }

        static Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();

        static XmlSerializer GetSerializerFor(Type typeOfT)
        {
            if (!serializers.ContainsKey(typeOfT))
            {
                var newSerializer = new XmlSerializer(typeOfT);
                serializers.Add(typeOfT, newSerializer);
            }
            return serializers[typeOfT];
        }

        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
        {
            #region IXmlSerializable Members
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

                bool wasEmpty = reader.IsEmptyElement;
                reader.Read();

                if (wasEmpty)
                    return;

                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("item");

                    reader.ReadStartElement("key");
                    TKey key = (TKey)keySerializer.Deserialize(reader);
                    reader.ReadEndElement();

                    reader.ReadStartElement("value");
                    TValue value = (TValue)valueSerializer.Deserialize(reader);
                    reader.ReadEndElement();

                    this.Add(key, value);

                    reader.ReadEndElement();
                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

                foreach (TKey key in this.Keys)
                {
                    writer.WriteStartElement("item");

                    writer.WriteStartElement("key");
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();

                    writer.WriteStartElement("value");
                    TValue value = this[key];
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
            #endregion
        }
    }

}
