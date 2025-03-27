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
            actor.UpdateAsObservable()
                .TakeUntil(cancellationToken)
                .Subscribe(actor, static (_, actor) =>
                {
                    var inputController = ServiceLocator.Resolve<InputController>();
                    var vector = inputController.InputActions.Player.Move.ReadValue<Vector2>();
                    actor.MovementController.Move(new Vector3(vector.x, 0, vector.y));
                });
        }
    }
}
