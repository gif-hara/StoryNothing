using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using StoryNothing.AreaControllers;
using TNRD;
using UnityEngine;
using UnityEngine.Assertions;

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

        private async UniTaskVoid Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            ServiceLocator.Register(new UserData(), destroyCancellationToken);

            uiViewGame = new UIViewGame(gameDocument);
            uiViewGame.Setup(destroyCancellationToken);
            uiViewGame.Open();
            areaData = initialAreaData;

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                var currentAreaData = areaData;
                areaData = null;
                foreach (var enterArea in currentAreaData.EnterAreaList)
                {
                    await enterArea.Value.EnterAsync(this, destroyCancellationToken);
                }

                await UniTask.WaitWhile(this, @this => @this.areaData == null, cancellationToken: destroyCancellationToken);
            }
        }

        public void SetNextArea(AreaData areaData)
        {
            Assert.IsNotNull(areaData, "AreaData cannot be null.");
            this.areaData = areaData;
        }

        public UniTask<int> CreateButtonsAsync(IEnumerable<string> buttonTexts, CancellationToken cancellationToken)
        {
            return uiViewGame.CreateButtonsAsync(buttonTexts, cancellationToken);
        }

        public void DestroyButtonAll()
        {
            uiViewGame.DestroyButtonAll();
        }
    }
}
