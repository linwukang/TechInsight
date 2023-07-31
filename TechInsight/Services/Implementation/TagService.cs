using StackExchange.Redis;

namespace TechInsight.Services.Implementation;

public class TagService : ITagService
{
    public TagService(IDatabase redis)
    {
        Redis = redis;
    }

    public readonly IDatabase Redis;

    public const string TagPrefix = "TagService:Tag:";

    public bool IsTagExists(string tag)
    {
        return 
            tag.Length != 0 
            && tag.Trim().Length != 0
            && Redis.KeyExists(TagPrefix + tag.Trim());
    }

    public long TagCount(string tag)
    {
        return
            tag.Length == 0 || tag.Trim().Length == 0 
                ? 0 
                : Redis.SetLength(TagPrefix + tag.Trim());
    }

    public bool NewTag(string tag)
    {
        if (tag.Length == 0 || tag.Trim().Length == 0 || IsTagExists(tag))
        {
            return false;
        }

        return Redis.SetAdd(TagPrefix + tag.Trim(), new RedisValue());
    }

    public bool DeleteTag(string tag)
    {
        if (tag.Length == 0 || tag.Trim().Length == 0 || !IsTagExists(tag))
        {
            return false;
        }

        return Redis.KeyDelete(TagPrefix + tag.Trim());
    }

    public bool AddArticleToTag(string tag, int articleId)
    {
        if (tag.Length == 0 || tag.Trim().Length == 0 || !IsTagExists(tag))
        {
            return false;
        }

        return Redis.SetAdd(TagPrefix + tag.Trim(), new RedisValue(articleId.ToString()));
    }

    public bool RemoveArticleFromTag(string tag, int articleId)
    {
        if (tag.Length == 0 || tag.Trim().Length == 0 || !IsTagExists(tag))
        {
            return false;
        }

        return Redis.SetRemove(TagPrefix + tag.Trim(), new RedisValue(articleId.ToString()));
    }

    public bool IsArticleInTag(string tag, int articleId)
    {
        if (tag.Length == 0 || tag.Trim().Length == 0 || !IsTagExists(tag))
        {
            return false;
        }

        return Redis.SetContains(TagPrefix + tag.Trim(), new RedisValue(articleId.ToString()));
    }
}