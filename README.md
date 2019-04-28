# LeDesert

Fichiers sources du projet de jeu intégrant de la génération procédurale.

## Principes du jeu
Nous nous sommes inspirés du jeu de société *Le Désert interdit* pour faire ce jeu.

### Règles en places
Le jeu se joue sur un plateau de 5x5.
Chaque tour, le joueur a 4 points d'actions. Pour 1 point, il peut : 
  - se déplacer d'une tuile
  - retourner/découvrir la tuile où il se trouve
  - retirer une couche de sable de la tuile
  - ramasser une pièce qui se trouve sur la même tuile que lui
  
A la fin du tour, il pioche un certain nombre de cartes selon le niveau de difficulté :
  - 1 pour une difficulté de 1 à 3
  - 2 pour une difficulté de 4 à 6
  - 3 pour une difficulté de 7 à 9
  - 4 pour une difficulté de 10 à 12
  - 5 pour une difficulté de 13 à 15

 **L'objectif du joueur** est de ramasser les 4 pièces puis de se rendre sur la case de fin non ensevelie.
 **Le joueur perd** si ses points de vie tombent à 0 ou si le niveau de chaleur (la difficulté) arrive à son maximum (16).

 Toutes les cases (ou tuiles) ont un type :
 - *HP? / VP?* : les indices horizontaux (*HP?*) et verticaux (*VP?*) des pièces. La pièce *?* se trouve sur la case au croisement de ces directions.
 - *STORM* : La tornade effectue des actions à la fin de chaque tour selon la carte piochée : 
  	- Elle se déplace de 1, 2, ou 3 cases. Lorsqu'elle bouge elle décale les tuiles et dépose du sable sur son passage .
  	- Elle entraîne une vague de chaleur qui fait perdre un point de vie au joueur.
   Le joueur ne peut pas aller sur cette case. 
  - *TECH* : actionne une ressource pour le joueur qui lui donne des points de vie ou des points d'action.
  - *SOURCE* : donne des points de vie.
  - *TUNNEL* : empèche le joueur de perdre des points de vie en cas de vague de chaleur.
  - *START / END* : le joueur commence sur *START* et doit finir sur *END*. Elles octoient un effet *TECH* lorsqu'on les retourne.
  
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
 - Ajout de nouvelles tuiles (mirage) et ressources
 - De nouvelles règles du jeu (par ex. utilisation des tunnels)
 - Le choix du niveau de difficulté de départ
 - Choisir un personnage avec des capacités spéciales
