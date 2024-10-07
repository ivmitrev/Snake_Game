using System;
using System.Reflection;
using System.Threading.Channels;

namespace SnakeGame
{
    class Program
    {
        struct Position
        {
            public int x;
            public int y;

            public Position(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private static int size = 10;
        private static LinkedList<Position> snake = new LinkedList<Position>();
        private static Position apple = new Position();
      

        static void Main(string[] args)
        { 
            Console.CursorVisible = false; 
            snake.AddLast(new Position(size / 2, size / 2));
            CreateApple();
            BuildGround();
            Direction currentDir = Direction.Up;

           while (true)
           {
               if (Console.KeyAvailable)
               {
                   var key = Console.ReadKey(true);
                   currentDir = ChangeDir(currentDir,key);
               }
               Thread.Sleep(200);
               
               var currentPos = snake.First.Value;
               Position newPos;
               switch (currentDir)
               {
                   case Direction.Up:
                       newPos = new Position(currentPos.x, currentPos.y - 1);
                       if (SnakeBody(newPos))
                       {
                           return;
                       }

                       snake.AddFirst(newPos);
                       break;
                   case Direction.Down:
                       newPos = new Position(currentPos.x, currentPos.y + 1);
                       if (SnakeBody(newPos))
                       {
                           return;
                       }

                       snake.AddFirst(newPos);
                       break;
                   case Direction.Left:
                       newPos = new Position(currentPos.x - 1, currentPos.y);
                       if (SnakeBody(newPos))
                       {
                           return;
                       }

                       snake.AddFirst(newPos);
                       break;
                   case Direction.Right:
                       newPos = new Position(currentPos.x + 1, currentPos.y);
                       if (SnakeBody(newPos))
                       {
                           return;
                       }

                       snake.AddFirst(newPos);
                       break;
               }

               currentPos = snake.First.Value;
               if (currentPos.x == apple.x && currentPos.y == apple.y)
               {
                   CreateApple();
               }
               else if (currentPos.x == -1 || currentPos.x == size || currentPos.y == -1 || currentPos.y == size)
               {
                   return;
               }
               else
               {
                   // removes when we don't take an apple and change direction to not add unnecessary body
                   RemoveTail();
                   
               }
                
               Console.SetCursorPosition( 2 * (currentPos.x + 1), currentPos.y + 1);
               Console.Write("S");

           }
           
        }

        static void RemoveTail()
        {
            Console.SetCursorPosition(2 * (snake.Last.Value.x + 1), snake.Last.Value.y + 1);
            Console.Write(" ");
            snake.RemoveLast();
        }

        static bool SnakeBody(Position newPos) // checks if the snake tries to go against its own body
        {
            if (snake.Any(cords => cords.x == newPos.x && cords.y == newPos.y))
            {
                return true;
            }

            return false;
        }

        static Direction ChangeDir(Direction currentDir, ConsoleKeyInfo key) // change direction
        {
            switch (key.Key)
            {
                case ConsoleKey.W:
                    if (currentDir != Direction.Down)
                    {
                        currentDir = Direction.Up;
                    }

                    break;
                case ConsoleKey.S:
                    if (currentDir != Direction.Up)
                    {
                        currentDir = Direction.Down;
                    }
                    break;
                case ConsoleKey.A:
                    if (currentDir != Direction.Right)
                    {
                        currentDir = Direction.Left;
                    }
                    break;
                case ConsoleKey.D:
                    if (currentDir != Direction.Left)
                    {
                        currentDir = Direction.Right;
                    }
                    break;

                   
            }
            return currentDir;
        }

        static void CreateApple()
        {
            Random rnd = new Random();
            apple = new Position(rnd.Next(1, size), rnd.Next(1, size));
            while (snake.Any(cords => { return cords.x == apple.x && cords.y == apple.y;}))
            {
                apple = new Position(rnd.Next(1, size), rnd.Next(1, size));
            }
            Console.SetCursorPosition(2 * (apple.x+1), apple.y + 1);
            Console.Write("@");
            
            
        }

        static void BuildGround()
        {
            for (int i = 0; i <= size+1; i++)
            {
                for (int j = 0; j <= size+1; j++)
                {
                    if (i == 0 || j == 0 || i == size+1 || j == size + 1)
                    {
                        Console.SetCursorPosition(i*2,j);
                        Console.Write("*");
                    }
                }  
            }
        }
    }
    
}
