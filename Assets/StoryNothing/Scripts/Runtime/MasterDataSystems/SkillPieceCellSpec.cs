using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCellSpec
    {
        public int Id;

        public int X;

        public int Y;

        [Serializable]
        public sealed class Group : Group<int, SkillPieceCellSpec>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
