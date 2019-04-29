using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CardsType
{
	HeatWave,
	DifficultyUp,
	MoveOneToLeft,
	MoveOneToRight,
	MoveOneToTop,
	MoveOneToBot,
	MoveTwoToLeft,
	MoveTwoToRight,
	MoveTwoToTop,
	MoveTwoToBot,
	MoveThreeToLeft,
	MoveThreeToRight,
	MoveThreeToTop,
	MoveThreeToBot,
};

public class DeckManager {
    public List<int> deck = new List<int>();
    public int indexToPick = 0;

    //
    int nbHeatWave = 3; 
    int nbDifficultyUp = 4; 
    //
    int nbMoveOneToLeft = 2; 
    int nbMoveOneToRight = 2; 
    int nbMoveOneToTop = 2;
    int nbMoveOneToBot = 2;
    //
    int nbMoveTwoToLeft = 2; 
    int nbMoveTwoToRight = 2; 
    int nbMoveTwoToTop = 2;
    int nbMoveTwoToBot = 2;
    //
    int nbMoveThreeToLeft = 2;
    int nbMoveThreeToRight = 2;
    int nbMoveThreeToTop = 2;
    int nbMoveThreeToBot = 2;

    // tableau de toutes les proba possible pour chaque type de cartes
    List<int> probabilities = new List<int> { 3, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

    public DeckManager()
    {
        CreateDeck();
        Shuffle();
    }

    void CreateDeck()
    {
        for (int i = 0; i < nbHeatWave; i++){ deck.Add((int)CardsType.HeatWave); }
        for (int i = 0; i < nbDifficultyUp; i++) { deck.Add((int)CardsType.DifficultyUp); }
        for (int i = 0; i < nbMoveOneToLeft; i++) { deck.Add((int)CardsType.MoveOneToLeft); }
        for (int i = 0; i < nbMoveOneToRight; i++) { deck.Add((int)CardsType.MoveOneToRight); }
        for (int i = 0; i < nbMoveOneToTop; i++) { deck.Add((int)CardsType.MoveOneToTop); }
        for (int i = 0; i < nbMoveOneToBot; i++) { deck.Add((int)CardsType.MoveOneToBot); }
        for (int i = 0; i < nbMoveTwoToLeft; i++) { deck.Add((int)CardsType.MoveTwoToLeft); }
        for (int i = 0; i < nbMoveTwoToRight; i++) { deck.Add((int)CardsType.MoveTwoToRight); }
        for (int i = 0; i < nbMoveTwoToTop; i++) { deck.Add((int)CardsType.MoveTwoToTop); }
        for (int i = 0; i < nbMoveTwoToBot; i++) { deck.Add((int)CardsType.MoveTwoToBot); }
        for (int i = 0; i < nbMoveThreeToLeft; i++) { deck.Add((int)CardsType.MoveThreeToLeft); }
        for (int i = 0; i < nbMoveThreeToRight; i++) { deck.Add((int)CardsType.MoveThreeToRight); }
        for (int i = 0; i < nbMoveThreeToTop; i++) { deck.Add((int)CardsType.MoveThreeToTop); }
        for (int i = 0; i < nbMoveThreeToBot; i++) { deck.Add((int)CardsType.MoveThreeToBot); }
    }

    void Shuffle() {
        // mélange de Fisher-Yates ou de Knuth
        for (int i = 0; i < deck.Count; i++)
        {
            int temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public int PickNextCard()
    {
        int cardToReturn;
        bool doProcedural = false; // booléen à changer pour utiliser la méthode tabRand ou l'ancienne façon

        if (doProcedural)
        {
            // méthode tabRand
            int s = probabilities.Sum();
            int n = Random.Range(0, s);
            int choice = 0;
            while (n - probabilities[choice] > 0)
            {
                n -= probabilities[choice];
                choice++;
            }
            // puis tirer la bonne carte
            switch (choice)
            {
                case 3:
                    cardToReturn = (int)CardsType.HeatWave;
                    break;
                case 4:
                    cardToReturn = (int)CardsType.DifficultyUp;
                    break;
                default: // 2
                    int typeMovement = Random.Range(1, 12);
                    switch (typeMovement)
                    {
                        case 1:
                            cardToReturn = (int)CardsType.MoveOneToLeft;
                            break;
                        case 2:
                            cardToReturn = (int)CardsType.MoveOneToRight;
                            break;
                        case 3:
                            cardToReturn = (int)CardsType.MoveOneToTop;
                            break;
                        case 4:
                            cardToReturn = (int)CardsType.MoveOneToBot;
                            break;
                        case 5:
                            cardToReturn = (int)CardsType.MoveTwoToLeft;
                            break;
                        case 6:
                            cardToReturn = (int)CardsType.MoveTwoToRight;
                            break;
                        case 7:
                            cardToReturn = (int)CardsType.MoveTwoToTop;
                            break;
                        case 8:
                            cardToReturn = (int)CardsType.MoveTwoToBot;
                            break;
                        case 9:
                            cardToReturn = (int)CardsType.MoveThreeToLeft;
                            break;
                        case 10:
                            cardToReturn = (int)CardsType.MoveThreeToRight;
                            break;
                        case 11:
                            cardToReturn = (int)CardsType.MoveThreeToTop;
                            break;
                        default:
                            cardToReturn = (int)CardsType.MoveThreeToBot;
                            break;
                    }
                    break;
            }       
        } else {
            cardToReturn = deck[indexToPick];
            // Incrémentation pour la prochaine pioche
            if (indexToPick == deck.Count - 1)
            {
                indexToPick = 0;
                Shuffle();
            }
            else { indexToPick++; }
        }

        return cardToReturn;
    }
}
