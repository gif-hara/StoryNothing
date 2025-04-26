using System.Collections.Generic;

namespace StoryNothing
{
    public class UserData
    {
        public readonly Dictionary<int, int> Items = new();

        public void AddItem(int itemId, int count)
        {
            if (Items.ContainsKey(itemId))
            {
                Items[itemId] += count;
            }
            else
            {
                Items[itemId] = count;
            }
        }

        public void RemoveItem(int itemId, int count)
        {
            if (Items.ContainsKey(itemId))
            {
                Items[itemId] -= count;
                if (Items[itemId] <= 0)
                {
                    Items.Remove(itemId);
                }
            }
        }

        public int GetItemCount(int itemId)
        {
            return Items.ContainsKey(itemId) ? Items[itemId] : 0;
        }

        public bool SatisfactionCheck(int itemId, int count)
        {
            return Items.ContainsKey(itemId) && Items[itemId] >= count;
        }
    }
}
