using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private OpenCharacterController characterController;

        public ActorMovementController MovementController { get; private set; }

        void Start()
        {
            MovementController = new ActorMovementController(this, characterController);
        }
    }
}
