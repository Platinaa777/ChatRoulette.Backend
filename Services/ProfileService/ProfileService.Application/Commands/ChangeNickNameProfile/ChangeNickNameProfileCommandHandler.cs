using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.ChangeNickNameProfile;

public class ChangeNickNameProfileCommandHandler
    : IRequestHandler<ChangeNickNameProfile.ChangeNickNameProfileCommand, Result>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeNickNameProfileCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangeNickNameProfile.ChangeNickNameProfileCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var existingProfile = await _userProfileRepository.FindUserByEmailAsync(request.Email);
        if (existingProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        var emailResult = existingProfile.SetNickName(request.NickName);
        if (emailResult.IsFailure)
            return Result.Failure(emailResult.Error);

        var isProfileUpdated = await _userProfileRepository.UpdateUserAsync(existingProfile);
        if (!isProfileUpdated)
            return Result.Failure(UserProfileErrors.CantUpdateUser);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}