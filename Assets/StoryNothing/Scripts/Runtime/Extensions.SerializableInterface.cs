using StoryNothing;
using StoryNothing.GameConditions;
using TNRD;

namespace MH3
{
    public static partial class Extensions
    {
        public static bool EvaluateSafe(this SerializableInterface<IGameCondition> condition, IGameController gameController)
        {
            if (condition == null || condition.Value == null)
            {
                return true;
            }

            return condition.Value.Evaluate(gameController);
        }
    }
}
