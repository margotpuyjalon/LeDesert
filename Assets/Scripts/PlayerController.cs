using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int hitPoints = 4;
	public int actionPoints = 4;
	//public Item item;

	public bool revealedPiece1 = false;
	public bool revealedPiece2 = false;
	public bool revealedPiece3 = false;
	public bool revealedPiece4 = false;

	// Start is called before the first frame update
	void Start()
    {
	}

    // Update is called once per frame
    void Update()
    {
		Move();
	}

	// Translate character sprite
	private void Move()
	{
		GameObject objStorm = GameObject.Find("Tile_storm(Clone)");
		if (GetStandingTile().nbSandBlocks < 2 && actionPoints != 0)  // si tuile sur laquelle on est n'est pas ensablee
		{
			if (Input.GetKeyUp("up") && gameObject.transform.position.y < 2.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1))) //Il n'y a pas la tornade
				{
					//if (GetNextTile(new Vector3(0, 1, 0)).nbSandBlocks < 2)
					//{
						gameObject.transform.Translate(0, 1, 0);
						actionPoints--;
					//}
				}
			}		 // Up
			if (Input.GetKeyUp("down") && gameObject.transform.position.y > -1.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1))) //Il n'y a pas la tornade
				{
					//if (GetNextTile(new Vector3(0, -1, 0)).nbSandBlocks < 2)
					//{
						gameObject.transform.Translate(0, -1, 0);
						actionPoints--;
					//}
				}
			}	 // Down
			if (Input.GetKeyUp("right") && gameObject.transform.position.x < 2.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y))) //Il n'y a pas la tornade
				{
					//if (GetNextTile(new Vector3(1, 0, 0)).nbSandBlocks < 2)
					//{
						gameObject.transform.Translate(1, 0, 0);
						actionPoints--;
					//}
				}
			}	 // Right
			if (Input.GetKeyUp("left") && gameObject.transform.position.x > -1.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y))) //Il n'y a pas la tornade
				{
					//if (GetNextTile(new Vector3(-1, 0, 0)).nbSandBlocks < 2)
					//{
						gameObject.transform.Translate(-1, 0, 0);
						actionPoints--;
					//}
				}
			}	 // Left
		}
	}

	// Change player hit points
	public void ChangeLife(int amount)
	{
		hitPoints += amount;
	}

	// Dig sand
	public void Dig() //Type TILE a integrer
	{
		if (actionPoints != 0)
		{
			if (GetStandingTile().transform.position.x == transform.position.x && GetStandingTile().transform.position.y == transform.position.y)
			{
				if (GetStandingTile().GetComponent<Tile>().nbSandBlocks != 0)
				{
					GetStandingTile().GetComponent<Tile>().RemoveSandblock();
					GameObject.Find("Tilemap").GetComponent<Map>().sandBlocksLeft++;
					actionPoints--;
				}
				else print("Cannot dig here !");
			}
		}
	}

	// Discover tile 
	public void Discover()
	{
		if (actionPoints != 0)
		{
			if (GetStandingTile().transform.position.x == transform.position.x && GetStandingTile().transform.position.y == transform.position.y)
			{
				Tile standingTile = GetStandingTile().GetComponent<Tile>();
				
				if (!standingTile.isDiscovered && standingTile.nbSandBlocks==0)
				{
					UseItem(standingTile.GetItem());
					GameObject.Find("Tilemap").GetComponent<Map>().AffichagePiece();
					actionPoints--;
				}
			}
		}
	}

	// Use an item
	public void UseItem(Item item)
	{
		item.UseItem(this.gameObject, this);
		// IL FAUT L'ENLEVER DE LA LISTE
	}

	// Get the tile where the player is on
	private Tile GetStandingTile()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile"); // get all inPlayTiles
		foreach (GameObject tile in tiles)
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
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile"); // get all inPlayTiles
		foreach (GameObject tile in tiles)
		{
			// Check the next tile
			if ((tile.transform.position.x - move.x) == transform.position.x && (tile.transform.position.y - move.y) == transform.position.y)
			{
				return tile.GetComponent<Tile>();
			}
		}
		return null;
	}

	public void GetPiece()
	{
		List<GameObject> pieces = new List<GameObject>();
		pieces.Add(GameObject.Find("bluePiece"));
		pieces.Add(GameObject.Find("greenPiece"));
		pieces.Add(GameObject.Find("redPiece"));
		pieces.Add(GameObject.Find("purplePiece"));

		if (GetStandingTile().nbSandBlocks == 0 && actionPoints != 0)
		{
			if (GetStandingTile().isDiscovered)
			{
				foreach (GameObject piece in pieces)
				{
					// Check the piece to gather
					if (piece.transform.position.x == transform.position.x && piece.transform.position.y == transform.position.y)
					{
						if (piece.name == "bluePiece") revealedPiece1 = true;
						if (piece.name == "greenPiece") revealedPiece2 = true;
						if (piece.name == "redPiece") revealedPiece3 = true;
						if (piece.name == "purplePiece") revealedPiece4 = true;

						piece.transform.Translate(1000, 0, 0);
						actionPoints--;
					}
				}
			}
			else print("I have to discover the tile first !");
		}
		else print("Cannot grab the piece !");
	}
}
