using Godot;
using System;

public partial class Jelinek : NPC
{
	private bool droppedDerivative = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		messages = GameData.NPC_MESSAGES["Jelinek"];
	}

	protected override void OnArea2DEntered(Area2D area)
	{
		base.OnArea2DEntered(area);

		if (area.Name == "PlayerArea")
		{
			if (currentMessageIndex < messages.Length)
			{
				SetTalking(true);
				ShowMessage(messages[currentMessageIndex]);
			}
			else
				SetTalking(false);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && playerInsideArea)
		{
			if (!droppedDerivative)
			{
				droppedDerivative = true;

				string itemType = "Derivative";

				PackedScene itemScene = GD.Load<PackedScene>($"res://scenes/Item.tscn");
				Item itemInstance = (Item)itemScene.Instantiate();

				GetParent().AddChild(itemInstance);
				itemInstance.Name = itemType;
				itemInstance.Position = GlobalPosition + GameData.itemDropOffset;
				itemInstance.SetItemType(itemType);

				AnimatedSprite2D itemSprite = itemInstance.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
				itemSprite.Animation = itemType;
			}

			currentMessageIndex++;

			if (currentMessageIndex < messages.Length)
			{
				SetTalking(true);
				ShowMessage(messages[currentMessageIndex]);
			}
			else
				SetTalking(false);
		}
	}
}
