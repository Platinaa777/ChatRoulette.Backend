using AuthService.Application.Queries;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;
using MediatR;

namespace AuthService.Application.Handlers;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserInformationResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserInformationResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return new UserInformationResponse();

        return new UserInformationResponse()
        {
            UserName = user.UserName.Value,
            Email = user.Email.Value,
            IsSubmitted = user.IsSubmittedEmail
        };
    }
}