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

        [field: SerializeField]
        public List<PlacementSkillPiece> PlacementSkillPieces { get; private set; } = new();

        public SkillBoardSpec SkillBoardSpec => ServiceLocator.Resolve<MasterData>().SkillBoardSpecs.Get(SkillBoardMasterDataId);

        public string Name => SkillBoardSpec.Name;

        public InstanceCharacter CreateInstanceCharacter(int characterSpecId)
        {
            return new InstanceCharacter(characterSpecId);
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

        public void AddPlacementSkillPiece(int instanceSkillPieceId, Vector2Int positionIndex, int rotationIndex)
        {
            PlacementSkillPieces.Add(new PlacementSkillPiece(instanceSkillPieceId, positionIndex, rotationIndex));
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

            public PlacementSkillPiece(int instanceSkillPieceId, Vector2Int positionIndex, int rotationIndex)
            {
                InstanceSkillPieceId = instanceSkillPieceId;
                PositionIndex = positionIndex;
                RotationIndex = rotationIndex;
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
