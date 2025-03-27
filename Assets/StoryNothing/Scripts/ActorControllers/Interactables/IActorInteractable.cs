using UnityEngine;

namespace StoryNothing.ActorControllers.Interactables
{
    public interface IActorInteractable
    {
        void Enter(Actor actor);

        void Exit(Actor actor);

        void Interact(Actor actor);
    }
}
