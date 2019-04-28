using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tiles from the evaluator view
/// </summary>
public enum Card
{
    FORBIDDEN,
    HIDDEN,
    DISCOVERED
    
}
/// <summary>
/// The map used for evaluations and tests 
/// </summary>
public class EvaluatedMap
{
    public Type[,] TypesMap;
    public Card[,] EvaluatorBoard;
    public int Mark;
    public int NbTurns;
    public int X, Y;
    public Vector2 Start;
    public Vector2 Piece1, Piece2, Piece3, Piece4;
    public Vector2 Tunnel1, Tunnel2;
    public Vector2 Storm;

    public EvaluatedMap(Type[,] t, int x, int y)
    {
        TypesMap = t;
        EvaluatorBoard = new Card[x + 2, y + 2];
        Mark = 0;
        NbTurns = 0;
        X = x; Y = y;
        Start = new Vector2(0, 0);
        Piece1 = new Vector2(0, 0);
        Piece2 = new Vector2(0, 0);
        Piece3 = new Vector2(0, 0);
        Piece4 = new Vector2(0, 0);
        Tunnel1 = new Vector2(0, 0);
        Tunnel2 = new Vector2(0, 0);
        Storm = new Vector2(0, 0);
    }
}

/// <summary>
/// A bot which performs tests and evaluations on an EvaluatedMap
/// </summary>
public class Evaluator
{
    int ActionPoints;
    Vector2 Position;

    public Evaluator(int nbActions)
    {
        ActionPoints = nbActions;
        Position = new Vector2(0, 0);
    }

    /// <summary>
    /// Initialize the evaluator's given map knowledge.
    /// Must be called before any evaluation or test of an EvaluatedMap.
    /// </summary>
    /// <param name="map">The map that will be evaluated</param>
    public void InitMapKnowledge(EvaluatedMap map)
    {
        bool firstTunnelFound = false;

        // Run through all tiles
        for (int j = 0; j < map.Y; j++)
        {
            for (int i = 0; i < map.X; i++)
            {
                Type current = map.TypesMap[i, j];

                // Set the forbidden tiles
                map.EvaluatorBoard[i + 1, j + 1] = Card.HIDDEN;

                // Get the storm location
                if (current == Type.STORM)
                {
                    map.Storm.Set(i,j);
                    map.EvaluatorBoard[i + 1, j + 1] = Card.FORBIDDEN;
                }
                // Get clue locations for pieces
                else if (current == Type.HP1) map.Piece1.x = i;
                else if (current == Type.VP1) map.Piece1.y = j;
                else if (current == Type.HP2) map.Piece2.x = i;
                else if (current == Type.VP2) map.Piece2.y = j;
                else if (current == Type.HP3) map.Piece3.x = i;
                else if (current == Type.VP3) map.Piece3.y = j;
                else if (current == Type.HP4) map.Piece4.x = i;
                else if (current == Type.VP4) map.Piece4.y = j;
                // Get 2 fisrt tunnels locations
                else if (current == Type.TUNNEL && !firstTunnelFound)
                {
                    map.Tunnel1.Set(i,j);
                    firstTunnelFound = true;
                }
                else if (current == Type.TUNNEL) map.Tunnel2.Set(i, j);
                // Get the start location
                else if (current == Type.START) map.Start.Set(i, j);
            }
        }
        // Display(map.EvaluatorBoard, 7, 7);
    }

    /// <summary>
    /// Give a mark to the given EvaluatedMap from its composition of tiles
    /// </summary>
    /// <param name="map">The map to evaluate</param>
    public void EvaluateMap(EvaluatedMap map)
    {
        // Check if the storm is on a border
        if (map.Storm.x == 0
            || map.Storm.x == map.X - 1
            || map.Storm.y == 0
            || map.Storm.y == map.Y - 1)
        {
            map.Mark += 20;
        }
        
        // Check if some pieces are on the same tile
        Vector2[] p = new Vector2[] { map.Piece1, map.Piece2, map.Piece3, map.Piece4 };
        if (ContainsDuplicates(p)) map.Mark += 30;

        // Check if 2 tunnels are too close
        if (map.Tunnel1.x == map.Tunnel2.x // same column
            && ((map.Tunnel1.y - map.Tunnel2.y) == 1) || (map.Tunnel2.y - map.Tunnel1.y) == 1)
            map.Mark += 10;
        if (map.Tunnel1.y == map.Tunnel2.y // same row
            && ((map.Tunnel1.x - map.Tunnel2.x) == 1) || (map.Tunnel2.x - map.Tunnel1.x) == 1)
            map.Mark += 10;

        // Debug.Log("Note map = " + map.Mark);
    }

