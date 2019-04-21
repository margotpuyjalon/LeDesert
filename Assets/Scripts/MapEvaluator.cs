using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluatedMap
{
    public Type[,] TypesMap { get; set; }
    public int Eval { get; set; }
    public int NbTurns { get; set; }

    public EvaluatedMap(Type[,] t)
    {
        TypesMap = t;
        Eval = 0;
        NbTurns = 0;
    }
}

public class PlayerEvaluator
{
    Vector2 Position { get; set; }
    int ActionPoints { get; set; }
    bool[] DiscoveredTiles { get; set; }

    public PlayerEvaluator(int nbActions)
    {
        Position = new Vector2(0, 0);
        ActionPoints = nbActions;
    }

    // Simulate a game on the map
    public void TestMap(EvaluatedMap map, int x, int y, int nbTimes)
    {
        int nbTurns = 0;

        // Testting the map nbTimes times
        for (int k = 0; k < nbTimes; k++)
        {
            Debug.Log("TEST MAP: "+k);
            Vector2 start = new Vector2();
            bool[,] forbiddenTiles = new bool[x + 2, y + 2];

            // Run through all tiles
            for (int j = 0; j < y; j++)
            {
                for (int i = 0; i < x; i++)
                {
                    Type current = map.TypesMap[i, j];

                    // Get the start location
                    if (current == Type.START) start.Set(i, j);

                    // Set the forbidden tiles
                    forbiddenTiles[i + 1, j + 1] = true;
                    if (current == Type.STORM) forbiddenTiles[i + 1, j + 1] = false;
                }
            }

            // Set the player position on start
            Position = start;
            Debug.Log("START : x:" + Position.x + " y:" + Position.y);
            // Play until victory
            bool endDiscovered = false;
            while (endDiscovered == false)
            {
                // Move the player until no more ActionPoints
                for(int i=0; i<ActionPoints; i++)
                {
                    forbiddenTiles[(int)Position.x + 1, (int)Position.y + 1] = false;
                    Display(forbiddenTiles, 7, 7);
                    // Check if player can move
                    if (!forbiddenTiles[(int)Position.x + 2,    (int)Position.y + 1]    //droite
                        && !forbiddenTiles[(int)Position.x,     (int)Position.y + 1]    //gauche
                        && !forbiddenTiles[(int)Position.x + 1, (int)Position.y + 2]    //bas
                        && !forbiddenTiles[(int)Position.x + 1, (int)Position.y])       //haut
                    {
                        Debug.Log("CAN'T MOVE");
                        endDiscovered = true;
                    }
                    else
                    {
                        Vector2 moveTo = Move(Position, forbiddenTiles, x - 1, y - 1);
                        Position = new Vector2(moveTo.x, moveTo.y);
                        if (map.TypesMap[(int)Position.x, (int)Position.y] == Type.END)
                        {
                            Debug.Log("END DISCOVERED");
                            endDiscovered = true;
                        }
                        Debug.Log("ap : " + (ActionPoints-i) + " / tile : x:" + Position.x + " y:" + Position.y);
                    }
                    if (endDiscovered) break;
                }
                nbTurns++;
            }
            Debug.Log("nbTurns : " + nbTurns);
        }

        // Average nuber of turns needed to win the map
        map.NbTurns = nbTurns / nbTimes;
        Debug.Log("map tunrs : " + map.NbTurns);
    }

    // Move of 1 tile in a random direction
    private Vector2 Move(Vector2 currentPos, bool[,] forbiddenTiles, int maxX, int maxY)
    {
        int[] availableDirections = new int[] { -1, 0, 1 };
        bool move = true;
        int x = 0, y = 0;

        while (move)
        {
            x = availableDirections[Mathf.RoundToInt(Random.value * 2f)];
            if (x == 0)
            {
                y = availableDirections[Mathf.RoundToInt(Random.value * 2f)];
                if (y == 0)
                    move = true;
            }
            else
                y = 0;
            move = false;

            // Forbidden tiles
            if (!forbiddenTiles[(int)Position.x + x + 1, (int)Position.y + y + 1]) move = true;
        }

        Vector2 tile = new Vector2(currentPos.x + x, currentPos.y + y);
        return tile;        
    }

    // FOR DEBUG
    public void Display(bool[,] m, int x, int y)
    {
        string msg = "forbidden tiles \n";
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                msg += m[j, i] + " ";
            }
            msg += "\n";
        }
        Debug.Log(msg);
    }
}

public class MapEvaluator
{
    // Evaluate a map
   public void EvaluateMap(EvaluatedMap map, int x, int y)
    {
        // Initialize positions to avoid conflics with true positions
        int hp1 = -1, vp1 = -1,
            hp2 = -2, vp2 = -2,
            hp3 = -3, vp3 = -3,
            hp4 = -4, vp4 = -4;

        // Run through all tiles
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                Type current = map.TypesMap[i, j];
                // Check if the storm is on a border
                if ((i == 0
                    || i == x - 1
                    || j == 0
                    || j == y - 1)
                    && current == Type.STORM)
                {
                    map.Eval += 10;
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
            new Vector2( hp1, vp1 ),
            new Vector2( hp2, vp2 ),
            new Vector2( hp3, vp3 ),
            new Vector2( hp4, vp4 )
        };
        if (ContainsDuplicates(p)) map.Eval += 20;

        Debug.Log("Note map = "+map.Eval);

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