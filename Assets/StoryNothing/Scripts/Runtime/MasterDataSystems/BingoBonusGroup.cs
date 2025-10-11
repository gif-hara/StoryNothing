using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class BingoBonusGroup
    {
        public int Id;

        public int SkillAttachGroupId;

        [Serializable]
        public sealed class Group : Group<int, BingoBonusGroup>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
