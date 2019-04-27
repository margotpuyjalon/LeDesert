using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	// Number of tile on play
    const int NB_TILE = 25;

	// Get instance of a map generator
    private MapGenerator typeMapGenerator;
	// Get instance of tile types
    private Type[,] typesMap;
	// Map difficulty
    private int difficulty = 1;

	// Get instance of each different object of the game
    public GameObject Tile_tech, Tile_source, Tile_start, Tile_end, Tile_tunnel, Tile_storm,
		Tile_HP1, Tile_HP2, Tile_HP3, Tile_HP4, Tile_VP1, Tile_VP2, Tile_VP3, Tile_VP4;
	// Get instance of a sotrm deck
    public DeckManager deck;
	// Number of sand blocks in the stockpile
    public int sandBlocksLeft = 40;
	// Boolean to check is the game has started
	public bool startedGame = false;

    // Start is called before the first frame update
    void Start()
    {
		typeMapGenerator = new MapGenerator();      // Get a new instance of map generator
	}
    // Update is called once per fram
    void Update()
    {
		// Check the loose conditions
        if (	startedGame
			&&(  sandBlocksLeft == 0														// If the stockpile is empty,
			||	GameObject.Find("player").GetComponent<PlayerController>().hitPoints == 0	// If the player has no life
			||	(difficulty > 15)))															// And if the game's difficulty reached 15
        {EndGame(false);}																	// Then end the game
    }

	// Start game actions
	public void NewGame()
	{
		deck = new DeckManager();																				// Get new instance of a storm deck
		typesMap = typeMapGenerator.GetMap(5, 5);																// Get a new map, ready to be displayed
		startedGame = true;																						// The game started
		Camera newPos = Camera.allCameras[0];																	// Moving the camera
		newPos.transform.Translate(new Vector3(0, -6, 0));
		Camera.allCameras[0] = newPos;
		DisplayMap(5, 5);																						// Display generated map
		GameObject.Find("player").transform.position = GameObject.Find("Tile_start(Clone)").transform.position;	// Move the player in the starting tile
		GameObject.Find("player").transform.Translate(0, 0, -2);												// Then move it to the starting 
	}

    // End game actions
    void EndGame(bool win)
    {
        if (win)																			// If it's a win
        {
			startedGame = false;															// Game ends
			sandBlocksLeft = 40;															// Reset stockpile
			difficulty = 1;																	// Reset difficulty
			GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");			// Get all tiles in play
			foreach (GameObject tile in tiles)												// For each tiles
			{
				Destroy(tile);																// Destroy it !
			}
			GameObject.Find("bluePiece").transform.position = new Vector3(1000, 1000, 3);   // Hide the pieces
			GameObject.Find("redPiece").transform.position = new Vector3(1000, 1000, 3);
			GameObject.Find("greenPiece").transform.position = new Vector3(1000, 1000, 3);
			GameObject.Find("purplePiece").transform.position = new Vector3(1000, 1000, 3);
			Camera newPos = Camera.allCameras[0];											// Moving the camera
			newPos.transform.Translate(new Vector3(0, 6, 0));
			Camera.allCameras[0] = newPos;
			print("Fin de la partie ! Vous avez gagné.");
        }
        else
        {
			startedGame = false;															// Game ends
			sandBlocksLeft = 40;															// Reset stockpile
			difficulty = 1;																	// Reset difficulty
			GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");			// Get all tiles in play
			foreach (GameObject tile in tiles)												// For each tiles
			{
				Destroy(tile);																// Destroy it !
			}
			GameObject.Find("bluePiece").transform.position = new Vector3(1000, 1000, 3);   // Hide the pieces
			GameObject.Find("redPiece").transform.position = new Vector3(1000, 1000, 3);
			GameObject.Find("greenPiece").transform.position = new Vector3(1000, 1000, 3);
			GameObject.Find("purplePiece").transform.position = new Vector3(1000, 1000, 3);
			Camera newPos = Camera.allCameras[0];											// Moving the camera
			newPos.transform.Translate(new Vector3(0, 6, 0));
			Camera.allCameras[0] = newPos;
			print("Fin de la partie ! Vous avez perdu.");
        }
    }
	
	// End turn actions
    public void NextTurn()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");	// Get all tiles in play
        GameObject objStorm = GameObject.Find("Tile_storm(Clone)");				// Get the Storm object
        GameObject player = GameObject.Find("player");							// Get the player object

        int nbOfPick = (int)Mathf.Ceil(difficulty / 3.0f);                      // Storm cards draw calculation, linked with the difficulty
		for (int i = 0; i < nbOfPick; i++)
        {
            CardsType card = (CardsType)deck.PickNextCard();					// Draw a card
            print(card);														// Display the card (CONSOLE FOR NOW)
            switch ((int)card)													// Select the card effect
            {
                case (int)CardsType.DifficultyUp:								// Up the difficulty
                    ChangeDifficulty();
                    break;
				case (int)CardsType.HeatWave:                                   // Heat wave
					player.GetComponent<PlayerController>().ChangeLife(-1);		// Reduce player's life
					print("Argh...La chaleur augmente... ! " + "Points de vie restants : " + player.GetComponent<PlayerController>().hitPoints);
					break;
				case (int)CardsType.MoveOneToBot:								// Move the Storm 1 tile down
                    SwitchTiles(0, -1, 1);
                    break;
                case (int)CardsType.MoveOneToTop:                               // Move the Storm 1 tile up
					SwitchTiles(0, 1, 1);
                    break;
                case (int)CardsType.MoveOneToLeft:                              // Move the Storm 1 tile left
					SwitchTiles(-1, 0, 1);
                    break;
                case (int)CardsType.MoveOneToRight:                             // Move the Storm 1 tile right
					SwitchTiles(1, 0, 1);
                    break;
                case (int)CardsType.MoveTwoToBot:                               // Move the Storm 2 tile down
					SwitchTiles(0, -1, 2);
                    break;
                case (int)CardsType.MoveTwoToTop:                               // Move the Storm 2 tiles up
					SwitchTiles(0, 1, 2);
                    break;
                case (int)CardsType.MoveTwoToLeft:                              // Move the Storm 2 tiles left
					SwitchTiles(-1, 0, 2);
                    break;
                case (int)CardsType.MoveTwoToRight:                             // Move the Storm 2 tiles right
					SwitchTiles(1, 0, 2);
                    break;
                case (int)CardsType.MoveThreeToBot:                             // Move the Storm 3 tiles down
					SwitchTiles(0, -1, 3);
                    break;
                case (int)CardsType.MoveThreeToTop:                             // Move the Storm 3 tiles up
					SwitchTiles(0, 1, 3);
                    break;
                case (int)CardsType.MoveThreeToLeft:                            // Move the Storm 3 tiles left
					SwitchTiles(-1, 0, 3);
                    break;
                case (int)CardsType.MoveThreeToRight:                           // Move the Storm 3 tiles right
					SwitchTiles(1, 0, 3);
                    break;
                default:
                    break;
            }
        }
        print("---");
        player.GetComponent<PlayerController>().actionPoints = 4;				// Then set the player's action points to 4
    }

	// Display the map
    void DisplayMap(int x, int y)
    {
        for (int i = 0; i < x; i++)														// Horizontal display of x tiles
		{
            for (int j = 0; j < y; j++)													// Vertical display of y tiles
			{
                GameObject tileType = null;
                switch ((Type)typesMap[i,j])                                    // For each tile in the map (generated in the Start())
				{
                    case Type.SOURCE:                                           // Check if it's a Source
						tileType = Tile_source;
                        break;
                    case Type.TECH:                                             // Check if it's a Tech
						tileType = Tile_tech;
                        break;
                    case Type.START:                                            // Check if it's the Start
						tileType = Tile_start;
                        break;
                    case Type.END:                                              // Check if it's the End
						tileType = Tile_end;
                        break;
                    case Type.TUNNEL:											// Check if it's a Tunnel
						tileType = Tile_tunnel;
                        break;
                    case Type.STORM:                                            // Check if it's the Storm
						tileType = Tile_storm;
                        break;
					case Type.HP1:                                              // Check if it's the First Horizontal clue
						tileType = Tile_HP1;
						break;
					case Type.HP2:                                              // Check if it's the Second Horizontal clue
						tileType = Tile_HP2;
						break;
					case Type.HP3:                                              // Check if it's the Third Horizontal clue
						tileType = Tile_HP3;
						break;
					case Type.HP4:                                              // Check if it's the Fourth Horizontal clue
						tileType = Tile_HP4;
						break;
					case Type.VP1:                                              // Check if it's the First Vertical clue
						tileType = Tile_VP1;
						break;
					case Type.VP2:												// Check if it's the Second Vertical clue
						tileType = Tile_VP2;
						break;
					case Type.VP3:												// Check if it's the Third Vertical clue
						tileType = Tile_VP3;
						break;
					case Type.VP4:												// Check if it's the Fourth Vertical clue
						tileType = Tile_VP4;
						break;
					default:
                        tileType = Tile_tech;
                        break;
                }
                GameObject tile = Instantiate(tileType);								// Instantiate a new Tile object
				tile.tag = "InPlayTile";												// Add the "InPlayTile" tag
				tile.transform.position = new Vector3(-1.5f + i, 2.5f - j, 0);			// And set it's right position on the scene
            }
        }
    }

	// Switch tiles positions to move the Storm
	private void SwitchTiles(int x, int y, int nbtimes)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");				// Get all tiles in play
		GameObject objStorm = GameObject.Find("Tile_storm(Clone)");							// Get the Storm object
		GameObject player = GameObject.Find("player");										// Get the player object

		for (int i = 0; i < nbtimes; i++)													// For each given nbtimes
		{
			foreach (GameObject go in tiles)												// Check on each tiles
			{
				if (go.transform.position.x == objStorm.transform.position.x + x 
					&& go.transform.position.y == objStorm.transform.position.y + y)		// If the tile in if the Sotrm's path
				{
					go.transform.Translate(new Vector3(-x, -y, 0));							// Translate the tile on the Storm position
					objStorm.transform.Translate(new Vector3(x, y, 0));						// Translate the Storm on it's new free position
					if (player.transform.position.x == objStorm.transform.position.x
						&& player.transform.position.y == objStorm.transform.position.y)	// If the player was on the translated tile
					{ player.transform.Translate(new Vector3(-x, -y, 0)); }					// Also translate the player
				    go.GetComponent<Tile>().AddSandblock();									// Add sand block on the tile
                    sandBlocksLeft--;														// Remove a sandblock from the stockpile
					break;
				}
			}
		}
		DisplayPieces();																	// Display discovered pieces
	}

	// Change the difficulty of the game
    void ChangeDifficulty()
    {
        difficulty++;	// Up the difficlty of 1 level
        print("Wow... Le vent devient encore plus violent, encore " + (16 - difficulty) + " fois et je vais y passer !");
    }

	// Display all discovered pieces, if their clues are also discovered
	public void DisplayPieces()
	{
		float HP1 = -40;															// Set default locations for the Horizontal and Vertical clues
		float HP2 = -40;
		float HP3 = -40;
		float HP4 = -40;
		float VP1 = -40;
		float VP2 = -40;
		float VP3 = -40;
		float VP4 = -40;
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");       // Get all tiles in play
		foreach (GameObject tile in tiles)											// For each tiles
		{
			if (tile.GetComponent<Tile>().isDiscovered)								// If a tile is discovered
			{
				switch (tile.GetComponent<Tile>().type)								// Check its type
				{
					case (int)Type.HP1:												// If its the First Horizontal clue
						HP1 = tile.transform.position.y;
						break;
					case (int)Type.HP2:                                             // If its the Second Horizontal clue
						HP2 = tile.transform.position.y;
						break;
					case (int)Type.HP3:                                             // If its the Third Horizontal clue
						HP3 = tile.transform.position.y;
						break;
					case (int)Type.HP4:                                             // If its the Fourth Horizontal clue
						HP4 = tile.transform.position.y;
						break;
					case (int)Type.VP1:                                             // If its the First Vertical clue
						VP1 = tile.transform.position.x;
						break;
					case (int)Type.VP2:                                             // If its the Second Vertical clue
						VP2 = tile.transform.position.x;
						break;
					case (int)Type.VP3:                                             // If its the Third Vertical clue
						VP3 = tile.transform.position.x;
						break;
					case (int)Type.VP4:                                             // If its the Fourth Vertical clue
						VP4 = tile.transform.position.x;
						break;
				}
			}
		}

		GameObject player = GameObject.Find("player");								// Get the player object
		Debug.Log(HP1+""+VP1 + "" + HP2 + "" + VP2 + "" + HP3 + "" + VP3 + "" + HP4 + "" + VP4);
		if (HP1 > -2 && VP1 > -2)
		{ 
			if (!player.GetComponent<PlayerController>().piece1) GameObject.Find("bluePiece").transform.position = new Vector3(VP1, HP1, -3);
		}												// If the two clues of the First piece have been discovered
		if (HP2 > -2 && VP2 > -2)
		{
			if (!player.GetComponent<PlayerController>().piece2) GameObject.Find("greenPiece").transform.position = new Vector3(VP2, HP2, -3);
		}												// If the two clues of the Second piece have been discovered
		if (HP3 > -2 && VP3 > -2)
		{
			if(!player.GetComponent<PlayerController>().piece3) GameObject.Find("redPiece").transform.position = new Vector3(VP3, HP3, -3);
		}												// If the two clues of the Third piece have been discovered
		if (HP4 > -2 && VP4 > -2)
		{
			if (!player.GetComponent<PlayerController>().piece4) GameObject.Find("purplePiece").transform.position = new Vector3(VP4, HP4, -3);
		}												// If the two clues of the Fourth piece have been discovered

		// FOR DEBUG
		//void Display(int x, int y)
		//{
		//    print("map");
		//    for (int i = 0; i < x; i++)
		//    {
		//        for (int j = 0; j < y; j++)
		//        {
		//            print(map[i, j].type);
		//        }
		//    }
		//}
	}
}
