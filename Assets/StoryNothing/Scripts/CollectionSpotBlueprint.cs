using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace StoryNothing
{
    /// <summary>
    /// <see cref="CollectionSpot"/>を生成するクラス
    /// </summary>
    [Serializable]
    public class CollectionSpotBlueprint
    {
        /// <summary>
        /// 採集可能回数の最小値
        /// </summary>
        [SerializeField]
        private int collectionNumberMin;

        [SerializeField]
        private int collectionNumberMax;

        [SerializeField]
        private List<ItemWeight> itemWeights;

        public CollectionSpot Create()
        {
            Assert.IsTrue(collectionNumberMin >= 0, "採集可能回数の最小値が負の値になっています。");
            Assert.IsTrue(collectionNumberMax >= 0, "採集可能回数の最大値が負の値になっています。");
            Assert.IsTrue(collectionNumberMin <= collectionNumberMax, "採集可能回数の最小値が最大値を超えています。");

            var count = UnityEngine.Random.Range(collectionNumberMin, collectionNumberMax + 1);
            return new CollectionSpot(count, itemWeights);
        }
    }
}
