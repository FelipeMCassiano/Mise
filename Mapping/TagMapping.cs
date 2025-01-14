using Mise.Entities;

namespace Mise.Mapping;

public static class TagMapping
{

    public static TagDetailsDto ToDetailsDto(this Tag tag)
    {
        return new TagDetailsDto(tag.Id, tag.Name);
    }

}