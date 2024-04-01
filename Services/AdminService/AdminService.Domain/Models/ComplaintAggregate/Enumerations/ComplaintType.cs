using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate.Enumerations;

public class ComplaintType : Enumeration
{
    public static ComplaintType VoiceAbuse = new ComplaintType(1, nameof(VoiceAbuse));
    public static ComplaintType VideoAbuse = new ComplaintType(2, nameof(VideoAbuse));
    public static ComplaintType TextAbuse = new ComplaintType(3, nameof(TextAbuse));
    public static ComplaintType InappropriateBehavior = new ComplaintType(4, nameof(InappropriateBehavior));

    protected ComplaintType(int id, string name) : base(id, name)
    {
    }

    public static ComplaintType? FromName(string? name)
    {
        var collection = GetAll<ComplaintType>();
        foreach (var type in collection)
        {
            if (type.Name == name)
                return type;
        }

        return null;
    }
    
    public static ComplaintType? FromValue(int id)
    {
        var collection = GetAll<ComplaintType>();
        foreach (var type in collection)
        {
            if (type.Id == id)
                return type;
        }

        return null;
    }
}