# The Filter

![ingame image](./assets/ingame.png)

## Popis programu
Jedná se o single-player hru složenou z několika herních žánrů, primárně tower-defense, rpg a puzzle. Cílem projektu není vytvořit výjimečnou hru z hlediska gameplaye, ale spíše snaha sjednotit různé herní mechaniky do jednoho celku - jedná se tedy spíše o takový pokus než o seriózní projekt. Na druhou stranu budou scripty volně dostupné ke stažení, takže může sloužit jako boilerplate pro uživatele, jež by si chtěli nějak hru dále upravit, pokud obsahuje featury, které by se jim pro jejich projekt hodily.

Hra je naprogramována v herním enginu Godot za pomoci C#.

## Popis hry
Cílem hráče je ubránit jeskynního muže před okny, která se snaží dostat do jeho jeskyně. Bránění funguje jako tower-defense, kde hráč může pokládat tučňáky, ktere dostane právě od jeskynního muže, a ti útočí na nepřátelská okna. Dalšími nepřáteli jsou tzv. "multiplikátory", které při kolizi s oknem zvýší počet jeho životů na dvojnásobek. Na "multiplikátory" útočí derivace, což je další typ věží. Derivaci hráč obdrží od "Jelínka" po krátkém dialogu. Dále může za pomoci tužky, kterou získá od spolužáka, a papíru tuto derivaci pěstovat a vytvořit jich tak více. Všichni nepřátele přichází ve vlnách a pohybují se po cestě na mapě.

## Spuštění programu
Pro spuštění programu je nutné udělat:
- Nainstalovat Godot s podporou C#.
- Stáhnout soubory z tohoto GitHub repozitáře (hlavně assets, scenes, scripts a zbytek souborů až na README.md, poznamky a programatorska_dokumentace.md).
- Spustit Godot a otevřít složku, do níž byly staženy soubory, jako projekt.
- Spustit projekt a pomocí klávesy F5 spustit hru.

Zatím nejsou k dispozici executables daného programu, jelikož je neustále aktualizován.

## Spuštění hry
Jakmile spustíte program, objeví se Vám hlavní menu, kde máte 2 možnosti:
- Nová hra, což spustí samotnout hru.
- Nápověda, kde je vysvětleno ovládání a stručný text o samotné hře.
![menu image](./assets/menu.png)

## Ukončení programu
Zavřením okna, ve kterém hra běží.

## Formát vstupu / ovládání
Uživatel se v menu pohybuje za pomoci myši a ve hře za pomoci kláves:
* WASD - pohyb,
* E - interakce (NPC, předměty, turrety...),
* TAB - zobrazení inventáře,
* ESC - pausa,

## Gameplay
Cílem hráče je dostat se k jedné z finálních scén. Nyní jsou pouze 2 takové scény, takže přidání dalších konců je jedno z možných vylepšení tohoto projektu.

## Herní featury
Ačkoliv je hra poměrně jednoduchá, je v ní naimplementována poměrně dost herních featur z různých herních žánrů. Jedná se primárně o:
- Pohyb hráče pomocí kláves.
- Inventář (hráč má nějaký svůj seznam předmětů, který se v průběhu hry mění a lze jej zobrazit pomocí klávesy TAB) a interakce s předměty (sbírání, pokládání, používání...).
- Ekonomika (hráč může utrácet zabitá okna).
- Stavění a vylepšování věží.
- Pěstování a sbírání předmětů (derivace).
- Interakce s NPCs (dialogy, koupě předmětů, získávání předmětů, spouštení fází hry závislých na progressu dialogu).
- Fungování věží (zaměřování nepřátel, rozlišování nepřátel).
- Interakce nepřátel mezi sebou (jeden nepřítel vylepšuje druhého).

## Potřebné knihovny
* Godot
* základní systémové knihovny (.NET)
