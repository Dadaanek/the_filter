using Godot;
using System;

public partial class Enemy : PathFollow2D
{
	// create custom signal
	[Signal]
    public delegate void DiedEventHandler(Enemy enemy);

	private int speed = 0;

	private TextureProgressBar healthBar;
	private string enemyType;
	private AnimatedSprite2D sprite;
	private Area2D enemyArea;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		healthBar = GetNode<TextureProgressBar>("HealthBar");
		sprite = GetNode<AnimatedSprite2D>("EnemyCharacter/AnimatedSprite2D");
		enemyArea = GetNode<Area2D>("EnemyCharacter/Area2D");

		enemyArea.AreaEntered += OnAreaEntered;
	}

	public void TakeHit(int damage)
	{
		if(healthBar.Value <= 0)
			return;

		if (enemyType == "Multiplier")
		{
			healthBar.Value -= healthBar.MaxValue / 2;

			if (healthBar.Value != 0)
				sprite.Frame++;
		}
		else
		{
			healthBar.Value -= damage;
		}

		if (healthBar.Value <= 0)
		{
			Destroy();
		}
	}

	public void SetType(string type)
	{
		enemyType = type;

		if(enemyType == "WindowBig")
		{
			sprite.Animation = "Window"; // use the same sprite for Window and WindowBig
			sprite.Scale = new Vector2(1.5f, 1.5f);
		}
		else
			sprite.Animation = enemyType;

		if (enemyType == "Multiplier")
			healthBar.Visible = false;
	}

	public string GetType()
	{
		return enemyType;
	}

	private void Destroy()
	{
		this.QueueFree();
		EmitSignal(SignalName.Died, this);
	}

	public void SetHealth(int health)
	{
		healthBar.MaxValue = health;
		healthBar.Value = health;
	}

	public void SetSpeed(int speed)
	{
		this.speed = speed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	void Move(double delta)
	{
		this.Progress += speed * (float)delta;
	}

	private void OnAreaEntered(Area2D area)
	{
		if (Array.IndexOf(GameData.MULTIPLIABLE_ENEMIES, enemyType) >= 0 && area.GetParent().GetParent() is Enemy)
		{
			Enemy enemyEntered = (Enemy) area.GetParent().GetParent();
			if (enemyEntered.GetType() == "Multiplier")
			{
				healthBar.MaxValue *= 2;
				healthBar.Value *= 2;

				enemyEntered.QueueFree();
			}
		}
	}
}
