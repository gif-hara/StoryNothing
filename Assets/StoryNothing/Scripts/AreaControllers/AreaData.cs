using System.Collections.Generic;
using StoryNothing.GameEvents;
using TNRD;
using UnityEngine;

namespace StoryNothing.AreaControllers
{
    [CreateAssetMenu(fileName = "AreaData", menuName = "StoryNothing/AreaData")]
    public class AreaData : ScriptableObject
    {
        [SerializeField]
        private List<SerializableInterface<IGameEvent>> enterAreaList = new();
        public List<SerializableInterface<IGameEvent>> EnterAreaList => enterAreaList;
    }
}
