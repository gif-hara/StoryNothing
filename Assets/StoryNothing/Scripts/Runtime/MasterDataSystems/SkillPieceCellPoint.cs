using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCellPoint
    {
        public int Id;

        public int X;

        public int Y;

        [Serializable]
        public sealed class Group : Group<int, SkillPieceCellPoint>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
