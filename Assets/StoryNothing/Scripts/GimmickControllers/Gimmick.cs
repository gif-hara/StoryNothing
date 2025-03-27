using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using StoryNothing.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace StoryNothing.GimmickControllers
{
    public class Gimmick : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> onEnterSequences;

        [SerializeReference, SubclassSelector]
        private List<ISequence> onExitSequences;

        void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(this, static (other, @this) =>
                {
                    var actor = other.attachedRigidbody.GetComponent<Actor>();
                    if (actor != null)
                    {
                        @this.PlaySequences(@this.onEnterSequences);
                    }
                })
                .RegisterTo(destroyCancellationToken);

            this.OnTriggerExitAsObservable()
                .Subscribe(this, static (other, @this) =>
                {
                    var actor = other.attachedRigidbody.GetComponent<Actor>();
                    if (actor != null)
                    {
                        @this.PlaySequences(@this.onExitSequences);
                    }
                })
                .RegisterTo(destroyCancellationToken);
        }

        private void PlaySequences(List<ISequence> sequences)
        {
            var container = new Container();
            container.Register("Actor", GetComponent<Actor>());
            container.Register("Gimmick", this);
            var sequencer = new Sequencer(container, sequences);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }
    }
}
