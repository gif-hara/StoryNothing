using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class Actor : MonoBehaviour
    {
        [SerializeField]
        private OpenCharacterController characterController;

        public ActorMovementController MovementController { get; private set; }

        public ActorGimmickController GimmickController { get; private set; }

        void Awake()
        {
            MovementController = new ActorMovementController(this, characterController);
            GimmickController = new ActorGimmickController(this);
        }
    }
}
