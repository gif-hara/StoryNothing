using System.Threading;
using HK;
using R3;
using R3.Triggers;
using UnityEngine;

namespace StoryNothing.ActorControllers
{
    public class PlayerController : MonoBehaviour
    {
        public static void Attach(Actor actor, CancellationToken cancellationToken)
        {
            var inputController = ServiceLocator.Resolve<InputController>();
            var fieldCameraController = ServiceLocator.Resolve<FieldCameraController>();
            var gameRules = ServiceLocator.Resolve<GameRules>();
            actor.MovementController.MoveSpeed = gameRules.PlayerMoveSpeed;
            actor.MovementController.RotationSpeed = gameRules.PlayerRotationSpeed;
            actor.UpdateAsObservable()
                .Subscribe((actor, inputController, fieldCameraController), static (_, t) =>
                {
                    var (actor, inputController, fieldCameraController) = t;
                    var inputVector = inputController.InputActions.Player.Move.ReadValue<Vector2>();
                    if (inputVector.magnitude < 0.1f)
                    {
                        return;
                    }
                    var cameraForward = fieldCameraController.ControlledCamera.transform.forward;
                    cameraForward.y = 0;
                    cameraForward.Normalize();
                    var cameraRight = fieldCameraController.ControlledCamera.transform.right;
                    cameraRight.y = 0;
                    cameraRight.Normalize();
                    var vector = (cameraForward * inputVector.y + cameraRight * inputVector.x).normalized;
                    actor.MovementController.Move(new Vector3(vector.x, 0, vector.z));
                    actor.MovementController.Rotation(Quaternion.LookRotation(vector));
                })
                .RegisterTo(cancellationToken);
        }
    }
}
