using System.Runtime.Serialization;

namespace Utils.Interface;

public interface ISerializer<T>
{
    byte[] Serialize(T obj);

    T Deserialize(byte[] data);
}