using DomainDriverDesignAbstractions;

namespace Chat.Domain.Aggregates.Game;

public class TranslationGame : AggregateRoot<Guid>
{
    public TranslationGame(
        Guid id,
        string wordToTranslate,
        string correctTranslation,
        List<string> chooseList)
    {
        Id = id;
        WordToTranslate = wordToTranslate;
        ChooseList = chooseList;
        CorrectTranslation = correctTranslation;
    }

    public string WordToTranslate { get; private set; }
    public string CorrectTranslation { get; private set; }
    public List<string> ChooseList { get; private set; }
    
    private TranslationGame() {}
}