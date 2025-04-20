using System.Collections.Generic;
using TNRD;
using UnityEngine;

namespace StoryNothing.AreaControllers
{
    [CreateAssetMenu(fileName = "AreaData", menuName = "StoryNothing/AreaData")]
    public class AreaData : ScriptableObject
    {
        [SerializeField]
        private List<SerializableInterface<IEnterArea>> enterAreaList = new();
        public List<SerializableInterface<IEnterArea>> EnterAreaList => enterAreaList;
    }
}
