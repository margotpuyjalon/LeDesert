using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluatedMap
{
    public Type[,] TypesMap { get; set; }
    public int Eval { get; set; }
    public int NbTours { get; set; }

    public EvaluatedMap(Type[,] t)
    {
        TypesMap = t;
        Eval = 0;
        NbTours = 0;
    }
}

public class EvalutaionPlayer
{
    Vector2 Position { get; set; }
    int ActionPoints { get; set; }

    public EvalutaionPlayer()
    {
        Position = new Vector2(0, 0);
        ActionPoints = 4;
    }

    public void TestMap(EvaluatedMap map, int nbTimes)
    {

    }
}

public class MapEvaluator
{
    // Evaluate a map
   public void EvaluateMap(EvaluatedMap m, int x, int y)
    {
        // Initialize positions to avoid conflics with true positions
        int hp1 = -1, vp1 = -1,
            hp2 = -2, vp2 = -2,
            hp3 = -3, vp3 = -3,
            hp4 = -4, vp4 = -4;
        int start = -1, end = -1;

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
                    || j == y - 1)
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
}
