using System;
using System.Collections.Generic;
using StoryNothing.GameEvents;
using TNRD;
using UnityEngine;

namespace StoryNothing
{
    [Serializable]
    public class CreateButtonData
    {
        [SerializeField]
        private string buttonText = string.Empty;
        public string ButtonText => buttonText;

        [SerializeField]
        private List<SerializableInterface<IGameEvent>> onClickEvents = new();
        public List<SerializableInterface<IGameEvent>> OnClickEvents => onClickEvents;
    }
}
