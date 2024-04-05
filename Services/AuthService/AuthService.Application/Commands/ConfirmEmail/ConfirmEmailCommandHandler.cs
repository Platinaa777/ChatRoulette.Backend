using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmEmailCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<string>(emailResult.Error);
        
        var user = await _userRepository.FindUserByEmailAsync(emailResult.Value);

        if (user == null)
            return Result.Failure<string>(UserError.UserNotFound);
        
        user.SubmitEmail();
        var result = await _userRepository.UpdateUserAsync(user);

        if (!result)
            return Result.Failure<string>(UserError.CantUpdateUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return user.Id.Value;
    }
}