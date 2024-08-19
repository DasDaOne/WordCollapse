using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedRandom<T>
{
    private readonly List<KeyValuePair<T, float>> sortedWeights;

    public WeightedRandom(Dictionary<T, float> values)
    {
        sortedWeights = values.OrderBy(x => x.Value).ToList();
    }

    public T GetValue()
    {
        float randomValue = Random.value * sortedWeights.Sum(x => x.Value);

        foreach (var weight in sortedWeights)
        {
            randomValue -= weight.Value;

            if (randomValue <= 0)
                return weight.Key;
        }

        return default;
    }
}
