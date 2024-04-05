using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.HttpModels.Responses;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserInformationResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<UserInformationResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<UserInformationResponse>(emailResult.Error);
        
        var user = await _userRepository.FindUserByEmailAsync(emailResult.Value);

        if (user == null)
            return Result.Failure<UserInformationResponse>(UserError.UserNotFound);
        
        return new UserInformationResponse()
        {
            UserName = user.UserName.Value,
            Email = user.Email.Value,
            IsSubmitted = user.IsSubmittedEmail
        };
    }
}