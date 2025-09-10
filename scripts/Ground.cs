using Godot;
using System;

public partial class Ground : TileMapLayer
{
	private int[,] PLANTED_CROPS = new int[(int) GameData.tileMapSize.X, (int) GameData.tileMapSize.Y];
	private int[,] BUILT = new int[(int) GameData.tileMapSize.X, (int) GameData.tileMapSize.Y];
	private Node[,] CONNECTED_NODE = new Node[(int) GameData.tileMapSize.X, (int) GameData.tileMapSize.Y];

	public void ConnectNode(int x, int y, Node node)
	{
		CONNECTED_NODE[x + GameData.coordOffset, y + GameData.coordOffset] = node;
	}

	public void DisconnectNode(int x, int y)
	{
		CONNECTED_NODE[x + GameData.coordOffset, y + GameData.coordOffset] = null;
	}

	public Node GetNode(int x, int y)
	{
		return CONNECTED_NODE[x + GameData.coordOffset, y + GameData.coordOffset];
	}

	public void Build(int x, int y, int id)
	{
		BUILT[x + GameData.coordOffset, y + GameData.coordOffset] = id;
	}

	public int GetBuiltID(int x, int y)
	{
		return BUILT[x + GameData.coordOffset, y + GameData.coordOffset];
	}

	public bool IsBuilt(int x, int y)
	{
		return !(BUILT[x + GameData.coordOffset, y + GameData.coordOffset] == -1);
	}

	public void PlantCrop(int x, int y, int id)
	{
		PLANTED_CROPS[x + GameData.coordOffset, y + GameData.coordOffset] = id;
	}

	public bool IsPlanted(int x, int y)
	{
		return !(PLANTED_CROPS[x + GameData.coordOffset, y + GameData.coordOffset] == -1);
	}

	public void PickCrop(int x, int y)
	{
		PLANTED_CROPS[x + GameData.coordOffset, y + GameData.coordOffset] = -1;
	}

	public int GetPlantID(int x, int y)
	{
		return PLANTED_CROPS[x + GameData.coordOffset, y + GameData.coordOffset];
	}

	public Ground()
	{
		for (int i = 0; i < GameData.tileMapSize.X; i++)
		{
			for (int j = 0; j < GameData.tileMapSize.Y; j++)
			{
				// -1 shouldn't be the id of any plant
				PLANTED_CROPS[i, j] = -1;
			}
		}

		for (int i = 0; i < GameData.tileMapSize.X; i++)
		{
			for (int j = 0; j < GameData.tileMapSize.Y; j++)
			{
				// -1 shouldn't be the id of any building
				BUILT[i, j] = -1;
			}
		}
	}
}
