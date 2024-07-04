using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static List<T> Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }   
        return list;
    }

}
