using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserHistoryAggregate;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.AddDoomSlayerPoint;

public class AddDoomSlayerPointCommandHandler
    : IRequestHandler<AddDoomSlayerPointCommand, Result>
{
    private readonly IUserHistoryRepository _historyRepository;
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddDoomSlayerPointCommandHandler(
        IUserHistoryRepository historyRepository,
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork)
    {
        _historyRepository = historyRepository;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AddDoomSlayerPointCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var userProfile = await _profileRepository.FindUserByEmailAsync(request.Email);
        if (userProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        var userHistory = await _historyRepository.FindByUserId(userProfile.Id);
        if (userHistory is null)
        {
            userHistory = UserHistory.Create(
                id: Guid.NewGuid().ToString(),
                userProfile.Id.Value.ToString(),
                0).Value;

            await _historyRepository.AddHistory(userHistory);
        }
        
        userHistory.IncreaseDoomSlayerPoints();

        await _historyRepository.UpdateHistory(userHistory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}