// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int y = 0; // depth
int x = 0; // horizontal position

StreamReader sr = new StreamReader("data.txt");

while (!sr.EndOfStream)
{
    string subCommand = sr.ReadLine();

    string[] parts = subCommand.Split(' ');

    int movement = int.Parse(parts[1]);

    if (parts[0] == "forward")
    {
        x += movement;
    }
    else if(parts[0] == "down")
    {
        y += movement;
    }
    else if(parts[0] == "up")
    {
        y -= movement;
    }
}

sr.Close();
Console.WriteLine("Part 1 - x={0}, y={1}, x*y={2}", x, y, x * y);

y = 0; // depth
x = 0; // horizontal position
int aim = 0;

sr = new StreamReader("data.txt");

while (!sr.EndOfStream)
{
    string subCommand = sr.ReadLine();

    string[] parts = subCommand.Split(' ');

    int movement = int.Parse(parts[1]);

    if (parts[0] == "forward")
    {
        x += movement;
        y += aim * movement;
    }
    else if (parts[0] == "down")
    {
        aim += movement;
    }
    else if (parts[0] == "up")
    {
        aim -= movement;
    }
}

Console.WriteLine("Part 2 - x={0}, y={1}, aim={3}, x*y={2}", x, y, aim, x * y);

sr.Close();


