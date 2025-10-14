using System;
using System.Collections.Generic;
using System.Linq;
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

        [field: SerializeField]
        public List<PlacementSkillPiece> PlacementSkillPieces { get; private set; } = new();

        [field: SerializeField]
        public List<int> BingoBonusSkillSpecIds { get; private set; } = new();

        public SkillBoardSpec SkillBoardSpec => ServiceLocator.Resolve<MasterData>().SkillBoardSpecs.Get(SkillBoardMasterDataId);

        public string Name => SkillBoardSpec.Name;

        public InstanceCharacter CreateInstanceCharacter(int characterSpecId, UserData userData)
        {
            var gameRule = ServiceLocator.Resolve<GameRule>();
            var characterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(characterSpecId);
            var bingoCount = GetHorizontalBingoIndexes(userData).Count + GetVerticalBingoIndexes(userData).Count;
            var skillEffects = PlacementSkillPieces
                .SelectMany(x => x.InstanceSkillPiece.SkillEffects)
                .Concat(BingoBonusSkillSpecIds
                    .Take(bingoCount)
                    .SelectMany(x => ServiceLocator.Resolve<MasterData>().SkillEffects.Get(x)));
            var hitPointAdditional = 0;
            hitPointAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Red)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceHitPointUp;
            hitPointAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.HitPointUp)
                .Sum(x => (int)x.Amount);
            var hitPointAdditionalRate = 0.0f;
            hitPointAdditionalRate += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.BingoBonusHitPointUp)
                .Sum(x => x.Amount * bingoCount);
            var hitPoint = new CharacterParameter(
                characterSpec.HitPoint,
                hitPointAdditional,
                hitPointAdditionalRate
            );
            var attackAdditional = 0;
            attackAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Orange)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPiecePhysicalAttackUp;
            attackAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.AttackUp)
                .Sum(x => (int)x.Amount);
            var attackAdditionalRate = 0.0f;
            attackAdditionalRate += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.BingoBonusAttackUp)
                .Sum(x => x.Amount * bingoCount);
            var attack = new CharacterParameter(
                characterSpec.Attack,
                attackAdditional,
                attackAdditionalRate
            );
            var defenseAdditional = 0;
            defenseAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.WhiteGray)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPiecePhysicalDefenseUp;
            defenseAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.DefenseUp)
                .Sum(x => (int)x.Amount);
            var defenseAdditionalRate = 0.0f;
            defenseAdditionalRate += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.BingoBonusDefenseUp)
                .Sum(x => x.Amount * bingoCount);
            var defense = new CharacterParameter(
                characterSpec.Defense,
                defenseAdditional,
                defenseAdditionalRate
            );
            var speedAdditional = 0;
            speedAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Green)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceSpeedUp;
            speedAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.SpeedUp)
                .Sum(x => (int)x.Amount);
            var speedAdditionalRate = 0.0f;
            speedAdditionalRate += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.BingoBonusSpeedUp)
                .Sum(x => x.Amount * bingoCount);
            var speed = new CharacterParameter(
                characterSpec.Speed,
                speedAdditional,
                speedAdditionalRate
            );
            return new InstanceCharacter(
                characterSpecId,
                hitPoint,
                attack,
                defense,
                speed
            );
        }

        public InstanceSkillBoard(int instanceId, int masterDataSkillBoardId, List<Vector2Int> holes, List<int> bingoBonusSkillSpecIds)
        {
            InstanceId = instanceId;
            SkillBoardMasterDataId = masterDataSkillBoardId;
            Holes = holes;
            BingoBonusSkillSpecIds = bingoBonusSkillSpecIds;
        }

        public static InstanceSkillBoard Create(int instanceId, int skillBoardMasterDataId)
        {
            var masterData = ServiceLocator.Resolve<MasterData>();
            var holes = new List<Vector2Int>();
            var skillBoardSpec = masterData.SkillBoardSpecs.Get(skillBoardMasterDataId);
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
            var bingoBonusSkillSpecIds = new List<int>();
            var bingoBonusGroup = masterData.BingoBonusGroups.Get(skillBoardSpec.BingoBonusGroupId);
            foreach (var bingoBonus in bingoBonusGroup)
            {
                var skillAttachGroup = masterData.SkillAttachGroups.Get(bingoBonus.SkillAttachGroupId);
                while (true)
                {
                    var skillSpecId = skillAttachGroup[UnityEngine.Random.Range(0, skillAttachGroup.Count)].SkillSpecId;
                    if (bingoBonusSkillSpecIds.Contains(skillSpecId))
                    {
                        continue;
                    }
                    bingoBonusSkillSpecIds.Add(skillSpecId);
                    break;
                }
            }

            return new InstanceSkillBoard(instanceId, skillBoardMasterDataId, holes, bingoBonusSkillSpecIds);
        }

        public bool CanPlacementSkillPiece(UserData userData, InstanceSkillPiece instanceSkillPiece, Vector2Int positionIndex, int rotationIndex)
        {
            var cellPoints = instanceSkillPiece.SkillPieceCellSpec.GetCellPoints(rotationIndex);
            var skillPieceSize = instanceSkillPiece.SkillPieceCellSpec.GetSize(rotationIndex);
            var skillPieceCenterIndex = new Vector2Int(skillPieceSize.x / 2, skillPieceSize.y / 2);
            foreach (var cellPoint in cellPoints)
            {
                var boardPosition = positionIndex + cellPoint - skillPieceCenterIndex;
                if (boardPosition.x < 0 || boardPosition.x >= SkillBoardSpec.X || boardPosition.y < 0 || boardPosition.y >= SkillBoardSpec.Y)
                {
                    return false;
                }
                if (Holes.Contains(boardPosition))
                {
                    return false;
                }
                foreach (var placementSkillPiece in PlacementSkillPieces)
                {
                    var placedInstanceSkillPiece = userData.GetInstanceSkillPiece(placementSkillPiece.InstanceSkillPieceId);
                    var placedCellPoints = placedInstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(placementSkillPiece.RotationIndex);
                    var placedPieceSize = placedInstanceSkillPiece.SkillPieceCellSpec.GetSize(placementSkillPiece.RotationIndex);
                    var placedPieceCenterIndex = new Vector2Int(placedPieceSize.x / 2, placedPieceSize.y / 2);
                    foreach (var placedCellPoint in placedCellPoints)
                    {
                        var placedBoardPosition = placementSkillPiece.PositionIndex + placedCellPoint - placedPieceCenterIndex;
                        if (boardPosition == placedBoardPosition)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void AddPlacementSkillPiece(int instanceSkillPieceId, Vector2Int positionIndex, int rotationIndex, UserData userData)
        {
            PlacementSkillPieces.Add(new PlacementSkillPiece(instanceSkillPieceId, positionIndex, rotationIndex, userData));
        }

        public void RemovePlacementSkillPiece(int instanceSkillPieceId)
        {
            PlacementSkillPieces.RemoveAll(x => x.InstanceSkillPieceId == instanceSkillPieceId);
        }

        public List<int> GetHorizontalBingoIndexes(UserData userData)
        {
            var bingoIndexes = new List<int>();
            for (int y = 0; y < SkillBoardSpec.Y; y++)
            {
                var isBingo = true;
                var colorType = Define.SkillPieceColor.Gray;
                for (int x = 0; x < SkillBoardSpec.X; x++)
                {
                    if (Holes.Contains(new Vector2Int(x, y)))
                    {
                        isBingo = false;
                        break;
                    }
                    var isFilled = false;
                    foreach (var placementSkillPiece in PlacementSkillPieces)
                    {
                        if (placementSkillPiece.ContainsPositionIndex(new Vector2Int(x, y), userData))
                        {
                            if (colorType != Define.SkillPieceColor.Gray && colorType != placementSkillPiece.InstanceSkillPiece.ColorType)
                            {
                                isBingo = false;
                                break;
                            }
                            isFilled = true;
                            colorType = placementSkillPiece.InstanceSkillPiece.ColorType;
                            break;
                        }
                    }
                    if (!isFilled)
                    {
                        isBingo = false;
                        break;
                    }
                }
                if (isBingo)
                {
                    bingoIndexes.Add(y);
                }
            }
            return bingoIndexes;
        }

        public List<int> GetVerticalBingoIndexes(UserData userData)
        {
            var bingoIndices = new List<int>();
            for (int x = 0; x < SkillBoardSpec.X; x++)
            {
                var isBingo = true;
                var colorType = Define.SkillPieceColor.Gray;
                for (int y = 0; y < SkillBoardSpec.Y; y++)
                {
                    if (Holes.Contains(new Vector2Int(x, y)))
                    {
                        isBingo = false;
                        break;
                    }
                    var isFilled = false;
                    foreach (var placementSkillPiece in PlacementSkillPieces)
                    {
                        if (placementSkillPiece.ContainsPositionIndex(new Vector2Int(x, y), userData))
                        {
                            if (colorType != Define.SkillPieceColor.Gray && colorType != placementSkillPiece.InstanceSkillPiece.ColorType)
                            {
                                isBingo = false;
                                break;
                            }
                            isFilled = true;
                            colorType = placementSkillPiece.InstanceSkillPiece.ColorType;
                            break;
                        }
                    }
                    if (!isFilled)
                    {
                        isBingo = false;
                        break;
                    }
                }
                if (isBingo)
                {
                    bingoIndices.Add(x);
                }
            }
            return bingoIndices;
        }

        [Serializable]
        public class PlacementSkillPiece
        {
            [field: SerializeField]
            public int InstanceSkillPieceId { get; private set; }

            [field: SerializeField]
            public Vector2Int PositionIndex { get; private set; }

            [field: SerializeField]
            public int RotationIndex { get; private set; }

            public InstanceSkillPiece InstanceSkillPiece { get; private set; }

            public PlacementSkillPiece(int instanceSkillPieceId, Vector2Int positionIndex, int rotationIndex, UserData userData)
            {
                InstanceSkillPieceId = instanceSkillPieceId;
                PositionIndex = positionIndex;
                RotationIndex = rotationIndex;
                InstanceSkillPiece = userData.GetInstanceSkillPiece(InstanceSkillPieceId);
            }

            public bool ContainsPositionIndex(Vector2Int positionIndex, UserData userData)
            {
                var instanceSkillPiece = userData.GetInstanceSkillPiece(InstanceSkillPieceId);
                var cellPoints = instanceSkillPiece.SkillPieceCellSpec.GetCellPoints(RotationIndex);
                foreach (var cellPoint in cellPoints)
                {
                    var boardPosition = PositionIndex + cellPoint - new Vector2Int(instanceSkillPiece.SkillPieceCellSpec.GetSize(RotationIndex).x / 2, instanceSkillPiece.SkillPieceCellSpec.GetSize(RotationIndex).y / 2);
                    if (boardPosition == positionIndex)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
