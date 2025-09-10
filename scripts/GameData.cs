using Godot;
using System;
using System.Collections.Generic;

public static class GameData
{
	// PLAYER
	public static readonly int playerMovementSpeed = 30;

	// GROUND
	public static readonly int PAPER_ID = 2;
	public static readonly int GRASS_ID = 5;
	public static readonly int DANDELION_ID = 8;

	public static readonly Vector2 tileMapSize = new Vector2(200, 200);
	public static readonly int coordOffset = 100;

	// PLANTS
	public static readonly float DERIVATIVE_GROW_TIME = 100f;
	public static readonly int DERIVATE_GROW_STAGES = 2;

	public static readonly Dictionary<string, int> harvestAmount = new Dictionary<string, int>
	{
		{ "Derivative", 2 },
	};

    // ITEMS
	public static readonly string[] ITEMS = new string[] { "Nothing", "Derivative", "Pencil", "Penguin" };
	public static readonly string[] PLANTABLE_ITEMS = new string[] { "Derivative" };
	public static readonly string[] BUILDABLE_ITEMS = new string[] { "Penguin", "Derivative", };

	public static readonly Dictionary<string, int> ITEM_TO_ID_DICT = new Dictionary<string, int>();
	public static readonly Dictionary<int, string> ID_TO_ITEM_DICT = new Dictionary<int, string>();
	public static readonly Dictionary<string, bool> IS_PLANTABLE_DICT = new Dictionary<string, bool>();
	public static readonly Dictionary<string, bool> IS_BUILDABLE_DICT = new Dictionary<string, bool>();

	public static readonly Dictionary<string, string> ITEM_TRANSLATION_TO_CZECH = new Dictionary<string, string>
	{
		{ "Nothing", "Nic" },
		{ "Derivative", "Derivace" },
		{ "Pencil", "Tužka" },
		{ "Penguin", "Tučňák" },
	};

	public static readonly Dictionary<string, string[]> PLANT_BOOSTERS = new Dictionary<string, string[]>
	{
		{ "Derivative", new string[] { "Pencil" } },
	};

    // converts item name to its id
	public static int ITEM_TO_ID(string item)
	{
		if (!ITEM_TO_ID_DICT.ContainsKey(item))
			throw new ArgumentException("key does not exist");
		return ITEM_TO_ID_DICT[item];
	}

	public static string ID_TO_ITEM(int id)
	{
		if (!ID_TO_ITEM_DICT.ContainsKey(id))
			throw new ArgumentException("key does not exist");
		return ID_TO_ITEM_DICT[id];
	}

	public static bool CAN_BOOST(string plant, string item)
	{
		if (PLANT_BOOSTERS.ContainsKey(plant))
		{
			string[] boosters = PLANT_BOOSTERS[plant];

			bool canBoost = false;
			for (int i = 0; i < boosters.Length; i++)
			{
				if (boosters[i] == item)
					canBoost = true;
			}

			return canBoost;
		}

		return false;
	}

	public static bool IS_PLANTABLE(string item)
	{
		return IS_PLANTABLE_DICT[item];
	}

	public static bool IS_BUILDABLE(string item)
	{
		return IS_BUILDABLE_DICT[item];
	}

	// ENEMIES
	public static readonly string[] MULTIPLIABLE_ENEMIES = new string[] { "Window", "WindowBig" };
	public static readonly string[] DEADLY_ENEMIES = new string[] { "Window", "WindowBig" };
	// name, health, speed
	public static readonly Dictionary<string, Tuple<int, int>> ENEMY_STATS = new Dictionary<string, Tuple<int, int>>
	{
		{ "Window", new Tuple<int, int>(10, 70) },
		{ "Multiplier", new Tuple<int, int>(100, 60) },
		{ "WindowBig", new Tuple<int, int>(30, 150) },
	};

	// ENEMY WAWES
	public static readonly float[] waveOffsets = new float[] { 500f, 5f, 5f, 5f };

	// enemy, time before next enemy (information for each wave)
	public static readonly Tuple<string, float>[][] WAVE_DATA = new Tuple<string, float>[][]
	{
		new Tuple<string, float>[]
		{
			new Tuple<string, float>("WindowBig", 0.2f),
			new Tuple<string, float>("Window", 2f),
			new Tuple<string, float>("Multiplier", 0.2f)
		},
		new Tuple<string, float>[]
		{
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 2f),
			new Tuple<string, float>("Multiplier", 0.2f)
		},
		new Tuple<string, float>[]
		{
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("Window", 1f)
		},
		new Tuple<string, float>[]
		{
			new Tuple<string, float>("WindowBig", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("WindowBig", 1f),
			new Tuple<string, float>("Window", 1f),
			new Tuple<string, float>("WindowBig", 1f),
			new Tuple<string, float>("Multiplier", 0.2f),
			new Tuple<string, float>("Multiplier", 0.2f),
			new Tuple<string, float>("Multiplier", 0.2f),
			new Tuple<string, float>("Multiplier", 0.2f),
			new Tuple<string, float>("Multiplier", 0.2f),
		},
	};

	public static readonly int totalNumberOfWaves = WAVE_DATA.Length;

