using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using R3.Triggers;
using StoryNothing.ActorControllers;
using UnityEngine;
using UnitySequencerSystem;

namespace StoryNothing.GimmickControllers
{
    public class Gimmick : MonoBehaviour, IGimmick
    {
        [SerializeReference, SubclassSelector]
        private List<ISequence> sequences;

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
            var container = new Container();
            container.Register("Actor", actor);
            var sequencer = new Sequencer(container, sequences);
            sequencer.PlayAsync(destroyCancellationToken).Forget();
        }
    }
}
