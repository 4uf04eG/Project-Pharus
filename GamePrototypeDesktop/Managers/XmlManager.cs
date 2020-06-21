using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GamePrototypeDesktop.Managers
{
    public static class XmlManager<TOut>
    {
        public static TOut Load(string xmlPath, Type rootType)
        {
            var absolutePath = StateManager.Instance.ContentManager.RootDirectory + xmlPath;
            
            using (var reader = new StreamReader(absolutePath))
                return (TOut) new XmlSerializer(rootType).Deserialize(reader);
        }
        
        public static void Save(string xmlPath, TOut item, Type rootType)
        {
            var absolutePath = StateManager.Instance.ContentManager.RootDirectory + xmlPath;
            using (var writer = new StreamWriter(absolutePath))
                new XmlSerializer(rootType).Serialize(writer, item);
        }
    }
}