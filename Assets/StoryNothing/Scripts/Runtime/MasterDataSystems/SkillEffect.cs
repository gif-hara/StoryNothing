using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillEffect
    {
        public int Id;

        public Define.SkillEffectType SkillEffectType;

        public float Amount;

        [Serializable]
        public sealed class Group : Group<int, SkillEffect>
        {
            public Group() : base(x => x.Id)
            {
            }
        }
    }
}
