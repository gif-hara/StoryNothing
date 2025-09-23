using System;
using System.Collections.Generic;
using StoryNothing.InstanceData;
using UnityEngine;

namespace StoryNothing
{
    [Serializable]
    public sealed class UserData
    {
        [field: SerializeField]
        public List<InstanceSkillBoard> SkillBoards { get; private set; } = new();

        [field: SerializeField]
        public int AddInstanceSkillBoardCount { get; private set; } = 0;

        [field: SerializeField]
        public int EquipInstanceSkillBoardId { get; set; } = -1;
    }
}
