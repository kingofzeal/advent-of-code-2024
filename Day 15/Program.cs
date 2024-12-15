using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

var inputFile = File.ReadAllLines("./input.txt");
//var inputFile = File.ReadAllLines("./sample.txt");

var input = inputFile.ToList();

long Part1()
{
	(char[,] warehouse, List<Direction> directions, (int posX, int posY) botPos) ParseInput()
	{
		var whSep = input.IndexOf(string.Empty);
		var warehouse = new char[input[0].Length, whSep];
		var botPos = (0, 0);

		for (var i = 0; i < whSep; i++)
		{
			for (var j = 0; j < input[i].Length; j++)
			{
				if (input[i][j] == '@')
				{
					botPos = (i, j);
				}
				warehouse[i, j] = input[i][j];
			}
		}

		var directions = new List<Direction>();

		for (var i = whSep + 1; i < input.Count; i++)
		{
			foreach (var ins in input[i])
			{
				switch (ins)
				{
					case '<': directions.Add(Direction.Left); break;
					case '>': directions.Add(Direction.Right); break;
					case '^': directions.Add(Direction.Up); break;
					case 'v': directions.Add(Direction.Down); break;
				}
			}
		}

		return (warehouse, directions, botPos);
	}

	var (warehouse, directions, botPos) = ParseInput();

	(bool blocked, (int posX, int posY)? nextOpen) IsBlocked(Direction direction, (int posX, int posY) pos, char[,] warehouse)
	{
		switch (direction)
		{
			case Direction.Up:
				for (var x = pos.posX; x >= 0; x--)
				{
					switch (warehouse[x, pos.posY])
					{
						case '.':
							return (false, (x, pos.posY));
						case '#':
							return (true, null);
					}
				}
				break;
			case Direction.Down:
				for (var x = pos.posX; x < warehouse.GetLength(0); x++)
				{
					switch (warehouse[x, pos.posY])
					{
						case '.':
							return (false, (x, pos.posY));
						case '#':
							return (true, null);
					}
				}
				break;
			case Direction.Left:
				for (var y = pos.posY; y >= 0; y--)
				{
					switch (warehouse[pos.posX, y])
					{
						case '.':
							return (false, (pos.posX, y));
						case '#':
							return (true, null);
					}
				}
				break;
			case Direction.Right:
				for (var y = pos.posY; y < warehouse.GetLength(1); y++)
				{
					switch (warehouse[pos.posX, y])
					{
						case '.':
							return (false, (pos.posX, y));
						case '#':
							return (true, null);
					}
				}
				break;
		}

		return (true, null);
	}

	var t = 0;

	foreach (var direction in directions)
	{
		if (t++ % 100 == 0)
		{
			WriteWarehouse(warehouse);
		}

		var (blocked, nextOpen) = IsBlocked(direction, botPos, warehouse);
		if (blocked)
		{
			continue;
		}

		switch (direction)
		{
			case Direction.Up:
				for (var x = nextOpen.Value.posX; x < botPos.posX; x++)
				{
					warehouse[x, botPos.posY] = warehouse[x + 1, botPos.posY];
				}

				warehouse[botPos.posX, botPos.posY] = '.';
				botPos.posX--;
				break;
			case Direction.Down:
				for (var x = nextOpen.Value.posX; x > botPos.posX; x--)
				{
					warehouse[x, botPos.posY] = warehouse[x - 1, botPos.posY];
				}

				warehouse[botPos.posX, botPos.posY] = '.';
				botPos.posX++;
				break;
			case Direction.Left:
				for (var y = nextOpen.Value.posY; y < botPos.posY; y++)
				{
					warehouse[botPos.posX, y] = warehouse[botPos.posX, y + 1];
				}

				warehouse[botPos.posX, botPos.posY] = '.';
				botPos.posY--;
				break;
			case Direction.Right:
				for (var y = nextOpen.Value.posY; y > botPos.posY; y--)
				{
					warehouse[botPos.posX, y] = warehouse[botPos.posX, y - 1];
				}

				warehouse[botPos.posX, botPos.posY] = '.';
				botPos.posY++;
				break;
		}
	}

	var total = 0;

	for (var i = 0; i < warehouse.GetLength(0); i++)
	{
		for (var j = 0; j < warehouse.GetLength(1); j++)
		{
			if (warehouse[i, j] == 'O')
			{
				total += (100 * i) + j;
			}
		}
	}

	return total;
}

