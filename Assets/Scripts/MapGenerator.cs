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

public class MapGenerator
{
    private const int NB_MAP_TO_CREATE = 10;
    private const int NB_TEST_PER_MAP = 3;

    // Create n basic map
    void CreateNewMaps(int n, List<EvaluatedMap> map)
    {
        for (int i = 0; i < n; i++)
        {
            map.Add(
                new EvaluatedMap(
                    new Type[,] //TOFIX : POUVOIR SAISIR LA TAILLE DU TABLEAU DYNAMIQUEMENT
                    {
                        {
                            Type.SOURCE,
                            Type.TUNNEL, Type.TUNNEL,
                            Type.START,
                            Type.END
                        },
                        {
                            Type.HP1, Type.VP1,
                            Type.HP2, Type.VP2,
                            Type.HP3
                        },
                        {
							Type.VP3,
							Type.HP4, Type.VP4,
							Type.STORM, Type.TECH
                        },
                        {
                            Type.TECH, Type.TECH, Type.TECH, Type.TECH, Type.TECH
                        },
                        {
                            Type.TECH, Type.TECH, Type.TECH, Type.TECH, Type.TECH
                        }
                    }));
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

    // EvaluatedMap comparer
    // The best map has got the lowest score
    private static int MapComparator (EvaluatedMap x, EvaluatedMap y)
    {
        int comparison = 0;
        // x greater
        if (x.Eval > y.Eval)
            comparison = 1;
        // y greater
        else if (y.Eval > x.Eval)
            comparison = -1;
        return comparison;
    }


    // The function called by Map.cs
    public Type[,] GetMap(int x, int y)
    {
        List<EvaluatedMap> mapList = new List<EvaluatedMap>();
        PlayerEvaluator player = new PlayerEvaluator(4);
        MapEvaluator evaluator = new MapEvaluator();
        System.Random rnd = new System.Random(5);

        CreateNewMaps(NB_MAP_TO_CREATE, mapList);

        foreach (EvaluatedMap m in mapList)
        {
            ShuffleTiles(m.TypesMap, rnd);  // Mutate the map
            evaluator.EvaluateMap(m, x, y); // Give a mark according to piles positions
            player.TestMap(m, x, y, NB_TEST_PER_MAP);    // Give a mark according to number of turns before success or fail
        }

        mapList.Sort(MapComparator);

        Display(mapList[0], x, y);
        return mapList[0].TypesMap; // Return the best map
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
