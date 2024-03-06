using System.Reflection;

namespace Chat.Domain.SeedWork;

public abstract class Enumeration : IComparable
{
    private Enumeration() { }
    
    public string Name { get; private set; }

    public int Id { get; private set; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var enumerationType = typeof(T);

        var fields = enumerationType.GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(field => enumerationType.IsAssignableFrom(field.FieldType))
            .Select(field => (T)field.GetValue(default)!);

        return fields;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
}