using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int type;
    public bool isDiscovered;
    public int nbSandBlocks;
	public Text m_Text;

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
		m_Text.text = "" + nbSandBlocks;
	}
}
