using System;
using System.Collections.Generic;
using System.Linq;
using HK;
using UnityEngine;

namespace StoryNothing.MasterDataSystems
{
    [Serializable]
    public sealed class SkillPieceCellSpec
    {
        public int Id;

        public int GroupId;

        public List<Vector2Int> CellPoints => ServiceLocator.Resolve<MasterData>().SkillPieceCellPoints.Get(Id).Select(x => new Vector2Int(x.X, x.Y)).ToList();

        [Serializable]
        public sealed class DictionaryList : DictionaryList<int, SkillPieceCellSpec>
        {
            public DictionaryList() : base(x => x.Id)
            {
            }
        }
    }
}
