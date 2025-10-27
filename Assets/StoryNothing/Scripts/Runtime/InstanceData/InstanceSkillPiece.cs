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
        public int SkillPieceSpecId { get; private set; }

        [field: SerializeField]
        public int SkillPieceCellSpecId { get; private set; }

        [field: SerializeField]
        public Define.SkillPieceColor ColorType { get; private set; }

        [field: SerializeField]
        public List<int> SkillSpecIds { get; private set; } = new();

        public SkillPieceSpec SkillPieceSpec => ServiceLocator.Resolve<MasterData>().SkillPieceSpecs.Get(SkillPieceSpecId);

        public SkillPieceCellSpec SkillPieceCellSpec => ServiceLocator.Resolve<MasterData>().SkillPieceCellSpecs.Get(SkillPieceCellSpecId);

        public IEnumerable<SkillEffect> SkillEffects => SkillSpecIds
            .SelectMany(x => ServiceLocator.Resolve<MasterData>().SkillEffects.Get(x));

        public string Name
        {
            get
            {
                var prefix = ColorType switch
                {
                    Define.SkillPieceColor.Red => "赤の",
                    Define.SkillPieceColor.Orange => "橙の",
                    Define.SkillPieceColor.WhiteGray => "白灰の",
                    Define.SkillPieceColor.Green => "緑の",
                    _ => throw new ArgumentOutOfRangeException()
                };

                return $"{prefix}{SkillPieceSpec.Name}【{SkillPieceCellSpec.Name}】";
            }
        }

        public InstanceSkillPiece(
            int instanceId,
            int skillPieceSpecId,
            int skillPieceCellSpecId,
            Define.SkillPieceColor colorType,
            IEnumerable<int> skillSpecIds
            )
        {
            InstanceId = instanceId;
            SkillPieceSpecId = skillPieceSpecId;
            SkillPieceCellSpecId = skillPieceCellSpecId;
            ColorType = colorType;
            SkillSpecIds.AddRange(skillSpecIds);
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
            if (createSkillPieceSpec.Green) { availableColorTypes.Add(Define.SkillPieceColor.Green); }
            var skillAttachCount = UnityEngine.Random.Range(createSkillPieceSpec.SkillAttachCountMin, createSkillPieceSpec.SkillAttachCountMax + 1);
            var skillSpecIds = new List<int>();
            for (int i = 0; i < skillAttachCount; i++)
            {
                var skillAttachGroup = masterData.SkillAttachGroups.Get(createSkillPieceSpec.SkillAttachGroupId);
                var skillSpecId = skillAttachGroup[UnityEngine.Random.Range(0, skillAttachGroup.Count)].SkillSpecId;
                if (skillSpecIds.Contains(skillSpecId))
                {
                    i--;
                    continue;
                }
                skillSpecIds.Add(skillSpecId);
            }
            return new InstanceSkillPiece(
                instanceId,
                createSkillPieceSpec.SkillPieceSpecId,
                skillPieceCellSpec.Id,
                availableColorTypes[UnityEngine.Random.Range(0, availableColorTypes.Count)],
                skillSpecIds
                );
        }

        public bool IsMatch(SkillPieceFilterData filter)
        {
            if (filter == null)
            {
                return true;
            }
            if (filter.cellNumber != 0 && SkillPieceCellSpec.GroupId != filter.cellNumber)
            {
                return false;
            }

            if (filter.color != Define.SkillPieceColor.Gray && ColorType != filter.color)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(filter.cellName) && SkillPieceCellSpec.Name != filter.cellName)
            {
                return false;
            }

            return true;
        }
    }
}
