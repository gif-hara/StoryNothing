using StoryNothing.ActorControllers;
using UnityEngine;

namespace StoryNothing
{
    public class GameSceneController : MonoBehaviour
    {
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
            Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation, playerParent);
            Instantiate(fieldCameraControllerPrefab);
        }
    }
}
