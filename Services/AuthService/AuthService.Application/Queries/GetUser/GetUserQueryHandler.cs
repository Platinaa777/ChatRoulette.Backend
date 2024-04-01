using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Repos;
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
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

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