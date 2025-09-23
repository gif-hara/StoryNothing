namespace StoryNothing.GameConditions
{
    public interface IGameCondition
    {
        bool Evaluate(IGameController gameController);
    }
}
