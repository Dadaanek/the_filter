using Godot;
using System;

public partial class Plant : StaticBody2D
{
	private double growTime;
	private int growStages;

	private AnimatedSprite2D plantSprite;
	private CollisionShape2D collision;
	private Timer timer;

	private int animationFrame = 0;
	private string plantType;
	private bool grown = false;
	private int cropsCount = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		plantSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		collision = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		switch (plantType)
		{
			case "Derivative":
				growTime = GameData.DERIVATIVE_GROW_TIME;
				growStages = GameData.DERIVATE_GROW_STAGES;

				timer.Start(growTime);

				break;
		}

		collision.Disabled = true;
	}

	public void Grow()
	{
		collision.Disabled = false;
		
		switch (plantType)
		{
			case "Derivative":
				cropsCount++;
				grown = true;
				animationFrame++;
				plantSprite.Frame = animationFrame;

				break;
		}
	}

	// bool tells whether the item was actually used
	public bool UseItem(Item item)
	{
		switch (item.GetItemType())
		{
			case "Pencil":
				if (grown == false)
				{
					switch (plantType)
					{
						case "Derivative":
							Grow();
							return true;

						default:
							return false;
					}
				}

				return false;

			default:
				return false;
		}
	}

	// public so that the animation can be changed after the plant type and so on is assigne
	// this is beaing assigned later so that godot doesnt rewrite it somehow
	public void StartAnimation()
	{
		animationFrame = 1;

		plantSprite.Animation = plantType;
		plantSprite.Play();
	}

	public string GetPlantType()
	{
		return plantType;
	}

	public void SetPlantType(string plantType)
	{
		this.plantType = plantType;
	}

	// plant has grown
	public void OnTimerTimeout()
	{
		GD.Print("time");
		timer.Stop();

		animationFrame++;
		plantSprite.Frame = animationFrame - 1;

		if (animationFrame == growStages)
		{
			collision.Disabled = false;

			Grow();
		}
		else
		{
			timer.Start(growTime);
		}
	}
}
