namespace StoryNothing
{
    public class ItemWeight
    {
        public int ItemId { get; }

        public int Weight { get; }

        public ItemWeight(int itemId, int weight)
        {
            ItemId = itemId;
            Weight = weight;
        }
    }
}
