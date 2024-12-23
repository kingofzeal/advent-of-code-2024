using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

var inputFile = File.ReadAllLines("./input.txt");
//var inputFile = File.ReadAllLines("./sample.txt");

var input = inputFile.Select(long.Parse).ToList();

long Mix(long num1, long num2)
{
	return num1 ^ num2;
}

long Prune(long value, long pruneVal = 16777216)
{
	return value % pruneVal;
}

long GenerateSecret(long val)
{
	var newVal = val * 64;
	newVal = Mix(newVal, val);
	val = Prune(newVal);

	newVal = val / 32;
	newVal = Mix(newVal, val);
	val = Prune(newVal);

	newVal = val * 2048;
	newVal = Mix(newVal, val);

	return Prune(newVal);
}

long Part1()
{
	var total = 0L;

	foreach (var val in input)
	{
		var tmpVal = val;
		for (var i = 0; i < 2000; i++)
		{
			tmpVal = GenerateSecret(tmpVal);
		}

		Console.WriteLine(tmpVal);
		total += tmpVal;
	}

	return total;
}

long Part2()
{
	var buyersLists = new Dictionary<long, List<long>>();

	foreach (var val in input)
	{
		buyersLists.Add(val, new List<long> { val });
		var tmpVal = val;

		for (var i = 0; i < 2000; i++)
		{
			tmpVal = GenerateSecret(tmpVal);
			buyersLists[val].Add(tmpVal);
		}
	}

	var resultSet = new Dictionary<long, List<((int v1, int v2, int v3, int v4), int price)>>();

	for (var i = 0; i < buyersLists.Count; i++)
	{
		resultSet.Add(input[i], new List<((int v1, int v2, int v3, int v4), int price)>());

		for (var j = 1; j < buyersLists[input[i]].Count - 3; j++)
		{
			var process = ((int)(buyersLists[input[i]][j] % 10 - buyersLists[input[i]][j - 1] % 10),
				(int)(buyersLists[input[i]][j + 1] % 10 - buyersLists[input[i]][j] % 10),
				(int)(buyersLists[input[i]][j + 2] % 10 - buyersLists[input[i]][j + 1] % 10),
				(int)(buyersLists[input[i]][j + 3] % 10 - buyersLists[input[i]][j + 2] % 10));

			resultSet[input[i]].Add((process, (int)(buyersLists[input[i]][j + 3] % 10)));
		}
	}

	var maxPrice = 0;
	var flattened = resultSet.SelectMany(x => x.Value).ToList();

	var byGroup = flattened.Where(x => x.Item1 != default).GroupBy(x => x.Item1).OrderByDescending(x => x.Count());

	foreach (var item in byGroup)
	{
		var maxPossible = flattened.Count(x => x.Item1 == item.Key);
		if (maxPossible * 9 < maxPrice)
		{
			Console.WriteLine($"{item.Key}: Not possible: only {maxPossible}, current max {maxPrice}");
			//No way possible any more, all other entries will have less than this
			break;
		}

		var totalPrice = 0;
		var ct = 0;

		for (var i = 0; i < resultSet.Count; i++)
		{
			var firstMatch = resultSet[input[i]].FirstOrDefault(x => x.Item1 == item.Key);

			if (firstMatch != default)
			{
				totalPrice += firstMatch.price;
				ct++;
			}
		}

		Console.WriteLine($"{item.Key.ToString().PadLeft(16)} has {ct:000} of {maxPossible:000} max earning {totalPrice:0000} (current leader {maxPrice} - {maxPrice / 9} limit).");

		if (totalPrice > maxPrice)
		{
			maxPrice = totalPrice;
		}
	}

	return maxPrice;
}

long p1 = 0;
long p2 = 0;

p1 = Part1();
p2 = Part2();

Console.WriteLine();
Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");
