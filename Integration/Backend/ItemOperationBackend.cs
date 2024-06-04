using System.Collections.Concurrent;
using System.Text.Json;
using Common;
using RabbitMQService;
using Repository;

namespace Integration.Backend;

public sealed class ItemOperationBackend
{
    private int _identitySequence;
    private ItemRepository _itemRepository { get; set; } = new();
    private Publisher _publisher { get; set; } = new();

    /// <summary>
    /// RabbitMQ tarafından veri kuyruğa eklenir.
    /// </summary>
    /// <param name="itemContent"></param>
    /// <returns></returns>
    public Item SaveItem(string itemContent)
    {
        var item = new Item();
        item.Content = itemContent;
        item.Id = GetNextIdentity();

        var bodyString = JsonSerializer.Serialize(item);
        _publisher.Publish(bodyString);

        return item;
    }

    public List<Item> FindItemsWithContent(string itemContent)
    {
        return _itemRepository.FindItemsWithContent(itemContent);
    }

    private int GetNextIdentity()
    {
        return Interlocked.Increment(ref _identitySequence);
    }

    public List<Item> GetAllItems()
    {
        return _itemRepository.GetAllItems();
    }
}