using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class ActorMovementController
    {
        private OpenCharacterController characterController;

        private Vector3 moveVelocity;

        public ActorMovementController(OpenCharacterController characterController)
        {
            this.characterController = characterController;
        }

        public void Move(Vector3 move)
        {
            moveVelocity += move;
        }
    }
}
