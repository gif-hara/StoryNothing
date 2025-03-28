using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private OpenCharacterController characterController;

        public ActorMovementController MovementController { get; private set; }

        public ActorInteractController InteractController { get; private set; }

        void Awake()
        {
            MovementController = new ActorMovementController(this, characterController);
            InteractController = new ActorInteractController(this);
        }
    }
}
