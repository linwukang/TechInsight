using StackExchange.Redis;
using System.Collections;
using Utils.Interface;
using Utils.Redis.Implementation;

namespace Utils.Redis.Values.Implementation;

public class RedisSortedSetValues<TKey, TValue> : IRedisSortedSetValues<TKey, TValue>
{
    public IDatabase Database { get; private set; }
    public string FullKeyString { get; private set; }
    public RedisDictionary<TKey, TValue> RedisDictionary { get; private set; }
    public IScorer<TValue> Scorer { get; private set; }

    public RedisSortedSetValues(RedisDictionary<TKey, TValue> redisDictionary, TKey key, IScorer<TValue> scorer)
    {
        RedisDictionary = redisDictionary;
        Database = redisDictionary.Database;
        FullKeyString = redisDictionary.FullKeyPrefixAndSeparator + redisDictionary.KeyToString(key);
        Scorer = scorer;
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        var values = Database.SortedSetRangeByRank(FullKeyString);
        return values
            .Select(v => (string) v)
            .Select(v => RedisDictionary.ValueSerializer.Deserialize(RedisDictionary.Encoding.GetBytes(v)))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ICollection<TValue>.Add(TValue item)
    {
        Database.SortedSetAdd(
            FullKeyString, 
            RedisDictionary.ValueToString(item), 
            Scorer.Score(item));
    }

    public void ExceptWith(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public void IntersectWith(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool IsProperSubsetOf(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool IsProperSupersetOf(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSubsetOf(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool IsSupersetOf(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool Overlaps(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public bool SetEquals(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public void SymmetricExceptWith(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    public void UnionWith(IEnumerable<TValue> other)
    {
        throw new NotImplementedException();
    }

    bool ISet<TValue>.Add(TValue item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(TValue item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(TValue[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(TValue item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
}