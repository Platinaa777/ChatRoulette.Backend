using AdminService.Domain.Errors;
using AdminService.Domain.Models.FeedbackAggregate;

namespace Domain.Tests;

public class FeedbackTests
{
    [Fact]
    public void HandleInitialization_WhenEmailInvalid_ShouldBeEmailError()
    {
        var feedback = Feedback.Create(
            Guid.NewGuid().ToString(), "wrong-email", "content");
        
        Assert.True(feedback.IsFailure);
        Assert.Equal(ComplaintError.InvalidEmail, feedback.Error);
    }
    
    [Fact]
    public void HandleInitialization_WhenEmptyContent_ShouldBeContentError()
    {
        var feedback = Feedback.Create(
            Guid.NewGuid().ToString(), "test@mail.ru", "");
        
        Assert.True(feedback.IsFailure);
        Assert.Equal(FeedbackError.NotEnoughLength, feedback.Error);
    }
}