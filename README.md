# LeDesert

Fichiers sources du projet de jeu intégrant de la génération procédurale.

## Principes du jeu
Nous nous sommes inspirés du jeu de société *Le Désert interdit* pour faire ce jeu.

## Génération de la carte du jeu
Ci-dessous, les règles utilisées pour générer une "bonne" carte.

### L'évaluation du placement des tuiles 
On génère aléatoirement (par mutation d'une carte originale) plusieurs cartes.
On pénalise les cartes sur la base de critères d'acceptation : 
 - La tornade ne doit pas commencer sur un côté
 - Deux pièces de puzzle ne sont pas sur la même tuile (= les indices de ces pièces ne pointent pas vers la même tuile)
 - Tous les tunnels ne sont pas à côté (on en test 2 pour garantir au moins 1 paire utile)

 **On n'évalue en profondeur que les n meilleures cartes.**

### L'évaluation de la complexité du jeu
Un joueur artificiel se déplace sur la carte.
Il préfère aller sur les tuiles non découvertes et ne va pas sur les tuiles interdites (hors de la carte et tornade).
Si il a le choix de la direction, il se déplace d'abord vers le haut, puis vers la droite, le bas et en dernier la gauche (*axe d'amélioration : déplacement plus naturel*).
Il découvre chaque tuile sur laquelle il se trouve si elle ne l'est pas déjà.

On compte le nombre de tour qu'il faut à ce joueur pour remplir les conditions de victoire :
 - découvrir tous les indices
 - récolter toutes les pièces
 - découvrir la fin 

On sélectionne la carte avec un nombre de tour moyen sur toutes les cartes testées.

## A venir ...
 - Utiliser un algorithme de bruit pour sélectionner les tuiles à placer sur la carte
 - Test de nouvelles tuiles
 - Test de nouvelles règles du jeu
 - Intégration de niveaux de difficulté
 - Intégration du choix du personnage
 - Intégration des capacité spéciale des personnages
