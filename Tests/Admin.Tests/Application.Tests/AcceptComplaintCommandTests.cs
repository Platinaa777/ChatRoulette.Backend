using AdminService.Application.Commands.AcceptComplaint;
using AdminService.Domain.Errors;
using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Events;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class AcceptComplaintCommandTests
{
    [Fact]
    public async Task HandleAcceptComplaint_WhenComplaintIdNotFound_ShouldReturnComplaintNotFoundError()
    {
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync((Complaint?)null);

        var handler = new AcceptComplaintCommandHandler(mockComplaintRepository.Object, mockUnitOfWork.Object);

        var command = new AcceptComplaintCommand() { ComplaintId = Guid.NewGuid().ToString(), MinutesDuration = 15 };

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(ComplaintError.ComplaintNotFound, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async Task HandleAcceptComplaint_WhenComplaintAlreadyHandle_ShouldReturnAlreadyHandledError()
    {
        Complaint complaint = Complaint.Create(Guid.NewGuid().ToString(), "content", "test@mail.ru", "test2@mail.ru",
            "VoiceAbuse", true).Value;
        
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync(complaint);

        var handler = new AcceptComplaintCommandHandler(mockComplaintRepository.Object, mockUnitOfWork.Object);

        var command = new AcceptComplaintCommand() { ComplaintId = Guid.NewGuid().ToString(), MinutesDuration = 15 };

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(ComplaintError.AlreadyHandled, result.Error);
        Assert.Empty(complaint.GetDomainEvents());
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async Task HandleAcceptComplaint_WhenComplaintIsNotHandle_ShouldReturnSuccess()
    {
        Complaint complaint = Complaint.Create(Guid.NewGuid().ToString(), "content", "test@mail.ru", "test2@mail.ru",
            "VoiceAbuse", false).Value;
        
        var mockComplaintRepository = new Mock<IComplaintRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockComplaintRepository.Setup(x => x.UpdateComplaint(It.IsAny<Complaint>()))
            .ReturnsAsync(true);
        
        mockComplaintRepository.Setup(x => x.FindComplaintById(It.IsAny<Id>()))
            .ReturnsAsync(complaint);

        var handler = new AcceptComplaintCommandHandler(mockComplaintRepository.Object, mockUnitOfWork.Object);

        var command = new AcceptComplaintCommand() { ComplaintId = Guid.NewGuid().ToString(), MinutesDuration = 15 };

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        Assert.NotEmpty(complaint.GetDomainEvents());
        
        Assert.IsType<ApprovedComplaintDomainEvent>(complaint.GetDomainEvents()[0]);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}