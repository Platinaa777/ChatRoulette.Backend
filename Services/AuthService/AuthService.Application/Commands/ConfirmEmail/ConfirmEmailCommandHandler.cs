using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<string>>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return Result.Failure<string>(UserError.UserNotFound);
        
        user.SubmitEmail();
        var result = await _userRepository.UpdateUserAsync(user);

        if (!result)
            return Result.Failure<string>(UserError.CantUpdateUser);
        
        return user.Id;
    }
}