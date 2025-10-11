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

        public SkillBoardSpec SkillBoardSpec => ServiceLocator.Resolve<MasterData>().SkillBoardSpecs.Get(SkillBoardMasterDataId);

        public string Name => SkillBoardSpec.Name;

        public InstanceCharacter CreateInstanceCharacter(int characterSpecId)
        {
            var gameRule = ServiceLocator.Resolve<GameRule>();
            var characterSpec = ServiceLocator.Resolve<MasterData>().CharacterSpecs.Get(characterSpecId);
            var hitPointAdditional = 0;
            var skillEffects = PlacementSkillPieces
                .SelectMany(x => x.InstanceSkillPiece.SkillEffects);
            hitPointAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Red)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceHitPointUp;
            hitPointAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.HitPointUp)
                .Sum(x => (int)x.Amount);
            var hitPoint = new CharacterParameter(
                characterSpec.HitPoint,
                hitPointAdditional,
                0.0f
            );
            var physicalAttackAdditional = 0;
            physicalAttackAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Orange)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPiecePhysicalAttackUp;
            physicalAttackAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.PhysicalAttackUp)
                .Sum(x => (int)x.Amount);
            var physicalAttack = new CharacterParameter(
                characterSpec.PhysicalAttack,
                physicalAttackAdditional,
                0.0f
            );
            var physicalDefenseAdditional = 0;
            physicalDefenseAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.WhiteGray)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPiecePhysicalDefenseUp;
            physicalDefenseAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.PhysicalDefenseUp)
                .Sum(x => (int)x.Amount);
            var physicalDefense = new CharacterParameter(
                characterSpec.PhysicalDefense,
                physicalDefenseAdditional,
                0.0f
            );
            var magicalAttackAdditional = 0;
            magicalAttackAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Purple)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceMagicalAttackUp;
            magicalAttackAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.MagicalAttackUp)
                .Sum(x => (int)x.Amount);
            var magicalAttack = new CharacterParameter(
                characterSpec.MagicalAttack,
                magicalAttackAdditional,
                0.0f
            );
            var magicalDefenseAdditional = 0;
            magicalDefenseAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Water)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceMagicalDefenseUp;
            magicalDefenseAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.MagicalDefenseUp)
                .Sum(x => (int)x.Amount);
            var magicalDefense = new CharacterParameter(
                characterSpec.MagicalDefense,
                magicalDefenseAdditional,
                0.0f
            );
            var speedAdditional = 0;
            speedAdditional += PlacementSkillPieces
                .Where(x => x.InstanceSkillPiece.ColorType == Define.SkillPieceColor.Green)
                .Sum(x => x.InstanceSkillPiece.SkillPieceCellSpec.GetCellPoints(0).Count) * gameRule.SkillPieceSpeedUp;
            speedAdditional += skillEffects
                .Where(x => x.SkillEffectType == Define.SkillEffectType.SpeedUp)
                .Sum(x => (int)x.Amount);
            var speed = new CharacterParameter(
                characterSpec.Speed,
                speedAdditional,
                0.0f
            );
            return new InstanceCharacter(
                characterSpecId,
                hitPoint,
                physicalAttack,
                physicalDefense,
                magicalAttack,
                magicalDefense,
                speed
            );
        }

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
