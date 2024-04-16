using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.ChangeUserNameProfile;

public class ChangeUserNameProfileCommandHandler
    : IRequestHandler<ChangeUserNameProfileCommand, Result>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeUserNameProfileCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangeUserNameProfileCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);

        var existingProfile = await _userProfileRepository.FindUserByEmailAsync(request.Email);
        if (existingProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        var emailResult = existingProfile.UpdateUserName(request.UserName);
        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        var isProfileUpdated = await _userProfileRepository.UpdateUserAsync(existingProfile);
        if (!isProfileUpdated)
            return Result.Failure(UserProfileErrors.CantUpdateUser);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}