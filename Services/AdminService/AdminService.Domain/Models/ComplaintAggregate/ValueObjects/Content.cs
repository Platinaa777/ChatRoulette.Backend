using AdminService.Domain.Errors;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate.ValueObjects;

public class Content : ValueObject
{
    public string Value { get; set; }

    public static Result<Content> Create(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return Result.Failure<Content>(ComplaintError.EmptyContentMessage);

        return new Content(content);
    }
    
    public Content(string content)
    {
        Value = content;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}