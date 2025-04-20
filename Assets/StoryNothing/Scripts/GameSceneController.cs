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
        private AreaData homeAreaData;

        [SerializeField]
        private HKUIDocument gameDocument;

        private AreaData areaData;

        private UIViewGame uiViewGame;

        private CancellationTokenSource currentAreaScope;

        private List<CollectionSpot> collectionSpots = new();

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

        public void SetNextAreaAsHome()
        {
            SetNextArea(homeAreaData);
        }

        public List<Button> CreateButtons(IEnumerable<string> buttonTexts, CancellationToken cancellationToken)
        {
            return uiViewGame.CreateButtons(buttonTexts, cancellationToken);
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
            return collectionSpot.Collection();
        }

        public bool CanCollection(int collectionId)
        {
            Assert.IsTrue(collectionId >= 0 && collectionId < collectionSpots.Count, "Collection ID is out of range.");
            var collectionSpot = collectionSpots[collectionId];
            return collectionSpot.CanCollection();
        }
    }
}
