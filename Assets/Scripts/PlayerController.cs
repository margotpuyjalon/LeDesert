using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int hitPoints = 4;
	public int actionPoints = 4;
	//public Item item;

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
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1)))
				{
					gameObject.transform.Translate(0, 1, 0);
					actionPoints--;
				}
			}		 // Up
			if (Input.GetKeyUp("down") && gameObject.transform.position.y > -1.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1)))
				{
					gameObject.transform.Translate(0, -1, 0);
					actionPoints--;
				}
			}	 // Down
			if (Input.GetKeyUp("right") && gameObject.transform.position.x < 2.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y)))
				{
					gameObject.transform.Translate(1, 0, 0);
					actionPoints--;
				}
			}	 // Right
			if (Input.GetKeyUp("left") && gameObject.transform.position.x > -1.5)
			{
				if (!(objStorm.transform.position == new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y)))
				{
					gameObject.transform.Translate(-1, 0, 0);
					actionPoints--;
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
				
				if (!standingTile.isDiscovered)
				{
					UseItem(standingTile.GetItem());
					actionPoints--;
				}
				
			}
			Map m = new Map();
			m.AffichagePiece();
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
}
