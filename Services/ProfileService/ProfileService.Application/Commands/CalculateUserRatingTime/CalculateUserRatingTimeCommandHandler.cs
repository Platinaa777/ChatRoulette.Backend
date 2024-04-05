using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.CalculateUserRatingTime;

public class CalculateUserRatingTimeCommandHandler
    : IRequestHandler<CalculateUserRatingTime.CalculateUserRatingTimeCommand, Result> 
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CalculateUserRatingTimeCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(CalculateUserRatingTime.CalculateUserRatingTimeCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var existingUser = await _userProfileRepository.FindUserByEmailAsync(request.Email);
        if (existingUser is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        var userWithIncreaseRating = existingUser.IncreaseRating(request.DurationMinites);
        if (userWithIncreaseRating.IsFailure)
            return Result.Failure(userWithIncreaseRating.Error);

        var result = await _userProfileRepository.UpdateUserAsync(existingUser);
        if (!result)
            return Result.Failure(UserProfileErrors.CantUpdateUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);        
        return Result.Success();
    }
}