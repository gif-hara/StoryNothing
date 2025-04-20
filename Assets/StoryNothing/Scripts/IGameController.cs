using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using StoryNothing.AreaControllers;
using UnityEngine.UI;

namespace StoryNothing
{
    public interface IGameController
    {
        void SetNextArea(AreaData areaData);

        List<Button> CreateButtons(IEnumerable<string> buttonTexts, CancellationToken cancellationToken);

        void DestroyButtonAll();

        void AddMessage(string message, CancellationToken cancellationToken);
    }
}
