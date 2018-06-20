# Project 1 - Simplexity 

* Diogo Maia - a21704165
* Ianis Arquissandas - a21700021
* Nuno Carriço - a21701393

## Who did what?

**Diogo Maia:**
* Start checking neighbourhood;
* Add unexplored and explored tiles;
* Add exploration of top, below, left and right tiles of player;
* Add pick up of Map;
* Add Food and Gun classes;
* Add Startof NPC.
* Add Fluxogram and UML.

**Ianis Arquissandas:**
* Start Player Movement;
* Add Player Movement Restrictions;
* Add Trap to Map;
* Add information view for all itens and traps;
* Improve CheckForTraps();
* Add random trap placement;
* Improve Traps;
* Add weight to itens and player;
* Add pick up for all itens;
* Add drop and use item methods;
* Error messages for pick up, drop and use;
* Help on UML;
* Add colors to interface.

**Nuno Carriço:**
* Create beginner classes and Main Menu;
* Add Visualization - begginning;
* Add Player and Exit randomly to grid;
* Improve Player Movement;
* Revamp Grid;
* Improve Error Detection in Main Menu
* Add High Score System;
* Add game interfaces;
* Fix Trap Spawning;
* Add comments to all classes;
* Add Procedural Generation for itens, NCPs and traps;
* Add combat to game;
* Fix Bugs of certain classes and visualization;
* Add save and load to game;
* Add Doxygen file and README.

## Our solution

### Fluxogram

