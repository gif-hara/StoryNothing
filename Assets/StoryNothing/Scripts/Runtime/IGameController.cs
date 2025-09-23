using System.Collections.Generic;
using System.Threading;
using R3;
using StoryNothing.AreaControllers;

namespace StoryNothing
{
    public interface IGameController
    {
        void SetNextArea(AreaData areaData);

        void SetNextAreaAsHome();

        void PushButtons(IEnumerable<CreateButtonData> buttonDatabase, CancellationToken cancellationToken);

        void PushButtonsNoStack(IEnumerable<CreateButtonData> buttonDatabase, CancellationToken cancellationToken);

        void PopButtons();

        void DestroyButtonAll();

        void AddMessage(string message);

        void SetupCollectionSpot(List<CollectionSpot> collectionSpots);

        int Collection(int collectionId);

        bool CanCollection(int collectionId);

        IEnumerable<CreateButtonData> CreateUserDataItemsButtonDatabase();

        /// <summary>
        /// 何かしらゲームの状態に変更があった際のイベント
        /// </summary>
        Observable<Unit> UpdateGameState { get; }
    }
}
