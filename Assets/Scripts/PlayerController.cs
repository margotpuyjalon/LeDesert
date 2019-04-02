using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public int hitPoints = 4;
	public int actionPoints = 4;
	//public Item items[];

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
		if (actionPoints != 10)
		{
			if (Input.GetKeyUp("up") && gameObject.transform.position.y < 2.5)
				{ gameObject.transform.Translate(0, 1, 0); actionPoints--; }      // Up
			if (Input.GetKeyUp("down") && gameObject.transform.position.y > -1.5)
				{ gameObject.transform.Translate(0, -1, 0); actionPoints--;}      // Down
			if (Input.GetKeyUp("right") && gameObject.transform.position.x < 2.5)
				{ gameObject.transform.Translate(1, 0, 0); actionPoints--;}       // Right
			if (Input.GetKeyUp("left") && gameObject.transform.position.x > -1.5)
				{ gameObject.transform.Translate(-1, 0, 0); actionPoints--; }     // Left
		}
	}

	// Change player hit points
	public void changeLife(int amount)
	{
		hitPoints += amount;
	}

	/* Dig sand
	public void dig(Tile tile) //Type TILE a integrer
	{
		if (actionPoints != 0)
		{
			actionPoints--;
			tile.nbSandBlocks--;
		}
	}

	// Discover tile
	public void discover(Tile tile)
	{
		if (actionPoints != 0)
		{
			actionPoints--;
			tile.isDiscovered = true;
		}
	}

	// Use an item
	public void useItem(Item item)
	{
		//ON ENLEVE L'ITEM SELECTIONNE DE LA LISTE
		if (item.type == "blaster")
		{
			dig(new Tile(gameObject.transform.position.x, gameObject.transform.position.y + 1));
			dig(new Tile(gameObject.transform.position.x, gameObject.transform.position.y - 1));
			dig(new Tile(gameObject.transform.position.x + 1, gameObject.transform.position.y));
			dig(new Tile(gameObject.transform.position.x - 1, gameObject.transform.position.y));
		}
		if (item.type == "gourde")
		{
			hitPoints += 2;
		}
		if (item.type == "jetpack")
		{
			if (Input.GetMouseButtonUp(0))
			{
				bool hasMoved = false;
				while (!hasMoved)
				{
					// On recupere la position choisie
					gameObject.transform.Translate(Input.mousePosition);
					if (Input.GetMouseButtonUp(0)) hasMoved = true;
				}
			}
		}
	}
	*/
}
