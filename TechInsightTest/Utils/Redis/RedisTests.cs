using Org.BouncyCastle.Utilities.Collections;
using Utils.Interface.Implementation;
using Utils.Redis;
using Utils.Redis.Implementation;

namespace TechInsightTest.Utils.Redis;

[TestClass]
public class RedisTests
{
    IRedisDictionary<string, string> redis;

    [TestInitialize]
    public void Init()
    {
        redis = new RedisDictionary<string, string>(
            "localhost:6379", 
            0, 
            new StringSerializer(), 
            new StringSerializer(), 
            "Test", 
            ":");
    }

    /// <summary>
    /// 综合测试
    /// </summary>
    [TestMethod]
    public void TestRedisSynthesis()
    {
        Assert.AreEqual(0, redis.Count);

        redis["1"] = "aaa";
        redis["2"] = "bbb";
        redis["3"] = "ccc";
        redis["4"] = "ddd";
        redis["5"] = "eee";

        Assert.AreEqual(5, redis.Count);

        Assert.AreEqual("aaa", redis["1"]);
        Assert.AreEqual("bbb", redis["2"]);
        Assert.AreEqual("ccc", redis["3"]);
        Assert.AreEqual("ddd", redis["4"]);
        Assert.AreEqual("eee", redis["5"]);

        var list = new List<string> { "1", "2", "3", "4", "5" };
        foreach (var redisKey in redis.Keys)
        {
            Assert.IsTrue(list.Contains(redisKey));
        }


        Assert.IsTrue(redis.ContainsKey("1"));
        Assert.IsTrue(redis.ContainsKey("2"));
        Assert.IsTrue(redis.ContainsKey("3"));
        Assert.IsTrue(redis.ContainsKey("4"));
        Assert.IsTrue(redis.ContainsKey("5"));

        Assert.IsTrue(redis.Remove("1"));
        Assert.IsTrue(redis.Remove("5"));

        Assert.IsFalse(redis.ContainsKey("1"));
        Assert.IsFalse(redis.ContainsKey("5"));

        redis.Clear();
        Assert.AreEqual(0, redis.Count);
    }
}