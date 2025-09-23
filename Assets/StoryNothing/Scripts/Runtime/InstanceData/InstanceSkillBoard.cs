using System;
using System.Collections.Generic;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    [Serializable]
    public sealed class InstanceSkillBoard
    {
        [field: SerializeField]
        public int InstanceId { get; private set; }

        [field: SerializeField]
        public int MasterDataSkillBoardId { get; private set; }

        [field: SerializeField]
        public List<Vector2Int> Holes { get; private set; } = new();
    }
}
