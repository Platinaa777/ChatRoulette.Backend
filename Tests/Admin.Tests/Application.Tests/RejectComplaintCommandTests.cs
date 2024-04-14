using AdminService.Application.Commands.RejectComplaint;
using AdminService.Domain.Errors;
using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class RejectComplaintCommandTests
{
    [Fact]
    public async Task HandleRejectComplaint_WhenComplaintDoesNotExist_ShouldReturnNotFoundError()
    {
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync((Complaint?)null);

        var command = new RejectComplaintCommand() { ComplaintId = Guid.NewGuid().ToString() };
        var result = await new RejectComplaintCommandHandler(
                mockComplaintRepository.Object, mockUnitOfWork.Object)
                .Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(ComplaintError.ComplaintNotFound, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async Task HandleRejectComplaint_WhenComplaintAlreadyHandled_ShouldReturnAlreadyHandledResult()
    {
        var complaint = Complaint.Create(Guid.NewGuid().ToString(), "content", "test@mail.ru", "test2@mail.ru",
            "VoiceAbuse", true).Value;
        
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync(complaint);

        var command = new RejectComplaintCommand() { ComplaintId = Guid.NewGuid().ToString() };
        var result = await new RejectComplaintCommandHandler(
                mockComplaintRepository.Object, mockUnitOfWork.Object)
            .Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(ComplaintError.AlreadyHandled, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async Task HandleRejectComplaint_WhenComplaintIsNotHandled_ShouldReturnSuccess()
    {
        var complaint = Complaint.Create(Guid.NewGuid().ToString(), "content", "test@mail.ru", "test2@mail.ru",
            "VoiceAbuse", false).Value;
        
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync(complaint);
        mockComplaintRepository.Setup(x => x.UpdateComplaint(It.IsAny<Complaint>()))
            .ReturnsAsync(true);

        var command = new RejectComplaintCommand() { ComplaintId = Guid.NewGuid().ToString() };
        var result = await new RejectComplaintCommandHandler(
                mockComplaintRepository.Object, mockUnitOfWork.Object)
            .Handle(command, default);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}