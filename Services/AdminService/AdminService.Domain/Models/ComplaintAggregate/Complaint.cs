using AdminService.Domain.Errors;
using AdminService.Domain.Models.ComplaintAggregate.Enumerations;
using AdminService.Domain.Models.ComplaintAggregate.Events;
using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate;

public class Complaint : AggregateRoot<Id>
{
    private Complaint(
        Id id,
        Content content,
        Email senderEmail,
        Email violatorEmail,
        ComplaintType complaintType,
        bool isHandled = false)
    {
        Id = id;
        Content = content;
        SenderEmail = senderEmail;
        ViolatorEmail = violatorEmail;
        ComplaintType = complaintType;
        IsHandled = isHandled;
    }

    public Id Id { get; private set; }
    public Content Content { get; private set; }
    public Email SenderEmail { get; private set; }
    public Email ViolatorEmail { get; private set; }
    public ComplaintType ComplaintType { get; private set; }
    public bool IsHandled { get; private set; }

    public void SetAccepted(int minutesToBan)
    {
        RaiseDomainEvent(new ApprovedComplaintDomainEvent(
            Id.Value.ToString(),
            SenderEmail.Value,
            ViolatorEmail.Value,
            minutesToBan,
            ComplaintType.Name));
        
        IsHandled = true;
    }

    public void SetRejected()
    {
        IsHandled = true;
    }

    public bool IsHandledByAdmin()
    {
        return IsHandled;
    }
    

    public static Result<Complaint> Create(
        string id,
        string content,
        string senderEmail,
        string violatorEmail,
        string complaintType,
        bool isHandled = false)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<Complaint>(idResult.Error);

        var contentResult = Content.Create(content);
        if (contentResult.IsFailure)
            return Result.Failure<Complaint>(contentResult.Error);

        var emailSenderResult = Email.Create(senderEmail);
        if (emailSenderResult.IsFailure)
            return Result.Failure<Complaint>(emailSenderResult.Error);

        var possibleIntruderResult = Email.Create(violatorEmail);
        if (possibleIntruderResult.IsFailure)
            return Result.Failure<Complaint>(possibleIntruderResult.Error);

        var complaintTypeResult = ComplaintType.FromName(complaintType);
        if (complaintTypeResult is null)
            return Result.Failure<Complaint>(ComplaintError.InvalidComplaintType);

        return new Complaint(
            idResult.Value,
            contentResult.Value,
            emailSenderResult.Value,
            possibleIntruderResult.Value,
            complaintTypeResult,
            isHandled);
    }
}