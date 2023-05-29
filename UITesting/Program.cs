


using ConsoleUI;
using Pastel;
using System.Drawing;

ConsoleDisplayAdapter cda = new();


UIString iString = new("Hellooo?, this will now be a longer string, just so that it wraps around a bit. I'd like to test the wrapping again.", new(10, 10, 28, 20));
UIString iString2 = new("world?, wait. If I make this a bit longer?", new(50, 1, 20, 4), true);

Border border = new(new(0, 0, 30, 20), iString);

Canvas ca = new(cda.WindowSize);

ca.AddSubDisplay(iString2);
ca.AddSubDisplay(border);

Display dp = new(cda, ca);
dp.Refresh();


while (true)
{
    ConsoleKeyInfo c = Console.ReadKey(true);
    if (c.Key == ConsoleKey.RightArrow) border.Bounds.MoveRect(0, 0, 1, 0);
    if (c.Key == ConsoleKey.LeftArrow) border.Bounds.MoveRect(0, 0, -1, 0);
    if (c.Key == ConsoleKey.UpArrow) border.Bounds.MoveRect(0, 0, 0, -1);
    if (c.Key == ConsoleKey.DownArrow) border.Bounds.MoveRect(0, 0, 0, 1);
    if (c.Key == ConsoleKey.W) border.Bounds.MoveRect(0, -1, 0, 0);
    if (c.Key == ConsoleKey.A) border.Bounds.MoveRect(-1, 0, 0, 0);
    if (c.Key == ConsoleKey.S) border.Bounds.MoveRect(0, 1, 0, 0);
    if (c.Key == ConsoleKey.D) border.Bounds.MoveRect(1, 0, 0, 0);
    dp.Refresh();

}