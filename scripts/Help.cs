using Godot;
using System;

public partial class Help : Control
{
	private RichTextLabel backLabel;
	private RichTextLabel textLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		textLabel = GetNode<RichTextLabel>("MarginContainer/Text");
		textLabel.Text = GameData.helpText;
		textLabel.AddThemeFontSizeOverride("normal_font_size", GameData.textLabelSize);

		var fontFile = ResourceLoader.Load<FontFile>("res://assets/Pixelify_Sans/static/PixelifySans-Medium.ttf");
		textLabel.AddThemeFontOverride("normal_font", fontFile);

		backLabel = GetNode<RichTextLabel>("MarginContainer/Back/RichTextLabel");
		backLabel.Text = "Zpět";
		backLabel.AddThemeFontSizeOverride("normal_font_size", GameData.mainMenuLabelSize);

		backLabel.AddThemeFontOverride("normal_font", fontFile);
	}
}
