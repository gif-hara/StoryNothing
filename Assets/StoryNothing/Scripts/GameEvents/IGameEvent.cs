using Cysharp.Threading.Tasks;

namespace StoryNothing.GameEvents
{
    public interface IGameEvent
    {
        UniTask InvokeAsync();
    }
}
