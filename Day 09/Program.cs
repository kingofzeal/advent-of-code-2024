using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

var input = File.ReadAllText("./input.txt");
//var input = File.ReadAllText("./sample.txt");

long Part1()
{
	var fs = new List<int?>();

	var isFile = true;

	for (var i = 0; i < input.Length; i++)
	{
		int? val = i % 2 == 0 ? i / 2 : null;
		fs.AddRange(Enumerable.Repeat(val, int.Parse(input[i].ToString())));
		isFile = !isFile;
	}

	for (var i = fs.Count - 1; i >= 0; i--)
	{
		if (fs[i] == null)
		{
			continue;
		}

		var freeSpace = fs.IndexOf(null);

		if (freeSpace >= i)
		{
			break;
		}

		fs[freeSpace] = fs[i];
		fs[i] = null;
	}

	Console.WriteLine(string.Join('-', fs));

	long result = 0;

	for (var i = 0; i < fs.Count; i++)
	{
		var toAdd = (fs[i] ?? 0) * i;
		Console.WriteLine($"{fs[i]} {i} = {toAdd}");
		result += toAdd;
	}

	return result;
}

long Part2()
{
	var desc = input.Select((val, idx) => new Parsed
	{
		EmptySize = idx % 2 == 1 ? int.Parse(val.ToString()) : 0,
		FileSize = idx % 2 == 0 ? int.Parse(val.ToString()) : 0,
		Index = idx % 2 == 0 ? idx / 2 : 0
	}).ToList();
	//var desc = input.Select((val, idx) => idx % 2 == 0 ? (file: int.Parse(val.ToString()), idx: idx / 2, empty: 0) : (file: 0, idx: 0, empty: int.Parse(val.ToString()))).ToList();

	for (var i = desc.Count - 1; i > 0; i--)
	{
		var file = desc[i];

		if (file.IsEmpty || file.IsMoved)
		{
			continue;
		}

		var idx = desc.FindIndex(0, x => x.EmptySize >= file.FileSize);

		if (idx < 0 || idx >= i)
		{
			continue;
		}

		var empty = desc[idx];

		desc.Remove(file);
		desc.Insert(idx, file);
		file.IsMoved = true;

		if (empty.EmptySize == file.FileSize)
		{
			desc.RemoveAt(idx + 1);
		}
		else
		{
			empty.EmptySize -= file.FileSize;
		}
		desc.Insert(i, new Parsed
		{
			EmptySize = file.FileSize
		});
	}

	var fs = new List<int?>();

	foreach (var val in desc)
	{
		if (val.IsFile)
		{
			fs.AddRange(Enumerable.Repeat((int?)val.Index, val.FileSize));
		}
		else
		{
			fs.AddRange(Enumerable.Repeat((int?)null, val.EmptySize));
		}
	}

	Console.WriteLine(string.Join('-', fs));

	long result = 0;

	for (var i = 0; i < fs.Count; i++)
	{
		var toAdd = (fs[i] ?? 0) * i;
		//Console.WriteLine($"{fs[i]} {i} = {toAdd}");
		result += toAdd;
	}

	return result;
}

long p1 = 0;
long p2 = 0;

//p1 = Part1();
p2 = Part2();

Console.WriteLine($"Part 1: {p1}");
Console.WriteLine($"Part 2: {p2}"); //6225046593422


class Parsed
{
	public int Index { get; set; }
	public int FileSize { get; set; }
	public int EmptySize { get; set; }
	public bool IsMoved { get; set; }

	public bool IsEmpty => EmptySize > 0;
	public bool IsFile => FileSize > 0;
}