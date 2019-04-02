using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Type
{
    SOURCE,
    TECH,
    START,
    END,
    TUNNEL,
    HP1,
    VP1,
    HP2,
    VP2,
    HP3,
    VP3,
    HP4,
    VP4
}

public class Map : MonoBehaviour
{
    const int NB_TILE = 25;

    public GameObject Tile_tech, Tile_source, Tile_start, Tile_end, Tile_tunnel;

    private Tile[,] map;
    private Tile[] mendatoryTiles;
    private int difficulty;
    private int sandBlocksLeft;

    // Start is called before the first frame update
    void Start()
    {
        mendatoryTiles = new Tile[]
            {
                new Tile((int)Type.SOURCE),
                new Tile((int)Type.TUNNEL), new Tile((int)Type.TUNNEL),
                new Tile((int)Type.START),
                new Tile((int)Type.END),
                new Tile((int)Type.HP1), new Tile((int)Type.VP1),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH)
            };
        StartGame(5, 5);
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
        ShuffleTiles();
        GenerateMatrix(x, y);
        DisplayMap(x, y);
    }

    // Shuffle the mendatoryTiles tab
    void ShuffleTiles()
    {
        for (int i = 0; i < mendatoryTiles.Length; i++)
        {
            Tile temp = mendatoryTiles[i];
            int randomIndex = Random.Range(i, mendatoryTiles.Length);
            mendatoryTiles[i] = mendatoryTiles[randomIndex];
            mendatoryTiles[randomIndex] = temp;
        }
    }

    // Fill map with Tiles
    void GenerateMatrix(int x, int y)
    {
        int k = 0;
        map = new Tile[x, y];
        for(int i=0; i<x; i++)
        {
            for(int j=0; j<y; j++)
            {
                map[i, j] = mendatoryTiles[k];
                k++;
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
