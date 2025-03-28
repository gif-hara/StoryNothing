using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using StoryNothing.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace StoryNothing
{
    public class Gimmick : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> onEnterSequences;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onExitSequences;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onFocusedSequences;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onUnfocusedSequences;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onInteractSequences;

        void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(this, static (other, @this) =>
                {
                    var actor = other.attachedRigidbody.GetComponent<Actor>();
                    if (actor != null)
                    {
                        actor.GimmickController.AddInteractableAsync(@this).Forget();
                    }
                })
                .RegisterTo(destroyCancellationToken);

            this.OnTriggerExitAsObservable()
                .Subscribe(this, static (other, @this) =>
                {
                    var actor = other.attachedRigidbody.GetComponent<Actor>();
                    if (actor != null)
                    {
                        actor.GimmickController.RemoveInteractableAsync(@this).Forget();
                    }
                })
                .RegisterTo(destroyCancellationToken);
        }

        public UniTask PlayEnterSequencesAsync(Actor actor)
        {
            return PlaySequencesAsync(actor, onEnterSequences);
        }

        public UniTask PlayExitSequencesAsync(Actor actor)
        {
            return PlaySequencesAsync(actor, onExitSequences);
        }

        public UniTask PlayFocusedSequencesAsync(Actor actor)
        {
            return PlaySequencesAsync(actor, onFocusedSequences);
        }

        public UniTask PlayUnfocusedSequencesAsync(Actor actor)
        {
            return PlaySequencesAsync(actor, onUnfocusedSequences);
        }

        public UniTask PlayInteractSequencesAsync(Actor actor)
        {
            return PlaySequencesAsync(actor, onInteractSequences);
        }

        private UniTask PlaySequencesAsync(Actor actor, List<ISequence> sequences)
        {
            var container = new Container();
            container.Register("Actor", actor);
            container.Register("Gimmick", this);
            var sequencer = new Sequencer(container, sequences);
            return sequencer.PlayAsync(destroyCancellationToken);
        }
    }
}
