using Godot;
using System;

public partial class Classmate : NPC
{
	private int pencilsLeft;
	private int interactIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		pencilsLeft = GameData.classmatePencilsLeft;

		messages = GameData.NPC_MESSAGES["Classmate"];
	}

	protected override void OnArea2DEntered(Area2D area)
	{
		if (!(area.GetParent() is Player))
			return;
			
		GD.Print("entered");
		base.OnArea2DEntered(area);

		bool showMessage = false;

		if (interactIndex % 2 == 0)
		{
			if (area.Name == "PlayerArea")
			{
				SetTalking(true);
				showMessage = true;

				ShowMessage(messages[0]);
			}
		}

		if (!showMessage)
			SetTalking(false);
		
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact") && playerInsideArea)
		{
			interactIndex++;

			GD.Print(interactIndex);

			if (interactIndex % 2 == 0)
			{
				GD.Print("talk");
				SetTalking(true);

				if (pencilsLeft > 0)
				{
					pencilsLeft--;
					string itemType = "Pencil";

					PackedScene itemScene = GD.Load<PackedScene>($"res://scenes/Item.tscn");
					Item itemInstance = (Item)itemScene.Instantiate();

					GetParent().AddChild(itemInstance);

					itemInstance.Name = itemType;
					itemInstance.Position = GlobalPosition + GameData.itemDropOffset;
					itemInstance.SetItemType(itemType);

					// this is done later so that some information is not rewritten 
					// once the scene is initiated and so that the sprite corresponds 
					// to the item type
					itemInstance.StartAnimation();
				}

				if (currentMessageIndex < GameData.NPC_MESSAGES["Classmate"].Length - 1)
					currentMessageIndex++;

				ShowMessage(messages[currentMessageIndex]);
			}
			else
			{
				SetTalking(false);
			}
		}
	}
}
