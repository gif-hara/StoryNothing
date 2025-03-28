using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace StoryNothing.ActorControllers
{
    public class ActorInteractController
    {
        private int currentTarget = -1;

        private List<Interactable> interactables = new();

        public async UniTask AddInteractableAsync(Interactable interactable)
        {
            interactables.Add(interactable);
            await interactables[currentTarget].OnEnteredAsync.Invoke();
            if (currentTarget == -1)
            {
                currentTarget = 0;
                await interactables[currentTarget].OnFocusedAsync.Invoke();
            }
        }

        public async UniTask RemoveInteractableAsync(string id)
        {
            var index = interactables.FindIndex(x => x.Id == id);
            if (index == -1)
            {
                return;
            }

            await interactables[index].OnExitedAsync.Invoke();

            if (index == currentTarget)
            {
                await interactables[index].OnUnfocusedAsync.Invoke();
            }


            interactables.RemoveAt(index);

            if (index <= currentTarget)
            {
                currentTarget -= 1;
                if (currentTarget >= 0)
                {
                    await interactables[currentTarget].OnFocusedAsync.Invoke();
                }
            }
        }

        public async UniTask InteractAsync()
        {
            if (currentTarget == -1)
            {
                return;
            }
            await interactables[currentTarget].OnInteractedAsync.Invoke();
        }


        public void ChangeTarget(int index)
        {
            interactables[currentTarget].OnUnfocusedAsync.Invoke();
            currentTarget = index;
            interactables[currentTarget].OnFocusedAsync.Invoke();
        }
    }
}
