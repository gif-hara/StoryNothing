using System;
using System.Collections.Generic;
using HK;
using UnityEngine;
using UnityEngine.Assertions;

namespace StoryNothing
{
    /// <summary>
    /// 採集ポイント
    /// </summary>
    [Serializable]
    public class CollectionSpot
    {
        /// <summary>
        /// 採集可能回数
        /// </summary>
        [SerializeField]
        private int count;

        [SerializeField]
        private List<ItemWeight> itemWeights;

        public CollectionSpot(int count, List<ItemWeight> itemWeights)
        {
            this.count = count;
            this.itemWeights = itemWeights;
        }

        public int Collection()
        {
            Assert.IsTrue(count >= 0, "採集可能回数が負の値になっています。");
            count--;

            return itemWeights.Lottery(i => i.Weight).ItemId;
        }

        public bool CanCollection()
        {
            return count > 0;
        }
    }
}
