using System;
using System.Collections.Generic;
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

        public static InstanceSkillPiece Create(int instanceId, int createSkillPieceSpecId)
        {
            var masterData = ServiceLocator.Resolve<MasterData>();
            var createSkillPieceSpec = masterData.CreateSkillPieceSpecs.Get(createSkillPieceSpecId);
            var skillPieceSpec = masterData.SkillPieceSpecs.Get(createSkillPieceSpec.SkillPieceSpecId);
            var skillPieceCellSpec = masterData.SkillPieceCellSpecs.List
                .Where(x => x.GroupId == skillPieceSpec.SkillPieceCellSpecGroupId)
                .OrderBy(_ => Guid.NewGuid())
                .First();
            var availableColorTypes = new List<Define.SkillPieceColor>();
            if (createSkillPieceSpec.Red) { availableColorTypes.Add(Define.SkillPieceColor.Red); }
            if (createSkillPieceSpec.Orange) { availableColorTypes.Add(Define.SkillPieceColor.Orange); }
            if (createSkillPieceSpec.WhiteGray) { availableColorTypes.Add(Define.SkillPieceColor.WhiteGray); }
            if (createSkillPieceSpec.Purple) { availableColorTypes.Add(Define.SkillPieceColor.Purple); }
            if (createSkillPieceSpec.Water) { availableColorTypes.Add(Define.SkillPieceColor.Water); }
            if (createSkillPieceSpec.Green) { availableColorTypes.Add(Define.SkillPieceColor.Green); }
            return new InstanceSkillPiece(
                instanceId,
                createSkillPieceSpec.SkillPieceSpecId,
                skillPieceCellSpec.Id,
                availableColorTypes[UnityEngine.Random.Range(0, availableColorTypes.Count)]
                );
        }
    }
}
