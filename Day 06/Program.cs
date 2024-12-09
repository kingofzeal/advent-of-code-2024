using System.Linq;
using System.Text;

//var input = File.ReadAllLines("./input.txt").Select(x => x.ToCharArray().ToList()).ToList();
var input = File.ReadAllLines("./sample.txt").Select(x => x.ToCharArray().ToList()).ToList();
int Part1()
{

	var modMap = input.ToList();
	var row = modMap.Single(x => x.Contains('^'));
	var posX = modMap.IndexOf(row);
	var posY = row.IndexOf('^');

	var direction = (-1, 0);

	void PrintMap()
	{
		Console.SetCursorPosition(0, 0);
		var buf = new StringBuilder();

		foreach (var item in modMap)
		{
			buf.AppendJoin(string.Empty, item);
			buf.Append('\n');
		}

		Console.Write(buf);
	}


	while (posX >= 0 && posX < modMap.Count &&
				 posY >= 0 && posY < modMap[posX].Count)
	{
		modMap[posX][posY] = 'X';

		PrintMap();

		posX += direction.Item1;
		posY += direction.Item2;

		if (posX < modMap.Count - 1 && posY < modMap[posX].Count - 1 &&
				modMap[posX + direction.Item1][posY + direction.Item2] == '#')
		{
			//rotate
			switch (direction)
			{
				case (-1, 0): direction = (0, 1); break; //Right
				case (0, 1): direction = (1, 0); break; //Down
				case (1, 0): direction = (0, -1); break; //Left
				case (0, -1): direction = (-1, 0); break; //Up
			}
		}
	}

	return modMap.SelectMany(x => x.Where(y => y == 'X')).Count();
}

int Part2()
{
	var modMap = input.ToList();
	var row = modMap.Single(x => x.Contains('^'));
	var posX = modMap.IndexOf(row);
	var posY = row.IndexOf('^');

	var direction = (-1, 0);

	void PrintMap()
	{
		Console.SetCursorPosition(0, 0);
		var buf = new StringBuilder();

		foreach (var item in modMap)
		{
			buf.AppendJoin(string.Empty, item);
			buf.Append('\n');
		}

		Console.Write(buf);
	}

	var newObs = 0;

	while (posX >= 0 && posX < modMap.Count &&
				 posY >= 0 && posY < modMap[posX].Count)
	{
		if (modMap[posX][posY] == 'X')
		{
			var nxtPosX = posX + direction.Item1;
			var nxtPosY = posY + direction.Item2;

			if (nxtPosX >= 0 && nxtPosX < modMap.Count &&
					nxtPosY >= 0 && nxtPosY < modMap[nxtPosX].Count &&
					modMap[nxtPosX][nxtPosY] == '.')
			{
				newObs++;
			}
		}

		modMap[posX][posY] = 'X';

		PrintMap();

		posX += direction.Item1;
		posY += direction.Item2;

		var nextPosX = posX + direction.Item1;
		var nextPosY = posY + direction.Item2;

		if (nextPosX >= 0 && nextPosX < modMap.Count &&
				nextPosY >= 0 && nextPosY < modMap[nextPosX].Count &&
				modMap[nextPosX][nextPosY] == '#')
		{
			//rotate
			switch (direction)
			{
				case (-1, 0): direction = (0, 1); break; //Right
				case (0, 1): direction = (1, 0); break; //Down
				case (1, 0): direction = (0, -1); break; //Left
				case (0, -1): direction = (-1, 0); break; //Up
			}
		}
	}

	return newObs;
}

int p1 = 0;
int p2 = 0;

//p1 = Part1();
p2 = Part2();

Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");