	// NPCS
	// name, messages
	public static readonly Dictionary<string, string[]> NPC_MESSAGES = new Dictionary<string, string[]>
	{
		{
			"Jelinek", new string[]
			{
				"Dobrý den, Vítejte v této hře... Zmáčkněte tlačítko 'E' pro interakci se mnou.",
				"Toto je velice mocná zbraň, s jejíž pomocí můžete zjednodušit např. různé rovnice.",
				"Mohlo by se vám to hodit. Dávejte si však pozor, protože jakmile ji jednou použijete, už ji nelze zvednout zpátky...",
				"Vidíte tamten papír? Existuje způsob, jak derivaci 'rozmnožit', ale víc vám k tomu nepovím...",
				"Přeji hodně štěstí.",
			}
		},
		{
			"Caveman_random_help", new string[]
			{
				"Ať už je to cokoliv, máš to už vědět z předchozího gameplaye!",
				"Přeji hodně štěstí :)",
				"Najdi si to, ale nedívej se na internet, to by bylo k ničemu...",
			}
		},
		{
			"Caveman_random_hate", new string[]
			{
				"Tak co, už to máš? Předešlému hráči to šlo teda rychleji...",
				"Jak tě tak vidím, nevypadá to se mnou zrovna růžově..."
			}
		},
		{
			"Caveman", new string[]
			{
				"Mám pro tebe důležitý úkol. Musíš mi pomoct! Zase po mně jdou...",
				"Musíš mě uchránit před těmi zlými okny, které se mi snaží dostat do mé jeskyně.",
				"Slyšel jsem, že se dají zahubit pouze za pomoci tučňáků, tak ti dám tři.",
				"Ale šetři si je, protože další už nedostaneš...",
				"Pokud mi pár těch oken doneseš, možná ti za to něco dám.",
				"Jo ještě něco... Pokud bys chtěl s něčím poradit, ozvi se tlačítkem 'E'.",
				"Co zase chceš?"
			}
		},
		{
			"Caveman_final", new string[]
			{
				"Sice to nebyl zrovna nejlepší výkon, ale prošel jsi :|",
				"No... Tak tohle se ti teda moc nepovedlo..."
			}
		},
		{
			"Classmate", new string[]
			{
				"Čau, potřebuješ něco?",
				"No dobře, tu máš...",
				"Už zase? Tak co mám dělat...",
				"Ale tentokrát už naposledy... Co s tím pořád děláš???",
				"Ne, žádnou už ti nedám, protože jich sám mám málo..."
			}
		},
		{
			"Player_to_classmate", new string[]
			{
				"Ahoj, nemáš náhodou tužku na půjčení?",
				"Čau, nemáš další tužku na 'půjčení'?",
			}
		}
	};

	public static readonly float messagesSpeed = 4.0f;

	// 2 = intro message + decline message
	public static readonly int classmatePencilsLeft = NPC_MESSAGES["Classmate"].Length - 2;

	public static readonly int penguinDropIndex = 3;
	public static readonly int penguinsToDrop = 3;

	public static readonly Vector2 itemDropOffset = new Vector2(-10, 0);

	// UI
	public static readonly int popupWidth = 100;
	public static readonly int popupHeight = 30;
	public static readonly int screenWidth = 320;
	public static readonly int popupLabelSize = 10;
	public static readonly Color popupLabelBGColor = new Color(0.2f, 0.2f, 0.2f);
	public static readonly int popupLabelMargin = 2;
	public static readonly int mainMenuLabelSize = 15;
	public static readonly int textLabelSize = 7;
	public static readonly Vector2 itemIconSize = new Vector2(16, 16);
	public static readonly Vector2 inventoryOffset = new Vector2(0, -16);

	public static readonly string helpText = @"
	Ovládání: 
		E - interakce. Obvykle s NPC, ale také pro sbírání a používání předmětů...
		Q - přehazování předmětů v ruce
		WASD - pohyb
		ESC - pausnutí hry
		TAB - toggle mezi ukazováním inventáře
	Popis hry:
		Jedná se o mix tower defense a puzzle stylů. Hra je schválně málo vysvětlená, 
	aby si na jednotlivé mechaniky musel hráč přijít sám. Jelikož není moc komplexní, 
	nedává smysl hráči zjednodušovat průchod hrou, jelikož by pak hra nebyla už vůbec 
	záživná.";

	public static readonly string[] endMessages = new string[]
	{
		"Podařilo se ti uchránit jeskynního muže. Skvělá práce :)",
		"Bohužel se ti nepodařilo ubránit jeskynního muže. Můžeš to zkusit znova."
	};

	// RUNS WHEN CLASS IS CREATED
	static GameData()
	{
		for (int i = 0; i < ITEMS.Length; i++)
		{
			ITEM_TO_ID_DICT.Add(ITEMS[i], i);
			ID_TO_ITEM_DICT.Add(i, ITEMS[i]);

			IS_PLANTABLE_DICT.Add(ITEMS[i], false);
			IS_BUILDABLE_DICT.Add(ITEMS[i], false);
		}

		for (int i = 0; i < PLANTABLE_ITEMS.Length; i++)
		{
			IS_PLANTABLE_DICT[PLANTABLE_ITEMS[i]] = true;
		}

		for (int i = 0; i < BUILDABLE_ITEMS.Length; i++)
		{
			IS_BUILDABLE_DICT[BUILDABLE_ITEMS[i]] = true;
		}
	}

	// TURRETS
	public static readonly int turretUpgradeCost = 10;
	public static readonly int penguinCost = 10;
	public static readonly int penguinTurretUpgradeDamage = 1000;
	public static readonly string[] penguinTargets = new string[] { "Window", "WindowBig" };
}
