using Chat.Core.Repositories;

namespace Chat.Application.Handlers;

public class DialogRoomHandler
{
    private readonly IDialogRoomRepository _repository;

    public DialogRoomHandler(IDialogRoomRepository repository)
    {
        _repository = repository;
    }
    
    
}