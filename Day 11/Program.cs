using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

var input = File.ReadAllText("./input.txt").Split(' ').Select(long.Parse).ToList();
//var input = File.ReadAllText("./sample.txt").Split(' ').Select(long.Parse).ToList();

long Part1()
{
	var lst = input;

	for (var i = 0; i < 25; i++)
	{
		Console.WriteLine(string.Join(' ', lst));

		var newLst = new List<long>();
		foreach (var t in lst)
		{
			if (t == 0)
			{
				newLst.Add(1);
				continue;
			}

			if (t.ToString().Length % 2 == 0)
			{
				var numStr = t.ToString();
				newLst.Add(long.Parse(numStr.Substring(0, numStr.Length / 2)));
				newLst.Add(long.Parse(numStr.Substring(numStr.Length / 2)));
				continue;
			}

			newLst.Add(t * 2024);
		}

		lst = newLst;
	}

	return lst.Count;
}

long Part2()
{
	var lst = input;

	for (var i = 0; i < 75; i++)
	{
		if (i < 25)
		{
			Console.WriteLine(string.Join(' ', lst));
		}
		else
		{
			Console.WriteLine($"{i}: {lst.Count}");
		}

		var newLst = new List<long>();
		foreach (var t in lst)
		{
			if (t == 0)
			{
				newLst.Add(1);
				continue;
			}

			if (t.ToString().Length % 2 == 0)
			{
				var numStr = t.ToString();
				newLst.Add(long.Parse(numStr.Substring(0, numStr.Length / 2)));
				newLst.Add(long.Parse(numStr.Substring(numStr.Length / 2)));
				continue;
			}

			newLst.Add(t * 2024);
		}

		lst = newLst;
	}

	return lst.Count;
}

long p1 = 0;
long p2 = 0;

p1 = Part1();
p2 = Part2();

Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");