using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ItemType
{
	BLASTER,
	JETPACK,
	PARASOL,
	ACCELERATEUR,
	COKTAIL,
	TERRASCOPE,
	GOURDE,
	PIECE1,
	PIECE2,
	PIECE3,
	PIECE4
}

public class Item
{
	public int type;
	
	public Item GenerateItem(int num)
	{
		Item newItem = new Item();
		if (num == 0) newItem.type = (int)ItemType.GOURDE;
		else
		{
			float r = Random.value;
			newItem.type = (int)(r * 6);
		}
		return newItem;
	}

	public void UseItem(GameObject playerObject, PlayerController playerInfo)
	{
		switch (type)
		{
			case ((int)ItemType.BLASTER):
				//playerInfo.Dig(new Tile(player.transform.position.x, player.transform.position.y + 1));
				//playerInfo.Dig(new Tile(player.transform.position.x, player.transform.position.y - 1));
				//playerInfo.Dig(new Tile(player.transform.position.x + 1, player.transform.position.y));
				//playerInfo.Dig(new Tile(player.transform.position.x - 1, player.transform.position.y));
				break;
			case ((int)ItemType.JETPACK):
				break;
			case ((int)ItemType.PARASOL):
				break;
			case ((int)ItemType.ACCELERATEUR):
				break;
			case ((int)ItemType.COKTAIL):
				break;
			case ((int)ItemType.TERRASCOPE):
				break;
			case ((int)ItemType.GOURDE):
				playerInfo.ChangeLife(2);
				break;
		}
			
	}
}