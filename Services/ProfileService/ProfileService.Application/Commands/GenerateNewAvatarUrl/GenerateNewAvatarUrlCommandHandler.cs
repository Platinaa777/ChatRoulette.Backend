using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Constants;
using ProfileService.Application.Models;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using S3.Client;

namespace ProfileService.Application.Commands.GenerateNewAvatarUrl;

public class GenerateNewAvatarUrlCommandHandler
    : IRequestHandler<GenerateNewAvatarUrlCommand, Result<AvatarInformation>>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IS3Client _s3Client;

    public GenerateNewAvatarUrlCommandHandler(
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork,
        IS3Client s3Client)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _s3Client = s3Client;
    }
    
    public async Task<Result<AvatarInformation>> Handle(GenerateNewAvatarUrlCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);

        var profile = await _profileRepository.FindUserByEmailAsync(request.Email);
        if (profile is null)
            return Result.Failure<AvatarInformation>(UserProfileErrors.EmailNotFound);
        
        if (!profile.Avatar.IsExists())
            return Result.Failure<AvatarInformation>(UserProfileErrors.AvatarDoesNotExists);

        var url = await _s3Client.FindFileAsync(
            bucket: S3Buckets.Avatar,
            profile.Id.Value.ToString());
        
        if (url is null || url.Link is null)
            return Result.Failure<AvatarInformation>(UserProfileErrors.AvatarUploadError);
        
        profile.RefreshAvatar(url.Link);

        await _profileRepository.UpdateUserAsync(profile);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AvatarInformation() { Url = url.Link };
    }
}