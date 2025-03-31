using System.ComponentModel.Design;
using Cysharp.Threading.Tasks;
using HK;
using R3;
using R3.Triggers;
using StoryNothing.ActorControllers;
using StoryNothing.BattleSystems;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField]
        private GameRules gameRules;

        [SerializeField]
        private Actor playerPrefab;

        [SerializeField]
        private Transform playerSpawnPoint;

        [SerializeField]
        private Transform playerParent;

        [SerializeField]
        private FieldCameraController fieldCameraControllerPrefab;

        [SerializeField]
        private HKUIDocument battleDocumentPrefab;

        private BattleController battleController;

        private void Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            ServiceLocator.Register(new UserData(), destroyCancellationToken);
            var player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerParent);
            var fieldCameraController = Instantiate(fieldCameraControllerPrefab);
            fieldCameraController.SetDefaultTrackingTarget(player.transform);
            ServiceLocator.Register(fieldCameraController, destroyCancellationToken);
            PlayerController.Attach(player, destroyCancellationToken);

            this.UpdateAsObservable()
                .Subscribe(this, static (_, @this) =>
                {
                    if (Keyboard.current.digit1Key.wasPressedThisFrame)
                    {
                        @this.BeginBattleAsync().Forget();
                    }
                })
                .RegisterTo(destroyCancellationToken);
        }

        private async UniTask BeginBattleAsync()
        {
            if (battleController != null)
            {
                return;
            }
            battleController = new BattleController();
            await battleController.BeginAsync(battleDocumentPrefab);
            battleController = null;
        }
    }
}
