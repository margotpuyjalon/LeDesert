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
    private int difficulty = 0;
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

    void NextTurn()
    {
        print(deck.PickNextCard());
        // TODO déplacement de la tornade
        // A remplacer par l'accès direct au vecteur de mvt ???
        switch (deck.PickNextCard())
        {
            case (int)DeckManager.CardsType.DifficultyUp:
                ChangeDifficulty();
                break;

            case (int)DeckManager.CardsType.MoveOneToBot:

                break;
            case (int)DeckManager.CardsType.MoveOneToTop:

                break;
            case (int)DeckManager.CardsType.MoveOneToLeft:

                break;
            case (int)DeckManager.CardsType.MoveOneToRight:

                break;

            case (int)DeckManager.CardsType.MoveTwoToBot:

                break;
            case (int)DeckManager.CardsType.MoveTwoToTop:

                break;
            case (int)DeckManager.CardsType.MoveTwoToLeft:

                break;
            case (int)DeckManager.CardsType.MoveTwoToRight:

                break;

            case (int)DeckManager.CardsType.MoveThreeToBot:

                break;
            case (int)DeckManager.CardsType.MoveThreeToTop:

                break;
            case (int)DeckManager.CardsType.MoveThreeToLeft:

                break;
            case (int)DeckManager.CardsType.MoveThreeToRight:

                break;

            case (int)DeckManager.CardsType.HeatWave:
                break;
            default:
                break;
        }
    }

    void ChangeDifficulty()
    {
        difficulty++;
        if (difficulty == 4)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        print("Fin de la partie ! Vous avez perdu.");
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
