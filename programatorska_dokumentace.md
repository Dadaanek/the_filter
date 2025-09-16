# The Filter

## úvod
Hra je napsána v jazyce C# verze 8.0.19 za pomoci herního enginu Godot verze 4.4.1.

## architektura projektu
Data jsou rozdělena do tří složek - assets, scenes a scripts.
Scény jsou řízeny scénou SceneHandler, která postupně vytváří (spuštění hry) a zabíjí/zneviditelňuje ostatní scény (menu po zapnutí hry). 
## dělení skriptů
### data (konstanty, dialogy...)
Většina konstant (vlny, dialogy, staty nepřátel...) je uložena ve skriptu scripts/GameData.cs, což umožňuje poměrně snadné vytváření nového obsahu (nové vlny, nepřátelé, změna dialogů, změna statů...). 

Např. přidání vlny lze udělat pouhým přidáním tuple tupleů do proměnné WAVE_DATA a přidáním floatu do proměnné waveOffsets. 
GameData.cs slouží jako zcela veřejný skript dostupný odkudkoliv, což pro žádný jiný skript neplatí - ostatní skripty jsou co nejvíce odděleny, aby na sobě byly nezávislé. 
### řídící skripty
Zbytek scriptů řídí logiku nodeů, jimž jsou přiřazeny. Obecně má každá duležitá část projektu svůj skript (hráč, SceneHandler, NPC, nepřátelé, jednotlivé postavy, svět - řídí vlny...), který má veřejné jen některé části nutné pro ostatní skripty (např. hráč má metodu GetItemInHand, která vrátí ID předmětu, který právě hráč drží, což může být užitečné např. pro TextureRect zobrazující předmět, který hráč drží v ruce...). Obecně se projekt snaží ve skriptech dodržovat objektově orientované programování. Většina informací se mezi oddělenými částmi přenáší pomocí signálů - vstup hráče do zóny NPC, ...
#### Nejdůležitější řídící scripty a jejich stručný popis:
- Player.cs - zde jsou vysvětleny jednotlivé classy, jelikož se jedná o nejobsáhlejší script a je poměrně dost členěný:
    - Player - řeší veškerou logiku nutnou pro fungování hlavní postavy za pomoci funkcí níže (ovládání, interakce s okolím, animace spriteu hráče)
    - MovementController - řídí pohyb a ovládání hlavní postavy a animace spriteu hráče
    - Inventory - ukládá si aktuální stav inventáře hráče a umožňuje inventář měnit a zobrazovat
    - ItemInteractionContext - uchovává hlavní proměnné potřebné při interakci hráče s okolím
    - ItemInteraction - řeší interakce hráče s okolím (stavění věží, interakce s předměty, sázení derivací)
    - NPCInteractionContext - uchovává hlavní proměnné potřebné při interakci hráče s NPCs
    - NPCInteraction - řeší interakce hráče s NPCs

- World.cs - řeší primárně spawnování vln nepřátel, detekci konce hry, funkci jednotlivých políček (např. zda lze na dané políčko položit věž), měnu v podobě oken a její využití (zda hráč nasbíral dostatek oken na vylepšení věže)
* MOŽNÉ VYLEPŠENÍ: odtud by se dala přesunout funkce ohledně měny přesunout do scriptu Player.cs

- SceneHandler.cs - řeší přepínání mezi různými scénami (menu, hra, finální scéna...) a ukazatele v průběhu hry (předmět, který hráč drží v ruce, zobrazování inventáře...)

- NPC.cs - řeší velikosti popupů při dialogu, sprite daného NPC, kolize, dialogy

- CaveMan.cs, Jelinek.cs, Classmate.cs - všechny hlavní classy v těchto scriptech dědí classu NPC a akorát dodávají speciální dialogové hlášky a dropování různých předmětů. CaveMan navíc sleduje, zda jakmile vstoupí area do jeho area, jde o nepřítele (speciálně okno) a pokud ano, tak se prohlásí za mrtvého

- Enemy.cs - pamatuje si důležité informace pro konkrétního nepřítele (typ, životy, rychlost) a umožňuje zvenku dostat ránu, čímž se mu sníží počet životů a pokud je menší nebo roven 0, tak zavolá Destroy(), čímž emituje signál, že byl daný nepřítel zničen. Dále sleduje, kdo se dostal do jeho kolizní area a pokud je nepřítel, kterému tento script patří, typu multiplier a narazí do okna, tak oknu vylepší a zničí se.

## diagram scén
- SceneHander
    - Menu
        - New Game
        - Help
    - World
        - Player
        - TileMapLayers
        - NPCs
        - ...

## hlavní smyčka
Složitější výpočty jsou obvykle řešeny pomocí signálů (kolize, vstoupení nepřítele do range věže), zatímco jednodušší operace jsou řešeny ve smyčce Process (aktualizace počtu zabitých oken).

## potenciální updatey
Hra je zatím v Alpha verzi, takže není moc záživná ani komplexní. Na druhou stranu je však velká část featur, které by hru výrazně obohatily, z nějaké části již naimplementována (herní ekonomika, inventář, interakce hráče s okolím, logika věží, vylepšení věží, místa vyhrazená různým věžím, interakce NPCs s hráčem...), takže potenciální update hry by se měl zaměřit na přidání nového kontentu nebo reimplementaci některých featur, než na implementaci výrazného množství nových featur.
