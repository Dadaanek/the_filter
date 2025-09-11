using Godot;
using System;
using System.Collections.Generic;

#nullable enable

class MovementController
{
	private CharacterBody2D player;
	private AnimatedSprite2D animatedSprite;
	private int speed;

	public MovementController(CharacterBody2D player, AnimatedSprite2D animatedSprite, int speed)
	{
		this.player = player;
		this.animatedSprite = animatedSprite;
		this.speed = speed;
	}

	public void HandleMovement()
	{
		Vector2 velocity = Vector2.Zero;

		if (Input.IsActionPressed("ui_right"))
		{
			velocity.X += 1;
			animatedSprite.Animation = "walk_horizontal";
			animatedSprite.FlipH = false;
		}
		if (Input.IsActionPressed("ui_left"))
		{
			velocity.X -= 1;
			animatedSprite.Animation = "walk_horizontal";
			animatedSprite.FlipH = true;
		}
		if (Input.IsActionPressed("ui_down"))
		{
			velocity.Y += 1;
			animatedSprite.Animation = "walk_down";
		}
		if (Input.IsActionPressed("ui_up"))
		{
			velocity.Y -= 1;
			animatedSprite.Animation = "walk_up";
		}

		if (velocity != Vector2.Zero)
		{
			velocity = velocity.Normalized() * speed;
		}
		else
		{
			animatedSprite.Animation = "idle";
		}

		player.Velocity = velocity;
		player.MoveAndSlide();

		animatedSprite.Play();
	}
}

class Inventory
{
	private Dictionary<string, int> inventory = new Dictionary<string, int>();

	public Inventory()
	{
		for (int i = 0; i < GameData.ITEMS.Length; i++)
		{
			inventory.Add(GameData.ITEMS[i], 0);
		}
		inventory["Nothing"] = 1;
	}

	public List<(string item, int amount)> GetItems()
	{
		Dictionary<string, int>.KeyCollection keys = inventory.Keys;

		List<(string item, int amount)> result = new List<(string item, int amount)>();

		foreach ( string key in keys )
		{
			if (inventory[key] > 0)
				result.Add((key, inventory[key]));
		}

		return result;
	}

	public void AddItem(string itemType, int amount)
	{
		if (!inventory.ContainsKey(itemType))
			inventory.Add(itemType, 0);

		inventory[itemType] += amount;
	}

	public void RemoveItem(string itemType, int amount)
	{
		if (inventory.ContainsKey(itemType))
		{
			inventory[itemType] = Math.Max(0, inventory[itemType] - amount);
		}
	}

	public int ItemAmount(string itemType)
	{
		if (inventory.ContainsKey(itemType))
			return inventory[itemType];
		else
			return 0;
	}
}

class ItemInteractionContext
{
	// warnings connected to this can be overlooked
	public Vector2 globalPosition;
	public Vector2 localPos;
	public Vector2I tileCoords;
	public Vector2 worldCoords;
	public int tileId;
	public Area2D interactArea;
	public Inventory inventory;
	public string item;
}

static class ItemInteraction
{
	private static bool checkBuilding(Ground groundMapLayer, ItemInteractionContext context)
	{
		if(context.item == "Penguin")
		{
			if (!((World)groundMapLayer.GetParent()).CanPlaceTurret(context.globalPosition))
				return false;
			return true;
		}
		return true;
	}

	public static int ItemInteractWithGround(Inventory inventory, string item, Vector2 globalPosition, Ground groundMapLayer, Area2D interactArea)
	{
		ItemInteractionContext context = new ItemInteractionContext();
		context.globalPosition = globalPosition;
		context.localPos = groundMapLayer.ToLocal(globalPosition);
		context.tileCoords = groundMapLayer.LocalToMap(context.localPos);
		context.worldCoords = groundMapLayer.MapToLocal(context.tileCoords);
		context.tileId = groundMapLayer.GetCellSourceId(context.tileCoords);
		context.interactArea = interactArea;
		context.inventory = inventory;
		context.item = item;

		int selectedItemId = GameData.ITEM_TO_ID(item);

		// plant / drop
		if (context.item != "Nothing")
		{
			if (!groundMapLayer.IsPlanted(context.tileCoords.X, context.tileCoords.Y) && !groundMapLayer.IsBuilt(context.tileCoords.X, context.tileCoords.Y))
			{
				GD.Print("x");
				if (context.tileId == GameData.PAPER_ID && GameData.IS_PLANTABLE(context.item))
				{
					PlantItem(groundMapLayer, context);
				}
				
				else if ((context.tileId == GameData.GRASS_ID || context.tileId == GameData.DANDELION_ID) && GameData.IS_BUILDABLE(context.item) && checkBuilding(groundMapLayer, context))
				{
					BuildItem(groundMapLayer, context);
				}
				else
					DropItem(groundMapLayer, context);
			}
			else
			{
				if (context.tileId == GameData.PAPER_ID && !GameData.IS_PLANTABLE(context.item)
					&& GameData.CAN_BOOST(GameData.ID_TO_ITEM(groundMapLayer.GetPlantID(context.tileCoords.X, context.tileCoords.Y)), context.item))
				{
					GD.Print("boost");
					BoostCrop(groundMapLayer, context);
				}
				else
					DropItem(groundMapLayer, context);
			}

			inventory.RemoveItem(item, 1);
		}
		// pick up
		else
		{
			selectedItemId = PickUpItem(groundMapLayer, context);
		}

		return selectedItemId;
	}

