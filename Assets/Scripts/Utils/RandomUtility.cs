using System.Collections.Generic;
using UnityEngine;

public interface IWeighted
{
    float Weight { get; }
}

/// <summary>
///  RandomUtility cung cấp một phương thức để chọn ngẫu nhiên một phần tử từ một tập hợp các phần tử có trọng số khác nhau.
/// </summary>
public static class RandomUtility
{
    public static T GetRandomWeighted<T>(IEnumerable<T> items) where T : IWeighted
    {
        float totalWeight = 0f;
        foreach (var item in items) totalWeight += item.Weight;

        float roll = Random.value * totalWeight;

        foreach (var item in items)
        {
            if (roll < item.Weight) return item;
            roll -= item.Weight;
        }

        // Tr? v? ph?n t? d?u tiên n?u có l?i làm tròn s?
        using (var enumerator = items.GetEnumerator())
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }
    }
}