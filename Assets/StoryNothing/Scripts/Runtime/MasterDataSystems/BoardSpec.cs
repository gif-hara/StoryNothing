using System;
using HK;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class BoardSpec
    {
        public int Id;

        public int X;

        public int Y;

        public int HoleCount;

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, BoardSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
