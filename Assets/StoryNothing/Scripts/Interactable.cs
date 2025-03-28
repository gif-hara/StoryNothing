using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnitySequencerSystem;

namespace StoryNothing
{
    public class Interactable
    {
        public string Id { get; }

        public List<ISequence> OnEnteredSequences { get; set; }

        public Func<UniTask> OnExitedAsync { get; set; }

        public Func<UniTask> OnFocusedAsync { get; set; }

        public Func<UniTask> OnUnfocusedAsync { get; set; }

        public Func<UniTask> OnInteractedAsync { get; set; }

        public Interactable(string id)
        {
            Id = id;
        }
    }
}
