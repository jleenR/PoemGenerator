namespace PoemGenerator.BusinessLogic
{
    public interface IRuleParsingEngine
    {
        void EvaluateLineComponent(string word);
        string GetPoem();
        void PrepareCollections(string[] FileLines);
    }
}