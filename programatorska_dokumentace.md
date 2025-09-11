# The Filter

## úvod
Hra je napsána v jazyce C# verze 8.0.19 za pomoci herního enginu Godot verze 4.4.1.

## architektura projektu
Data jsou rozdělena do tří složek - assets, scenes a scripts.
Scény jsou řízeny scénou SceneHandler, která postupně vytváří (spuštění hry) a zabíjí/zneviditelňuje ostatní scény (menu po zapnutí hry). Většina konstant (vlny, dialogy, staty nepřátel...) je uložena ve skriptu scripts/GameData.cs, což umožňuje poměrně snadné vytváření nového obsahu (nové vlny, nepřátelé, změna dialogů, změna statů...). Např. přidání vlny lze udělat pouhým přidáním tuple tupleů do proměnné WAVE_DATA a přidáním floatu do proměnné waveOffsets. GameData.cs slouží jako zcela veřejný skript dostupný odkudkoliv, což pro žádný jiný skript neplatí - ostatní skripty jsou co nejvíce odděleny, aby na sobě byly nezávislé. Zbytek scriptů řídí logiku nodeů, jimž jsou přiřazeny. Obecně má každá duležitá část projektu svůj skript (hráč, SceneHandler, NPC, nepřátelé, jednotlivé postavy, svět - řídí vlny...), který má veřejné jen některé části nutné pro ostatní skripty (např. hráč má metodu GetItemInHand, která vrátí ID předmětu, který právě hráč drží, což může být užitečné např. pro TextureRect zobrazující předmět, který hráč drží v ruce...). Obecně se projekt snaží ve skriptech dodržovat objektově orientované programování.

## diagram scén
SceneHander
    - Menu
        - New Game
        - Help
    - World
        - Player
        - TileMapLayers
        - NPCs
        - ...

## hlavní smyčka
Složitější výpočty jsou obvykle řešeny diskrétně (interakce hráče s NPCs) nebo pomocí signálů (kolize, vstoupení nepřítele do range věže), zatímco jednodušší operace jsou řešeny ve smyčce Process (aktualizace počtu zabitých oken).

## potenciální updatey
Hra je zatím v Alpha verzi, takže není moc záživná ani komplexní. Na druhou stranu je však velká část featur, které by hru výrazně obohatily, z nějaké části již naimplementována (herní ekonomika, inventář, interakce hráče s okolím, logika věží, vylepšení věží, místa vyhrazená různým věžím, interakce NPCs s hráčem...), takže potenciální update hry by se měl zaměřit na přidání nového kontentu nebo reimplementaci některých featur, než na implementaci výrazného množství nových featur.
