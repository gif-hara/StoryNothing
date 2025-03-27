using UnityEngine;

namespace StoryNothing.ActorControllers.Interactables
{
    public interface IActorInteractable
    {
        void OnEnter(Actor actor);

        void OnExit(Actor actor);

        void Interact(Actor actor);
    }
}
