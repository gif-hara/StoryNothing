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

        [field: SerializeField]
        public Define.SkillPieceColor ColorType { get; private set; }

        public SkillPieceSpec SkillPieceSpec => ServiceLocator.Resolve<MasterData>().SkillPieceSpecs.Get(SkillPieceSpecMasterDataId);

        public SkillPieceCellSpec SkillPieceCellSpec => ServiceLocator.Resolve<MasterData>().SkillPieceCellSpecs.Get(SkillPieceCellSpecMasterDataId);

        public string Name => SkillPieceSpec.Name;

        public InstanceSkillPiece(
            int instanceId,
            int skillPieceSpecMasterDataId,
            int skillPieceCellSpecMasterDataId,
            Define.SkillPieceColor colorType
            )
        {
            InstanceId = instanceId;
            SkillPieceSpecMasterDataId = skillPieceSpecMasterDataId;
            SkillPieceCellSpecMasterDataId = skillPieceCellSpecMasterDataId;
            ColorType = colorType;
        }

        public static InstanceSkillPiece Create(int instanceId, int skillPieceSpecMasterDataId)
        {
            var masterData = ServiceLocator.Resolve<MasterData>();
            var skillPieceSpec = masterData.SkillPieceSpecs.Get(skillPieceSpecMasterDataId);
            var skillPieceCellSpec = masterData.SkillPieceCellSpecs.List
                .Where(x => x.GroupId == skillPieceSpec.SkillPieceCellSpecGroupId)
                .OrderBy(_ => Guid.NewGuid())
                .First();
            var colorTypes = Enum.GetValues(typeof(Define.SkillPieceColor))
                .Cast<Define.SkillPieceColor>()
                .Where(x => x != Define.SkillPieceColor.Gray)
                .ToArray();
            var colorType = colorTypes[UnityEngine.Random.Range(0, colorTypes.Length)];
            return new InstanceSkillPiece(instanceId, skillPieceSpecMasterDataId, skillPieceCellSpec.Id, colorType);
        }
    }
}
