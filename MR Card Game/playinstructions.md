# MR Card Game Instructions

## What is the MR Card Game?

<div><center><a class="image main"><img src="https://user-images.githubusercontent.com/44223481/157049038-248b81c4-30b8-4a06-b05d-1a28b1708b5e.png" alt="" style="max-width: 40%;"/></a></center></div>


MR Card Game is an educational, customizable AR (Augmented Reality) tower defense card game application that allows for playful learning by combining quiz questions with engaging real-time strategy gameplay. The goal of the game is to defend your castle against approaching attackers by casting spells, setting traps and building powerful defensive towers to slow down the invaders. In order to gain access to these defenses, players need to answer questions from a chosen field or topic. If answered correctly, players can use varying cards to defeat wave after wave of a wide variety of attackers.

## How to Setup the Game

### AR Image Targets
To play the MR Card Game, printing out the AR Image Targets is required. These are physical card that you can find here. Simply download and print out the sheet in this .pdf-file and cut out the individual cards with scissors or a different cutting device.

<div> <ul class="icons alt">
			<li><a href="https://github.com/JulianStaab/MR-Card-Game/raw/UIRevision/MR%20Card%20Game/Assets/ImageTargetsModified.pdf" class="button icon fa-download">Image Targets</a></li>
	  </ul>
</div>

