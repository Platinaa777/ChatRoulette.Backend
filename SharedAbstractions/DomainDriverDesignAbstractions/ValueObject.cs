namespace DomainDriverDesignAbstractions;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other) =>
        other is not null && ValuesAreEqual(other);

    public override bool Equals(object? obj) =>
        obj is ValueObject other && ValuesAreEqual(other);

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(default(int), HashCode.Combine);

    private bool ValuesAreEqual(ValueObject other) => 
        GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
}