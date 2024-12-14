using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

var inputFile = File.ReadAllLines("./input.txt"); var maxX = 101; var maxY = 103;
//var inputFile = File.ReadAllLines("./sample.txt"); var maxX = 11; var maxY = 7;

var input = inputFile.ToList();

IEnumerable<Robot> ParseInput()
{
	var rgX = new Regex(@"p=(?<startX>\d+),(?<startY>\d+) v=(?<moveX>-?\d+),(?<moveY>-?\d+)");

	foreach (var line in input)
	{
		var parsed = rgX.Match(line);

		yield return new Robot()
		{
			PosX = int.Parse(parsed.Groups["startX"].Value),
			PosY = int.Parse(parsed.Groups["startY"].Value),
			MoveX = int.Parse(parsed.Groups["moveX"].Value),
			MoveY = int.Parse(parsed.Groups["moveY"].Value)
		};
	}
}

long Part1()
{
	var robots = ParseInput().ToList();

	for (var i = 0; i < 100; i++)
	{
		foreach (var robot in robots)
		{
			robot.Move(maxX, maxY);
		}
	}

	var yAxis = (maxX - 1) / 2;
	var xAxis = (maxY - 1) / 2;

	var q1Ct = robots.Count(x => x.PosX < yAxis && x.PosY < xAxis);
	var q2Ct = robots.Count(x => x.PosX > yAxis && x.PosY < xAxis);
	var q3Ct = robots.Count(x => x.PosX < yAxis && x.PosY > xAxis);
	var q4Ct = robots.Count(x => x.PosX > yAxis && x.PosY > xAxis);

	return q1Ct * q2Ct * q3Ct * q4Ct;
}

Bitmap DisplayRobots(List<Robot> robots)
{
	var bitmap = new Bitmap(maxX, maxY);

	for (var i = 0; i < maxY; i++)
	{
		for (var j = 0; j < maxX; j++)
		{
			//Console.SetCursorPosition(j, i);
			var bots = robots.Count(x => x.PosX == j && x.PosY == i);
			//Console.Write(bots != 0 ? bots : ".");
			bitmap.SetPixel(j, i, bots > 0 ? Color.Black : Color.White);
		}
	}

	return bitmap;
}

long Part2()
{
	var robots = ParseInput().ToList();
	var seconds = 0;

	while (!Console.KeyAvailable)
	{
		foreach (var robot in robots)
		{
			robot.Move(maxX, maxY);
		}

		seconds++;

		var bitmap = DisplayRobots(robots);
		//File.WriteAllBytes("seconds.bmp", bitmap);
		bitmap.Save($"img/{seconds}.bmp", ImageFormat.Bmp);
		Console.WriteLine($"\n{seconds}");
	}

	return seconds;
}

long p1 = 0;
long p2 = 0;

p1 = Part1();
p2 = Part2();

Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");

public class Robot
{
	public int PosX { get; set; }

	public int PosY { get; set; }

	public int MoveX { get; set; }
	public int MoveY { get; set; }

	public void Move(int maxX, int maxY)
	{
		PosX += MoveX;

		if (PosX >= maxX)
		{
			PosX -= maxX;
		}
		else if (PosX < 0)
		{
			PosX += maxX;
		}

		PosY += MoveY;

		if (PosY >= maxY)
		{
			PosY -= maxY;
		}
		else if (PosY < 0)
		{
			PosY += maxY;
		}
	}
}