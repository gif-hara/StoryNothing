using System;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    [Serializable]
    public sealed class InstanceSkillPeace
    {
        [field: SerializeField]
        public int InstanceId { get; private set; }

        [field: SerializeField]
        public int SkillPeaceSpecMasterDataId { get; private set; }

        public InstanceSkillPeace(int instanceId, int skillPeaceSpecMasterDataId)
        {
            InstanceId = instanceId;
            SkillPeaceSpecMasterDataId = skillPeaceSpecMasterDataId;
        }
    }
}
