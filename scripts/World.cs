using Godot;
using System;
using System.Threading.Tasks;

public partial class World : Node2D
{
	private Marker2D enemySpawn;
	private PackedScene enemyScene;
	private Timer wavesOffsetsTimer;
	private Path2D enemyPath;
	private CaveMan caveman;
	private RichTextLabel windowsLabel;
	private TileMapLayer turretSpots;

	private int currentWave = 0;
	private int windowsDestroyed = 0;
	private int windowsUsed = 0;
	private bool ended = false;
	private bool? won = null;
	private bool startedWaves = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyPath = GetNode<Path2D>("Road/Path2D");
		enemySpawn = GetNode<Marker2D>("EnemySpawn");
		enemyScene = GD.Load<PackedScene>($"res://scenes/Enemy.tscn");
		wavesOffsetsTimer = GetNode<Timer>("WavesOffsetsTimer");
		windowsLabel = GetParent().GetNode<RichTextLabel>("UI/Windows/HBoxContainer/TextureRect/RichTextLabel");
		turretSpots = GetNode<TileMapLayer>("TurretSpots");

		caveman = GetNode<CaveMan>("CaveMan");

		wavesOffsetsTimer.Timeout += OnWavesOffsetsTimerTimeout;
		wavesOffsetsTimer.Start(1e10);

		windowsLabel.Text = windowsDestroyed.ToString();
	}

	public bool CanPlaceTurret(Vector2 worldPos)
	{
		Vector2I cell = turretSpots.LocalToMap(turretSpots.ToLocal(worldPos));
		int tileId = turretSpots.GetCellSourceId(cell);

		return tileId == 0;
	}

	public void startWaves()
	{
		if(!startedWaves)
		{
			startedWaves = true;
			wavesOffsetsTimer.Stop();
			wavesOffsetsTimer.Start(0.1f);
		}
	}

	public override void _Process(double delta)
	{
		windowsLabel.Text = (windowsDestroyed - windowsUsed).ToString();

		if (caveman.IsDead())
		{
			GD.Print("Caveman is dead");
			won = false;
			startedWaves = false;
		}
		else if (currentWave >= GameData.totalNumberOfWaves && enemyPath.GetChildCount() == 0)
		{
			if (!caveman.IsDead())
				won = true;
		}
	}

	public bool? Won()
	{
		return won;
	}

	Tuple<string, float>[] GetWaveData()
	{
		if (GameData.WAVE_DATA.Length > currentWave)
			return GameData.WAVE_DATA[currentWave];
		else
			return null;
	}

	public int GetCurrentWave()
	{
		return currentWave;
	}

	public int GetEnemiesCount()
	{
		return enemyPath.GetChildCount();
	}

	public bool UpgradeTurret()
	{
		if(windowsDestroyed - windowsUsed >= GameData.turretUpgradeCost)
		{
			windowsUsed += GameData.turretUpgradeCost;
			return true;
		}
		return false;
	}

	public bool BuyPenguin()
	{
		if(windowsDestroyed - windowsUsed >= GameData.penguinCost)
		{
			windowsUsed += GameData.penguinCost;
			return true;
		}
		return false;
	}

	async Task SpawnEnemies(Tuple<string, float>[] waveData)
	{
		for (int i = 0; i < waveData.Length; i++)
		{
			string enemyType = waveData[i].Item1;
			float enemyTime = waveData[i].Item2;

			Enemy enemyInstance = (Enemy)enemyScene.Instantiate();

			enemyPath.AddChild(enemyInstance);
			enemyInstance.Name = enemyType;
			enemyInstance.Position = enemySpawn.GlobalPosition;

			int health = GameData.ENEMY_STATS[enemyType].Item1;
			int speed = GameData.ENEMY_STATS[enemyType].Item2;

			enemyInstance.SetHealth(health);
			enemyInstance.SetSpeed(speed);
			enemyInstance.SetType(enemyType);
			
			// prevent duplicate signals
			// enemyInstance.Died -= OnEnemyDied;
			
			enemyInstance.Died += OnEnemyDied;

			await ToSignal(GetTree().CreateTimer(enemyTime), "timeout");
		}
	}

	private async Task StartNextWave()
	{
		Tuple<string, float>[] waveData = GetWaveData();

		// _ basically tells it that I don't want to await it
		await SpawnEnemies(waveData);

		currentWave++;
		GD.Print(currentWave);

		if (currentWave < GameData.WAVE_DATA.Length)
		{
			wavesOffsetsTimer.Start(GameData.waveOffsets[currentWave]);
		}
	}

	private void OnWavesOffsetsTimerTimeout()
	{
		GD.Print("finished");
		wavesOffsetsTimer.Stop();

		StartNextWave();
	}

	private void OnEnemyDied(Enemy enemy)
	{
		windowsDestroyed++;
	}
}
