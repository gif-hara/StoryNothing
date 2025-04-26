using System;
using System.Collections.Generic;
using StoryNothing.GameConditions;
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
        private SerializableInterface<IGameCondition> canCreate = new();
        public SerializableInterface<IGameCondition> CanCreate => canCreate;

        [SerializeField]
        private List<SerializableInterface<IGameEvent>> onClickEvents = new();
        public List<SerializableInterface<IGameEvent>> OnClickEvents => onClickEvents;

        [SerializeField]
        private List<SerializableInterface<IGameEvent>> onPointerEnterEvents = new();
        public List<SerializableInterface<IGameEvent>> OnPointerEnterEvents => onPointerEnterEvents;

        [SerializeField]
        private List<SerializableInterface<IGameEvent>> onPointerExitEvents = new();
        public List<SerializableInterface<IGameEvent>> OnPointerExitEvents => onPointerExitEvents;

        public CreateButtonData(
            string buttonText,
            SerializableInterface<IGameCondition> canCreate = null,
            List<SerializableInterface<IGameEvent>> onClickEvents = null,
            List<SerializableInterface<IGameEvent>> onPointerEnterEvents = null,
            List<SerializableInterface<IGameEvent>> onPointerExitEvents = null
            )
        {
            this.buttonText = buttonText;
            this.canCreate = canCreate ?? new SerializableInterface<IGameCondition>();
            this.onClickEvents = onClickEvents ?? new List<SerializableInterface<IGameEvent>>();
            this.onPointerEnterEvents = onPointerEnterEvents ?? new List<SerializableInterface<IGameEvent>>();
            this.onPointerExitEvents = onPointerExitEvents ?? new List<SerializableInterface<IGameEvent>>();
        }
    }
}
