﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Type
{
    SOURCE,
    TECH,
    START,
    END,
    TUNNEL,
    STORM,
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

    public GameObject Tile_tech, Tile_source, Tile_start, Tile_end, Tile_tunnel, Tile_storm;

    private Tile[,] map;
    private Tile[] mendatoryTiles;
    private int difficulty = 0;
    private int sandBlocksLeft = 40;

    public DeckManager deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = new DeckManager(); 
        mendatoryTiles = new Tile[]
            {
                new Tile((int)Type.SOURCE),
                new Tile((int)Type.TUNNEL), new Tile((int)Type.TUNNEL),
                new Tile((int)Type.START),
                new Tile((int)Type.END),
                new Tile((int)Type.HP1), new Tile((int)Type.VP1),
                new Tile((int)Type.TECH), new Tile((int)Type.STORM), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH),
                new Tile((int)Type.TECH), new Tile((int)Type.TECH), new Tile((int)Type.TECH)
            };
        StartGame(5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        NextTurn();
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
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("InPlayTile");
        print(deck.PickNextCard());
        // TODO déplacement de la tornade
        // A remplacer par l'accès direct au vecteur de mvt ???
        switch (deck.PickNextCard())
        {
            case (int)DeckManager.CardsType.DifficultyUp:
                ChangeDifficulty();
                break;

            case (int)DeckManager.CardsType.MoveOneToBot:
                for (int i = 0; i < 3; i++) // on ne check pas pour i = 4 puisque la tempete ne peut pas bouger dans ce cas
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Tile t = map[i, j];
                        if (t.type == (int)Type.STORM) // On inverse les deux tuiles
                        {
                            // dans la matrice
                            Tile temp = map[i + 1, j];
                            map[i + 1, j] = t; 
                            map[i, j] = temp;

                            // dans le visuel
                            GameObject objStorm = GameObject.Find("Tile_storm");
                            foreach (GameObject go in tiles)
                            {
                                if (go.transform.position.x == objStorm.transform.position.x - 1 && go.transform.position.y == objStorm.transform.position.y)
                                {
                                    go.transform.position = new Vector3(-1.5f + i, 2.5f - j, 1);
                                    break;
                                }
                            }
                            

                            objStorm.transform.position = new Vector3(-1.5f + (i + 1), 2.5f - (j + 1), 1); 
                        }
                    }
                }
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
                //PlayerController.changeLife(-1);
                break;
            default:
                break;
        }
    }

    void ChangeDifficulty()
    {
        difficulty++;
        print("Arg... La chaleur augmente, encore " + (4 - difficulty) + " fois et je vais y passer !");
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
