using Godot;
using System;

public partial class Item : StaticBody2D
{
	private string itemType;
	private AnimatedSprite2D itemSprite;

	public override void _Ready()
	{
		itemSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public void SetItemType(string itemType)
	{
		this.itemType = itemType;
	}

	public string GetItemType()
	{
		return itemType;
	}

	public void StartAnimation()
	{
		itemSprite.Animation = itemType;
		itemSprite.Play();
	}
}
