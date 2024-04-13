using AuthService.Domain.Errors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.BanUser;

public class BanUserCommandHandler
    : IRequestHandler<BanUserCommand, Result>
{
    private readonly IUserHistoryRepository _historyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BanUserCommandHandler(
        IUserHistoryRepository historyRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _historyRepository = historyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(BanUserCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.ViolatorEmail).Value;
        var user = await _userRepository.FindUserByEmailAsync(email);

        if (user is null)
            return Result.Failure(UserError.UserNotFound);

        var history = await _historyRepository.FindByUserId(user.Id);

        if (history is null)
        {
            return Result.Failure(HistoryError.HistoryNotFound);
        }
        
        history.BanUser(request.MinutesToBan);

        await _historyRepository.UpdateHistory(history);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}