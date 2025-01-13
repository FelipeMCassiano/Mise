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

    public async Task<OneOf<List<Tag>, NotFoundMultipleTagsError>> GetMultipleTagsByNameAsync(List<string> names)
    {
        var existingTags = await _dbContext.Tags.Where(t => names.Contains(t.Name)).ToListAsync();

        var existingTagNames = existingTags.Select(t => t.Name).ToHashSet();
        var missingTagNames = names.Except(existingTagNames).ToList();

        if (missingTagNames.Any())
        {
            var err = new NotFoundMultipleTagsError(missingTagNames);

            return err;
        }

        return existingTags;
    }

    public async Task<List<Tag>> GetAllTagsAsync()
    {
        return await _dbContext.Tags.Select(t => t).ToListAsync();
    }

    public async Task<OneOf<Tag, NotFoundTagError>> GetTagAsync(Guid id)
    {

        var tag = await _dbContext.Tags.FindAsync(id);
        if (tag is null)
        {
            return new NotFoundTagError();
        }

        return tag;

    }


}