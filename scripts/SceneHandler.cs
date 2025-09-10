using Godot;
using System;
using System.Collections.Generic;

public partial class SceneHandler : Node
{
	private TextureButton newGame;
	private TextureButton help;
	private World gameInstance;
	private Control helpInstance;
	private Control mainMenuInstance;
	private Control pauseInstance;
	private Control gameOverInstance;
	private InventoryUI inventoryInstance;
	private HBoxContainer inventoryContainer;
	private TextureRect selectedItem;
	private CanvasLayer canvasLayer;

	private bool showingInventory = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainMenuInstance = GetNode<Control>("MainMenu");
		pauseInstance = GetNode<Control>("Pause");
		canvasLayer = GetNode<CanvasLayer>("UI");
		inventoryContainer = GetNode<HBoxContainer>("UI/InventoryUI/HBoxContainer");
		inventoryInstance = GetNode<InventoryUI>("UI/InventoryUI");
		selectedItem = GetNode<TextureRect>("UI/InventoryUI/SelectedItem");
		gameOverInstance = GetNode<Control>("GameOverLayer/GameOver");

		canvasLayer.Visible = false;
		pauseInstance.Visible = false;
		inventoryContainer.Visible = false;
		selectedItem.Visible = false;
		gameOverInstance.Visible = false;

		inventoryContainer.Position += GameData.inventoryOffset;

		newGame = GetNode<TextureButton>("MainMenu/MarginContainer/VBoxContainer/NewGame");
		newGame.Pressed += OnNewGamePressed;

		help = GetNode<TextureButton>("MainMenu/MarginContainer/VBoxContainer/Help");
		help.Pressed += OnHelpPressed;
	}

	public override void _Process(double delta)
	{
		if (gameInstance != null)
		{
			if (gameInstance.Won() != null)
			{
				GD.Print("Game ended");
				bool won = (bool)gameInstance.Won();
				gameOverInstance.Visible = true;

				RichTextLabel endMessage = gameOverInstance.GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");

				if (won)
				{
					endMessage.Text = GameData.endMessages[0];
				}
				else
				{
					endMessage.Text = GameData.endMessages[1];
				}
			}
		}
	}

	void OnNewGamePressed()
	{
		if (mainMenuInstance != null)
			mainMenuInstance.QueueFree();

		if (helpInstance != null)
			helpInstance.QueueFree();
			
		if (gameInstance != null)
			gameInstance.QueueFree();

		PackedScene gameScene = GD.Load<PackedScene>("res://scenes/World.tscn");
		gameInstance = (World)gameScene.Instantiate();

		AddChild(gameInstance);
		canvasLayer.Visible = true;

		selectedItem.Visible = true;
	}

	void OnHelpPressed()
	{
		mainMenuInstance.Visible = false;

		if (helpInstance != null)
			helpInstance.QueueFree();

		PackedScene helpScene = GD.Load<PackedScene>("res://scenes/Help.tscn");
		helpInstance = (Control) helpScene.Instantiate();

		AddChild(helpInstance);

		TextureButton backButton = helpInstance.GetNode<TextureButton>("MarginContainer/Back");
		backButton.Pressed += OnBackButtonPressed;
	}

	void OnBackButtonPressed()
	{
		if (helpInstance != null)
			helpInstance.QueueFree();

		mainMenuInstance.Visible = true;

		helpInstance.QueueFree();
		helpInstance = null;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			// it should not happen that the player is in some menu and gameInstance != null
			if (gameInstance != null)
			{
				if (gameInstance.ProcessMode == Node.ProcessModeEnum.Disabled)
				{
					// pause scene has higher ordering than game scene
					gameInstance.ProcessMode = Node.ProcessModeEnum.Inherit;
					// gui does not inherit visibility from its parent
					// this way the viewport moves to (0, 0) where pause scene starts
					Camera2D playerCamera = gameInstance.GetNode<Camera2D>("Player/Camera2D");

					playerCamera.Enabled = true;
					gameInstance.Visible = true;
					pauseInstance.Visible = false;
					selectedItem.Visible = true;
				}
				else
				{
					gameInstance.ProcessMode = Node.ProcessModeEnum.Disabled;
					CanvasLayer GUI = gameInstance.GetNode<CanvasLayer>("GUI");
					Camera2D playerCamera = gameInstance.GetNode<Camera2D>("Player/Camera2D");

					playerCamera.Enabled = false;
					gameInstance.Visible = false;
					selectedItem.Visible = false;
					inventoryContainer.Visible = false;
					pauseInstance.Visible = true;
				}
			}
		}

		if (@event.IsActionPressed("showInventory"))
		{
			if (gameInstance != null)
			{
				if (showingInventory)
				{
					showingInventory = false;

					inventoryContainer.Visible = false;
				}
				else
				{
					showingInventory = true;

					Player player = gameInstance.GetNode<Player>("Player");
					List<(string item, int amount)> items = player.GetInventoryItems();

					inventoryInstance.RecreateInventoryBar(items);
					inventoryContainer.Visible = true;
				}
			}
		}
	}
}
