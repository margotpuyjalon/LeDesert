using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Type
{
    SOURCE,
    TECH,
    START,
    END,
    TUNNEL,
    STORM,
    HP1,
    VP1,
    HP2,
    VP2,
    HP3,
    VP3,
    HP4,
    VP4
}

public class EvaluatedMap
{
    public Type[,] TypesMap { get; set; }
    public int Eval { get; set; }

    public EvaluatedMap(Type[,] t, int e)
    {
        TypesMap = t;
        Eval = e;
    }
}

public class MapGeneration : MonoBehaviour
{
    private const int NB_MAP = 10;

    // Create n basic map
    void CreateNewMaps(int n, List<EvaluatedMap> map)
    {
        for (int i = 0; i < n; i++)
        {
            map.Add(
                new EvaluatedMap(
                    new Type[,]
                    {
                        {
                            Type.SOURCE,
                            Type.TUNNEL, Type.TUNNEL,
                            Type.START,
                            Type.END
                        },
                        {
                            Type.HP1, Type.VP1,
                            Type.TECH,
                            Type.TECH, Type.TECH
                        },
                        {
                            Type.STORM, Type.TECH, Type.TECH, Type.TECH, Type.TECH
                        },
                        {
                            Type.TECH, Type.TECH, Type.TECH, Type.TECH, Type.TECH
                        },
                        {
                            Type.TECH, Type.TECH, Type.TECH, Type.TECH, Type.TECH
                        }
                    }, 10));
        }
    }

    // Shuffle the array passed as a parameter
    void ShuffleTiles(Type[,] array, System.Random random)
    {
        int lengthRow = array.GetLength(1);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int i0 = i / lengthRow;
            int i1 = i % lengthRow;

            int j = random.Next(i + 1);
            int j0 = j / lengthRow;
            int j1 = j % lengthRow;

            Type temp = array[i0, i1];
            array[i0, i1] = array[j0, j1];
            array[j0, j1] = temp;
        }
    }

    // Evaluate a map
    void EvaluateMap(EvaluatedMap m, int x, int y)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // Check if the storm is on a border
                if ((i == 0
                    || i == x - 1
                    || j == 0 
                    || j == y-1)
                    && m.TypesMap[j, i] == Type.STORM)
                {
                    m.Eval = 0;
                }
            }
        }
    }

    // EvaluatedMap comparer
    private static int EvMapComparer (EvaluatedMap x, EvaluatedMap y)
    {
        int comparison = 0;
        // x greater
        if (x.Eval > y.Eval)
            comparison = -1;
        // y greater
        else if (y.Eval > x.Eval)
            comparison = 1;
        return comparison;
    }


    // The function called by Map.cs
    public Type[,] GetMap(int x, int y)
    {
        List<EvaluatedMap> mapList = new List<EvaluatedMap>();
        System.Random rnd = new System.Random();

        CreateNewMaps(NB_MAP, mapList);

        foreach (EvaluatedMap m in mapList)
        {
            ShuffleTiles(m.TypesMap, rnd);
            EvaluateMap(m, x, y);
        }

        mapList.Sort(EvMapComparer);

        return mapList[0].TypesMap;
    }

    // FOR DEBUG
    //void Display(EvaluatedMap m, int x, int y)
    //{
    //    string msg = "map e = "+m.Eval + "\n";
    //    for (int i = 0; i < x; i++)
    //    {
    //        for (int j = 0; j < y; j++)
    //        {
    //            msg += m.TypesMap[j, i] + " ";
    //        }
    //        msg += "\n";
    //    }
    //    print(msg);
    //}
}
