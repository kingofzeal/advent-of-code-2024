using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

var input = File.ReadAllLines("./input.txt");
// var input = File.ReadAllLines("./sample.txt");

int Part1(){
    var rules = input.Where(x => x.Contains('|')).Select(x => x.Split('|').ToList()).ToList();
    var pages = input.Where(x => x.Contains(',')).Select(x => x.Split(',').ToList()).ToList();

    var middleSum = 0;

    foreach (var pageList in pages){
        var valid = true;
        Console.WriteLine($"Checking {string.Join(',', pageList)}...");

        var applicableRules = rules.Where(x => pageList.Contains(x[0]) && pageList.Contains(x[1])).ToList();

        foreach (var rule in applicableRules){
            if (pageList.IndexOf(rule[1]) <= pageList.IndexOf(rule[0])){
                Console.WriteLine($"--{rule[1]} is before {rule[0]} - {string.Join(',', pageList)} is invalid page order.");
                valid = false;
                break;
            }
        }

        if (!valid) continue;

        var middleNum = int.Parse(pageList[(pageList.Count - 1) / 2]);
        Console.WriteLine($"{string.Join(',', pageList)} valid. Adding {middleNum} to total");

        middleSum += middleNum;
    }

    Console.WriteLine($"Part 1: {middleSum}");
    return middleSum;
}

int Part2(){
    var rules = input.Where(x => x.Contains('|')).Select(x => x.Split('|').ToList()).ToList();
    var pages = input.Where(x => x.Contains(',')).Select(x => x.Split(',').ToList()).ToList();

    var middleSum = 0;

    (bool result, List<string>? failedRule) CheckRules(List<List<string>> rules, List<string> pageList){
        foreach (var rule in rules){
            if (pageList.IndexOf(rule[1]) <= pageList.IndexOf(rule[0])){
                // Console.WriteLine($"--{rule[1]} is before {rule[0]} - {string.Join(',', pageList)} is invalid page order.");
                return (false, rule);
            }
        }
        return (true, null);
    }

    foreach (var pageList in pages){
        Console.Write($"Checking {string.Join(',', pageList)}...");

        var applicableRules = rules.Where(x => pageList.Contains(x[0]) && pageList.Contains(x[1])).ToList();

        var (valid, breakingRule) = CheckRules(applicableRules, pageList);
        if (valid){
            Console.WriteLine(" Valid.");
            continue;
        }

        Console.WriteLine(" Invalid.");

        var currentPageSet = pageList.ToList();

        var swaps = 0;

        while (!valid){
            swaps++;
            var idx0 = currentPageSet.IndexOf(breakingRule![0]);
            var idx1 = currentPageSet.IndexOf(breakingRule[1]);

            currentPageSet[idx0] = breakingRule[1];
            currentPageSet[idx1] = breakingRule[0];
            
            (valid, breakingRule) = CheckRules(applicableRules, currentPageSet);
        }

        var middleNum = int.Parse(currentPageSet[(currentPageSet.Count - 1) / 2]);
        Console.WriteLine($"         {string.Join(',', currentPageSet)} is the valid order after {swaps} swaps. Adding {middleNum} to total");

        middleSum += middleNum;
    }

    // Console.WriteLine($"Part 2: {middleSum}");
    return middleSum;
}

//var p1 = Part1(); Console.WriteLine($"Part 1: {p1}");
var p2 = Part2(); Console.WriteLine($"Part 2: {p2}");
