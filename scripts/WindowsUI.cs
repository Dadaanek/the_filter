using Godot;
using System;
using System.Collections.Generic;

public partial class WindowsUI : Control
{
	private HBoxContainer container;
	private TextureRect icon;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		container = GetNode<HBoxContainer>("HBoxContainer");
		icon = GetNode<TextureRect>("HBoxContainer/TextureRect");

		icon.Texture = GD.Load<Texture2D>("res://assets/itemTextures/windows.png");
		icon.CustomMinimumSize = 2 * GameData.itemIconSize;
	}
}
