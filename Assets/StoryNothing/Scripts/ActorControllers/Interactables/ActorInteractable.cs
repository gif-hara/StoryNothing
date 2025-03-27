using UnityEngine;

namespace StoryNothing.ActorControllers.Interactables
{
    public class ActorInteractable : MonoBehaviour, IActorInteractable
    {
        public void Interact(Actor actor)
        {
            OnInteract(actor);
        }

        protected virtual void OnInteract(Actor actor)
        {
        }
    }
}
