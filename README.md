# LeDesert

Fichiers sources du projet de jeu intégrant de la génération procédurale.

## Principes du jeu
Nous nous sommes inspirés du jeu de société *Le Désert interdit* pour faire ce jeu.

### Règles en places
Le joueur a 4 points d'actions. Avec, il peut : 
 - se déplacer d'une tuile (coute 1 point)
 - découvrir une tuile (coute 1 point)
 - retirer du sable (1 couche coute 1 point)
 - ramasser une pièce (coute 1 point)

 **L'objectif du joueur** est de ramasser toutes les pièces (il y en a 4) et de se rendre sur la case de fin.

 Toutes les cases (ou tuiles) ont une fonction : 
  - *TECH* : donne une ressource au joueur (des points de vie ou des points d'action).
  - *SOURCE* : donne des points de vie.
  - *TUNNEL* : empèche le joueur de perdre des points de vie en cas de vague de chaleur.
  - *START / END* : le joueur commence sur *START* et peut y récupérer une *TECH*. Il doit finir sur *END*.
  - *HP? / VP?* : les indices horizontaux (*HP?*) et verticaux (*VP?*) des pièces. La pièce *?* se trouve sur la case au croisement de ces directions.
  - *STORM* : le joueur ne peut pas aller sur la case Tornade. Elle effectue des actions à la fin du tour de jeu du joueur (quand il n'a plus de point d'action) : 
  	- Elle se déplace et dépose du sable sur son passage.
  	- La difficulté du jeu augmente. A partir d'un certain seuil, la Tornade effectue plus d'action pendant son tour (elle commence à 1).
  	- Une vague de chaleur fait perdre des points de vie au joueur.

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
S'il a découvert tous les indices d'une pièce, il va la chercher si elle n'est pas trop loin (2 cases).

On compte le nombre de tour qu'il faut à ce joueur pour remplir les conditions de victoire :
 - découvrir tous les indices
 - récolter toutes les pièces
 - découvrir la fin 

On sélectionne la carte avec un nombre de tour moyen sur toutes les cartes testées.

## A venir ...
 - Une interface plus sympa (affichage des actions de la Tornade sous forme de carte)
 - De nouvelles tuiles
 - De nouvelles règles du jeu
 - Des niveaux de difficulté
 - Le choix du personnage avec des capacités spéciales