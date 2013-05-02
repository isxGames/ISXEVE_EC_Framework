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

namespace EveComFramework.Core
{
    public class Settings
    {
        public static string CharName { get; set; }
        public bool CharBased { get; private set; }
        private FileSystemWatcher watcher;
        
        static Settings()
        {
            CharName = "Default";
        }

        public Settings(bool CharBased = true)
        {
            this.CharBased = CharBased;
            this.Load();
            if(!Directory.Exists(GetConfigFilePath()))
            {
                Directory.CreateDirectory(GetConfigFilePath());
            }
            watcher = new FileSystemWatcher(GetConfigFilePath(), GetConfigFileName());
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
 	        this.Load();
        }

        private static string GetConfigFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\configs";
        }

        private string GetConfigFileName()
        {
            return (CharBased ? CharName + "." : "") + this.GetType().Name + ".xml";
        }

        public void Save()
        {
            XDocument settingsDoc = new XDocument();
            XElement settingRoot = new XElement("Settings");
            TypeConverter stringConverter = TypeDescriptor.GetConverter(typeof(string));
            settingsDoc.Add(settingRoot);
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
                settingRoot.Add(fieldElement);
            }
            settingsDoc.Save(GetConfigFilePath() + "\\" + GetConfigFileName());
        }

        public void Load()
        {
            if (File.Exists(GetConfigFilePath() + "\\" + GetConfigFileName()))
            {
                XDocument settingsDoc = XDocument.Load(GetConfigFilePath() + "\\" + CharName + "." + this.GetType().Name + ".xml");
                XElement settingRoot = settingsDoc.Root;
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
    }
}
