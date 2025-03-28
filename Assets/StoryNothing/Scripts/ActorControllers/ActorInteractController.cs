using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace StoryNothing.ActorControllers
{
    public class ActorInteractController
    {
        private Actor actor;

        private int currentTarget = -1;

        private List<Gimmick> interactables = new();

        public ActorInteractController(Actor actor)
        {
            this.actor = actor;
        }

        public async UniTask AddInteractableAsync(Gimmick gimmick)
        {
            interactables.Add(gimmick);
            await gimmick.PlayEnterSequencesAsync(actor);
            if (currentTarget == -1)
            {
                currentTarget = 0;
                await gimmick.PlayFocusedSequencesAsync(actor);
            }
        }

        public async UniTask RemoveInteractableAsync(Gimmick gimmick)
        {
            var index = interactables.FindIndex(x => x == gimmick);
            if (index == -1)
            {
                return;
            }

            await gimmick.PlayExitSequencesAsync(actor);

            if (index == currentTarget)
            {
                await gimmick.PlayUnfocusedSequencesAsync(actor);
            }

            interactables.RemoveAt(index);

            if (index <= currentTarget)
            {
                currentTarget -= 1;
                if (currentTarget >= 0)
                {
                    await interactables[currentTarget].PlayFocusedSequencesAsync(actor);
                }
            }
        }

        public async UniTask InteractAsync()
        {
            if (currentTarget == -1)
            {
                return;
            }
            await interactables[currentTarget].PlayInteractSequencesAsync(actor);
        }


        public void ChangeTarget(int index)
        {
            if (currentTarget == index)
            {
                return;
            }

            if (currentTarget != -1)
            {
                interactables[currentTarget].PlayUnfocusedSequencesAsync(actor).Forget();
            }

            currentTarget = index;
            interactables[currentTarget].PlayFocusedSequencesAsync(actor).Forget();
        }
    }
}
