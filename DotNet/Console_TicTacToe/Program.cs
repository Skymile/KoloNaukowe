bool?[] grid = new bool?[TicTacToe.Width * TicTacToe.Height];
bool player = true, lined = false;
do
{
    Console.Clear();
    Console.WriteLine(TicTacToe.GridToString(grid));

    int key;
    if ((key = Console.ReadKey().KeyChar - '1') < 0 || key > 8 || grid[key].HasValue)
        continue;

    grid[key] = player;
    if (lined = TicTacToe.IsLined(grid))
        break;
    player ^= true;

} while (!TicTacToe.IsFull(grid));

Console.Clear();

Console.WriteLine(lined ? "Player '{0}' won" : "Draw!", player ? "x" : "y");
Console.ReadLine();
