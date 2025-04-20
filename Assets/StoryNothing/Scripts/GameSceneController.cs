using Cysharp.Threading.Tasks;
using HK;
using StoryNothing.AreaControllers;
using TNRD;
using UnityEngine;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private AreaData initialAreaData;

        [SerializeField]
        private HKUIDocument backgroundDocument;

        private AreaData nextAreaData;

        private async UniTaskVoid Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            ServiceLocator.Register(new UserData(), destroyCancellationToken);

            var uiViewBackground = new UIViewBackground(backgroundDocument);
            uiViewBackground.Setup(destroyCancellationToken);
            uiViewBackground.Open();

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                foreach (var enterArea in initialAreaData.EnterAreaList)
                {
                    await enterArea.Value.EnterAsync(destroyCancellationToken);
                }

                await UniTask.WaitWhile(this, @this => @this.nextAreaData == null, cancellationToken: destroyCancellationToken);
            }
        }
    }
}
