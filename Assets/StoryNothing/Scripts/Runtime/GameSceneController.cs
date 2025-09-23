using HK;
using R3;
using R3.Triggers;
using StoryNothing.MasterDataSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private MasterData masterData;

        [SerializeField]
        private HKUIDocument gameDocument;

        private UserData userData;

        private Subject<Unit> updateGameState = new();
        public Observable<Unit> UpdateGameState => updateGameState;

        private void Start()
        {
            ServiceLocator.Register(masterData, destroyCancellationToken);

            userData = new UserData();

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
        }
    }
}
