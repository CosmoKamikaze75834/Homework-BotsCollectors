public class DirectPickupStrategy : IResourceCollectionStrategy
{
    public void Collect(Collector collector) => collector.AttempToPickupCurrentResource();
}