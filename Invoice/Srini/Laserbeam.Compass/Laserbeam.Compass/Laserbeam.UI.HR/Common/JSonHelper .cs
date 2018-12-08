using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
public class JSonHelper
{
    public string ConvertObjectToJSon<T>(T obj)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream();
        ser.WriteObject(ms, obj);
        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        ms.Close();
        return jsonString;
    }

    public T ConvertJSonToObject<T>(string jsonString)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
        MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        T obj = (T)serializer.ReadObject(ms);
        return obj;
    }


}
