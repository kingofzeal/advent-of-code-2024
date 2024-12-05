using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

var input = File.ReadAllLines("./input.txt");

void Part1(){
    int[] N = new[] { 0, 1, 2, 3 };

    var arr = input.Select(x => x.ToCharArray()).ToList();

    var yMax = arr.Count;
    var xMax = arr[0].Count();

    int count = 0;
    for (int y = 0; y < yMax; y++)
    {
        for (int x = 0; x < xMax; x++)
        {
            List<char[]> s = new();
            if (x <= xMax - 4)
                s.Add(N.Select(i => arr[y][x + i]).ToArray());
            if (y <= yMax - 4)
                s.Add(N.Select(i => arr[y + i][x]).ToArray());
            if (x <= xMax - 4 && y <= yMax - 4)
                s.Add(N.Select(i => arr[y + i][x + i]).ToArray());
            if (x >= 3 && y <= yMax - 4)
                s.Add(N.Select(i => arr[y + i][x - i]).ToArray());
            count += s.AsEnumerable().Count(t => t.SequenceEqual("XMAS") || t.SequenceEqual("SAMX"));
        }
    }

    //2398
    Console.WriteLine($"Part 1: {count}");
}

void Part2(){
    int[] N = new[] { 0, 1, 2, 3 };
    string[] grid = input.ToArray();
    int[] dx = new[] { 1, 1, -1, -1 };
    int[] dy = new[] { 1, -1, 1, -1 };
    int count = 0;
    for (int y = 1; y <= grid.Length - 2; y++)
    {
        for (int x = 1; x <= grid[0].Length - 2; x++)
        {
            if (grid[y][x] != 'A') 
                continue;
            char[] nxts = N.Select(i => grid[y + dy[i]][x + dx[i]]).ToArray();
            if (nxts.All(n => n == 'M' || n == 'S') && nxts[0] != nxts[3] && nxts[1] != nxts[2])
                count += 1;
        }
    }

    Console.WriteLine($"Part 2: {count}");
}

Part1();
Part2();
