using System.Collections;
using System.Text;
using StackExchange.Redis;
using Utils.Interface;

namespace Utils.Redis.Implementation;
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

public class RedisDictionary<TKey, TValue> : IRedisDictionary<TKey, TValue>
{
    public IDatabase Database { get; private set; }

    public ISerializer<TKey> KeySerializer { get; private set; }
    public ISerializer<TValue> ValueSerializer { get; private set; }
    public TKey KeyPrefix { get; private set; }
    public TKey KeySeparator { get; private set; }

    public string FullKeyPrefixAndSeparator { get; private set; }
    public Encoding Encoding { get; private set; }

    public RedisDictionary(string host, int database, ISerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer, TKey keyPrefix, TKey keySeparator)
    {
        var config = ConfigurationOptions.Parse(host);
        var connect = ConnectionMultiplexer.Connect(config);

        Init(connect.GetDatabase(database), keySerializer, valueSerializer, keyPrefix, keySeparator);
    }

    public RedisDictionary(IDatabase database, ISerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer, TKey keyPrefix, TKey keySeparator)
    {
        Init(database, keySerializer, valueSerializer, keyPrefix, keySeparator);
    }

    private void Init(IDatabase database, ISerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer,
        TKey keyPrefix, TKey keySeparator)
    {
        Database = database;
        KeySerializer = keySerializer;
        ValueSerializer = valueSerializer;
        KeyPrefix = keyPrefix;
        KeySeparator = keySeparator;

        Encoding = Encoding.UTF8;
        var keyPrefixBytes = KeySerializer.Serialize(KeyPrefix);
        var keySeparatorBytes = KeySerializer.Serialize(KeySeparator);

        FullKeyPrefixAndSeparator = Encoding.GetString(keyPrefixBytes) + Encoding.GetString(keySeparatorBytes);
    }


    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        this[item.Key] = item.Value;
    }

    public void Clear()
    {
        foreach (var key in Keys)
        {
            Remove(key);
        }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        if (TryGetValue(item.Key, out TValue value))
        {
            return value?.Equals(item.Value) ?? false;
        }

        return false;
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!ContainsKey(item.Key)) return false;

        if (!TryGetValue(item.Key, out TValue value)) return false;

        return Equals(value, item.Value) && Remove(item.Key);
    }

    public int Count
    {
        get
        {
            var redisResult = Database.Execute("KEYS", FullKeyPrefixAndSeparator + "*");
            var keys = (RedisKey[]?)redisResult;
            return keys?.Length ?? 0;
        }
    }

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        Database.StringSet(FullKey(key), ValueToString(value));
    }

    public bool ContainsKey(TKey key)
    {
        var redisResult = Database.Execute("KEYS", FullKey(key));
        var keys = (RedisKey[]?)redisResult;
        return keys?.Length == 1;
    }

    public bool Remove(TKey key)
    {
        return Database.KeyDelete(FullKey(key));
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var redisValue = Database.StringGet(FullKey(key));

        if (!redisValue.HasValue)
        {
#pragma warning disable CS8601 // 引用类型赋值可能为 null。
            value = default;
#pragma warning restore CS8601 // 引用类型赋值可能为 null。
            return false;
        }

        var stringValue = redisValue.ToString();
        var bytes = Encoding.GetBytes(stringValue);
        value = ValueSerializer.Deserialize(bytes);
        return true;
    }

    public TValue this[TKey key]
    {
        get
        {
            var redisValue = Database.StringGet(FullKey(key));
            var stringValue = redisValue.ToString();
            var bytes = Encoding.GetBytes(stringValue);
            return ValueSerializer.Deserialize(bytes);
        }
        set
        {
            var bytes = ValueSerializer.Serialize(value);

            Database.StringSet(FullKey(key), Encoding.GetString(bytes));
        }
    }

    public ICollection<TKey> Keys
    {
        get
        {
            var redisResult = Database.Execute("KEYS", FullKeyPrefixAndSeparator + "*");
            var keys = (RedisKey[]?)redisResult;

            return keys
                ?.ToList()
                ?.Select(fullKey => RawKey(fullKey.ToString()))
                ?.Select(rawKey =>
                {
                    var bytes = Encoding.GetBytes(rawKey);
                    return KeySerializer.Deserialize(bytes);
                })
                ?.ToList()
                   ?? new List<TKey>();
        }
    }

    public ICollection<TValue> Values => throw new NotImplementedException();

    public string KeyToString(TKey key)
    {
        var bytes = KeySerializer.Serialize(key);
        return Encoding.GetString(bytes);
    }

    public string ValueToString(TValue value)
    {
        var bytes = ValueSerializer.Serialize(value);
        return Encoding.GetString(bytes);
    }

    public string FullKey(TKey rawKey)
    {
        return FullKeyPrefixAndSeparator + KeyToString(rawKey);
    }

    public string RawKey(string fullKey)
    {
        return fullKey[FullKeyPrefixAndSeparator.Length..];
    }
}