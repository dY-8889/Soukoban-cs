
using System;

enum ObjectType
{
    Player,
    Box,
    Wall,
    Gool,
    None,
}

enum Direction
{
    Left,
    Right,
    Up,
    Down,
}

struct Position
{
    public byte X;
    public byte Y;

    public Position(byte x, byte y) { X = x; Y = y; }

    public void Left()
    {
        int x = X - 1;
        if (!(x == -1)) X = (byte)x;
    }
    public void Right(int lenght)
    {
        int x = X + 1;
        if (!(lenght - 1 < x)) X = (byte)x;
    }
    public void Up()
    {
        int y = Y - 1;
        if (!(y == -1)) Y = (byte)y;
    }
    public void Down(int lenght)
    {
        int y = Y + 1;
        if (!(lenght - 1 < y)) Y = (byte)y;
    }

    public override readonly string ToString() => $"X: {X} Y: {Y}";
}

class Game
{
    const ObjectType P = ObjectType.Player;
    const ObjectType B = ObjectType.Box;
    const ObjectType W = ObjectType.Wall;
    const ObjectType G = ObjectType.Gool;
    const ObjectType N = ObjectType.None;

    static readonly ObjectType[,] STAGE1 = {
        {W, W, W, W, W, W, W, W, W, W, W, W, W, W, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, G, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, B, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, B, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, G, W},
        {W, W, W, W, W, W, W, W, W, W, W, W, W, W, W},
    };
    static readonly ObjectType[,] STAGE2 = {
        {W, W, W, W, W, W, W, W, W, W, W, W, W, W, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, G, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, W, W, W, W, W, W, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, W, N, N, N, W},
        {W, N, N, N, N, W, N, B, N, N, W, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, W, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, W, N, N, N, W},
        {W, N, N, N, N, W, N, N, N, N, W, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, B, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, N, W},
        {W, N, N, N, N, N, N, N, N, N, N, N, N, G, W},
        {W, W, W, W, W, W, W, W, W, W, W, W, W, W, W},
    };

    public static readonly ObjectType[][,] STAGE_LIST = {
        STAGE1,
        STAGE2
    };

    ObjectType[,] field;

    Position position;
    Position b_position;

    Direction direction = Direction.Right;

    byte stage = 0;

    public Game(Position position)
    {
        ObjectType[,] field = STAGE_LIST[stage];
        field[position.Y, position.X] = ObjectType.Player;

        this.field = field;
        this.position = position;
        b_position = position;
    }

    public void Draw()
    {
        //Console.Write("\x1b[H");
        Console.Clear();

        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(0); x++)
            {
                Console.Write(" ");
                switch (field[y, x])
                {
                    //case ObjectType.Player: Console.Write("🟩"); break;
                    //case ObjectType.Box: Console.Write("⬜️"); break;
                    //case ObjectType.Wall: Console.Write("🟫"); break;
                    //case ObjectType.Gool: Console.Write("🔶"); break;
                    //case ObjectType.None: Console.Write("  "); break;
                    case ObjectType.Player: Console.Write("P"); break;
                    case ObjectType.Box: Console.Write("B"); break;
                    case ObjectType.Wall: Console.Write("W"); break;
                    case ObjectType.Gool: Console.Write("G"); break;
                    case ObjectType.None: Console.Write(" "); break;
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine($"Position {position}\nDirection: {direction}");
    }

    public void MovePlayer()
    {
        CheckWall();

        field[b_position.Y, b_position.X] = ObjectType.None;

        if (field[position.Y, position.X] == ObjectType.Box)
        {
            Position pos = ReturnDirection();
            byte x = pos.X;
            byte y = pos.Y;

            if (field[y, x] == ObjectType.Wall || field[y, x] == ObjectType.Box)
                UndoPosition();
            else
                field[y, x] = ObjectType.Box;
        }

        field[position.Y, position.X] = ObjectType.Player;

        PositioinBuf();
    }

    Position ReturnDirection()
    {
        Position pos = position;
        switch (direction)
        {
            case Direction.Left: pos.X -= 1; break;
            case Direction.Right: pos.X += 1; break;
            case Direction.Up: pos.Y -= 1; break;
            case Direction.Down: pos.Y += 1; break;
        }
        return pos;
    }

    public void NextStage()
    {
        stage++;
        if (STAGE_LIST.Length - 1 < stage) stage = 0;
        field = STAGE_LIST[stage];
    }

    void CheckWall()
    {
        byte x = position.X;
        byte y = position.Y;
        if (field[y, x] == ObjectType.Wall) UndoPosition();
    }

    void PositioinBuf() => b_position = position;

    void UndoPosition() => position = b_position;

    public void Left()
    {
        position.Left();
        direction = Direction.Left;
    }
    public void Right()
    {
        position.Right(field.Length);
        direction = Direction.Right;
    }
    public void Up()
    {
        position.Up();
        direction = Direction.Up;
    }
    public void Down()
    {
        position.Down(field.Length);
        direction = Direction.Down;
    }
}

public class Program
{
    public static void Main()
    {
        // カーソル非表示
        Console.Write("\x1b[?25l");

        MainGame();

        // カーソル表示
        Console.Write("\x1b[?25h");
    }

    static void MainGame()
    {
        Game game = new Game(new Position(2, 2));
        while (true)
        {
            game.MovePlayer();
            game.Draw();

            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow: game.Left(); break;
                case ConsoleKey.RightArrow: game.Right(); break;
                case ConsoleKey.UpArrow: game.Up(); break;
                case ConsoleKey.DownArrow: game.Down(); break;
                case ConsoleKey.N: game.NextStage(); break;
                case ConsoleKey.Q: return;
                default: break;
            }
        }
    }
}
