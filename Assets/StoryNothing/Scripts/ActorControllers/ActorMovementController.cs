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

        private Quaternion rotation;

        private Vector3 gravity;

        public float MoveSpeed { get; set; }

        public float RotationSpeed { get; set; }

        public ActorMovementController(Actor actor, OpenCharacterController characterController)
        {
            this.characterController = characterController;
            actor.UpdateAsObservable()
                .Subscribe((this, actor), static (_, t) =>
                {
                    var (@this, actor) = t;
                    if (!@this.characterController.isGrounded)
                    {
                        @this.gravity += Physics.gravity * Time.deltaTime;
                        @this.characterController.Move(@this.gravity * Time.deltaTime);
                    }
                    else
                    {
                        @this.gravity = Vector3.zero;
                    }

                    @this.characterController.Move(@this.moveVelocity * Time.deltaTime * @this.MoveSpeed);
                    @this.moveVelocity = Vector3.zero;

                    actor.transform.localRotation = Quaternion.Lerp(actor.transform.localRotation, @this.rotation, Time.deltaTime * @this.RotationSpeed);
                })
                .RegisterTo(actor.destroyCancellationToken);
        }

        public void Move(Vector3 move)
        {
            moveVelocity += move;
        }

        public void Rotation(Quaternion rotation)
        {
            this.rotation = rotation;
        }
    }
}
