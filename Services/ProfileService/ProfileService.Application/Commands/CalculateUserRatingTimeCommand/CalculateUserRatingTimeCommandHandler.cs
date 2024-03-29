using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.CalculateUserRatingTimeCommand;

public class CalculateUserRatingTimeCommandHandler
    : IRequestHandler<CalculateUserRatingTimeCommand, Result> 
{
    private readonly IUserProfileRepository _userProfileRepository;

    public CalculateUserRatingTimeCommandHandler(
        IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task<Result> Handle(CalculateUserRatingTimeCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userProfileRepository.FindUserByEmailAsync(request.Email);
        if (existingUser is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        var userWithIncreaseRating = existingUser.IncreaseRating(request.DurationMinites);
        if (userWithIncreaseRating.IsFailure)
            return Result.Failure(userWithIncreaseRating.Error);

        var result = await _userProfileRepository.UpdateUserAsync(existingUser);
        if (!result)
            return Result.Failure(UserProfileErrors.CantUpdateUser);
        
        return Result.Success();
    }
}