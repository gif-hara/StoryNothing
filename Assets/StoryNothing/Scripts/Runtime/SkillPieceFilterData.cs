namespace StoryNothing
{
    public class SkillPieceFilterData
    {
        public int cellNumber = 0;

        public Define.SkillPieceColor color = Define.SkillPieceColor.Gray;

        public string cellName = string.Empty;

        public SkillPieceFilterData()
        {
        }

        public SkillPieceFilterData(SkillPieceFilterData other)
        {
            cellNumber = other.cellNumber;
            color = other.color;
            cellName = other.cellName;
        }
    }
}
