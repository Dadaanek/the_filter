using Godot;
using System;

public partial class CaveMan : NPC
{
	private Random rnd = new Random();
	private World world;

	private string[] randomHelpMessages;
	private string[] randomHateMessages;
	private string[] finalMessages;

	private bool shownIntroDialogue = false;
	private bool dead = false;

	private Timer randomTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		messages = GameData.NPC_MESSAGES["Caveman"];
		randomHelpMessages = GameData.NPC_MESSAGES["Caveman_random_help"];
		randomHateMessages = GameData.NPC_MESSAGES["Caveman_random_hate"];
		finalMessages = GameData.NPC_MESSAGES["Caveman_final"];

		world = (World) GetParent();

		randomTimer = GetNode<Timer>("RandomTimer");
		randomTimer.Timeout += OnRandomTimerTimeout;
	}

	public bool IsDead()
	{
		return dead;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (randomTimer.TimeLeft == 0 && shownIntroDialogue == true && popupLabel.Text == "")
			randomTimer.Start(rnd.Next(20, 50));

		if (dead)
		{
			ShowMessage(finalMessages[1]);
			popupLabel.Visible = true;
		}
		else if (world.GetCurrentWave() == GameData.totalNumberOfWaves + 1 && world.GetEnemiesCount() == 0)
		{
			ShowMessage(finalMessages[0]);
			popupLabel.Visible = true;
		}
	}

	protected override void OnArea2DEntered(Area2D area)
	{
		base.OnArea2DEntered(area);

		if (area.Name == "PlayerArea")
		{
			if(currentMessageIndex < messages.Length)
			{
				SetTalking(true);
				ShowMessage(messages[currentMessageIndex]);
			}
		}

		if (area.GetParent().GetParent() is Enemy)
		{
			Enemy enemyEntered = (Enemy)area.GetParent().GetParent();
			if (Array.IndexOf(GameData.DEADLY_ENEMIES, enemyEntered.GetType()) >= 0)
			{
				dead = true;
			}
			else if(enemyEntered.GetType() == "Multiplier")
			{
				enemyEntered.QueueFree();
			}
		}
	}

	private void DropPenguin(Vector2I offset)
	{
		string itemType = "Penguin";

		PackedScene itemScene = GD.Load<PackedScene>($"res://scenes/Item.tscn");
		Item itemInstance = (Item)itemScene.Instantiate();

		GetParent().AddChild(itemInstance);
		itemInstance.Name = itemType;
		itemInstance.Position = GlobalPosition + GameData.itemDropOffset + offset;
		itemInstance.SetItemType(itemType);

		AnimatedSprite2D itemSprite = itemInstance.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		itemSprite.Animation = itemType;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && playerInsideArea)
		{
			currentMessageIndex++;

			if (currentMessageIndex < messages.Length - 1)
			{
				ShowMessage(messages[currentMessageIndex]);

				if (currentMessageIndex == GameData.penguinDropIndex)
				{
					for (int i = 0; i < GameData.penguinsToDrop; i++)
					{
						DropPenguin(new Vector2I(i * -5, 0));
					}
				}

			}
			else if(((World)GetParent()).BuyPenguin())
			{
				DropPenguin(new Vector2I(0, 0));
			}
			else
			{
				shownIntroDialogue = true;
				world.startWaves();

				ShowMessage(randomHelpMessages[rnd.Next(0, randomHelpMessages.Length)]);
			}
		}
	}

	private void OnRandomTimerTimeout()
	{
		randomTimer.Stop();

		if(popupLabel.Text == "")
			ShowMessage(randomHateMessages[rnd.Next(0, randomHateMessages.Length)]);
	}
}
