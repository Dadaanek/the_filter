using Godot;
using System;

public partial class NPC : CharacterBody2D
{
	protected float popupWidth = GameData.popupWidth;
	protected float messagesSpeed = GameData.messagesSpeed;
	protected string[] messages;

	protected RichTextLabel popupLabel;
	protected AnimatedSprite2D animatedSprite2D;
	protected CollisionShape2D collisionShape2D;

	protected int currentMessageIndex = 0;
	protected bool playerInsideArea = false;
	protected bool talking = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite2D.Play();
		// string currentAnimation = animatedSprite2D.Animation;
		// SpriteFrames frames = animatedSprite2D.SpriteFrames;
		// Global.StartAnimation(animatedSprite2D, (float) frames.GetAnimationSpeed(currentAnimation), frames.GetFrameCount(currentAnimation));

		popupLabel = GetNode<RichTextLabel>("RichTextLabel");

		Global.CustomizeLabel(
			popupLabel,
			GameData.popupWidth,
			new Vector2(GameData.popupWidth, 0),
			GameData.popupLabelBGColor,
			GameData.popupLabelMargin,
			GameData.popupLabelSize,
			"res://assets/Pixelify_Sans/static/PixelifySans-Medium.ttf"
		);


		Area2D npcArea = GetNode<Area2D>("Area2D");
		npcArea.AreaEntered += OnArea2DEntered;
		npcArea.AreaExited += OnArea2DExited;
	}

	public override void _Process(double delta)
	{
		// GD.Print(talking);
		if (talking && playerInsideArea)
			popupLabel.Visible = true;
		else
			popupLabel.Visible = false;
	}

	public void SetTalking(bool value)
	{
		talking = value;
	}

	protected virtual void OnArea2DEntered(Area2D area)
	{
		if (area.Name == "PlayerArea")
			playerInsideArea = true;
	}

	protected virtual void OnArea2DExited(Area2D area)
	{
		if (area.Name == "PlayerArea")
		{
			playerInsideArea = false;
			SetTalking(false);
		}
	}

	protected void ShowMessage(string message)
	{
		popupLabel.Text = message;
	}
}