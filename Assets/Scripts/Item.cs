using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
			newItem.type = (int)(r * 2.9);
		}
		return newItem;
	}

	public void UseItem(GameObject playerObject, PlayerController playerInfo)
	{
		switch (type)
		{
			case ((int)ItemType.ACCELERATEUR):
				playerInfo.actionPoints += 1;
                Debug.Log("accélérateur de temps (+1 pts d'action)");
                GameObject.Find("InfoTextBox").GetComponent<Text>().text = "What ?! Its the time throttle ! This will allow me to get a step ahead of the tornado (+1 action point)";
                break;
			case ((int)ItemType.COKTAIL):
				playerInfo.actionPoints += 1;
				playerInfo.ChangeLife(1);
				Debug.Log("flasque (+1 pts de vie / +1 pts d'action)");
                GameObject.Find("InfoTextBox").GetComponent<Text>().text = "Someone has lost his flask, get my strenght back ! (+1 hit point / +1 action point)";
                break;
			case ((int)ItemType.GOURDE):
				playerInfo.ChangeLife(1);
				Debug.Log("gourde d'eau (+1 pts de vie)");
                GameObject.Find("InfoTextBox").GetComponent<Text>().text = "A half empty water bottle, enough to refresh myself. (+1 hit point)";
                break;
		}
			
	}
}