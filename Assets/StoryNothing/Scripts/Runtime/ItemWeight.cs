using System;
using UnityEngine;

namespace StoryNothing
{
    [Serializable]
    public class ItemWeight
    {
        [SerializeField]
        private int itemId;
        public int ItemId => itemId;

        [SerializeField]
        private int weight;
        public int Weight => weight;

        public ItemWeight(int itemId, int weight)
        {
            this.itemId = itemId;
            this.weight = weight;
        }
    }
}
