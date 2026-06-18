public interface IResourceStorage
{
    int GetAmount(ResourceType type);//получить текущее количество ресурсов

    void RemoveResource(ResourceType type, int amount);//списать ресурсы
}