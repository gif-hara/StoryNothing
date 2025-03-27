using System.Collections.Generic;

namespace StoryNothing
{
    public class UserData
    {
        private readonly Dictionary<int, int> inventory = new();

        public void AddItem(int itemId, int count)
        {
            if (inventory.ContainsKey(itemId))
            {
                inventory[itemId] += count;
            }
            else
            {
                inventory[itemId] = count;
            }
        }

        public void RemoveItem(int itemId, int count)
        {
            if (inventory.ContainsKey(itemId))
            {
                inventory[itemId] -= count;
                if (inventory[itemId] <= 0)
                {
                    inventory.Remove(itemId);
                }
            }
        }

        public int GetItemCount(int itemId)
        {
            return inventory.ContainsKey(itemId) ? inventory[itemId] : 0;
        }

        public bool SatisfactionCheck(int itemId, int count)
        {
            return inventory.ContainsKey(itemId) && inventory[itemId] >= count;
        }
    }
}