	private static void BoostCrop(Ground groundMapLayer, ItemInteractionContext context)
	{
		Plant plant = (Plant)groundMapLayer.GetNode(context.tileCoords.X, context.tileCoords.Y);

		if (plant != null)
		{
			plant.Grow();
		}
	}

	private static void BuildItem(Ground groundMapLayer, ItemInteractionContext context)
	{
		// atm buildings are just turrets, could be changed easily if needed
		groundMapLayer.Build(context.tileCoords.X, context.tileCoords.Y, GameData.ITEM_TO_ID(context.item));

		string buildingType = context.item;

		PackedScene turretScene = GD.Load<PackedScene>($"res://scenes/Turret.tscn");
		Turret turretInstance = (Turret)turretScene.Instantiate();

		groundMapLayer.GetParent().AddChild(turretInstance);

		turretInstance.Name = buildingType;

		turretInstance.Position = groundMapLayer.ToGlobal(context.worldCoords);
		turretInstance.SetTurretType(buildingType);

		turretInstance.StartAnimation();

		groundMapLayer.ConnectNode(context.tileCoords.X, context.tileCoords.Y, turretInstance);
	}

	private static void PlantItem(Ground groundMapLayer, ItemInteractionContext context)
	{
		groundMapLayer.PlantCrop(context.tileCoords.X, context.tileCoords.Y, GameData.ITEM_TO_ID(context.item));

		string plantType = context.item;

		PackedScene plantScene = GD.Load<PackedScene>($"res://scenes/Plant.tscn");
		Plant plantInstance = (Plant)plantScene.Instantiate();

		groundMapLayer.GetParent().AddChild(plantInstance);

		plantInstance.Name = plantType;
		plantInstance.Position = groundMapLayer.ToGlobal(context.worldCoords);
		plantInstance.SetPlantType(plantType);

		plantInstance.StartAnimation();

		groundMapLayer.ConnectNode(context.tileCoords.X, context.tileCoords.Y, plantInstance);
	}

	private static void DropItem(Ground groundMapLayer, ItemInteractionContext context)
	{
		string itemType = context.item;

		PackedScene itemScene = GD.Load<PackedScene>($"res://scenes/Item.tscn");
		Item itemInstance = (Item)itemScene.Instantiate();

		groundMapLayer.GetParent().AddChild(itemInstance);

		itemInstance.Name = itemType;
		itemInstance.Position = context.globalPosition;
		itemInstance.SetItemType(itemType);
		itemInstance.StartAnimation();
	}

	private static int PickUpItem(Ground groundMapLayer, ItemInteractionContext context)
	{
		int selectedItemId = GameData.ITEM_TO_ID(context.item);

		if (context.interactArea != null)
		{
			var overlapping_areas = context.interactArea.GetOverlappingAreas();
			bool pickedItem = false;

			if (overlapping_areas.Count != 0)
			{
				Node area = null;
				for(int i = 0; i < overlapping_areas.Count; i++)
				{
					if (overlapping_areas[i].GetParent() is Plant || overlapping_areas[i].GetParent() is Item)
					{
						area = overlapping_areas[i].GetParent();
						break;
					}
				}

				switch (area)
				{
					case Plant plant:
						GD.Print(plant.GetPlantType());
						pickedItem = true;

						selectedItemId = GameData.ITEM_TO_ID(plant.GetPlantType());
						context.inventory.AddItem(GameData.ITEMS[selectedItemId], GameData.harvestAmount[GameData.ITEMS[selectedItemId]]);

						groundMapLayer.PickCrop(context.tileCoords.X, context.tileCoords.Y);

						break;

					case Item item:
						pickedItem = true;

						selectedItemId = GameData.ITEM_TO_ID(item.GetItemType());
						context.inventory.AddItem(GameData.ITEMS[selectedItemId], 1);

						break;
				}
				if (pickedItem)
					area.QueueFree();
			}
		}

		return selectedItemId;
	}
}

