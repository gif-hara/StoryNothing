using System.Collections.Generic;
using System.Threading;
using StoryNothing.AreaControllers;
using UnityEngine.UI;

namespace StoryNothing
{
    public interface IGameController
    {
        void SetNextArea(AreaData areaData);

        void SetNextAreaAsHome();

        void PushButtons(IEnumerable<CreateButtonData> buttonDatabase, CancellationToken cancellationToken);

        void DestroyButtonAll();

        void AddMessage(string message);

        void SetupCollectionSpot(List<CollectionSpot> collectionSpots);

        int Collection(int collectionId);

        bool CanCollection(int collectionId);
    }
}
