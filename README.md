# Project 1 - Simplexity 

* Diogo Maia - a21704165
* Ianis Arquissandas - a21700021
* Nuno Carriço - a21701393

## Who did what?

**Diogo Maia:**

**Ianis Arquissandas:**

**Nuno Carriço**


## Our solution

### Fluxogram

![Roguelike_Fluxogram_-_Page_1__1_]()

**Picture 1** - Our Fluxogram represents the flow of the program, in this case, the Game Loop.

### UML Diagram

![UML]()

**Picture 2** - Our UML Diagram shows the structure of our program as well as the relationships between our classes. 

### Data structures



### Algorithms

 Our game starts by entering the `GetMenuOption()` method. This method calls the main menu of the game and according to user input will do something different.
 If the player chooses "New Game", a new game will begin and the `SetInitialPositions()` method is called. This method sets the initial positions for all game objects (Player, Exit, Map, Items and NPCs). From here the player can move, pick up items, drop items, use items, attack NPCs or use the information screen.
 If the player chooses to move, then his position is changed according to where he wants to move and he moves on the grid. By moving, the player is removed from the list (tile) where he previously was and is added to the tile where he moved to. However, a null element has to be added to the old list so the  number of elements in that tile stay the same.

 Movement:
```c#
	OldPlayerPos is updated;
    PlayerPos changes;
	Player is Removed from precious list;
	A null is added to the list;
	Player is added to new tile;
```

If the player chooses to pick up an item and there is an item on the tile he is currently on, the pick up screen is called. There the player has to either go back or choose an item to pick up. If he goes back a turn will not be spent. However, if he chooses to pick up an item an algorithm is called to make sure he picks up the item he chose.
This algorithm starts by creating a counter and initialising it as the user input. Because the index of the tile is always the same or more than the user input. If the current index of the tile is the same as the counter, an item is picked up. Picking an item up means removing the item from the current tile and adding it to the player inventory.
However, if it's more than the counter, the counter cycles through the tile with the counter as the index. If the currently analysed spot has an item, it is picked up. But if it's not, the counter increments by 1. This cycle repeats until an item is picked up.

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
				if(tile[counter] is an item)
					remove item from tile;
					add null to tile;
					add item to player inventory;
					add weight to player weight;
				else increment counter by 1;
			while (player didn't pick up an item)
```

If the player chooses to drop or use an item, a similar algorithm is called. However, this one goes through the player inventory and it does not have to do a cycle because we simply have to decrease 1 to the input of the player to know what item to drop
Droping an item means removing the item from the player inventory and it's weight from the player and add it to the current tile. 
Using an item uses the same process with the exception that the item is not put back to the current tile. If the item is Food, the HP of the player increases according to the item type. If the item is a Weapon and the player has no weapon equipped, the chosen weapon is equipped. But if the player has a wespon equipped, that weapon is put on the player inventory and the chosen weapon is equipped.
This algorithm only occurs if the player has an item in his inventory.

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
			if(inventory[counter] is an item)
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
			ifinventory[counter] is an item)
				if(inventory[counter] is Food)
					Increase player HP;
				if(inventory[counter] is Weapon)
					if(player has no equipped weapon)
						equip weapon;
					if(player has equipped weapon)
						unequip weapon adding it to 
						equip chosen weapon;
				remove item from inventory;
				remvoe weight from player weight;
```

## Conclusions



## References ##

#### Other Colaborators
> **Rui Martins, Diogo Martins**
Our group and this one, worked together on most of the logic for the program. In that way, most of the logic of our code is the same. However, the coding itsef was done separately.
