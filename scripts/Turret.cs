using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// name, range, damage
// Dictionary<string, Tuple<int, int>>[] turretData = new Dictionary<string, Tuple<int, int>>[]
// {
// 	new Dictionary<string, Tuple<int, int>>("string", Tuple.Create(30, 100)),
// };

public partial class Turret : Node2D
{
	private AnimatedSprite2D turretSprite;
	private const float turretRange = 30f;
	private const float firingDelay = 0.5f;
	private int damage = 30;

	private bool playerInsideArea = false;
	private Player player;

	private List<Enemy> enemyList = new List<Enemy>();
	private Enemy trackedEnemy;
	private World world;
	private bool readyToFire = true;
	private string turretType;
	private int animationFrame = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		turretSprite = GetNode<AnimatedSprite2D>("TurretSprite");

		CircleShape2D circleShape = (CircleShape2D)GetNode<CollisionShape2D>(
			"Range/CollisionShape2D").Shape;
		circleShape.Radius = turretRange;

		Area2D rangeArea = GetNode<Area2D>("Range");
		rangeArea.AreaEntered += OnAreaEntered;
		rangeArea.AreaExited += OnAreaExited;

		Area2D interactArea = GetNode<Area2D>("InteractArea");
		interactArea.AreaEntered += OnInteractAreaEntered;
		interactArea.AreaExited += OnInteractAreaExited;

		world = (World) GetParent();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (enemyList.Count > 0)
		{
			trackedEnemy = (Enemy) SelectEnemy(enemyList);

			TurnToTrackedEnemy();

			if (trackedEnemy != null && readyToFire)
			{

				_ = Fire(); // _ basically tells it that I don't want to await it
			}
		}
	}

	private async Task Fire()
	{
		readyToFire = false;

		trackedEnemy.TakeHit(damage);

		await ToSignal(GetTree().CreateTimer(firingDelay), "timeout");
		readyToFire = true;
	}

	public string GetTurretType()
	{
		return turretType;
	}

	public void SetTurretType(string turretType)
	{
		this.turretType = turretType;
	}

	public void StartAnimation()
	{
		animationFrame = 1;

		turretSprite.Animation = turretType;
		turretSprite.Play();
	}

	private Enemy SelectEnemy(List<Enemy> enemyList)
	{
		Enemy maxProgressEnemy = null;
		float maxProgress = 0f;

		for (int i = 0; i < enemyList.Count; i++)
		{
			if (enemyList[i].Progress > maxProgress)
			{
				switch (turretType)
				{
					case "Derivative":
						if (enemyList[i].GetType() == "Multiplier")
						{
							maxProgress = enemyList[i].Progress;
							maxProgressEnemy = enemyList[i];
						}

						break;

					case "Penguin":
						if (Array.IndexOf(GameData.penguinTargets, enemyList[i].GetType()) >= 0)
						{
							maxProgress = enemyList[i].Progress;
							maxProgressEnemy = enemyList[i];
						}

						break;
				}
				
			}
		}

		return maxProgressEnemy;
	}

	private void TurnToTrackedEnemy()
	{
		if(trackedEnemy != null)
			turretSprite.LookAt(trackedEnemy.Position);
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.GetParent().GetParent() is Enemy enemy)
			enemyList.Add(enemy);
	}

	private void OnAreaExited(Area2D area)
	{
		GD.Print("exited");

		if(area.GetParent().GetParent() is Enemy enemy)
			enemyList.Remove(enemy);
	}

	private void OnInteractAreaEntered(Area2D area)
	{
		if (area.GetParent() is Player player_)
		{
			GD.Print("player entered");
			playerInsideArea = true;
			player = player_;
		}
	}

	private void OnInteractAreaExited(Area2D area)
	{
		if(area.GetParent() is Player player_)
		{
			GD.Print("player exited");
			playerInsideArea = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			GD.Print("interacted with turret");
			GD.Print(playerInsideArea);
			GD.Print(player.GetItemInHand());
			if(turretType == "Penguin" && playerInsideArea && player.GetItemInHand() == "Nothing" && world.UpgradeTurret())
			{
				damage += GameData.penguinTurretUpgradeDamage;
				this.Scale *= new Vector2(1.5f, 1.5f);
			}
		}
	}
}
