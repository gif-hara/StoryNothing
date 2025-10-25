using HK;

namespace StoryNothing
{
    public sealed class UIElementSkillPieceFilter
    {
        private readonly HKUIDocument document;

        public UIElementSkillPieceFilter(HKUIDocument document)
        {
            this.document = document;
            document.gameObject.SetActive(false);
        }
    }
}
