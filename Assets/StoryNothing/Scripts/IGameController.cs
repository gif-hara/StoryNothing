using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using StoryNothing.AreaControllers;

namespace StoryNothing
{
    public interface IGameController
    {
        void SetNextArea(AreaData areaData);

        UniTask<int> CreateButtonsAsync(IEnumerable<string> buttonTexts, CancellationToken cancellationToken);

        void DestroyButtonAll();
    }
}
