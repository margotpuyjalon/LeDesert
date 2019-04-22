using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Card
{
    FORBIDDEN,
    HIDDEN,
    DISCOVERED
    
}

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

        Debug.Log("Note map = " + map.Eval);

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

public class PlayerEvaluator
{
    Vector2 Position { get; set; }
    int ActionPoints { get; set; }
    Card[,] PlayerBoard { get; set; }
    int X, Y; // Map's size

    public PlayerEvaluator(int nbActions, int x, int y)
    {
        Position = new Vector2(0, 0);
        ActionPoints = nbActions;
        PlayerBoard = new Card[x + 2, y + 2];
        X = x; Y = y;
    }

    // Simulate a game on the map
    public void TestMap(EvaluatedMap map, int nbTimes)
    {
        int nbTurns = 0;

        // Testting the map nbTimes times
        for (int k = 0; k < nbTimes; k++)
        {
            Debug.Log("TEST MAP: "+k);
            Vector2 start = new Vector2();
            

            // Run through all tiles
            for (int j = 0; j < Y; j++)
            {
                for (int i = 0; i < X; i++)
                {
                    Type current = map.TypesMap[i, j];

                    // Get the start location
                    if (current == Type.START) start.Set(i, j);

                    // Set the forbidden tiles
                    PlayerBoard[i + 1, j + 1] = Card.HIDDEN;
                    if (current == Type.STORM) PlayerBoard[i + 1, j + 1] = Card.FORBIDDEN;
                }
            }

            // Set the player position on start
            Position = start;
            Debug.Log("START : x:" + Position.x + " y:" + Position.y);
            // Play until victory
            // Have to find all clues and the end to win
            bool victory = false;
            bool[] clues = new bool[8];
            bool end = false;
            for (int j=0; j<1000; j++)
            {
                // Player time until no more ActionPoints
                nbTurns++;
                int ap = ActionPoints;
                while (ap > 0)
                {
                    Display(PlayerBoard, 7, 7);

                    // DISCOVER the tile under if not already discovered
                    // And check the discovered tile
                    Type tile = map.TypesMap[(int)Position.x, (int)Position.y];
                    if (ap > 0 && PlayerBoard[(int)Position.x + 1, (int)Position.y + 1] == Card.HIDDEN)
                    {
                        ap--; // Use one action point to discover
                        Debug.Log("DISC : ap = " + ap + " / tile : x:" + Position.x + " y:" + Position.y);
                        PlayerBoard[(int)Position.x + 1, (int)Position.y + 1] = Card.DISCOVERED;
                        switch (tile)
                        {
                            case Type.END:
                                end = true;
                                break;
                            case Type.HP1:
                                clues[0] = true;
                                break;
                            case Type.VP1:
                                clues[1] = true;
                                break;
                            case Type.HP2:
                                clues[2] = true;
                                break;
                            case Type.VP2:
                                clues[3] = true;
                                break;
                            case Type.HP3:
                                clues[4] = true;
                                break;
                            case Type.VP3:
                                clues[5] = true;
                                break;
                            case Type.HP4:
                                clues[6] = true;
                                break;
                            case Type.VP4:
                                clues[7] = true;
                                break;
                        }
                    }
                    if (clues[0] && clues[1] && clues[2] && clues[3] &&
                        clues[4] && clues[5] && clues[6] && clues[7] && end)
                    {
                        victory = true;
                    }

                    // MOVE
                    if (ap > 0)
                    {
                        ap--; // Use one action point to move
                        Vector2 moveTo = Move(Position, PlayerBoard);
                        Position = new Vector2(moveTo.x, moveTo.y);
                        Debug.Log("MOVE : ap = " + ap + " / tile : x:" + Position.x + " y:" + Position.y);
                    }

                    if (victory) break;
                }
                if (victory) break;
                if (j == 999) Debug.Log("OUT OF TIME FOR VICTORY");
                // Tornado time
            }
            Debug.Log("nbTurns : " + nbTurns);
        }

        // Average nuber of turns needed to win the map
        map.NbTurns = nbTurns / nbTimes;
        Debug.Log("map tunrs : " + map.NbTurns);
    }

    // Move of 1 tile
    private Vector2 Move(Vector2 currentPos, Card[,] playerBoard)
    {
        int[] xAvailableDirections = new int[] { -1, 0, 1 };
        int[] yAvailableDirections = new int[] { -1, 0, 1 };
        Vector2 direction = new Vector2();

        // Can't go on forbidden tiles
        if (playerBoard[(int)Position.x + 1, (int)Position.y] == Card.FORBIDDEN) // haut
            yAvailableDirections[0] = 0;
        if (playerBoard[(int)Position.x + 1, (int)Position.y + 2] == Card.FORBIDDEN) // bas
            yAvailableDirections[2] = 0;
        if (playerBoard[(int)Position.x, (int)Position.y + 1] == Card.FORBIDDEN) // gauche
            xAvailableDirections[0] = 0;
        if (playerBoard[(int)Position.x + 2, (int)Position.y + 1] == Card.FORBIDDEN) // droite
            xAvailableDirections[2] = 0;

        Debug.Log("x dir : " + xAvailableDirections[0] + ", " + xAvailableDirections[1] + ", " + xAvailableDirections[2] + "\n"
            + "y dir : " + yAvailableDirections[0] + ", " + yAvailableDirections[1] + ", " + yAvailableDirections[2]);

        // Prioritizes hidden tiles
        if (playerBoard[(int)Position.x + 1, (int)Position.y] == Card.HIDDEN) // haut
        { direction.x = 0; direction.y = -1; }
        else
        {
            if (playerBoard[(int)Position.x + 2, (int)Position.y + 1] == Card.HIDDEN) // droite
            { direction.x = 1; direction.y = 0; }
            else
            {
                if (playerBoard[(int)Position.x + 1, (int)Position.y + 2] == Card.HIDDEN) // bas
                { direction.x = 0; direction.y = 1; }
                else
                {
                    if (playerBoard[(int)Position.x, (int)Position.y + 1] == Card.HIDDEN) // gauche
                    { direction.x = -1; direction.y = 0; }
                    else
                        direction = ChooseDirection(xAvailableDirections, yAvailableDirections);
                }                
            }
        }
        

        Vector2 tile = new Vector2(currentPos.x + direction.x, currentPos.y + direction.y);
        return tile;        
    }

    // Choose a random available direction
    private Vector2 ChooseDirection(int[] xDir, int[] yDir)
    {
        bool directionChoosen = false;
        Vector2 direction = new Vector2();
        for(int i=0; i<1000; i++)
        {
            direction.x = xDir[Mathf.RoundToInt(Random.value * 2f)];
            if (direction.x == 0)
            {
                direction.y = yDir[Mathf.RoundToInt(Random.value * 2f)];
                if (direction.y != 0)
                    directionChoosen = true;
            }
            else
            {
                direction.y = 0;
                directionChoosen = true;
            }
            if (directionChoosen) break;
            if (i == 999) Debug.Log("OUT OF TIME !");
        }
        return direction;
    }

    // FOR DEBUG
    public void Display(Card[,] m, int x, int y)
    {
        string msg = "Player board \n";
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
