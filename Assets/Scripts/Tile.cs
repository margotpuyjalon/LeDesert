using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int type;
    public bool isDiscovered;
    public int nbSandBlocks;
    public Item item;
	public Text m_Text;

    public Sprite oneSandSprite;
    public Sprite severalSandSprite;

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
        switch (nbSandBlocks)
        {
            case 0:
                GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                break;
            case 1:
                GetComponent<SpriteRenderer>().sprite = oneSandSprite;
                break;
            default:
                GetComponent<SpriteRenderer>().sprite = severalSandSprite;
                break;
        }
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

	public void RemoveSandblock()
	{
		nbSandBlocks--;
		m_Text.text = "" + nbSandBlocks;
	}

	public void AddSandblock()
	{
		nbSandBlocks++;
		m_Text.text = "" + nbSandBlocks;
	}
}
