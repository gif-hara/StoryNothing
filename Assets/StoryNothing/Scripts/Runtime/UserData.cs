using System.Collections.Generic;
using StoryNothing.InstanceData;
using UnityEngine.Assertions;

namespace StoryNothing
{
    public sealed class UserData
    {
        public Dictionary<int, InstanceSkillBoard> SkillBoards { get; private set; } = new();

        public Dictionary<int, InstanceSkillPiece> SkillPieces { get; private set; } = new();

        public int AddInstanceSkillBoardCount { get; private set; } = 0;

        public int AddInstanceSkillPieceCount { get; private set; } = 0;

        public int EquipInstanceSkillBoardId { get; set; } = -1;

        public void AddInstanceSkillBoard(InstanceSkillBoard instanceSkillBoard)
        {
            SkillBoards.Add(instanceSkillBoard.InstanceId, instanceSkillBoard);
            AddInstanceSkillBoardCount++;
        }

        public void AddInstanceSkillPiece(InstanceSkillPiece instanceSkillPiece)
        {
            SkillPieces.Add(instanceSkillPiece.InstanceId, instanceSkillPiece);
            AddInstanceSkillPieceCount++;
        }

        public InstanceSkillBoard GetEquipInstanceSkillBoard()
        {
            var result = SkillBoards.TryGetValue(EquipInstanceSkillBoardId, out var skillBoard) ? skillBoard : null;
            Assert.IsNotNull(result, $"EquipInstanceSkillBoard is null: {EquipInstanceSkillBoardId}");
            return result;
        }
    }
}
