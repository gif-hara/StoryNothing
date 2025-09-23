using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCell
    {
        public int Id;

        public int X;

        public int Y;

        [Serializable]
        public sealed class Group : Group<int, SkillPieceCell>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
