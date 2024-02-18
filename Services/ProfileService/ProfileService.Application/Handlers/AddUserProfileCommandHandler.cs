using MediatR;
using ProfileService.Application.Commands;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Handlers;

public class AddUserProfileCommandHandler : IRequestHandler<AddUserProfileCommand, bool>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public AddUserProfileCommandHandler(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task<bool> Handle(AddUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = await _userProfileRepository.AddUserAsync(request.ToDomain());

        return result;
    }
}