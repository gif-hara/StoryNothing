using R3;
using R3.Triggers;
using StandardAssets.Characters.Physics;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class ActorMovementController
    {
        private OpenCharacterController characterController;

        private Vector3 moveVelocity;

        public ActorMovementController(Actor actor, OpenCharacterController characterController)
        {
            this.characterController = characterController;
            actor.UpdateAsObservable()
                .Subscribe(this, static (_, @this) =>
                {
                    @this.characterController.Move(@this.moveVelocity * Time.deltaTime);
                    @this.moveVelocity = Vector3.zero;
                })
                .RegisterTo(actor.destroyCancellationToken);
        }

        public void Move(Vector3 move)
        {
            moveVelocity += move;
        }
    }
}