    /// <summary>
    /// Simulate a game on the given EvaluatedMap and gives a mark according to specific criterias
    /// </summary>
    /// <param name="map">The map to test</param>
    public void TestMap(EvaluatedMap map)
    {
        int nbTurns = 0;

        // Set the player position on start
        Position = map.Start;
        // Debug.Log("START : x:" + Position.x + " y:" + Position.y);
        // Play until victory
        bool victory = false;
        int[] clues = new int[8];
        bool[] pieces = new bool[4];
        bool end = false;
        for (int j = 0; j < 1000; j++)
        {
            // Player time until no more ActionPoints
            nbTurns++;
            int ap = ActionPoints;
            while (ap > 0)
            {
                // Display(EvaluatorBoard, 7, 7);

                // FIND PIECES
                // 1
                if (ap > 0 && clues[0] != 0 && clues[1] != 0 && !pieces[0])
                {
                    double dist = CalculDist((int)Position.x, (int)Position.y, clues[0] - 1, clues[1] - 1);
                    // Debug.Log("dist = " + dist);
                    if (dist < 3)
                    {
                        int[] state = GoFindPiece(dist, ap);
                        nbTurns += state[0];
                        ap = state[1]; // Use action points to get the piece
                        pieces[0] = true;
                        //Debug.Log("FIND PIECE 1");
                    }
                }
                // 2
                if (ap > 0 && clues[2] != 0 && clues[3] != 0 && !pieces[1])
                {
                    double dist = CalculDist((int)Position.x, (int)Position.y, clues[2] - 1, clues[3] - 1);
                    // Debug.Log("dist = " + dist);
                    if (dist < 3)
                    {
                        int[] state = GoFindPiece(dist, ap);
                        nbTurns += state[0];
                        ap = state[1]; // Use action points to get the piece
                        pieces[1] = true;
                        //Debug.Log("FIND PIECE 2");
                    }
                }
                // 3
                if (ap > 0 && clues[4] != 0 && clues[5] != 0 && !pieces[2])
                {
                    double dist = CalculDist((int)Position.x, (int)Position.y, clues[4] - 1, clues[5] - 1);
                    // Debug.Log("dist = " + dist);
                    if (dist < 3)
                    {
                        int[] state = GoFindPiece(dist, ap);
                        nbTurns += state[0];
                        ap = state[1]; // Use one action point to get the piece
                        pieces[2] = true;
                        //Debug.Log("FIND PIECE 3");
                    }
                }
                // 4
                if (ap > 0 && clues[6] != 0 && clues[7] != 0 && !pieces[3])
                {
                    double dist = CalculDist((int)Position.x, (int)Position.y, clues[6] - 1, clues[7] - 1);
                    // Debug.Log("dist = " + dist);
                    if (dist < 3)
                    {
                        int[] state = GoFindPiece(dist, ap);
                        nbTurns += state[0];
                        ap = state[1]; // Use one action point to get the piece
                        pieces[3] = true;
                        //Debug.Log("FIND PIECE 4");
                    }
                }

                // DISCOVER the tile under if not already discovered
                if (ap > 0 && map.EvaluatorBoard[(int)Position.x + 1, (int)Position.y + 1] == Card.HIDDEN)
                {
                    ap--; // Use one action point to discover
                    int vertical = (int)Position.x + 1;
                    int horizontal = (int)Position.y + 1;
                    map.EvaluatorBoard[vertical, horizontal] = Card.DISCOVERED;
                    // Debug.Log("DISC : ap = " + ap + " / tile : x:" + Position.x + " y:" + Position.y);
                    // Check the discovered tile
                    Type tile = map.TypesMap[(int)Position.x, (int)Position.y];
                    switch (tile)
                    {
                        case Type.END:
                            end = true;
                            break;
                        case Type.HP1:
                            clues[0] = horizontal;
                            break;
                        case Type.VP1:
                            clues[1] = vertical;
                            break;
                        case Type.HP2:
                            clues[2] = horizontal;
                            break;
                        case Type.VP2:
                            clues[3] = vertical;
                            break;
                        case Type.HP3:
                            clues[4] = horizontal;
                            break;
                        case Type.VP3:
                            clues[5] = vertical;
                            break;
                        case Type.HP4:
                            clues[6] = horizontal;
                            break;
                        case Type.VP4:
                            clues[7] = vertical;
                            break;
                    }
                }

                // VICTORY
                if (pieces[0] && pieces[1] && pieces[2] && pieces[3] && end)
                {
                    victory = true;
                    break;
                }

                // MOVE
                if (ap > 0)
                {
                    ap--; // Use one action point to move
                    Vector2 moveTo = Move(Position, map.EvaluatorBoard);
                    Position = new Vector2(moveTo.x, moveTo.y);
                    // Debug.Log("MOVE : ap = " + ap + " / tile : x:" + Position.x + " y:" + Position.y);
                }
            }
            if (victory) break;
            // Tornado time
            // Tornado is not implemented yet
            if (j == 999) Debug.Log("OUT OF TIME FOR VICTORY");
        }
        // Number of turns needed to win the map
        map.NbTurns = nbTurns;
        // Debug.Log("map tunrs : " + map.NbTurns);
    }

