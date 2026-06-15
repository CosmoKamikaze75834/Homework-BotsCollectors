using System;
using Random = UnityEngine.Random;

public class RandomResourceGenerator
{
    private int _minValue = 0;

    public ResourceType GetRandomResourceType()
    {
        Array values = Enum.GetValues(typeof(ResourceType));

        int randomIndex = Random.Range(_minValue, values.Length);

        return (ResourceType)values.GetValue(randomIndex);
    }
}