using AdminService.Application.Commands.AddComplaint;
using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class AddComplaintCommandTests
{
    [Fact]
    public async Task HandleAddComplaint_WhenComplaintValid_ShouldReturnSuccess()
    {
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var command = new AddComplaintCommand(Guid.NewGuid().ToString(), "content", "test@mail.ru", "test2@mail.ru",
            "VoiceAbuse");

        var result =
            await new AddComplaintCommandHandler(mockComplaintRepository.Object, mockUnitOfWork.Object)
                .Handle(command, default);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        
        mockComplaintRepository.Verify(x => x.AddComplaint(It.IsAny<Complaint>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
        
    }
}