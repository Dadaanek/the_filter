using Godot;
using System;

public partial class GameOver : Control
{
	private RichTextLabel textLabel;
	private RichTextLabel buttonLabel;
	private TextureButton button;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textLabel = GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");
		buttonLabel = GetNode<RichTextLabel>("VBoxContainer/TextureButton/RichTextLabel");
		button = GetNode<TextureButton>("VBoxContainer/TextureButton");
		button.Pressed += OnButtonPressed;

		Global.CustomizeLabel(
			textLabel,
			GameData.screenWidth,
			new Vector2(GameData.popupWidth, 0),
			null,
			GameData.popupLabelMargin,
			GameData.mainMenuLabelSize,
			"res://assets/Pixelify_Sans/static/PixelifySans-Medium.ttf"
		);

		Global.CustomizeLabel(
			buttonLabel,
			GameData.screenWidth,
			new Vector2(GameData.popupWidth, GameData.popupHeight),
			null,
			GameData.popupLabelMargin,
			GameData.mainMenuLabelSize,
			"res://assets/Pixelify_Sans/static/PixelifySans-Medium.ttf"
		);
	}

	private void OnButtonPressed()
	{
		var packedScene = GD.Load<PackedScene>("res://scenes/SceneHandler.tscn");
		GetTree().ChangeSceneToPacked(packedScene);
	}
}
