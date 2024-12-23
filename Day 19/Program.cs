using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

var inputFile = File.ReadAllLines("./input.txt");
//var inputFile = File.ReadAllLines("./sample.txt");

var input = inputFile.ToList();

(long pt1, long pt2) Part1()
{
	var towels = input[0].Split(',').Select(x => x.Trim()).ToList();
	var combos = input[2..];

	var known = new Dictionary<string, long> { { "", 1 } };

	long Check(string val)
	{
		if (known.ContainsKey(val))
		{
			return known[val];
		}

		var matchTowels = towels.Where(val.StartsWith).ToList();

		if (!matchTowels.Any())
		{
			known.Add(val, 0);
			return 0;
		}

		var sum = 0L;

		foreach (var towel in matchTowels)
		{
			var sub = Check(val.Substring(towel.Length));

			sum += sub;
		}

		known.Add(val, sum);
		return sum;
	}

	var count = 0;
	var allCombos = 0L;

	foreach (var combo in combos)
	{
		Console.Write(combo);
		var patterns = Check(combo);
		if (patterns > 0)
		{
			count++;
			allCombos += patterns;
		}
		Console.WriteLine($" {patterns}");
	}

	return (count, allCombos);
}

long Part2()
{
	return 0;
}

long p1 = 0;
long p2 = 0;

(p1, p2) = Part1();
//p2 = Part2();

Console.WriteLine();
Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");
