using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HK;
using MH3;
using R3;
using R3.Triggers;
using StoryNothing.AreaControllers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour, IGameController
    {
        [SerializeField]
        private MasterData masterData;

        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private AreaData initialAreaData;

        [SerializeField]
        private AreaData homeAreaData;

        [SerializeField]
        private HKUIDocument gameDocument;

        private AreaData areaData;

        private UIViewGame uiViewGame;

        private CancellationTokenSource currentAreaScope;

        private List<CollectionSpot> collectionSpots = new();

        private UserData userData;

        private async UniTaskVoid Start()
        {
            ServiceLocator.Register(masterData, destroyCancellationToken);
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);

            userData = new UserData();
            uiViewGame = new UIViewGame(gameDocument);
            uiViewGame.Setup(destroyCancellationToken);
            uiViewGame.Open();
            areaData = initialAreaData;

#if DEBUG
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Keyboard.current.f1Key.wasPressedThisFrame)
                    {
                        foreach (var i in masterData.ItemSpecs.List)
                        {
                            userData.AddItem(i.Id, 99);
                        }
                        Debug.Log($"[DEBUG] Add Item");
                    }
                })
                .RegisterTo(destroyCancellationToken);
#endif

            while (this != null && !destroyCancellationToken.IsCancellationRequested || areaData != null)
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
                currentAreaScope?.Cancel();
                currentAreaScope?.Dispose();
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

        public void SetNextAreaAsHome()
        {
            SetNextArea(homeAreaData);
        }

        public void PushButtons(IEnumerable<CreateButtonData> buttonDatabase, CancellationToken cancellationToken)
        {
            uiViewGame.PushButtons(buttonDatabase, this, cancellationToken);
        }

        public void PushButtonsNoStack(IEnumerable<CreateButtonData> buttonDatabase, CancellationToken cancellationToken)
        {
            uiViewGame.PushButtonsNoStack(buttonDatabase, this, cancellationToken);
        }

        public void PopButtons()
        {
            uiViewGame.PopButtons();
        }

        public void DestroyButtonAll()
        {
            uiViewGame.DestroyButtonAll();
        }

        public void AddMessage(string message)
        {
            uiViewGame.CreateMessage(message);
        }

        public void SetupCollectionSpot(List<CollectionSpot> collectionSpots)
        {
            this.collectionSpots = collectionSpots;
        }

        public int Collection(int collectionId)
        {
            Assert.IsTrue(collectionId >= 0 && collectionId < collectionSpots.Count, "Collection ID is out of range.");
            var collectionSpot = collectionSpots[collectionId];
            var result = collectionSpot.Collection();
            userData.AddItem(result, 1);
            return result;
        }

        public bool CanCollection(int collectionId)
        {
            Assert.IsTrue(collectionId >= 0 && collectionId < collectionSpots.Count, "Collection ID is out of range.");
            var collectionSpot = collectionSpots[collectionId];
            return collectionSpot.CanCollection();
        }
    }
}