![RoguelikeFluxo](https://gitlab.com/NunoCarrico98/Project2_Roguelike/uploads/48f13900164b5a9989be9ab56caea2a4/RoguelikeFluxo.png)

**Picture 1** - Our Fluxogram represents the flow of the program, in this case, the Game Loop.

### UML Diagram

![UML](https://gitlab.com/NunoCarrico98/Project2_Roguelike/uploads/6accdc3258d96bbf51f55d185a84a592/RogueLike_UML.png)

**Picture 2** - Our UML Diagram shows the structure of our program as well as the relationships between our classes. 

### Data structures

For our game grid, we decided to use an object that could hold all game objects. In that way, we created a class `GameTile` which implements `List<T>`. We also created an interface called `IGameObject` so the class `GameTile` can be a a `list` of game objects (`List<IGameObject>`).
Therefore, all of our game objects implement `IGameObject` so they can be added to each tile.
However, this only creates one `tile`. To make a complete grid (8x8), we created a `mutidimensional array` of `GameTile`, creating a complete grid of `tiles` that can be manipulated to contain all game objects.


Besides that, we also used a `list` for the `player inventory`. This is a `list` of itens (`List<Item>`) so every Food and Weapon (that inherit from `Item`) can be added to the `inventory` and the `player` can hold them.
We also create new `lists` when the `player` uses the information screen. That is so we can print out all the `Food`, `Weapon` and `Traps` of the game with all of their important properties.


### Algorithms

Our game starts by entering the `GetMenuOption()` method. This method calls the `Main Menu` of the game and according to user `input` will do something different.
 If the `player` chooses "New Game", a new game will begin and the `SetInitialPositions()` method is called. This method sets the initial positions for all game objects (`Player`, `Exit`, `Map`, `Items` and `NPCs`). From here the player can move, pick up items, drop items, use items, attack `NPCs` or use the information screen.
 If the `player` chooses to move, then his `position` is changed according to where he wants to move and he moves on the grid. By moving, the `player` is removed from the `list` (`GameTile`) where he previously was and is added to the `tile` where he chose to move to. However, a `null` element has to be added to the old `list` so the number of elements in that `tile` stays the same.

 Movement:
```c#
	OldPlayerPos is updated;
    PlayerPos changes;
	Player is Removed from previous list;
	A null is added to the list;
	Player is added to new tile;
```

If the `player` chooses to pick up an `item` and there is an `item` on the `tile` he is currently on, the pick up screen is called. There the `player` has to either go back or choose an `item` to pick up. If he goes back a turn will not be spent. However, if he chooses to pick up an `item` an algorithm is called to make sure he picks up the `item` he chose.
This algorithm starts by creating a `counter` and initialising it as the user `input`. Because the `index` of the `tile` is always the same or more than the user `input`. If the current `index` of the `tile` is the same as the `counter`, that `item` is picked up. Picking an `item` up means removing the `item` from the current `tile` and adding it to the `player inventory`.
However, if it's more than the `counter`, the `counter` cycles through the `tile` with the `counter` as the `index`. If the currently analysed spot has an `item`, it is picked up. But if it's not, the `counter` increments by 1. This cycle repeats until an `item` is picked up.

Pick Up Item:
```c#
	Make sure there is item on tile;
	if(there is not)
		Call error screen;
	if(there is)
		Call pick up screen;
		if(player chooses to go back)
			go back without spending a turn;
		if(player chooses to pick up an item)
			do
				if(tile[counter - 1] is an item)
					remove item from tile;
					add null to tile;
					add item to player inventory;
					add weight to player weight;
				else increment counter by 1;
			while (player didn't pick up an item)
```

If the `player` chooses to drop or use an `item`, a similar algorithm is called. However, this one goes through the `player inventory` and it does not have to do a cycle because we simply have to decrease 1 to the `input` of the `player` to know what `item` to drop.
Droping an `item` means removing the `item` from the `player inventory` and it's `weight` from the `player's weight` and add it to the current `tile`. 
Using an `item` uses the same process with the exception that the `item` is not put back to the current `tile`. If the `item` is `Food`, the `HP` of the `player` increases according to the `Food` type. If the `item` is a `Weapon` and the `player` has no `weapon equipped`, the chosen `weapon` is equipped. But if the `player` has a `weapon` equipped, that `weapon` is put on the `player inventory` and the chosen `weapon` is equipped.
This algorithm only occurs if the `player` has an `item` in his `inventory`.

Drop Item:
```c#
	Make sure there is item on inventory;
	if(there is not)
		Call error screen;
	if(there is)
		Call drop screen;
		if(player chooses to go back)
			go back without spending a turn;
		if(player chooses to drop an item)
			if(inventory[input - 1] is an item)
				remove item from inventory;
				add item to tile;
				remvoe weight from player weight;
```
Use Item:
```c#
	Make sure there is item on inventory;
	if(there is not)
		Call error screen;
	if(there is)
		Call use item screen;
		if(player chooses to go back)
			go back without spending a turn;
		if(player chooses to use an item)
			ifinventory[input - 1] is an item)
				if(inventory[input - 1] is Food)
					Increase player HP;
				if(inventory[input - 1] is Weapon)
					if(player has no equipped weapon)
						equip weapon;
					if(player has equipped weapon)
						unequip weapon adding it to 
						equip chosen weapon;
				remove item from inventory;
				remvoe weight from player weight;
```

If the `player` chooses to attack an `NPC`, a similar algorithm to the one used to pick an `Item` is used. This one works almost the same as one to pick up an `item`, but instead of `items` it uses the `NPC` class. 
That means that it only works if there is an `NPC` on the current `player tile`.
Attacking an `NPC` means taking a random amount of `HP` of the chosen `NPC` between 0 and the `equipped weapon's attack power`. In this attack, the `equipped weapon` can break. If it does, the `weapon` is no longer usable and it disappears from the game.
Besides that, if the attacked `NPC` is not an `Hostile`, it becomes an `Hostile` after being attacked. Moreover, there is always a verification if the `NPC` is dead. If the `NPC` dies, there a chance that he will spawn a random number of `Items` (`Food` or `Weapon`) between 0 and 5.

Attacked NPC:
```c#
	counter = input of player;
	Make sure there is an NPC on tile;
	if(there is not)
		Call error screen;
	if(there is)
		Call attack screen;
		if(player chooses to go back)
			go back without spending a turn;
		if(player chooses to attack an NPC)
			do
				if(tile[counter - 1] is an NPC)
					take HP from NPC;
					if(weapon breaks)
						remove weapon to player inventory;
						remove the weapon's weight from the player's weight;
						if(NPC dies)
							remove NPC from tile;
							add null to tile;
				else increment counter by 1;
			while (player didn't pick up an item)
```

However, if the `player` moves and the `tile` he moves to contains a `trap`. The `trap` removes `HP` from the `player`. The same happens if the `tile` contains a `Hostile NPC`.
There is also the possibility that the `player` finds a `Map`. If he does, he can pick it up, revealing the entire `level`.
That is how one of the turns of the game works. Therefore, the game keeps going until the `player` decides to quit or he dies.


Instead of choosing "New game" in the `Main Menu`, the `player` can also load a previous save. If he does he will start where he saved and continue from there.
He can also see the best high scores and the credits (who made the game).

The highscore system works by creating a `list` of `Tuples` of `strings` and `floats`(Tuples<string, float>). This `list` contains the `name` and the last `level` the `player` played.
Once you a add a new `tuple` to the list, which only happens if the `score` of the player is bigger than the last `score` on the `list`, the `list` will be sorted on descending order.
This `list` is then written on a file so it is saved between each game.

## Conclusions

1. Better understanding of c# object-oriented programming;
2. Improvement of the use of lists;
3. Better understanding of the creation of roguelike games;
4. Development on the use of cursor position and colors;

## References ##

#### Other Colaborators
> **Rui Martins, Diogo Martins**
Our group and this one, worked together on most of the logic for the program. In that way, most of the logic of our code is the same. However, the coding itsef was done separately.
