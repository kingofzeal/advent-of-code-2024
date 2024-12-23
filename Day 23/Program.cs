using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

var inputFile = File.ReadAllLines("./input.txt");
//var inputFile = File.ReadAllLines("./sample.txt");

var input = inputFile.ToList();

int Part1()
{
	var allConnections = input.Select(x => x.Split('-')).ToList();
	var allComputers = allConnections.SelectMany(x => x).Distinct().ToList();

	var mapping = new Dictionary<string, List<string>>();

	foreach (var connection in allConnections)
	{
		foreach (var computer in connection)
		{
			if (!mapping.ContainsKey(computer))
			{
				mapping.Add(computer, new List<string>());
			}

			mapping[computer].AddRange(connection);
		}
	}

	var result = 0;
	var resultCombos = new List<List<string>>();

	foreach (var pool in mapping.Where(x => x.Key.StartsWith('t')))
	{
		var set = pool.Value.Distinct().ToList();
		var combos = set.Combinations(3).Where(x => x.Any(y => y.StartsWith('t')));

		foreach (var combo in combos)
		{
			var comboL = combo.ToList();
			if (resultCombos.Any(x => x.Intersect(comboL).Count() >= comboL.Count))
			{
				//We already know about this one
				continue;
			}

			var valid = true;
			foreach (var pair in comboL.Combinations(2))
			{
				var connections = allConnections.Any(x => x.Intersect(pair).Count() >= pair.Count());
				if (!connections)
				{
					valid = false;
					break;
				}
			}

			if (!valid)
			{
				continue;
			}

			resultCombos.Add(comboL);
			Console.WriteLine(string.Join(",", comboL));
			result++;
		}
	}

	return result;
}

string Part2()
{
	var allConnections = input.Select(x => x.Split('-')).ToList();
	var allComputers = allConnections.SelectMany(x => x).Distinct().ToList();

	var mapping = new Dictionary<string, List<string>>();

	foreach (var connection in allConnections)
	{
		foreach (var computer in connection)
		{
			if (!mapping.ContainsKey(computer))
			{
				mapping.Add(computer, new List<string>());
			}

			mapping[computer].AddRange(connection);
			mapping[computer] = mapping[computer].Distinct().ToList();
		}
	}

	var result = new List<string>();

	foreach (var map in mapping.OrderBy(x => x.Value.Count))
	{
		var biggestCombo = new List<string>();

		for (var i = map.Value.Count; i > result.Count; i--)
		{
			Console.WriteLine($"Generating combos for sets of {i} from {map.Value.Count}");

			var combos = map.Value.Combinations(i).ToList();

			foreach (var combo in combos)
			{
				var comboL = combo.ToList();

				var pairs = comboL.Combinations(2).ToList();
				var valid = true;

				foreach (var pair in pairs)
				{
					if (!allConnections.Any(x => x.Intersect(pair).Count() >= pair.Count()))
					{
						valid = false;
						break;
					}
				}

				if (!valid)
				{
					continue;
				}

				Console.WriteLine(string.Join(",", comboL));
				biggestCombo = comboL;
				break;
			}

			if (biggestCombo.Any())
			{
				break;
			}
		}

		if (biggestCombo.Count > result.Count)
		{
			result = biggestCombo;
		}
	}



	var largestNet = string.Join(",", result.Order());
	return largestNet;
}

int p1 = 0;
string p2 = "";

//p1 = Part1();
p2 = Part2();

Console.WriteLine();
Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}");


public static class Ext
{
	public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int k)
	{
		return k == 0
			? new[] { Array.Empty<T>() }
			: source.SelectMany((e, i) =>
				source.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
	}
}