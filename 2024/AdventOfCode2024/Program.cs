using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Day04_1();
        Day04_2();
    }

    private static void Day04_2()
    {
        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day04" + folderSeparator + "input04.txt";
        
        string[] lines = File.ReadAllLines(filePath);

        char[,] grid = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }

        int count = 0;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int r = 0; r < rows - 2; r++)
        {
            for (int c = 0; c < cols - 2; c++)
            {
                // Check for X-MAS pattern
                if ((grid[r, c] == 'M' && grid[r + 1, c + 1] == 'A' && grid[r + 2, c + 2] == 'S' &&
                     grid[r + 2, c] == 'M' && grid[r + 1, c + 1] == 'A' && grid[r, c + 2] == 'S') ||
                    (grid[r, c + 2] == 'M' && grid[r + 1, c + 1] == 'A' && grid[r + 2, c] == 'S' &&
                     grid[r, c] == 'M' && grid[r + 1, c + 1] == 'A' && grid[r + 2, c + 2] == 'S'))
                {
                    count++;
                }
            }
        }

        Console.WriteLine($"The X-MAS pattern appears {count} times.");

    }

    private static void Day04_1()
    {

        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day04" + folderSeparator + "input04.txt";

        string[] lines = File.ReadAllLines(filePath);
        char[,] grid = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }

        string word = "XMAS";
        int count = 0;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        int wordLength = word.Length;

        int[][] directions = new int[][]
        {
            new int[] { 0, 1 },   // right
            new int[] { 1, 0 },   // down
            new int[] { 1, 1 },   // down-right
            new int[] { 1, -1 },  // down-left
            new int[] { 0, -1 },  // left
            new int[] { -1, 0 },  // up
            new int[] { -1, -1 }, // up-left
            new int[] { -1, 1 }   // up-right
        };

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                foreach (var dir in directions)
                {
                    bool found = true;
                    for (int i = 0; i < wordLength; i++)
                    {
                        int newRow = r + i * dir[0];
                        int newCol = c + i * dir[1];

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || grid[newRow, newCol] != word[i])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        count++;
                    }
                }
            }
        }

        Console.WriteLine($"The word '{word}' appears {count} times.");
    }

    private static void Day03()
    {

        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day03" + folderSeparator + "input03.txt";

        string input = File.ReadAllText(filePath);

        // Define the regex pattern to match valid mul(X,Y) instructions
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        Regex regex = new Regex(pattern);

        // Find all matches in the input
        MatchCollection matches = regex.Matches(input);

        // Initialize the sum
        int sum = 0;

        // Process each match
        foreach (Match match in matches)
        {
            // Extract the numbers from the match
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);

            // Multiply the numbers and add to the sum
            sum += x * y;
        }

        // Output the result
        Console.WriteLine($"The sum of all valid multiplications is: {sum}");



        // Read the input file
        input = File.ReadAllText(filePath);

        // Define the regex patterns to match valid instructions
        string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        string doPattern = @"do\(\)";
        string dontPattern = @"don't\(\)";

        Regex mulRegex = new Regex(mulPattern);
        Regex doRegex = new Regex(doPattern);
        Regex dontRegex = new Regex(dontPattern);

        // Find all matches in the input
        MatchCollection mulMatches = mulRegex.Matches(input);
        MatchCollection doMatches = doRegex.Matches(input);
        MatchCollection dontMatches = dontRegex.Matches(input);

        // Initialize the sum and the state of mul instructions
        sum = 0;
        bool mulEnabled = true;

        // Process the input in order
        int currentIndex = 0;
        foreach (Match match in mulMatches)
        {
            // Process any do() or don't() instructions before this mul() instruction
            while (currentIndex < input.Length && currentIndex < match.Index)
            {
                if (doRegex.IsMatch(input, currentIndex))
                {
                    mulEnabled = true;
                    currentIndex = doRegex.Match(input, currentIndex).Index + doRegex.Match(input, currentIndex).Length;
                }
                else if (dontRegex.IsMatch(input, currentIndex))
                {
                    mulEnabled = false;
                    currentIndex = dontRegex.Match(input, currentIndex).Index + dontRegex.Match(input, currentIndex).Length;
                }
                else
                {
                    currentIndex++;
                }
            }

            // If mul instructions are enabled, process the current mul() instruction
            if (mulEnabled)
            {
                int x = int.Parse(match.Groups[1].Value);
                int y = int.Parse(match.Groups[2].Value);
                sum += x * y;
            }

            // Move the current index past the current mul() instruction
            currentIndex = match.Index + match.Length;
        }

        // Output the result
        Console.WriteLine($"The sum of all enabled multiplications is: {sum}");


    }

    private static void Day02()
    {

        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day02" + folderSeparator + "input.txt";

        string[] lines = File.ReadAllLines(filePath);
        int safeReportsCount = 0;

        foreach (var line in lines)
        {
            int[] levels = line.Split(' ').Select(int.Parse).ToArray();
            bool increasing = false;
            bool decreasing = false;
            bool validDifference = true;

            for (int i = 0; i < levels.Length - 1; i++)
            {
                int diff = levels[i + 1] - levels[i];
                if (diff < 1 || diff > 3)
                {
                    validDifference = false;
                }
                if (levels[i] < levels[i + 1])
                {
                    increasing = true;
                } else
                {
                    decreasing = true;
                }
            }

            if (validDifference)
            {
                if (increasing != decreasing)
                    safeReportsCount++;
            } 
        }

        Console.WriteLine($"The number of safe reports is: {safeReportsCount}");


        // Regular expression to match valid mul instructions
        string fileContent = File.ReadAllText(filePath);
        
        string pattern = @"mul\(\d+,\d+\)";

        // Find all matches in the text
        MatchCollection matches = Regex.Matches(fileContent, pattern);

        // Initialize the sum of results
        long totalSum = 0;

        // Evaluate each match and add the result to the total sum
        foreach (Match match in matches)
        {
            // Extract the numbers from the match
            string[] numbers = Regex.Matches(match.Value, @"\d+").Select(m => m.Value).ToArray();
            int x = int.Parse(numbers[0]);
            int y = int.Parse(numbers[1]);

            // Multiply the numbers and add to the total sum
            totalSum += x * y;
        }

        // Print the result
        Console.WriteLine($"The sum of all valid mul instructions is: {totalSum}");





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