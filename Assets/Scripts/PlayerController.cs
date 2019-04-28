using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Player stats
	public int hitPoints = 4;
	public int actionPoints = 4;

	// Players pieces
	public bool piece1 = false;
	public bool piece2 = false;
	public bool piece3 = false;
	public bool piece4 = false;

	// Current map on play
	public Map currentMap;

	// Curent Textboxes
	public Text hitPointTextbox;
	public Text actionPointTextbox;

	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		if (currentMap.startedGame) Move();
		else
		{
			piece1 = false;
			piece2 = false;
			piece3 = false;
			piece4 = false;
			hitPoints = 4;
			actionPoints = 4;
		}
	}

	// Translate character sprite
	private void Move()
	{
		GameObject objStorm = GameObject.Find("Tile_storm(Clone)");																			// Get the Storm tile position
		if (GetStandingTile().nbSandBlocks < 2 && actionPoints != 0)																		// If the player can move
		{
			if (Input.GetKeyUp("up") && gameObject.transform.position.y < 2.5)																// If the player is not at the top border of the map
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1)))	//	And if the Storm is not on the path
				{
					//if (GetNextTile(new Vector3(0, 1, 0)).nbSandBlocks < 2) // FOR DIG AMELIORATION
					//{
					gameObject.transform.Translate(0, 1, 0);																				// Then translate the player
					actionPoints--;                                                                                                         // Use one action point
					actionPointTextbox.text = "Action Points : " + actionPoints;															// Set the textbox
					//}
				}
			}		 // Up
			if (Input.GetKeyUp("down") && gameObject.transform.position.y > -1.5)															// If the player is not at the bottom border of the map
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1)))	//	And if the Storm is not on the path
				{
					//if (GetNextTile(new Vector3(0, -1, 0)).nbSandBlocks < 2) // FOR DIG AMELIORATION
					//{
					gameObject.transform.Translate(0, -1, 0);                                                                               // Then translate the player
					actionPoints--;                                                                                                         // Use one action point
					actionPointTextbox.text = "Action Points : " + actionPoints;                                                            // Set the textbox
					//}
				}
			}	 // Down
			if (Input.GetKeyUp("right") && gameObject.transform.position.x < 2.5)                                                           // If the player is not at the right border of the map
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y)))    //	And if the Storm is not on the path
				{
					//if (GetNextTile(new Vector3(1, 0, 0)).nbSandBlocks < 2)// FOR DIG AMELIORATION
					//{
					gameObject.transform.Translate(1, 0, 0);                                                                                // Then translate the player
					actionPoints--;                                                                                                         // Use one action point
					actionPointTextbox.text = "Action Points : " + actionPoints;                                                            // Set the textbox
					//}
				}
			}	 // Right
			if (Input.GetKeyUp("left") && gameObject.transform.position.x > -1.5)                                                           // If the player is not at the left border of the map
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y)))    //	And if the Storm is not on the path
				{
					//if (GetNextTile(new Vector3(-1, 0, 0)).nbSandBlocks < 2)// FOR DIG AMELIORATION
					//{
					gameObject.transform.Translate(-1, 0, 0);                                                                               // Then translate the player
					actionPoints--;                                                                                                         // Use one action point
					actionPointTextbox.text = "Action Points : " + actionPoints;                                                            // Set the textbox
					//}
				}
			}	 // Left
		}
	}

	// Change player hit points
	public void ChangeLife(int amount)
	{
		hitPoints += amount;									// Change player's life amount, thanks to the given amount
		hitPointTextbox.text = "Hit Points : " + hitPoints;     // Set the textbox
	}

	// Dig sand
	public void Dig()
	{
		if (actionPoints != 0)														// If the player can act
		{
            if (GetStandingTile().GetComponent<Tile>().nbSandBlocks != 0)           // And if the tile has sand on it
            {
                GetStandingTile().GetComponent<Tile>().RemoveSandblock();           // Then Remove one block on it
                GameObject.Find("Tilemap").GetComponent<Map>().sandBlocksLeft++;    // Add the removed block to the block stockpile
                actionPoints--;                                                     // Use one action point
                actionPointTextbox.text = "Action Points : " + actionPoints;        // Set the textbox
            }
            else GameObject.Find("InfoTextBox").GetComponent<Text>().text = "Cannot dig here !"; print("Cannot dig here !");
		}
	}

	// Discover tile 
	public void Discover()
	{
		if (actionPoints != 0)														// If the player can act, 
		{
			Tile standingTile = GetStandingTile().GetComponent<Tile>();				// Get the standingTile
			if (!standingTile.isDiscovered && standingTile.nbSandBlocks==0)
			{
				UseItem(standingTile.GetItem());									// Use the items holded by the tile
				GameObject.Find("Tilemap").GetComponent<Map>().DisplayPieces();		// Check if a piece should appear after discovering the tile
				actionPoints--;                                                     // Use one acttion point
				actionPointTextbox.text = "Action Points : " + actionPoints;        // Set the textbox
			}
		}
	}

	// Get the tile where the player stands on
	private Tile GetStandingTile()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");	// get all inPlayTiles
		foreach (GameObject tile in tiles)										// Get the tile where the player stands on
		{
			if (tile.transform.position.x == transform.position.x && tile.transform.position.y == transform.position.y)
			{
				return tile.GetComponent<Tile>();
			}
		}
		return null;
	}

	// Get the tile where the player want to move
	private Tile GetNextTile(Vector3 move)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");	// get all inPlayTiles
		foreach (GameObject tile in tiles)										// Check the next tile to move, thank to the give move
		{
			if ((tile.transform.position.x - move.x) == transform.position.x && (tile.transform.position.y - move.y) == transform.position.y)	
			{
				return tile.GetComponent<Tile>();
			}
		}
		return null;
	}

	// Get one of the 4 pieces if the player stands on it
	public void GetPiece()
	{
		// Get all pieces position
		List<GameObject> pieces = new List<GameObject>();
		pieces.Add(GameObject.Find("bluePiece"));
		pieces.Add(GameObject.Find("greenPiece"));
		pieces.Add(GameObject.Find("redPiece"));
		pieces.Add(GameObject.Find("purplePiece"));


        if (actionPoints != 0)                                                                                                      // If the player can act,
        {
            if (GetStandingTile().nbSandBlocks == 0)                                                                                // If the tile have no sand on it
            {
                if (GetStandingTile().isDiscovered)                                                                                 // And if it has been discovered,
                {
                    foreach (GameObject piece in pieces)                                                                            // Then check the piece to grab
                    {
                        if (piece.transform.position.x == transform.position.x && piece.transform.position.y == transform.position.y) // by matching positions
                        {
                            if (piece.name == "bluePiece") piece1 = true;                                                           // Get the right piece
                            if (piece.name == "greenPiece") piece2 = true;
                            if (piece.name == "redPiece") piece3 = true;
                            if (piece.name == "purplePiece") piece4 = true;
                            piece.transform.Translate(1000, 0, 0);                                                                  // Translate it away from the map
                            actionPoints--;                                                                                         // Use one action point
                            actionPointTextbox.text = "Action Points : " + actionPoints;                                            // Set the textbox
                        }
                    }
                }
                else GameObject.Find("InfoTextBox").GetComponent<Text>().text = "I have to discover the tile first !"; print("I have to discover the tile first !");
            }
            else GameObject.Find("InfoTextBox").GetComponent<Text>().text = "Too much sand ! I have to dig the tile first."; print("Too much sand ! I have to dig the tile first.");
        }
        else GameObject.Find("InfoTextBox").GetComponent<Text>().text = "Cannot act anymore this turn..."; print("Cannot act anymore this turn...");
	}

	// Use an item (ITEMS NOT IMPLEMENTED)
	public void UseItem(Item item)
	{
		item.UseItem(this.gameObject, this);
		// IL FAUT L'ENLEVER DE LA LISTE
	}

	// Reset the action points of the player
	public void ResetActionPoints()
	{
		actionPoints = 4;												// Reset action points to 4
		actionPointTextbox.text = "Action Points : " + actionPoints;    // Set the textbox
	}
}
