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
    //Penser à faire une liste des tuiles du plateau
    //pour les intégrer dans la map lors de la génération

    const int NB_SOUCE = 1;
    const int NB_TUNNEL = 2;

    public GameObject Tile_tech;

    private Tile[,] map;
    private int difficulty;
    private int sandBlocksLeft;

    // Start is called before the first frame update
    void Start()
    {
        StartGame(25);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Crée une nouvelle matrice de tuiles
    // Place le joueur sur la tuile de départ
    // Affiche les éléments du jeu
    void StartGame(int nbTiles)
    {
        GenerateMatrix(5, 5);
        DisplayMap(5, 5);
    }

    // Fil map with Tiles
    void GenerateMatrix(int x, int y)
    {
        map = new Tile[x,y];
        for(int i=0; i<x; i++)
        {
            for(int j=0; j<y; j++)
            {
                map[i, j] = new Tile((int)Type.TECH);
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
                GameObject tileTech = Instantiate(Tile_tech);
                tileTech.transform.position = new Vector2(-1.5f + i, 2.5f - j);
            }
        }
    }

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
