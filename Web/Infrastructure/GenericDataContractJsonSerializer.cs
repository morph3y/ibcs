using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Web.Infrastructure
{
    public class GenericDataContractJsonSerializer<T>
    {
        public string Serialize(T @object)
        {
            var stream = new MemoryStream();
            new DataContractJsonSerializer(typeof(T)).WriteObject(stream, @object);
            var jsonString = Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();
            return jsonString;
        }

        public T Deserialize(string json)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
        }
    }
}