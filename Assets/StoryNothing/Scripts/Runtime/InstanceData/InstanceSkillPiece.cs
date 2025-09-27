using System;
using System.Linq;
using HK;
using StoryNothing.MasterDataSystems;
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

        [field: SerializeField]
        public int SkillPieceCellSpecMasterDataId { get; private set; }

        public SkillPieceSpec SkillPieceSpec => ServiceLocator.Resolve<MasterData>().SkillPieceSpecs.Get(SkillPieceSpecMasterDataId);

        public SkillPieceCellSpec SkillPieceCellSpec => ServiceLocator.Resolve<MasterData>().SkillPieceCellSpecs.Get(SkillPieceCellSpecMasterDataId);

        public string Name => SkillPieceSpec.Name;

        public InstanceSkillPiece(int instanceId, int skillPieceSpecMasterDataId, int skillPieceCellSpecMasterDataId = 0)
        {
            InstanceId = instanceId;
            SkillPieceSpecMasterDataId = skillPieceSpecMasterDataId;
            SkillPieceCellSpecMasterDataId = skillPieceCellSpecMasterDataId;
        }

        public static InstanceSkillPiece Create(int instanceId, int skillPieceSpecMasterDataId)
        {
            var masterData = ServiceLocator.Resolve<MasterData>();
            var skillPieceSpec = masterData.SkillPieceSpecs.Get(skillPieceSpecMasterDataId);
            var skillPieceCellSpec = masterData.SkillPieceCellSpecs.List
                .Where(x => x.GroupId == skillPieceSpec.SkillPieceCellSpecGroupId)
                .OrderBy(_ => Guid.NewGuid())
                .First();
            return new InstanceSkillPiece(instanceId, skillPieceSpecMasterDataId, skillPieceCellSpec.Id);
        }
    }
}
