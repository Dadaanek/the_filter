using Godot;
using System;

public partial class CanvasLayerGUI : Godot.CanvasLayer
{
	private Label itemInHandLabel;
	private Label integralsLabel;
	private Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		itemInHandLabel = GetNode<Label>("ItemInHand");
		integralsLabel = GetNode<Label>("Integrals");
		player = GetNode<Player>("../Player");
	}

	public override void _Process(double delta)
	{
		ChangeLabels();
	}

	public void ChangeLabels()
	{
		string itemInHand = player.GetItemInHand();

		itemInHandLabel.Text = "předmět: " + GameData.ITEM_TRANSLATION_TO_CZECH[itemInHand];

		integralsLabel.Text = "derivace: " + player.GetItemAmount("Derivative");
	}
}
