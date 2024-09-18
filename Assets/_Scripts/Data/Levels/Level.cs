using System;
using Newtonsoft.Json;
using UnityEngine;

public static class LevelParser
{
	public static Level ParseLevel(string json)
	{
		ParsedLevel parsedLevel = JsonConvert.DeserializeObject<ParsedLevel>(json);
		
		int sizeX = parsedLevel.LevelArrangement.GetLength(0);
		int sizeY = parsedLevel.LevelArrangement.GetLength(1);
		
		Level.CellType[,] levelArrangement = new Level.CellType[sizeY, sizeX];

		for (int x = 0; x < sizeX; x++)
		{
			for (int y = 0; y < sizeY; y++)
			{
				levelArrangement[y, sizeX - x - 1] = parsedLevel.LevelArrangement[x, y] switch
				{
					"." => Level.CellType.Empty,
					"0" => Level.CellType.Default,
					"I" => Level.CellType.Ice,
					"W" => Level.CellType.Wall,
					_ => throw new ArgumentOutOfRangeException(parsedLevel.LevelArrangement[x,y])
				};
			}
		}

		return new Level
		{
			LevelArrangement = levelArrangement,
			LevelGoals = parsedLevel.LevelGoals
		};
	}

	private struct ParsedLevel
	{
		public string[,] LevelArrangement;

		public Level.Goal[] LevelGoals;
	}
}

public struct Level
{
	public enum CellType
	{
		Empty = -1,
		Default = 0,
		Ice = 1,
		Wall = 2
	}
	
	public CellType[,] LevelArrangement;

	public enum GoalType
	{
		Letters,
		Words,
		IceBlocks
	}
	
	public struct Goal
	{
		public GoalType GoalType;
		public int GoalAmount;
	}

	public Goal[] LevelGoals;
}