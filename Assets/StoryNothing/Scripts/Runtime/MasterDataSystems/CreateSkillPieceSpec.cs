using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class CreateSkillPieceSpec
    {
        public int Id;

        public int SkillPieceSpecId;

        public int SkillAttachGroupId;

        public int SkillAttachCountMin;

        public int SkillAttachCountMax;

        public bool Red;

        public bool Orange;

        public bool WhiteGray;

        public bool Purple;

        public bool Water;

        public bool Green;


        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, CreateSkillPieceSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
