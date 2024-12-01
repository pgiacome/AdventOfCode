internal class Program
{
    private static void Main(string[] args)
    {
        Day01();
    }

    private static void Day01()
    {

        InputFolder = Environment.CurrentDirectory;

        folderSeparator = System.IO.Path.DirectorySeparatorChar.ToString();

        CurrentDay = DownloadFolder +  folderSeparator + "Day01" + folderSeparator + "input.txt";

        var lines = System.IO.File.ReadAllLines(filePath);

    }
}