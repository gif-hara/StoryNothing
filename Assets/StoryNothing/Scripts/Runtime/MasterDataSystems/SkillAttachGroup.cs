using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillAttachGroup
    {
        public int Id;

        public int SkillSpecId;


        [Serializable]
        public sealed class Group : Group<int, SkillAttachGroup>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
