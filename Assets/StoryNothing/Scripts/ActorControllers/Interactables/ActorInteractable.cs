using UnityEngine;

namespace StoryNothing.ActorControllers.Interactables
{
    public class ActorInteractable : MonoBehaviour, IActorInteractable
    {
        public void Enter(Actor actor)
        {
            OnEnter(actor);
        }

        public void Exit(Actor actor)
        {
            OnExit(actor);
        }

        public void Interact(Actor actor)
        {
            OnInteract(actor);
        }

        protected virtual void OnEnter(Actor actor)
        {
        }

        protected virtual void OnExit(Actor actor)
        {
        }

        protected virtual void OnInteract(Actor actor)
        {
        }
    }
}
