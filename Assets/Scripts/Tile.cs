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

	public Map currentMap;

	public Tile(int t)
    {
        type = t;
        isDiscovered = false;
        nbSandBlocks = 0;
	}

	// Start is called before the first frame update
	void Start()
    {
		gameObject.transform.Find("PeuEnsable").gameObject.SetActive(false);
		gameObject.transform.Find("TresEnsable").gameObject.SetActive(false);
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
		return item;
	}

	public void RemoveSandblock()
	{
		nbSandBlocks--;
		m_Text.text = "" + nbSandBlocks;
		ChooseSkin();
	}

	public void AddSandblock()
	{
		nbSandBlocks++;
		m_Text.text = "" + nbSandBlocks;
		ChooseSkin();
	}

	private void ChooseSkin()
	{
		switch (nbSandBlocks)
		{
			case 0:
				gameObject.transform.Find("PeuEnsable").gameObject.SetActive(false);
				gameObject.transform.Find("TresEnsable").gameObject.SetActive(false);
				break;
			case 1:
				gameObject.transform.Find("PeuEnsable").gameObject.SetActive(true);
				gameObject.transform.Find("TresEnsable").gameObject.SetActive(false);
				break;
			default:
				gameObject.transform.Find("TresEnsable").gameObject.SetActive(true);
				break;
		}
	}
}
