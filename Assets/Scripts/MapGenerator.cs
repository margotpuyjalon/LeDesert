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
    private const int NB_MAP_TO_CREATE = 100;
    private const int NB_MAP_TO_EVAL = 10;
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
							Type.STORM,
                            Type.TECH
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
        // x Eval too high
        if (x.Eval > y.Eval)
            comparison = 1;
        // good
        else if (y.Eval > x.Eval)
            comparison = -1;
        return comparison;
    }


    // The function called by Map.cs
    public Type[,] GetMap(int x, int y)
    {
        List<EvaluatedMap> mapList = new List<EvaluatedMap>();
        PlayerEvaluator player = new PlayerEvaluator(4, x, y);
        MapEvaluator evaluator = new MapEvaluator();
        System.Random rnd = new System.Random(5);

        CreateNewMaps(NB_MAP_TO_CREATE, mapList);

        // First evaluation
        foreach (EvaluatedMap m in mapList)
        {
            ShuffleTiles(m.TypesMap, rnd);  // Mutate the map
            evaluator.EvaluateForbiddenMap(m, x, y); // Give a mark according to piles positions
        }
        mapList.Sort(MapComparator);

        // Deep evaluation on the bests maps
        int averageNbTurn = 0;
        for (int i=0; i<NB_MAP_TO_EVAL; i++)
        {
            player.TestMap(mapList[i], NB_TEST_PER_MAP); // Give a mark according to number of turns before success or fail
            averageNbTurn += mapList[i].NbTurns;
        }
        averageNbTurn = averageNbTurn / NB_MAP_TO_EVAL;

        for(int i=0; i< NB_MAP_TO_EVAL; i++)Display(mapList[i], x, y);
        return mapList.Find(
            m => m.NbTurns > (averageNbTurn - 2) 
            && m.NbTurns < (averageNbTurn + 2)).TypesMap; // Return the best map
    }

    // FOR DEBUG
    void Display(EvaluatedMap m, int x, int y)
    {
        string msg = "map e = " + m.Eval + " et nbT = " + m.NbTurns + "\n";
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
