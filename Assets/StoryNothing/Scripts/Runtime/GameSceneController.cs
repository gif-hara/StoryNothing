using Cysharp.Threading.Tasks;
using HK;
using R3;
using R3.Triggers;
using StoryNothing.InstanceData;
using StoryNothing.MasterDataSystems;
using StoryNothing.UIViews;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private MasterData masterData;

        [SerializeField]
        private HKUIDocument outGameDocument;

        [field: SerializeField]
        private int initialSkillBoardMasterDataId = 0;

        private UserData userData;

        private Subject<Unit> updateGameState = new();
        public Observable<Unit> UpdateGameState => updateGameState;

        private async UniTaskVoid Start()
        {
            ServiceLocator.Register(masterData, destroyCancellationToken);

            userData = new UserData();

            // とりあえずスキルボードを作る
            {
                for (int i = 0; i < 10; i++)
                {
                    var instanceSkillBoard = InstanceSkillBoard.Create(userData.AddInstanceSkillBoardCount, initialSkillBoardMasterDataId);
                    userData.AddInstanceSkillBoard(instanceSkillBoard);
                    if (userData.EquipInstanceSkillBoardId == -1)
                    {
                        userData.SetEquipInstanceSkillBoard(instanceSkillBoard.InstanceId);
                    }
                }
            }

#if DEBUG
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (Keyboard.current.f1Key.wasPressedThisFrame)
                    {
                    }
                })
                .RegisterTo(destroyCancellationToken);
#endif

            var uiViewOutGame = new UIViewOutGame(outGameDocument, userData);
            await uiViewOutGame.BeginAsync(destroyCancellationToken);
        }
    }
}