class NPCInteractionContext
{
	public Area2D interactArea;
}

static class NPCInteraction
{
	private static int classmateInteractIndex = 0;
	private static string[] messagesToClassmate = GameData.NPC_MESSAGES["Player_to_classmate"];

	public static void InteractWithNPC(Area2D interactArea, RichTextLabel popupLabel, ref bool talking)
	{
		NPCInteractionContext context = new NPCInteractionContext();
		context.interactArea = interactArea;

		if (context.interactArea != null)
		{
			var overlapping_areas = context.interactArea.GetOverlappingAreas();

			if (overlapping_areas.Count != 0)
			{
				switch (overlapping_areas[0].GetParent())
				{
					case Classmate classmate:
						if (classmateInteractIndex % 2 == 0)
						{
							talking = true;

							popupLabel.Text = messagesToClassmate[classmateInteractIndex >= messagesToClassmate.Length ? messagesToClassmate.Length - 1 : classmateInteractIndex];
						}
						else
						{
							talking = false;
							popupLabel.Text = "";
						}

						classmateInteractIndex++;

						break;
				}
			}
		}
	}
}

public partial class Player : CharacterBody2D
{
	private int speed = GameData.playerMovementSpeed;

	private AnimatedSprite2D animatedSprite = null!;
	private Ground groundMapLayer = null!;
	private Area2D interactArea = null!;
	private RichTextLabel popupLabel;

	private int selectedItemId = 0;
	private bool talking = false;

	private MovementController movementController = null!;
	private Inventory inventory = null!;

	// called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		groundMapLayer = GetNode<Ground>("../Ground");
		interactArea = GetNode<Area2D>("PlayerArea");
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

		movementController = new MovementController(this, animatedSprite, speed);
		inventory = new Inventory();
	}

	public override void _Process(double delta)
	{
		if (talking && popupLabel.Text != "")
			popupLabel.Visible = true;
		else
			popupLabel.Visible = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		movementController.HandleMovement();

		var overlapping_areas = interactArea.GetOverlappingAreas();
		bool showPopup = false;

		foreach (var body in overlapping_areas)
		{
			if (body.GetParent() is NPC)
				showPopup = true;
		}

		if (!showPopup)
		{
			talking = false;
			popupLabel.Text = "";
		}
		else
			talking = true;
	}

	public List<(string item, int amount)> GetInventoryItems()
	{
		return inventory.GetItems();
	}

	//GUI methods
	public string GetItemInHand()
	{
		return GameData.ITEMS[selectedItemId];
	}

	public int GetItemAmount(string itemType)
	{
		return inventory.ItemAmount(itemType);
	}

	void HoldValidItem()
	{
		if (inventory.ItemAmount(GameData.ITEMS[selectedItemId]) == 0
		&& GameData.ITEMS[selectedItemId] != "nothing")
		{
			for (int i = 0; i < GameData.ITEMS.Length; i++)
			{
				if (!(GameData.ITEMS[selectedItemId] != "nothing"
					&& inventory.ItemAmount(GameData.ITEMS[selectedItemId]) == 0))
					break;
				selectedItemId = (selectedItemId + 1) % GameData.ITEMS.Length;
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact"))
		{
			if (groundMapLayer != null)
			{
				selectedItemId = ItemInteraction.ItemInteractWithGround(
					inventory,
					GameData.ITEMS[selectedItemId],
					GlobalPosition,
					groundMapLayer,
					interactArea
				);

				NPCInteraction.InteractWithNPC(interactArea, popupLabel, ref talking);

				HoldValidItem();
			}
		}

		if (@event.IsActionPressed("changeItem"))
		{
			int originalIndex = selectedItemId;

			selectedItemId = (originalIndex + 1) % GameData.ITEMS.Length;
			while (selectedItemId != originalIndex
				&& inventory.ItemAmount(GameData.ITEMS[selectedItemId]) == 0)
			{
				selectedItemId = (selectedItemId + 1) % GameData.ITEMS.Length;
			}
		}
	}
}
