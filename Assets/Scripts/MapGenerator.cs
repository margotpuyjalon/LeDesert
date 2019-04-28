using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Tile type
/// </summary>
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

/// <summary>
/// Generate a map to play with
/// </summary>
public class MapGenerator
{
    private const int NB_MAP_TO_CREATE = 10000;
    private const int NB_MAP_TO_TEST = 100;

    /// <summary>
    /// The function called by Map.cs 
    /// </summary>
    /// <param name="x">Map's width</param>
    /// <param name="y">Map's height</param>
    /// <returns>An array of Type used to create the map in game</returns>
    public Type[,] GetMap(int x, int y)
    {
        List<EvaluatedMap> mapList = new List<EvaluatedMap>();
        Evaluator player = new Evaluator(4);       
        System.Random rnd = new System.Random();

        CreateNewMaps(NB_MAP_TO_CREATE, mapList); // TO FIX : create map dynamically

        // First evaluation
        foreach (EvaluatedMap m in mapList)
        {
            ShuffleTiles(m.TypesMap, rnd);  // Mutate the map
            player.InitMapKnowledge(m);
            player.EvaluateMap(m); // Give a mark according to piles positions
        }
        mapList.Sort(MapComparator);

        // Deep evaluation on the bests maps
        int averageNbTurn = 0;
        for (int i=0; i< NB_MAP_TO_TEST; i++)
        {
            player.TestMap(mapList[i]); // Give a mark according to number of turns before success or fail
            averageNbTurn += mapList[i].NbTurns;
        }
        averageNbTurn = averageNbTurn / NB_MAP_TO_TEST;

        // for(int i=0; i< NB_MAP_TO_TEST; i++)Display(mapList[i], x, y);
        EvaluatedMap theMap = mapList.Find(
            m => m.NbTurns > (averageNbTurn - 5)
            && m.NbTurns < (averageNbTurn + 5));
        Display(theMap, 5, 5);
        return theMap.TypesMap; // Return the best map
    }

    //*********************************************************************************************//
    //**************************          PRIVATE FUNCTIONS           *****************************//
    //*********************************************************************************************//

    /// <summary>
    /// Create n basic EvaluatedMap
    /// </summary>
    /// <param name="n">Number of map to create</param>
    /// <param name="listMap">The list to fill</param>
    private void CreateNewMaps(int n, List<EvaluatedMap> listMap)
    {
        for (int i = 0; i < n; i++)
        {
            listMap.Add(
                new EvaluatedMap(
                    new Type[,] //TO FIX : POUVOIR SAISIR LA TAILLE DU TABLEAU DYNAMIQUEMENT
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
                    }, 5, 5));
        }
    }

    /// <summary>
    /// Shuffle an array according to a given random
    /// </summary>
    /// <param name="array">The array of Type</param>
    /// <param name="random">The random</param>
    private void ShuffleTiles(Type[,] array, System.Random random)
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

    /// <summary>
    /// EvaluatedMap comparer. 
    /// The best map has got the lowest score.
    /// </summary>
    /// <param name="a">EvaluatedMap a</param>
    /// <param name="b">EvaluatedMap b</param>
    /// <returns>0 : equals / 1 : b / -1 : a</returns>
    private static int MapComparator(EvaluatedMap a, EvaluatedMap b)
    {
        int comparison = 0;
        // x Eval too high
        if (a.Mark > b.Mark)
            comparison = 1;
        // good
        else if (b.Mark > a.Mark)
            comparison = -1;
        return comparison;
    }

    // FOR DEBUG
    void Display(EvaluatedMap m, int x, int y)
    {
        string msg = "map e = " + m.Mark + " et nbT = " + m.NbTurns + "\n";
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
