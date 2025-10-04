using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillSpec
    {
        public int Id;

        public string Name;


        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
