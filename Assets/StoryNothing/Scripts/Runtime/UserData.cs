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

        public void AddSkillBoard(InstanceSkillBoard skillBoard)
        {
            SkillBoards.Add(skillBoard);
            AddInstanceSkillBoardCount++;
        }

        public void RemoveSkillBoard(InstanceSkillBoard skillBoard)
        {
            SkillBoards.Remove(skillBoard);
        }
    }
}
