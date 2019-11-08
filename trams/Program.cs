using System;
using System.Security.Cryptography;
using System.Threading;
namespace TurtleGame
{
    public class Turtle
    {
        public Vector2 postition;
        public IMoveStrategy strategy;
        public Vector2 spawnLocation;
        public Turtle(int boardSize)
        {
            // generate spawn location and save 
            spawnLocation = new Vector2(boardSize);
            postition = spawnLocation; strategy = new YourStrategy();
        }
    }

    /* Implement this interface and help the turtle find his way home in the least amount of turns. Your strategy does not know where the goal is. The objective is to systematically search the grid for it. You will only be able to move 1 square at a time, otherwise your turtle wont move */
    public interface IMoveStrategy
    {
        /** * This function should return the delta, i.e. the direction you wish to move in. Values are capped at 1/1. * @turtlePosition: Your current position on the grid. * @boardSize: Board size. Board is a square, so if boardSize is 10, you have 100 squares. * @spawnLocation: Where you spawned on the board. */
        public Vector2 Move(Vector2 turtlePosition, int boardSize, Vector2 spawnLocation);
    }

    /* Sample implementation where the turtle keeps moving down */
    public class YourStrategy : IMoveStrategy
    {
        public Vector2 Move(Vector2 turtlePosition, int boardSize, Vector2 spawnLocation)
        {
            return new Vector2() { x = 1, y = 0, };
        }
    }

    public class Vector2
    {
        public int x;
        public int y;
        private RNGCryptoServiceProvider gen;

        public Vector2(int boardSize)
        {
            gen = new RNGCryptoServiceProvider(); x = Random(boardSize); y = Random(boardSize);
        }

        public Vector2(int x, int y) { this.x = x; this.y = y; }
        public Vector2() { }
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        private int Random(int max)
        {
            var data = new byte[sizeof(uint)]; gen.GetBytes(data);
            return (int)Math.Floor(max * BitConverter.ToUInt32(data, 0) / (uint.MaxValue + 1.0));
        }
    }

    public class Game
    {
        private Turtle turtle; private Vector2 home; private int boardSize = 25;
        public Game()
        {
            turtle = new Turtle(boardSize);
            home = new Vector2(boardSize);
            while (!HasWon()) { GameLoop(); }
            Console.WriteLine("VICTREEE");
            Console.Read();
        }

        void GameLoop()
        {
            MoveTurtle(); Print();
            Thread.Sleep(1000);
        }
        void MoveTurtle()
        {
            var delta = turtle.strategy.Move(turtle.postition, boardSize, turtle.spawnLocation);
            // dont move if x or y exceedes 1 
            if (delta.x <= 1 && delta.y <= 1) { turtle.postition += delta; }
        }
        void Print()
        {
            Console.Clear();
            for (var x = 0; x < boardSize; x++)
            {
                for (var y = 0; y < boardSize; y++)
                {
                    if (x == turtle.postition.x && y == turtle.postition.y)
                    { Console.Write("T"); }
                    else if (x == home.x && y == home.y)
                    { Console.Write("H"); }
                    else { Console.Write("-"); }
                }
                Console.Write("\n");
            }
        }

        bool HasWon() => home.x == turtle.postition.x && home.y == turtle.postition.y;
    }

    class Program { static void Main(string[] args) { new Game(); } }
}