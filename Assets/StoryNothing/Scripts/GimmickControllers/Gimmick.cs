using R3;
using R3.Triggers;
using UnityEngine;

namespace StoryNothing.ActorControllers.Interactables
{
    public class Gimmick : MonoBehaviour, IGimmick
    {
        void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(other =>
                {
                    var actor = other.attachedRigidbody.GetComponent<Actor>();
                    if (actor != null)
                    {
                        Interact(actor);
                    }
                })
                .RegisterTo(destroyCancellationToken);
        }

        public void Interact(Actor actor)
        {
        }
    }
}
