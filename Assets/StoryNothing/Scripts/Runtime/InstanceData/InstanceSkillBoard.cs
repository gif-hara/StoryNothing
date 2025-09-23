using System;
using System.Collections.Generic;
using HK;
using StoryNothing.MasterDataSystems;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    [Serializable]
    public sealed class InstanceSkillBoard
    {
        [field: SerializeField]
        public int InstanceId { get; private set; }

        [field: SerializeField]
        public int SkillBoardMasterDataId { get; private set; }

        [field: SerializeField]
        public List<Vector2Int> Holes { get; private set; } = new();

        public SkillBoardSpec SkillBoardSpec => ServiceLocator.Resolve<MasterData>().SkillBoardSpecs.Get(SkillBoardMasterDataId);

        public string Name => SkillBoardSpec.Name;

        public InstanceSkillBoard(int instanceId, int masterDataSkillBoardId, List<Vector2Int> holes = null)
        {
            InstanceId = instanceId;
            SkillBoardMasterDataId = masterDataSkillBoardId;
            Holes = holes ?? new List<Vector2Int>();
        }

        public static InstanceSkillBoard Create(int instanceId, int skillBoardMasterDataId)
        {
            var holes = new List<Vector2Int>();
            var skillBoardSpec = ServiceLocator.Resolve<MasterData>().SkillBoardSpecs.Get(skillBoardMasterDataId);
            for (int i = 0; i < skillBoardSpec.HoleCount; i++)
            {
                var hole = new Vector2Int(UnityEngine.Random.Range(0, skillBoardSpec.X), UnityEngine.Random.Range(0, skillBoardSpec.Y));
                if (holes.Contains(hole))
                {
                    i--;
                    continue;
                }
                holes.Add(hole);
            }
            return new InstanceSkillBoard(instanceId, skillBoardMasterDataId, holes);
        }
    }
}
