using System;
using System.Collections.Generic;
using StoryNothing.AreaControllers.EnterAreaEvents;
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
        private List<SerializableInterface<IEnterAreaEvent>> onClickEvents = new();
        public List<SerializableInterface<IEnterAreaEvent>> OnClickEvents => onClickEvents;
    }
}
