using System.IO;
using System.Xml.Serialization;

namespace Dal
{
    /// <summary>
    /// XML序列化提供类。
    /// </summary>
    public class XmlSerializerProvider 
    {
        /// <summary>
        ///  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Serialize<T>(string filePath, T entity)
        {
            if (string.IsNullOrEmpty(filePath) || entity == null)
            {
                return false;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), typeof(T).Name);
                Stream stream = new FileStream(filePath, FileMode.Create);
                xmlSerializer.Serialize(stream, entity);
                stream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public T Deserialize<T>(string filePath) where T : class
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), typeof(T).Name);
                Stream stream = new FileStream(filePath, FileMode.Open);
                object obj = xmlSerializer.Deserialize(stream);
                stream.Close();

                return obj as T;
            }
            catch
            {
                return null;
            }
        }
    }
}
