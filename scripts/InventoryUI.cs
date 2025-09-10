using Godot;
using System;
using System.Collections.Generic;

public partial class InventoryUI : Control
{
	private HBoxContainer container;
	private TextureRect selectedItem;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		container = GetNode<HBoxContainer>("HBoxContainer");
		selectedItem = GetNode<TextureRect>("SelectedItem");

		selectedItem.Texture = GD.Load<Texture2D>("res://assets/itemTextures/Nothing.png");
		selectedItem.CustomMinimumSize = 2 * GameData.itemIconSize;
	}

	public override void _Process(double delta)
	{
		if (GetNodeOrNull<Player>("../../World/Player") != null)
		{
			Player player = GetNode<Player>("../../World/Player");
			string item = player.GetItemInHand();

			selectedItem.Texture = GD.Load<Texture2D>("res://assets/itemTextures/" + item + ".png");
		}
	}

	public void RecreateInventoryBar(List<(string item, int amount)> items)
	{
		foreach (Node child in container.GetChildren())
		{
			child.QueueFree();
		}

		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].item != "Nothing")
			{
				PackedScene itemScene = GD.Load<PackedScene>("res://scenes/ItemUI.tscn");
				TextureRect itemInstance = (TextureRect)itemScene.Instantiate();
				RichTextLabel amountLabel = itemInstance.GetNode<RichTextLabel>("Amount");

				container.AddChild(itemInstance);

				amountLabel.Text = items[i].amount.ToString();
				amountLabel.AddThemeFontSizeOverride("normal_font_size", GameData.textLabelSize);

				itemInstance.CustomMinimumSize = GameData.itemIconSize;
				itemInstance.Texture = GD.Load<Texture2D>("res://assets/itemTextures/" + items[i].item + ".png");
			}
		}
	}
}
