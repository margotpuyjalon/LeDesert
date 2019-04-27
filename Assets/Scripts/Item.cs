using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ItemType
{
	ACCELERATEUR,
	COKTAIL,
	GOURDE
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
			newItem.type = (int)(r * 2);
		}
		return newItem;
	}

	public void UseItem(GameObject playerObject, PlayerController playerInfo)
	{
		switch (type)
		{
			case ((int)ItemType.ACCELERATEUR):
				playerInfo.actionPoints += 1;
                Debug.Log("Qu'est ce que ?! C'est l'accélérateur de temps ! Cela va me permettre de prendre un peu d'avance sur la tornade (+2 pts d'action)");
				break;
			case ((int)ItemType.COKTAIL):
				playerInfo.actionPoints += 1;
				playerInfo.ChangeLife(1);
				Debug.Log("Quelqu'un a fait tombé sa flasque, ça va me requinquer un peu ! (+2 pts de vie / +2 pts d'action)");
                break;
			case ((int)ItemType.GOURDE):
				playerInfo.ChangeLife(1);
				Debug.Log("Une gourde d'eau à moité vide, de auoi me désaltérer. (+2 pts de vie)");
				break;
		}
			
	}
}