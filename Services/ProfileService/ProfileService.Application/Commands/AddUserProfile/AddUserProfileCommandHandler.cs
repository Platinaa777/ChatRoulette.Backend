using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.AddUserProfile;

public class AddUserProfileCommandHandler : IRequestHandler<AddUserProfile.AddUserProfileCommand, Result>
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddUserProfileCommandHandler(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AddUserProfile.AddUserProfileCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var userDomainResult = request.ToDomain();
        if (userDomainResult.IsFailure)
            return Result.Failure(userDomainResult.Error);

        var isUserExist = await _userProfileRepository.FindUserByEmailAsync(request.Email);
        if (isUserExist is not null)
            return Result.Failure(UserProfileErrors.EmailAlreadyExist);
        
        var result = await _userProfileRepository.AddUserAsync(userDomainResult.Value);
        if (!result)
            return Result.Failure(UserProfileErrors.CantAddUserProfile);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}