using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace TechInsightDb.Models.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasQueryFilter(article => article.IsDeleted == null);
        builder.Property(article => article.Tags)
            .HasConversion(
                tags => JsonConvert.SerializeObject(tags),
                tagsString => JsonConvert.DeserializeObject<List<string>>(tagsString) ?? new List<string>());
    }
}