using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public class ItemSpec
    {

        public int Id;

        public string Name;

        [Serializable]
        public class DictionaryList : DictionaryList<int, ItemSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
