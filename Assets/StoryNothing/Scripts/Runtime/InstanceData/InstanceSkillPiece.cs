using System;
using UnityEngine;

namespace StoryNothing.InstanceData
{
    [Serializable]
    public sealed class InstanceSkillPiece
    {
        [field: SerializeField]
        public int InstanceId { get; private set; }

        [field: SerializeField]
        public int SkillPieceSpecMasterDataId { get; private set; }

        public InstanceSkillPiece(int instanceId, int skillPieceSpecMasterDataId)
        {
            InstanceId = instanceId;
            SkillPieceSpecMasterDataId = skillPieceSpecMasterDataId;
        }

        public static InstanceSkillPiece Create(int instanceId, int skillPieceSpecMasterDataId)
        {
            return new InstanceSkillPiece(instanceId, skillPieceSpecMasterDataId);
        }
    }
}
