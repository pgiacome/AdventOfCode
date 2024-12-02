using System.Collections;
using System;
using System.Security.Cryptography;

internal class Program
{
    private static void Main(string[] args)
    {
        Day01();
    }

    private static void Day01()
    {

        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder +  folderSeparator + "Day01" + folderSeparator + "input.txt";

        // Read the input file
        string[] lines = File.ReadAllLines(filePath);

        // Parse the input into two lists
        var leftList = lines.Select(line => int.Parse(line.Split("   ",StringSplitOptions.RemoveEmptyEntries)[0])).ToList();
        var rightList = lines.Select(line => int.Parse(line.Split("   ", StringSplitOptions.RemoveEmptyEntries)[1])).ToList();

        // Sort both lists
        leftList.Sort();
        rightList.Sort();

        // Calculate the total distance
        int totalDistance = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            totalDistance += Math.Abs(leftList[i] - rightList[i]);
        }

        // Output the result
        Console.WriteLine($"Total distance: {totalDistance}");


        //part 2
        // Create a dictionary to count occurrences in the right list
        var rightCount = new Dictionary<int, int>();
        foreach (var number in rightList)
        {
            if (rightCount.ContainsKey(number))
            {
                rightCount[number]++;
            }
            else
            {
                rightCount[number] = 1;
            }
        }

        // Calculate the similarity score
        int similarityScore = 0;
        foreach (var number in leftList)
        {
            if (rightCount.ContainsKey(number))
            {
                similarityScore += number * rightCount[number];
            }
        }

        // Output the result
        Console.WriteLine($"Similarity score: {similarityScore}");



    }
}