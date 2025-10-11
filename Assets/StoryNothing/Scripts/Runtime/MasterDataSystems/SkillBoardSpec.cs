using System;
using HK;
using UnityEngine;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillBoardSpec
    {
        public int Id;

        public string Name;

        public int X;

        public int Y;

        public int HoleCount;

        public int BingoBonusGroupId;

        public Vector2Int Size => new(X, Y);

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillBoardSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
