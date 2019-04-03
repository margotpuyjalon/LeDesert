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

public class MapGeneration
{
    private const int NB_MAP = 100;

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
                            Type.HP2, Type.VP2
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
                    }, 100));
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
        // Initialize clue positions to avoid conflics with true positions
        int hp1 = -1, vp1 = -1,
            hp2 = -2, vp2 = -2,
            hp3 = -3, vp3 = -3,
            hp4 = -4, vp4 = -4;

        // Run through all tiles
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Type current = m.TypesMap[j, i];
                // Check if the storm is on a border
                if ((i == 0
                    || i == x - 1
                    || j == 0 
                    || j == y-1)
                    && current == Type.STORM)
                {
                    m.Eval -= 10;
                }

                // Get clue location for ship's pieces
                if (current == Type.HP1) hp1 = i;
                if (current == Type.VP1) vp1 = j;
                if (current == Type.HP2) hp2 = i;
                if (current == Type.VP2) vp2 = j;
                if (current == Type.HP3) hp3 = i;
                if (current == Type.VP3) vp3 = j;
                if (current == Type.HP4) hp4 = i;
                if (current == Type.VP4) vp4 = j;
            }
        }

        // Check if some ship's pieces are on the same tile
        Vector2[] p = new Vector2[]
        {
            new Vector2( vp1, hp1 ),
            new Vector2( vp2, hp2 ),
            new Vector2( vp3, hp3 ),
            new Vector2( vp4, hp4 )
        };
        if (ContainsDuplicates(p)) m.Eval -= 20;
        
    }

    // Check duplicates in an array of Vectors
    private bool ContainsDuplicates(Vector2[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = i + 1; j < a.Length; j++)
            {
                if (a[i] == a[j]) return true;
            }
        }
        return false;
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

        foreach(EvaluatedMap m in mapList)
        {
            Display(m, 5, 5);
        }

        return mapList[0].TypesMap;
    }

    // FOR DEBUG
    void Display(EvaluatedMap m, int x, int y)
    {
        string msg = "map e = " + m.Eval + "\n";
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                msg += m.TypesMap[j, i] + " ";
            }
            msg += "\n";
        }
        Debug.Log(msg);
    }
}
