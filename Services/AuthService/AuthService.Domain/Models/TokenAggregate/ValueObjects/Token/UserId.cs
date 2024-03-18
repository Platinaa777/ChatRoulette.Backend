using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;

public class UserId : ValueObject
{
    public string Value { get; set; }

    private UserId(string id)
    {
        Value = id;
    }

    public static UserId CreateId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("id can not be empty");
        return new UserId(id);
    }
        
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}