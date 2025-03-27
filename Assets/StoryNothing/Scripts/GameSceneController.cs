using System.ComponentModel.Design;
using HK;
using StoryNothing.ActorControllers;
using UnityEngine;

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

        private void Start()
        {
            ServiceLocator.Register(gameRules, destroyCancellationToken);
            ServiceLocator.Register(new InputController(), destroyCancellationToken);
            var player = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerParent);
            var fieldCameraController = Instantiate(fieldCameraControllerPrefab);
            fieldCameraController.SetDefaultTrackingTarget(player.transform);
            ServiceLocator.Register(fieldCameraController, destroyCancellationToken);
            PlayerController.Attach(player, destroyCancellationToken);
        }
    }
}
