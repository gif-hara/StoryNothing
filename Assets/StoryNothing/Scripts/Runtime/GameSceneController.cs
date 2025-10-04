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

        [SerializeField]
        private PlayerInput playerInput;

        [field: SerializeField]
        private int[] initialSkillBoardMasterDataIds;

        [field: SerializeField]
        private int[] initialSkillPieceMasterDataIds;

        [field: SerializeField]
        private int playerCharacterSpecId;

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
                    foreach (var initialSkillBoardMasterDataId in initialSkillBoardMasterDataIds)
                    {
                        if (initialSkillBoardMasterDataId < 0)
                        {
                            continue;
                        }
                        var instanceSkillBoard = InstanceSkillBoard.Create(userData.AddInstanceSkillBoardCount, initialSkillBoardMasterDataId);
                        userData.AddInstanceSkillBoard(instanceSkillBoard);
                        if (userData.EquipInstanceSkillBoardId == -1)
                        {
                            userData.EquipInstanceSkillBoardId = instanceSkillBoard.InstanceId;
                        }
                    }
                }
            }

            // とりあえずスキルピースを作る
            {
                for (int i = 0; i < 500; i++)
                {
                    foreach (var initialSkillPieceMasterDataId in initialSkillPieceMasterDataIds)
                    {
                        if (initialSkillPieceMasterDataId < 0)
                        {
                            continue;
                        }
                        var instanceSkillPiece = InstanceSkillPiece.Create(userData.AddInstanceSkillPieceCount, initialSkillPieceMasterDataId);
                        userData.AddInstanceSkillPiece(instanceSkillPiece);
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

            var uiViewOutGame = new UIViewOutGame(outGameDocument, userData, playerInput, playerCharacterSpecId);
            await uiViewOutGame.BeginAsync(destroyCancellationToken);
        }
    }
}
