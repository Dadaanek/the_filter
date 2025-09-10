using Godot;
using System;

public partial class MainMenu : Control
{
	protected RichTextLabel newgameLabel;
	protected RichTextLabel helpLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		newgameLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/NewGame/Label");
		newgameLabel.Text = "Nová hra";
		newgameLabel.AddThemeFontSizeOverride("normal_font_size", GameData.mainMenuLabelSize);

		var fontFile = ResourceLoader.Load<FontFile>("res://assets/Pixelify_Sans/static/PixelifySans-Medium.ttf");
		newgameLabel.AddThemeFontOverride("normal_font", fontFile);

		helpLabel = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Help/RichTextLabel");
		helpLabel.Text = "Nápověda";
		helpLabel.AddThemeFontSizeOverride("normal_font_size", GameData.mainMenuLabelSize);

		helpLabel.AddThemeFontOverride("normal_font", fontFile);
	}
}
