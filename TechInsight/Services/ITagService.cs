namespace TechInsight.Services;

public interface ITagService
{
    /// <summary>
    /// 判断 tag 是否存在
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <returns>存在返回 true，否则返回 false</returns>
    bool IsTagExists(string tag);

    /// <summary>
    /// tag 中文章的数量
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <returns>文章的数量，如果 tag 不存在则返回 0</returns>
    long TagCount(string tag);

    /// <summary>
    /// 新建一个 tag
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <returns>新建成功返回 true，否则返回 false</returns>
    bool NewTag(string tag);

    /// <summary>
    /// 删除一个 tag
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <returns>删除成功返回 true， 否则返回 false</returns>
    bool DeleteTag(string tag);

    /// <summary>
    /// 将文章添加到 tag 中
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <param name="articleId">文章 id</param>
    /// <returns>添加成功返回 true， 否则返回 false</returns>
    bool AddArticleToTag(string tag, int articleId);

    /// <summary>
    /// 将文章从 tag 中移除
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <param name="articleId">文章 id</param>
    /// <returns>移除成功返回 true， 否则返回 false</returns>
    bool RemoveArticleFromTag(string tag, int articleId);

    /// <summary>
    /// 判断文章是否在 tag 中
    /// </summary>
    /// <param name="tag">tag 名</param>
    /// <param name="articleId">文章 id</param>
    /// <returns>在 tag 中返回 true， 否则返回 false</returns>
    bool IsArticleInTag(string tag, int articleId);
}