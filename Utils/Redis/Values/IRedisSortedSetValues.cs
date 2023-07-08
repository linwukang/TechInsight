namespace Utils.Redis.Values;

public interface IRedisSortedSetValues<TKey, TValue> : IRedisValues<TKey, TValue>, ISet<TValue>
{
    
}