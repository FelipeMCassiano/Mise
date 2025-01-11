using Microsoft.EntityFrameworkCore;
using Mise.Database;
using Mise.Entities;
using Mise.Errors;
using OneOf;

namespace Mise.Services;

public class TagsService
{
    private readonly AppDbContext _dbContext;

    public TagsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    public async Task<List<Tag>> GetOrCreateMultipleTagByNameAsync(List<string> names)
    {
        var existingTags = await _dbContext.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

        var existingTagNames = existingTags.Select(t => t.Name).ToHashSet();
        var missingTagNames = names.Except(existingTagNames).ToList();

        if (missingTagNames.Any())
        {
            var newTags = missingTagNames.Select(name => new Tag { Name = name, Id = Guid.NewGuid() }).ToList();
            await _dbContext.Tags.AddRangeAsync(newTags);
            await _dbContext.SaveChangesAsync();
            existingTags.AddRange(newTags);
        }

        return existingTags;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _dbContext.Tags.Select(t => t).ToListAsync();
    }


}