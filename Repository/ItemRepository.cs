using Common;
using System.Collections.Concurrent;


namespace Repository
{
    public class ItemRepository
    {
        public static ConcurrentBag<Item> SavedItems { get; set; } = new();
        private int _identitySequence;


        public Item SaveItem(Item item)
        {
            Thread.Sleep(2_000);

            SavedItems.Add(item);

            return item;
        }

        public List<Item> FindItemsWithContent(string itemContent)
        {
            return SavedItems.Where(x => x.Content == itemContent).ToList();
        }

        public List<Item> GetAllItems()
        {
            return SavedItems.ToList();
        }
    }
}