### Quizzes
In order to play the MR Card Game, you will also need a pre-build quiz from which the questions are asked during the game. You can either create this quiz yourself using the MR Card Game Question Creator Tool which you can download [here](https://github.com/rwth-acis/MR-Question-Creator-For-Card-Game), or you can simply download a pre-built quiz in the app. Simply select `Download Levels` in the Main Menu and click on any of the quizzes that interest you. They are immediately downloaded and ready to play in the `Play Levels` section in the Main Menu.

After creating or downloading a quiz, the quiz folder has to be placed on your device in the following folder:
- **Android:** `\Card\Android\data\com.RWTH-ACIS.MRCardGame\files\` 
- **Windows:** `C:\Users\USERNAME\AppData\LocalLow\RWTH-ACIS\MR Card Game`

This is important otherwise the quiz won't show up in the app. 


## Navigating the Main Menu
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/159561647-d021f722-c9f9-4a41-9a9c-fc74d451462a.PNG" alt="" style="max-width: 100%;"/></a></div>

After starting the application, you will be placed in the main menu. There you have three options to select: `Play Levels`, `Download Levels` and `Gallery`. Clicking on the cross button above the three options and to the right of the title will close the application. This button, although named differently can be found on all menu pages as either back or menu button sending you one page back or back to the main menu page entirely.
### Play Levels
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/155305352-bc0921a6-8392-4c7a-bc31-a0dba8879455.png" alt="" style="max-width: 100%;"/></a></div>

Clicking `Play Levels` is the standard way to start a game. The next page will show the folder structure of the application where you can select the quiz to use in the game. If everything was set-up properly, you should see the quiz that you downloaded and placed in this folder in the Setup section. If not, make sure all Quiz Setup steps are fulfilled. After selecting a quiz, you will see the quiz name and description and a start button in the form of a triangle. Press it to get to the start screen where you can press `Start` to go start the game.

### Download Levels
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/159561650-34e761dc-825c-4957-8fe0-a8f62d9b650a.PNG" alt="" style="max-width: 100%;"/></a></div>

Clicking `Download Levels` allows users to play the game without creating their own quizzes. The next page will show the pre-built quizzes which can all be downloaded by clicking on them. Clicking one will immediately download it and if you select back at the top and `Play Levels` in the Main Menu, you will see it as a playable quiz.

### Gallery
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/155305338-84d6d042-53bb-4f83-9a1e-db1a31f1a33f.PNG" alt="" style="max-width: 100%;"/></a></div>

Navigating the Gallery allows players to learn about all the different defenses they have available to them during the game. Each gallery page provides detailed information about all the kinds of towers, traps and spells. Each tower page holds the strength of its attack as *Attack*, the range of the attacks of the tower as *Range*, the frequency of its attacks as *Attackspeed* and sometimes a special effect as *Special*. Each trap page holds the strength of the trap's slowdown effect as *Slow Effect*, the size of the traps as *Size* and sometimes a special effect as *Special*. Each spell page holds different information depending on the type of spell. If applicable the different entries mean the following: The damage the spell card will apply onto enemies as *Damage*, the range of the spell's effect or damage as *Range*, the probability with which to draw the spell as *Number in deck*, the target of the spell's effect or damage as *Target*, the negative effects of the spell as *Malus*, the effects of the spell as *Effect*, the duration of the spell's effect or damage as *Duration* and sometimes a special effect as *Special*.

## How to Play the Game
After successfully starting a game as described in the `Play Levels` section, the game starts with the setup phase. In the in-game UI you will see the starting `Currency` of 150. Players can use this currency to buy towers, traps and spells in the game. The `Wave` counter will also be visible and currently set to 0 as no wave has started yet. With the start of each wave this counter will increase. Right next to it you can find the `Start next wave` button which upon being pressed, will start the first wave. Lastly, the cross in the top right corner allows you to give up and return to the main menu.
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/157259662-c580b794-8d4d-43b3-9936-4f7577e060e1.png" alt="" style="max-width: 100%;"/></a></div>

### The Game Board
<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/157259660-af5402f0-71b8-4ee6-a694-5f9554672814.png" alt="" style="max-width: 100%;"/></a></div>

To start, place the **Board** card firmly within the camera field, preferably on a flat surface. The AR camera will detect the card and display the game board on top of it. The board consists of many components. The castle that is connected to the stone path is your main base of defense. This is the castle you have to protect from the invading enemies. Every time an enemy enters the castle, the castle will take damage depending on the enemy type. The castle's health is displayed above it as a green bar that will grow increasingly red the more damage it takes. The stone path, the castle is connected to, is the walk path of the enemies. They will spawn at the Portal placed opposite of the castle and walk all the way around the woods in the middle along the stone path to reach the castle. But while they are walking toward the castle, they are vulnerable to your attacks. Use this to your advantage to strategically play and place defenses along this path to protect the castle.

<div><a class="image main"><img src="https://user-images.githubusercontent.com/19326682/157259669-1c4e35db-91f4-4464-a9e7-b3249a456624.png" alt="" style="max-width: 100%;"/></a></div>

### The Setup Phase (Wave 0)
The game always starts in the Setup Phase (Wave 0) and will only start Wave 1 if the `Start next wave` button is clicked. Until then, players have time to set up towers and traps to stop the attackers in the coming waves. How to play each type of defense will be explained in more detail in the upcoming sections. Use this setup time wisely and prepare for the first wave. If you are ready, start the first wave.

### Tower Cards
In order to build towers on the board, players have to place the **Tower** card within the camera field and on a position on the board. Towers can only be built on the game board. When placed correctly, two buttons will appear over the card: `Build Tower` and `Build Trap`. Click on `Build Tower` and select the kind of tower that you want to build in the menu that now opened. Towers that the player cannot afford aren't available. During this building process the game is paused. The question counter will increase by 1 and you will be able to press the new button in the top right corner to answer the question necessary to build the tower (see the *Answering Questions* section). If the player correctly answered the question, the tower will be built and immediately start attacking enemies in range. (For information on individual tower effects, check the in-game *Gallery*.)

### Trap Cards
In order to build traps on the board, players have to place the **Tower** card within the camera field and on a position on the board. Traps can only be built on the game board. When placed correctly, two buttons will appear over the card: `Build Tower` and `Build Trap`. Click on `Build Trap` and select the kind of trap that you want to build in the menu that now opened. Traps that the player cannot afford aren't available. During this building process the game is paused. The question counter will increase by 1 and you will be able to press the new button in the top right corner to answer the question necessary to build the trap (see the *Answering Questions* section). If the player correctly answered the question, the trap will be built and immediately effect enemies walking over it. (For information on individual trap effects, check the in-game *Gallery*.)

### Spell Cards
In order to cast spells, players have to place one of the seven different **Spell** cards within the camera field. A new text will appear on the card saying `Draw Spell`, which if clicked upon, will allow players to draw a spell card to later play it. The question counter will increase by 1 and you will be able to press the new button in the top right corner to answer the question necessary to unlock the spell (see the *Answering Questions* section). If the player correctly answered the question, the spell will be unlocked, meaning the next time the spell card is held into the camera, the player will be see a new text on the card saying `Play X` where *X* is the name of the spell. By pressing it, the spell is played and its effect resolved. (For information on individual spell effects, check the in-game *Gallery*.)

### Answering Questions
The answering of questions will always be prompted by the player if they want to play a spell, tower or trap card. If so, the question counter will increase by 1 and you will be able to press the new button in the top right corner to answer the question. A random question from the selected quiz will then appear on the lower half of the screen. Questions can either be multiple-choice or input questions. Read the question carefully and if it is a multiple-choice question, select one of the answers and press the `Enter` button. If you accidentally selected a wrong answer, pressing the answer again will unselect it. If it is an input question, click on the input bar and enter your answer using the keyboard. Afterwards, also press `Enter` to submit it. If your answer is correct, the respective effect of the card is resolved as described in the above sections. If not, the card needs to be played again.
