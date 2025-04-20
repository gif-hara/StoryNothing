using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using StoryNothing.AreaControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour, IGameController
    {
        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private AreaData initialAreaData;

        [SerializeField]
        private HKUIDocument gameDocument;

        private AreaData areaData;

        private UIViewGame uiViewGame;

        private CancellationTokenSource currentAreaScope;

        private async UniTaskVoid Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            ServiceLocator.Register(new UserData(), destroyCancellationToken);

            uiViewGame = new UIViewGame(gameDocument);
            uiViewGame.Setup(destroyCancellationToken);
            uiViewGame.Open();
            areaData = initialAreaData;

            while (!destroyCancellationToken.IsCancellationRequested || areaData != null)
            {
                currentAreaScope = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
                var currentAreaData = areaData;
                areaData = null;
                foreach (var i in currentAreaData.EnterAreaList)
                {
                    await i.Value.InvokeAsync(this, currentAreaScope.Token);
                }
                if (currentAreaScope != null)
                {
                    await UniTask.WaitUntilCanceled(currentAreaScope.Token);
                }
            }
        }

        public void SetNextArea(AreaData areaData)
        {
            Assert.IsNotNull(areaData, "AreaData cannot be null.");
            this.areaData = areaData;
            DestroyButtonAll();
            currentAreaScope?.Cancel();
            currentAreaScope?.Dispose();
            currentAreaScope = null;
        }

        public List<Button> CreateButtons(IEnumerable<string> buttonTexts, CancellationToken cancellationToken)
        {
            return uiViewGame.CreateButtons(buttonTexts, cancellationToken);
        }

        public void DestroyButtonAll()
        {
            uiViewGame.DestroyButtonAll();
        }

        public async UniTask AddMessageAsync(string message, CancellationToken cancellationToken)
        {
            await uiViewGame.CreateMessageAsync(message, cancellationToken);
        }
    }
}
