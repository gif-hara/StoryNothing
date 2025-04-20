using System.Threading;
using Cysharp.Threading.Tasks;

namespace StoryNothing.AreaControllers
{
    public interface IEnterArea
    {
        UniTask EnterAsync(CancellationToken cancellationToken);
    }
}
