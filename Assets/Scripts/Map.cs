using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    const int NB_TILE = 25;

    public GameObject Tile_tech, Tile_source, Tile_start, Tile_end, Tile_tunnel, Tile_storm;

    private MapGeneration typeMapGenerator;
    private Tile[,] map; 
    private Type[,] typesMap;

    private int difficulty = 1;
    private int sandBlocksLeft = 40;

    public DeckManager deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = new DeckManager();

        typeMapGenerator = new MapGeneration();
        typesMap = typeMapGenerator.GetMap(5,5);
        StartGame(5,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Crée une nouvelle matrice de tuiles
    // Place le joueur sur la tuile de départ
    // Affiche les éléments du jeu
    void StartGame(int x, int y)
    {
        GenerateMatrix(x, y);
        DisplayMap(x, y);
    }

    // Fill map with tiles
    void GenerateMatrix(int x, int y)
    {
        map = new Tile[x, y];
        for(int i=0; i<x; i++)
        {
            for(int j=0; j<y; j++)
            {
                map[i, j] = new Tile((int)typesMap[i,j]);
            }
        }
    }

    // Display the map
    void DisplayMap(int x, int y)
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GameObject tileType = null;
                switch ((Type)map[i,j].type)
                {
                    case Type.SOURCE:
                        tileType = Tile_source;
                        break;
                    case Type.TECH:
                        tileType = Tile_tech;
                        break;
                    case Type.START:
                        tileType = Tile_start;
                        break;
                    case Type.END:
                        tileType = Tile_end;
                        break;
                    case Type.TUNNEL:
                        tileType = Tile_tunnel;
                        break;
                    case Type.STORM:
                        tileType = Tile_storm;
                        break;
                    default:
                        tileType = Tile_tech;
                        break;
                }
                GameObject tile = Instantiate(tileType);
				tile.tag = "InPlayTile";
				tile.transform.position = new Vector3(-1.5f + i, 2.5f - j, 1);
            }
        }
    }

    public void NextTurn()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");
        GameObject objStorm = GameObject.Find("Tile_storm(Clone)");
        GameObject player = GameObject.Find("player");

        int nbOfPick = (int)Mathf.Ceil(difficulty / 3.0f);
        for (int i = 0; i < nbOfPick; i++)
        {
            CardsType card = (CardsType)deck.PickNextCard();
            print(card);
            switch ((int)card)
            {
                case (int)CardsType.DifficultyUp:
                    ChangeDifficulty();
                    break;
                case (int)CardsType.MoveOneToBot:
                    SwitchTiles(0, -1, 1);
                    break;
                case (int)CardsType.MoveOneToTop:
                    SwitchTiles(0, 1, 1);
                    break;
                case (int)CardsType.MoveOneToLeft:
                    SwitchTiles(-1, 0, 1);
                    break;
                case (int)CardsType.MoveOneToRight:
                    SwitchTiles(1, 0, 1);
                    break;
                case (int)CardsType.MoveTwoToBot:
                    SwitchTiles(0, -1, 2);
                    break;
                case (int)CardsType.MoveTwoToTop:
                    SwitchTiles(0, 1, 2);
                    break;
                case (int)CardsType.MoveTwoToLeft:
                    SwitchTiles(-1, 0, 2);
                    break;
                case (int)CardsType.MoveTwoToRight:
                    SwitchTiles(1, 0, 2);
                    break;
                case (int)CardsType.MoveThreeToBot:
                    SwitchTiles(0, -1, 3);
                    break;
                case (int)CardsType.MoveThreeToTop:
                    SwitchTiles(0, 1, 3);
                    break;
                case (int)CardsType.MoveThreeToLeft:
                    SwitchTiles(-1, 0, 3);
                    break;
                case (int)CardsType.MoveThreeToRight:
                    SwitchTiles(1, 0, 3);
                    break;
                case (int)CardsType.HeatWave:
                    player.GetComponent<PlayerController>().ChangeLife(-1);
                    print("Argh...La chaleur augmente... ! " + "Points de vie restants : " + player.GetComponent<PlayerController>().hitPoints);
                    break;
                default:
                    break;
            }
        }
        print("---");
        player.GetComponent<PlayerController>().actionPoints = 4; // après la pioche on remet les points d'action au max
    }

	private void SwitchTiles(int x, int y, int nbtimes)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");
		GameObject objStorm = GameObject.Find("Tile_storm(Clone)");
		GameObject player = GameObject.Find("player");

		for (int i = 0; i < nbtimes; i++)
		{
			foreach (GameObject go in tiles)
			{
				if (go.transform.position.x == objStorm.transform.position.x + x && go.transform.position.y == objStorm.transform.position.y + y)
				{
					
					go.transform.Translate(new Vector3(-x, -y, 0));
					objStorm.transform.Translate(new Vector3(x, y, 0));
					if (player.transform.position.x == objStorm.transform.position.x && player.transform.position.y == objStorm.transform.position.y) player.transform.Translate(new Vector3(-x, -y, 0));
					break;
				}
			}
		}
		AffichagePiece();
	}

    void ChangeDifficulty()
    {
        difficulty++;
        print("Wow... Le vent devient encore plus violent, encore " + (16 - difficulty) + " fois et je vais y passer !");
        if (difficulty > 15)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        print("Fin de la partie ! Vous avez perdu.");
    }

	public void AffichagePiece()
	{
		float HP1 = -40;
		float HP2 = -40;
		float HP3 = -40;
		float HP4 = -40;
		float VP1 = -40;
		float VP2 = -40;
		float VP3 = -40;
		float VP4 = -40;

		GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");

		foreach (GameObject tile in tiles)
		{
			if (tile.GetComponent<Tile>().isDiscovered)
			{
				switch (tile.GetComponent<Tile>().type)
				{
					case (int)Type.HP1:
						HP1 = tile.transform.position.x;
						break;
					case (int)Type.HP2:
						HP2 = tile.transform.position.x;
						break;
					case (int)Type.HP3:
						HP3 = tile.transform.position.x;
						break;
					case (int)Type.HP4:
						HP4 = tile.transform.position.x;
						break;
					case (int)Type.VP1:
						VP1 = tile.transform.position.y;
						break;
					case (int)Type.VP2:
						VP2 = tile.transform.position.y;
						break;
					case (int)Type.VP3:
						VP3 = tile.transform.position.y;
						break;
					case (int)Type.VP4:
						VP4 = tile.transform.position.y;
						break;
				}
			}
		}

		if (HP1 > -2 && VP1 > -2)
		{ //LA PIECE EST EN (HP1,VP1)
		}
		if (HP2 > -2 && VP2 > -2)
		{ //LA PIECE EST EN (HP2,VP2)
		}
		if (HP3 > -2 && VP3 > -2)
		{ //LA PIECE EST EN (HP3,VP3)
		}
		if (HP4 > -2 && VP4 > -2)
		{ //LA PIECE EST EN (HP4,VP4)
		}

		// FOR DEBUG
		//void Display(int x, int y)
		//{
		//    print("map");
		//    for (int i = 0; i < x; i++)
		//    {
		//        for (int j = 0; j < y; j++)
		//        {
		//            print(map[i, j].type);
		//        }
		//    }
		//}
	}

}
