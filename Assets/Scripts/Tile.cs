using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int type;
    public bool isDiscovered;
    public int nbSandBlocks;
    public Item item;

	public Tile(int t)
    {
        type = t;
        isDiscovered = false;
        nbSandBlocks = 0;
	}

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
	}

	public Item GetItem()
	{
		//transform.Translate(new Vector3(1000, 0, 200));
		isDiscovered = true;
		item = new Item();
		item = item.GenerateItem(type);
		gameObject.transform.Find("hidden").gameObject.SetActive(false);
		print(item.type);
		return item;
	}
}
