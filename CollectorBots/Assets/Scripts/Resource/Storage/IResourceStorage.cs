public interface IResourceStorage
{
    int GetAmount(ResourceType type);

    void RemoveResource(ResourceType type, int amount);
}