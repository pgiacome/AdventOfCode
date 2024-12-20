﻿using System.Collections;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

//test 
internal class Program
{
    private static void Main(string[] args)
    {
        /*Day04_1();
        Day04_2();
        Day05_1();
        Day05_2();
        */
        Day06_1(); ;
    }

    private static void Day06_1()
    {

        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day06" + folderSeparator + "input06.txt";

        // Read the input file
        string[] mapData = File.ReadAllLines(filePath);

        // Parse the map
        var labMap = new List<string>();
        foreach (var line in mapData)
        {
            labMap.Add(line);
        }

        // Find the initial position and direction of the guard
        (int x, int y, char direction) guard = (0, 0, ' ');
        for (int y = 0; y < labMap.Count; y++)
        {
            for (int x = 0; x < labMap[y].Length; x++)
            {
                if ("^>v<".Contains(labMap[y][x]))
                {
                    guard = (x, y, labMap[y][x]);
                    break;
                }
            }
        }

        // Initialize the set of visited positions
        var visitedPositions = new HashSet<(int, int)>();
        visitedPositions.Add((guard.x, guard.y));

        // Function to turn right
        char TurnRight(char direction)
        {
            return direction switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                '<' => '^',
                _ => direction,
            };
        }

        // Function to move forward
        (int, int) MoveForward((int x, int y) position, char direction)
        {
            return direction switch
            {
                '^' => (position.x, position.y - 1),
                '>' => (position.x + 1, position.y),
                'v' => (position.x, position.y + 1),
                '<' => (position.x - 1, position.y),
                _ => position,
            };
        }

        // Function to check if within bounds
        bool IsWithinBounds((int x, int y) position, List<string> mapData)
        {
            return position.y >= 0 && position.y < mapData.Count && position.x >= 0 && position.x < mapData[position.y].Length;
        }

        // Simulate the guard's movement
        while (IsWithinBounds((guard.x, guard.y), labMap))
        {
            var nextPosition = MoveForward((guard.x, guard.y), guard.direction);
            if (!IsWithinBounds(nextPosition, labMap) || labMap[nextPosition.Item2][nextPosition.Item1] == '#')
            {
                guard.direction = TurnRight(guard.direction);
                Console.WriteLine("guardia ha girato a dx");
            }
            else
            {
                guard = (nextPosition.Item1, nextPosition.Item2, guard.direction);
                visitedPositions.Add((guard.x, guard.y));
            }
        }

        // Output the number of distinct positions visited
        Console.WriteLine($"Distinct positions visited: {visitedPositions.Count}");


    }

    private static void Day05_2()
    {
        // Read the input file
        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();
        
        string filePath = InputFolder + folderSeparator + "Day05" + folderSeparator + "input05.txt";
        
        var input = File.ReadAllText(filePath);


        var sections = input.Split(new[] { "\n\n" }, StringSplitOptions.None);

        // Parse the rules
        var rules = sections[0].Split('\n')
            .Select(line => line.Split('|'))
            .Select(parts => (X: int.Parse(parts[0]), Y: int.Parse(parts[1])))
            .ToList();

        // Parse the updates
        var updates = sections[1].Split('\n')
            .Select(line => line.Split(',').Select(int.Parse).ToList())
            .ToList();

        int middlePagesSum = 0;

        foreach (var update in updates)
        {
            bool isCorrectOrder = true;
            foreach (var (X, Y) in rules)
            {
                if (update.Contains(X) && update.Contains(Y))
                {
                    if (update.IndexOf(X) > update.IndexOf(Y))
                    {
                        isCorrectOrder = false;
                        break;
                    }
                }
            }
            if (!isCorrectOrder)
            {
                var orderedUpdate = new List<int>(update);
                orderedUpdate.Sort((a, b) =>
                {
                    foreach (var (X, Y) in rules)
                    {
                        if (a == X && b == Y) return -1;
                        if (a == Y && b == X) return 1;
                    }
                    return 0;
                });
                middlePagesSum += orderedUpdate[orderedUpdate.Count / 2];
            }
        }

        Console.WriteLine($"The sum of the middle page numbers from the correctly-ordered updates is {middlePagesSum}.");
    }

    private static void Day05_1() 
    {
        string InputFolder = Environment.CurrentDirectory;

        string folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        string filePath = InputFolder + folderSeparator + "Day05" + folderSeparator + "input05.txt";

        var input = File.ReadAllText(filePath);
        var sections = input.Split(new[] { "\n\n" }, StringSplitOptions.None);
        
        // Parse the rules
        var rules = sections[0].Split('\n')
            .Select(line => line.Split('|'))
            .Select(parts => (X: int.Parse(parts[0]), Y: int.Parse(parts[1])))
            .ToList();
        
        // Parse the updates
        var updates = sections[1].Split('\n')
            .Select(line => line.Split(',').Select(int.Parse).ToList())
            .ToList();
        
        int middlePagesSum = 0;
        
        foreach (var update in updates)
        {
            bool isCorrectOrder = true;
            foreach (var (X, Y) in rules)
            {
                if (update.Contains(X) && update.Contains(Y))
                {
                    if (update.IndexOf(X) > update.IndexOf(Y))
                    {
                        isCorrectOrder = false;
                        break;
                    }
                }
            }
            if (isCorrectOrder)
            {
                middlePagesSum += update[update.Count / 2];
            }
        }
        
        Console.WriteLine($"The sum of the middle page numbers from the correctly-ordered updates is {middlePagesSum}.");




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