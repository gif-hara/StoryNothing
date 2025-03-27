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
                .Subscribe(_ =>
                {
                    var inputController = ServiceLocator.Resolve<InputController>();
                    var vector = inputController.InputActions.Player.Move.ReadValue<Vector2>();
                    Debug.Log(vector);
                });
        }
    }
}
