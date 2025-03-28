using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace StoryNothing.ActorControllers
{
    public class ActorGimmickController
    {
        private Actor actor;

        private int currentTarget = -1;

        private List<Gimmick> gimmicks = new();

        public ActorGimmickController(Actor actor)
        {
            this.actor = actor;
        }

        public async UniTask AddInteractableAsync(Gimmick gimmick)
        {
            gimmicks.Add(gimmick);
            await gimmick.PlayEnterSequencesAsync(actor);
            if (currentTarget == -1)
            {
                currentTarget = 0;
                await gimmick.PlayFocusedSequencesAsync(actor);
            }
        }

        public async UniTask RemoveInteractableAsync(Gimmick gimmick)
        {
            var index = gimmicks.FindIndex(x => x == gimmick);
            if (index == -1)
            {
                return;
            }

            await gimmick.PlayExitSequencesAsync(actor);

            if (index == currentTarget)
            {
                await gimmick.PlayUnfocusedSequencesAsync(actor);
            }

            gimmicks.RemoveAt(index);

            if (index <= currentTarget)
            {
                currentTarget -= 1;
                if (currentTarget >= 0)
                {
                    await gimmicks[currentTarget].PlayFocusedSequencesAsync(actor);
                }
            }
        }

        public async UniTask InteractAsync()
        {
            if (currentTarget == -1)
            {
                return;
            }
            await gimmicks[currentTarget].PlayInteractSequencesAsync(actor);
        }


        public void ChangeTarget(int index)
        {
            if (currentTarget == index)
            {
                return;
            }

            if (currentTarget != -1)
            {
                gimmicks[currentTarget].PlayUnfocusedSequencesAsync(actor).Forget();
            }

            currentTarget = index;
            gimmicks[currentTarget].PlayFocusedSequencesAsync(actor).Forget();
        }
    }
}