long Part2()
{
	(char[,] warehouse, List<Direction> directions, (int posX, int posY) botPos) ParseInput()
	{
		var whSep = input.IndexOf(string.Empty);
		var warehouse = new char[whSep, input[0].Length * 2];
		var botPos = (0, 0);

		for (var i = 0; i < whSep; i++)
		{
			for (var j = 0; j < input[i].Length; j++)
			{
				switch (input[i][j])
				{
					case '#':
						warehouse[i, j * 2] = '#';
						warehouse[i, j * 2 + 1] = '#';
						break;
					case 'O':
						warehouse[i, j * 2] = '[';
						warehouse[i, j * 2 + 1] = ']';
						break;
					case '.':
						warehouse[i, j * 2] = '.';
						warehouse[i, j * 2 + 1] = '.';
						break;
					case '@':
						warehouse[i, j * 2] = '@';
						warehouse[i, j * 2 + 1] = '.';
						botPos = (i, j * 2);
						break;
				}
			}
		}

		var directions = new List<Direction>();

		for (var i = whSep + 1; i < input.Count; i++)
		{
			foreach (var ins in input[i])
			{
				switch (ins)
				{
					case '<': directions.Add(Direction.Left); break;
					case '>': directions.Add(Direction.Right); break;
					case '^': directions.Add(Direction.Up); break;
					case 'v': directions.Add(Direction.Down); break;
				}
			}
		}

		return (warehouse, directions, botPos);
	}

	var (warehouse, directions, botPos) = ParseInput();

	var t = 0;

	(bool canMove, (int x, int y)? openSpot) CanMove(Direction direction, (int posX, int posY) pos)
	{
		switch (direction)
		{
			case Direction.Up:
				for (var x = pos.posX - 1; x >= 0; x--)
				{
					switch (warehouse[x, pos.posY])
					{
						case '.':
							return (true, (x, pos.posY));
						case '#':
							return (false, null);
						case '[':
							return (CanMove(direction, (x, pos.posY)).canMove && CanMove(direction, (x, pos.posY + 1)).canMove, null);
						case ']':
							return (CanMove(direction, (x, pos.posY - 1)).canMove && CanMove(direction, (x, pos.posY)).canMove, null);
					}
				}
				throw new ArgumentOutOfRangeException();
			case Direction.Down:
				for (var x = pos.posX + 1; x < warehouse.GetLength(0); x++)
				{
					switch (warehouse[x, pos.posY])
					{
						case '.':
							return (true, (x, pos.posY));
						case '#':
							return (false, null);
						case '[':
							return (CanMove(direction, (x, pos.posY)).canMove && CanMove(direction, (x, pos.posY + 1)).canMove, null);
						case ']':
							return (CanMove(direction, (x, pos.posY - 1)).canMove && CanMove(direction, (x, pos.posY)).canMove, null);
					}
				}
				throw new ArgumentOutOfRangeException();
			case Direction.Left:
				for (var y = pos.posY - 1; y >= 0; y--)
				{
					switch (warehouse[pos.posX, y])
					{
						case '.':
							return (true, (pos.posX, y));
						case '#':
							return (false, null);
					}
				}
				throw new ArgumentOutOfRangeException();
			case Direction.Right:
				for (var y = pos.posY + 1; y < warehouse.GetLength(1); y++)
				{
					switch (warehouse[pos.posX, y])
					{
						case '.':
							return (true, (pos.posX, y));
						case '#':
							return (false, null);
					}
				}

				throw new ArgumentOutOfRangeException();
			default:
				throw new NotImplementedException();
		}
	}

	void MoveBox(Direction direction, (int x, int y) pos)
	{
		var direct = direction == Direction.Up ? -1 : 1;

		if (warehouse[pos.x + direct, pos.y] is '[' or ']')
		{
			MoveBox(direction, (pos.x + direct, pos.y));
		}

		if (warehouse[pos.x, pos.y] == '[' && warehouse[pos.x + direct, pos.y + 1] is '[' or ']')
		{
			MoveBox(direction, (pos.x + direct, pos.y + 1));
		}

		if (warehouse[pos.x, pos.y] == ']' && warehouse[pos.x + direct, pos.y - 1] is '[' or ']')
		{
			MoveBox(direction, (pos.x + direct, pos.y - 1));
		}

		switch (warehouse[pos.x, pos.y])
		{
			case '[':
				warehouse[pos.x + direct, pos.y] = '[';
				warehouse[pos.x, pos.y] = '.';
				warehouse[pos.x + direct, pos.y + 1] = ']';
				warehouse[pos.x, pos.y + 1] = '.';
				break;
			case ']':
				warehouse[pos.x + direct, pos.y - 1] = '[';
				warehouse[pos.x, pos.y - 1] = '.';
				warehouse[pos.x + direct, pos.y] = ']';
				warehouse[pos.x, pos.y] = '.';
				break;
		}
	}

	foreach (var direction in directions)
	{
		if (t++ % 100 == 0)
		{
			WriteWarehouse(warehouse);
		}

		var (canMove, empty) = CanMove(direction, botPos);

		if (canMove)
		{
			switch (direction)
			{
				case Direction.Up:
					if (empty != null)
					{
						for (var x = empty.Value.x; x < botPos.posX; x++)
						{
							warehouse[x, botPos.posY] = warehouse[x + 1, botPos.posY];
						}

						warehouse[botPos.posX, botPos.posY] = '.';
						botPos.posX--;
						break;
					}

					MoveBox(direction, (botPos.posX - 1, botPos.posY));
					warehouse[botPos.posX - 1, botPos.posY] = '@';
					warehouse[botPos.posX, botPos.posY] = '.';
					botPos.posX--;
					break;
				case Direction.Down:
					if (empty != null)
					{
						for (var x = empty.Value.x; x > botPos.posX; x--)
						{
							warehouse[x, botPos.posY] = warehouse[x - 1, botPos.posY];
						}

						warehouse[botPos.posX, botPos.posY] = '.';
						botPos.posX++;
						break;
					}

					MoveBox(direction, (botPos.posX + 1, botPos.posY));
					warehouse[botPos.posX + 1, botPos.posY] = '@';
					warehouse[botPos.posX, botPos.posY] = '.';
					botPos.posX++;
					break;
				case Direction.Left:
					for (var y = empty.Value.y; y < botPos.posY; y++)
					{
						warehouse[botPos.posX, y] = warehouse[botPos.posX, y + 1];
					}

					warehouse[botPos.posX, botPos.posY] = '.';
					botPos.posY--;
					break;
				case Direction.Right:
					for (var y = empty.Value.y; y > botPos.posY; y--)
					{
						warehouse[botPos.posX, y] = warehouse[botPos.posX, y - 1];
					}

					warehouse[botPos.posX, botPos.posY] = '.';
					botPos.posY++;
					break;
			}
		}
	}

	var total = 0;

	for (var i = 0; i < warehouse.GetLength(0); i++)
	{
		for (var j = 0; j < warehouse.GetLength(1); j++)
		{
			if (warehouse[i, j] == '[')
			{
				total += (100 * i) + j;
			}
		}
	}

	return total;
}

long p1 = 0;
long p2 = 0;

p1 = Part1();
p2 = Part2();

Console.WriteLine();
Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");

void WriteWarehouse(char[,] warehouse)
{
	for (var i = 0; i < warehouse.GetLength(0); i++)
	{
		for (var j = 0; j < warehouse.GetLength(1); j++)
		{
			Console.SetCursorPosition(j, i);
			Console.Write(warehouse[i, j]);
		}
	}
}

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}
