using System.Text;

namespace Utils.Interface.Implementation;

public class StringSerializer : ISerializer<string>
{
    public Encoding Encoding { get; private set; }

    public StringSerializer(Encoding encoding)
    {
        Encoding = encoding;
    }
    public StringSerializer() : this(Encoding.UTF8)
    {
    }

    public byte[] Serialize(string obj)
    {
        return Encoding.GetBytes(obj);
    }

    public string Deserialize(byte[] data)
    {
        return Encoding.GetString(data);
    }
}