    //*********************************************************************************************//
    //**************************          PRIVATE FUNCTIONS           *****************************//
    //*********************************************************************************************//

    /// <summary>
    /// Check duplicates in an array of Vectors
    /// </summary>
    /// <param name="a">An array of Vector2 to compare</param>
    /// <returns>True if duplicates found</returns>
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

    /// <summary>
    /// Move of 1 tile 
    /// </summary>
    /// <param name="currentPos">Player current position</param>
    /// <param name="EvaluatorBoard">The current player's vision of the board</param>
    /// <returns>The tile to go onto</returns>
    private Vector2 Move(Vector2 currentPos, Card[,] EvaluatorBoard)
    {
        int[] xAvailableDirections = new int[] { -1, 0, 1 };
        int[] yAvailableDirections = new int[] { -1, 0, 1 };
        Vector2 direction = new Vector2(); // The direction to go to

        // Can't go on forbidden tiles
        if (EvaluatorBoard[(int)Position.x + 1, (int)Position.y] == Card.FORBIDDEN) // up
            yAvailableDirections[0] = 0;
        if (EvaluatorBoard[(int)Position.x + 1, (int)Position.y + 2] == Card.FORBIDDEN) // down
            yAvailableDirections[2] = 0;
        if (EvaluatorBoard[(int)Position.x, (int)Position.y + 1] == Card.FORBIDDEN) // left
            xAvailableDirections[0] = 0;
        if (EvaluatorBoard[(int)Position.x + 2, (int)Position.y + 1] == Card.FORBIDDEN) // right
            xAvailableDirections[2] = 0;

        // Debug.Log("x dir : " + xAvailableDirections[0] + ", " + xAvailableDirections[1] + ", " + xAvailableDirections[2] + "\n"
        //    + "y dir : " + yAvailableDirections[0] + ", " + yAvailableDirections[1] + ", " + yAvailableDirections[2]);

        // Prioritizes hidden tiles
        if (EvaluatorBoard[(int)Position.x + 1, (int)Position.y] == Card.HIDDEN) // haut
        { direction.x = 0; direction.y = -1; }
        else
        {
            if (EvaluatorBoard[(int)Position.x + 2, (int)Position.y + 1] == Card.HIDDEN) // droite
            { direction.x = 1; direction.y = 0; }
            else
            {
                if (EvaluatorBoard[(int)Position.x + 1, (int)Position.y + 2] == Card.HIDDEN) // bas
                { direction.x = 0; direction.y = 1; }
                else
                {
                    if (EvaluatorBoard[(int)Position.x, (int)Position.y + 1] == Card.HIDDEN) // gauche
                    { direction.x = -1; direction.y = 0; }
                    else
                        direction = ChooseDirection(xAvailableDirections, yAvailableDirections);
                }                
            }
        }
        
        Vector2 tile = new Vector2(currentPos.x + direction.x, currentPos.y + direction.y);
        return tile; // The tile to go onto
    }

    /// <summary>
    /// Choose a random available direction 
    /// </summary>
    /// <param name="xDir">Horizontal available directions</param>
    /// <param name="yDir">Vertical available directions</param>
    /// <returns>A direction to go to</returns>
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
            if (i == 999) Debug.Log("OUT OF TIME TO MOVE!");
        }
        return direction;
    }

    /// <summary>
    /// Calculate distance between 2 given tiles
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns>The distance calculated (in number of tile)</returns>
    private double CalculDist(int x1, int y1, int x2, int y2)
    {
        double distance = Mathf.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
        return System.Math.Ceiling(distance);
    }

    /// <summary>
    /// Return how many turn and action point are used to get the piece 
    /// </summary>
    /// <param name="dist">The distance between the player and the piece (in number of tile)</param>
    /// <param name="ap">Action points availables</param>
    /// <returns>[0] : number of additional turn needed / [1] : Action points left</returns>
    private int[] GoFindPiece(double dist, int ap)
    {
        int actionPoints = -ap + 1; // Get the piece
        for(int i=0; i<dist; i++)
        {
            actionPoints += 2; // Move to get the piece and go back
        }

        int nbTurn = 2;
        if (actionPoints < 0) nbTurn = 0;
        else if (actionPoints < 4) nbTurn = 1;

        if (nbTurn == 0) actionPoints = System.Math.Abs(actionPoints);
        else if (nbTurn == 1) actionPoints = 4 - actionPoints;

        // Debug.Log("turn sup = " + nbTurn + " / ac left = " + actionPoints);
        return new int[] { nbTurn, actionPoints };
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
