using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

var inputFile = File.ReadAllLines("./input.txt");
//var inputFile = File.ReadAllLines("./sample.txt");

var input = inputFile.ToList();

long Part1()
{
	var rgBut = new Regex(@"Button [A|B]: X\+(\d+), Y\+(\d+)");
	var rgPrize = new Regex(@"Prize: X=(\d+), Y=(\d+)");

	var prizeCount = 0;
	var prizeCost = 0;

	for (var i = 0; i < input.Count; i += 4)
	{
		var bA = input[i];
		var aMatch = rgBut.Match(bA);
		var aMvX = int.Parse(aMatch.Groups[1].Value);
		var aMvY = int.Parse(aMatch.Groups[2].Value);

		var bB = input[i + 1];
		var bMatch = rgBut.Match(bB);
		var bMvX = int.Parse(bMatch.Groups[1].Value);
		var bMvY = int.Parse(bMatch.Groups[2].Value);

		var p = input[i + 2];
		var pMatch = rgPrize.Match(p);
		var pX = int.Parse(pMatch.Groups[1].Value);
		var pY = int.Parse(pMatch.Groups[2].Value);

		Console.WriteLine($"{bA} ({aMvX}, {aMvY}), {bB} ({bMvX}, {bMvY}), {p} ({pX}, {pY})");

		var minCost = int.MaxValue;

		for (var aCt = 0; aCt < 100; aCt++)
		{
			if (aCt * aMvX > pX || aCt * aMvY > pY)
			{
				break;
			}

			for (var bCt = 0; bCt < 100; bCt++)
			{
				var xPos = (aMvX * aCt) + (bMvX * bCt);
				var yPos = (aMvY * aCt) + (bMvY * bCt);

				if (xPos > pX || yPos > pY)
				{
					break;
				}

				if (xPos == pX && yPos == pY)
				{
					Console.WriteLine($"{aCt} {bCt}");
					minCost = Math.Min(minCost, (aCt * 3) + bCt);
				}
			}
		}

		if (minCost < int.MaxValue)
		{
			prizeCount++;
			prizeCost += minCost;
		}
	}

	return prizeCost;
}

long Part2()
{
	var rgBut = new Regex(@"Button [A|B]: X\+(\d+), Y\+(\d+)");
	var rgPrize = new Regex(@"Prize: X=(\d+), Y=(\d+)");

	var prizeCount = 0;
	var prizeCost = 0L;

	for (var machine = 0; machine < input.Count; machine += 4)
	{
		var bA = input[machine];
		var aMatch = rgBut.Match(bA);
		var aMvX = int.Parse(aMatch.Groups[1].Value);
		var aMvY = int.Parse(aMatch.Groups[2].Value);

		var bB = input[machine + 1];
		var bMatch = rgBut.Match(bB);
		var bMvX = int.Parse(bMatch.Groups[1].Value);
		var bMvY = int.Parse(bMatch.Groups[2].Value);

		var p = input[machine + 2];
		var pMatch = rgPrize.Match(p);
		var pX = int.Parse(pMatch.Groups[1].Value) + 10000000000000;
		var pY = int.Parse(pMatch.Groups[2].Value) + 10000000000000;

		Console.WriteLine($"{bA} ({aMvX}, {aMvY}), {bB} ({bMvX}, {bMvY}), {p} ({pX}, {pY})");

		var bCt = (pY * aMvX - pX * aMvY) / (bMvY * aMvX - bMvX * aMvY);
		var aCt = (pX - bCt * bMvX) / aMvX;
		if (aCt >= 0 &&
				bCt >= 0 &&
				bCt * bMvX + aCt * aMvX == pX &&
				bCt * bMvY + aCt * aMvY == pY)
		{
			Console.WriteLine($"{aCt} {bCt}");
			prizeCost += aCt * 3 + bCt;
		}

	}

	return prizeCost;
}

long p1 = 0;
long p2 = 0;

p1 = Part1();
p2 = Part2();

Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");