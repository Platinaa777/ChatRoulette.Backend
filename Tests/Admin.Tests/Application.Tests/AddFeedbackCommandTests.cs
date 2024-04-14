using AdminService.Application.Commands.AddFeedback;
using AdminService.Domain.Errors;
using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class AddFeedbackCommandTests
{
    [Fact]
    public async Task HandleAddFeedback_WhenFeedbackSmallSize_ShouldReturnNotEnoughLengthError()
    {
        var mockFeedbackRepository = new Mock<IFeedbackRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var command = new AddFeedbackCommand() { Content = "content", EmailFrom = "test@mail.ru" };

        var result = await new AddFeedbackCommandHandler(mockFeedbackRepository.Object, mockUnitOfWork.Object)
            .Handle(command, default);

        Assert.True(result.IsFailure);
        Assert.Equal(FeedbackError.NotEnoughLength, result.Error);
        
        mockFeedbackRepository.Verify(x => x.AddFeedback(It.IsAny<Feedback>()), Times.Never);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async Task HandleAddFeedback_WhenFeedbackValid_ShouldReturnSuccess()
    {
        var mockFeedbackRepository = new Mock<IFeedbackRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var command = new AddFeedbackCommand() { Content = "content123456789", EmailFrom = "test@mail.ru" };

        var result = await new AddFeedbackCommandHandler(mockFeedbackRepository.Object, mockUnitOfWork.Object)
            .Handle(command, default);

        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        
        mockFeedbackRepository.Verify(x => x.AddFeedback(It.IsAny<Feedback>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}