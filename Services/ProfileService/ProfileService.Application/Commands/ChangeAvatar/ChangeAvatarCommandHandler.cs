using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using S3.Client;

namespace ProfileService.Application.Commands.ChangeAvatar;

public class ChangeAvatarCommandHandler
    : IRequestHandler<ChangeAvatarCommand, Result<AvatarInformation>>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IS3Client _s3Client;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeAvatarCommandHandler(
        IUserProfileRepository profileRepository,
        IS3Client s3Client,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _s3Client = s3Client;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<AvatarInformation>> Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var profile = await _profileRepository.FindUserByEmailAsync(request.Email);
        if (profile is null)
            return Result.Failure<AvatarInformation>(UserProfileErrors.EmailNotFound);

        if (profile.Avatar.IsExists())
        {
            await _s3Client.DeleteFile("bucket-chat-roulette", profile.Id.Value.ToString());
        }

        var url =await _s3Client.UploadFileAsync(
            request.Avatar!,
            bucket: "bucket-chat-roulette",
            profile.Id.Value.ToString(),
            request.ContentType);
        
        if (url is null || string.IsNullOrWhiteSpace(url.Link))
            return Result.Failure<AvatarInformation>(UserProfileErrors.AvatarUploadError);
        
        profile.ChangeAvatar(url.Link);

        await _profileRepository.UpdateUserAsync(profile);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AvatarInformation() { Url = url.Link };
    }
}