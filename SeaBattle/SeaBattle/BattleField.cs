using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    enum eDirection { up = 0, down, right, left };

    enum eOptionShots { missed = -1, repeatShot = 0, hit = 1, win = 2, destruction = 3 };


    class BattleField
    {
        //Size
        private const int SizeX = 10;
        private const int SizeY = 10;

        //Public information
        public bool Win;
        public int countOfMoves;


        //Logic AI
        private struct AI
        {
            public bool currentShip;

            public uint botComplexity;

            public int firstShotX;
            public int firstShotY;

            public eDirection direction;

            public List<eDirection> invalidDirection;
        }
        private AI aI = new AI();


        //information about reserved coordinates
        private bool[,] reserved;


        //information about the coordinates of the shots
        private bool[,] myShot;


        //information about the coordinates of the ship
        private BaseShip[] ships;


        //ctor
        public BattleField()
        {
            ships = new BaseShip[10];

            ships[0] = new FourDeckShip();
            ships[1] = new ThreeDeckShip();
            ships[2] = new ThreeDeckShip();
            ships[3] = new DoubleDeckShip();
            ships[4] = new DoubleDeckShip();
            ships[5] = new DoubleDeckShip();
            ships[6] = new SingleDeckShip();
            ships[7] = new SingleDeckShip();
            ships[8] = new SingleDeckShip();
            ships[9] = new SingleDeckShip();



            aI.currentShip = false;
            aI.invalidDirection = new List<eDirection>();
            aI.botComplexity = 20000;

            Win = false;
            countOfMoves = 0;

            //initialize array
            reserved = new bool[10, 10];
            myShot = new bool[10, 10];

        }





        //Рисуем попадания по нам + наши все корабли включая вибитии
        public void Draw(BattleField bot, int startX, int startY)
        {

            // Живие коробли
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int ship = 0; ship < ships.Length; ship++)
            {
                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    Console.SetCursorPosition(2 + startX + ships[ship].decks[deck].x, 2 + ships[ship].decks[deck].y + startY);
                    Console.Write("#");
                }
            }

            //Попадания
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (bot.myShot[x, y] == true)
                    {
                        Console.SetCursorPosition(2 + startX + x, 2 + startY + y);
                        Console.Write(".");
                    }
                }
            }


            // Вибитие корабли
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int ship = 0; ship < ships.Length; ship++)
            {
                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    if (ships[ship].decks[deck].Alive == false)
                    {
                        Console.SetCursorPosition(2 + startX + ships[ship].decks[deck].x, 2 + ships[ship].decks[deck].y + startY);
                        Console.Write("X");
                    }
                }
            }


            Console.ResetColor();
            // Кординати x
            Console.SetCursorPosition(0 + startX, 0 + startY);
            Console.Write("  ");
            for (int i = 0; i < 10; i++) { Console.Write(i); }

            // Кординати y
            for (int i = 0; i < 10; i++) { Console.SetCursorPosition(0 + startX, 2 + startY + i); Console.Write(i); }

            // Верхняя рамка
            Console.SetCursorPosition(1 + startX, 1 + startY);
            Console.WriteLine("*----------*");

            // Боковая рамка
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(1 + startX, 2 + i + startY); Console.WriteLine("|");
                Console.SetCursorPosition(12 + startX, 2 + i + startY); Console.WriteLine("|");
            }

            // Нижняя рамка
            Console.SetCursorPosition(1 + startX, 12 + startY);
            Console.WriteLine("*----------*");

        }
        //Рисуем наши попадания и вибитие кораблики bota
        public void DrawField(BattleField bot, int startX, int startY)
        {

            //Попадания
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (myShot[x, y] == true)
                    {
                        Console.SetCursorPosition(2 + startX + x, 2 + startY + y);
                        Console.Write(".");
                    }
                }
            }

            // Вибитие корабли
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int ship = 0; ship < bot.ships.Length; ship++)
            {
                for (int deck = 0; deck < bot.ships[ship].decks.Length; deck++)
                {
                    if (bot.ships[ship].decks[deck].Alive == false)
                    {
                        Console.SetCursorPosition(2 + startX + bot.ships[ship].decks[deck].x, 2 + bot.ships[ship].decks[deck].y + startY);
                        Console.Write("X");
                    }
                }
            }


            Console.ResetColor();
            // Кординати x
            Console.SetCursorPosition(0 + startX, 0 + startY);
            Console.Write("  ");
            for (int i = 0; i < 10; i++) { Console.Write(i); }

            // Кординати y
            for (int i = 0; i < 10; i++) { Console.SetCursorPosition(0 + startX, 2 + startY + i); Console.Write(i); }


            // Верхняя рамка
            Console.SetCursorPosition(1 + startX, 1 + startY);
            Console.WriteLine("*----------*");

            // Боковая рамка
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(1 + startX, 2 + i + startY); Console.WriteLine("|");
                Console.SetCursorPosition(12 + startX, 2 + i + startY); Console.WriteLine("|");
            }

            // Нижняя рамка
            Console.SetCursorPosition(1 + startX, 12 + startY);
            Console.WriteLine("*----------*");

        }
        //For debugging draw their alieu ships
        public void DrawShips(int startX, int startY)
        {

            // Живие коробли
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int ship = 0; ship < ships.Length; ship++)
            {
                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    Console.SetCursorPosition(2 + startX + ships[ship].decks[deck].x, 2 + ships[ship].decks[deck].y + startY);
                    Console.Write("#");
                }
            }


            Console.ResetColor();
            // Кординати x
            Console.SetCursorPosition(0 + startX, 0 + startY);
            Console.Write("  ");
            for (int i = 0; i < 10; i++) { Console.Write(i); }

            // Кординати y
            for (int i = 0; i < 10; i++) { Console.SetCursorPosition(0 + startX, 2 + startY + i); Console.Write(i); }

            // Верхняя рамка
            Console.SetCursorPosition(1 + startX, 1 + startY);
            Console.WriteLine("*----------*");

            // Боковая рамка
            for (int i = 0; i < 10; i++)
            {
                Console.SetCursorPosition(1 + startX, 2 + i + startY); Console.WriteLine("|");
                Console.SetCursorPosition(12 + startX, 2 + i + startY); Console.WriteLine("|");
            }

            // Нижняя рамка
            Console.SetCursorPosition(1 + startX, 12 + startY);
            Console.WriteLine("*----------*");

        }





        //bot shoots another bot
        public void ShotBot(BattleField bot)
        {

            //Проверка на потоплений корабль
            ShipsCheck(bot);

            //Проверка на подбитий корабль
            WreckedShipCheck(bot);

            //Проверка на победу
            CheckForVictory(bot);
            if (Win == true) { return; }

            countOfMoves++;


        NewShot:


            // Вистрел уже по стреляному кораблю
            if (aI.currentShip == true)
            {

            NextDir:

                //Вибор кординат и направления
                Random myRand = new Random();

                int myDir = myRand.Next(0, 4);

                int tempX1 = aI.firstShotX;
                int tempY1 = aI.firstShotY;


                //Установка управления
                switch (myDir)
                {
                    case (int)eDirection.up:
                        aI.direction = eDirection.up;
                        break;
                    case (int)eDirection.down:
                        aI.direction = eDirection.down;
                        break;
                    case (int)eDirection.right:
                        aI.direction = eDirection.right;
                        break;
                    case (int)eDirection.left:
                        aI.direction = eDirection.left;
                        break;
                }

                // Проверка на направления
                for (int i = 0; i < aI.invalidDirection.Count; i++)
                {
                    if (aI.direction == aI.invalidDirection[i]) { goto NextDir; }
                }


            //goto
            Shot:


                switch (aI.direction)
                {
                    case eDirection.up:
                        tempY1--;
                        break;
                    case eDirection.down:
                        tempY1++;
                        break;
                    case eDirection.right:
                        tempX1++;
                        break;
                    case eDirection.left:
                        tempX1--;
                        break;
                }

                // Проверка на вистрел в уже стреляемоє + goto
                if (!PointCheck(tempX1, tempY1))
                {
                    aI.invalidDirection.Add(aI.direction);
                    goto NextDir;
                }


                // Добавления простелених кординат
                AddUsingCoordinate(tempX1, tempY1);


                // Проверка всех кораблей на жизнепригодность
                ShipsCheck(bot);


                // Вистрел
                int currentShip = 0;

                if (HitCheck(bot, tempX1, tempY1, ref currentShip))
                {
                    // проверка или живой корабль
                    IsTheShipAlive(currentShip, bot);

                    //Если корабль не живой
                    if (bot.ships[currentShip].Alive == false)
                    {
                        aI.currentShip = false;
                        aI.invalidDirection.Clear();

                        AddShotCoordinatesShip(bot, currentShip);

                        goto NewShot;
                    }
                    //Если живой
                    else { goto Shot; }
                }
                else { aI.invalidDirection.Add(aI.direction); }

            }

            //Новий вистрел
            else
            {

                int randomCount = 0;

            //goto
            TryAgain:

                randomCount++;

                //Вибирае точки
                Random rand = new Random();

                int tempX = rand.Next(0, 10);
                int tempY = rand.Next(0, 10);


                // Защита от вечного цикла + опит бота
                if (randomCount > aI.botComplexity)
                {
                    WreckedShipCheck(bot);
                    if (Win == true) { return; }
                    for (int x = 0; x < SizeX; x++)
                    {
                        for (int y = 0; y < SizeY; y++)
                        {
                            if (CheckNegative(x, y))
                            {
                                tempX = x;
                                tempY = y;
                                randomCount = 0;
                            }
                        }
                    }
                }



                //Проверка на простреляние точки
                if (!PointCheck(tempX, tempY)) { goto TryAgain; }


                // Добавления елементов в постреляние точки
                AddUsingCoordinate(tempX, tempY);



                int wreckedShip = 0;

                // Проверка на попадание
                if (HitCheck(bot, tempX, tempY, ref wreckedShip))
                {
                    aI.firstShotX = tempX;
                    aI.firstShotY = tempY;
                }
                else { return; }


                int dir = rand.Next(0, 4);


            //goto
            NextShot:


                // Проверка на живой корабль у бота
                IsTheShipAlive(wreckedShip, bot);


                //Если корабль потоплен
                if (bot.ships[wreckedShip].Alive == false)
                {
                    //Очищаем AI
                    aI.currentShip = false;
                    aI.invalidDirection.Clear();

                    // Добавляем простреляние кординати
                    AddShotCoordinatesShip(bot, wreckedShip);

                    // Повторяем вистрел
                    goto TryAgain;
                }
                //Если не потоплен
                else
                {
                    aI.currentShip = true;

                    // Перемещаем кординати
                    switch (dir)
                    {
                        case (int)eDirection.up:
                            tempY--;
                            break;
                        case (int)eDirection.down:
                            tempY++;
                            break;
                        case (int)eDirection.right:
                            tempX++;
                            break;
                        case (int)eDirection.left:
                            tempX--;
                            break;
                    }


                    ////////////////////////
                    //Проверка на простреляние точки
                    if (!PointCheck(tempX, tempY))
                    {
                        dir = rand.Next(0, 4);
                        tempX = aI.firstShotX;
                        tempY = aI.firstShotY;

                        goto NextShot;
                    }


                    // Добавления пападающихся кординатов
                    AddUsingCoordinate(tempX, tempY);


                    //Проверка на попадания по даному кораблю
                    for (int i = 0; i < bot.ships[wreckedShip].decks.Length; i++)
                    {
                        if (tempX == bot.ships[wreckedShip].decks[i].x && tempY == bot.ships[wreckedShip].decks[i].y)
                        {
                            if (bot.ships[wreckedShip].decks[i].Alive == false) { continue; }
                            else
                            {
                                bot.ships[wreckedShip].decks[i].Alive = false;

                                goto NextShot;
                            }
                        }
                    }
                }
            }

        }
        // MAN shoots check eOptionShots
        public int ShotMan(BattleField bot, int x, int y)
        {
            //Проверка на подбитий корабль
            WreckedShipCheck(bot);



            //Проверка на потоплений корабль
            ShipsCheck(bot);


            //Проверка на победу
            CheckForVictory(bot);
            if (Win == true) { return (int)eOptionShots.win; }


            countOfMoves++;


            //Проверка на простреляние точки
            if (!PointCheck(x, y)) { return (int)eOptionShots.repeatShot; }


            // Добавления елементов в постреляние точки
            AddUsingCoordinate(x, y);


            // Проверка на попадание
            int wreckedShip = 0;
            if (HitCheck(bot, x, y, ref wreckedShip)) { }
            //Непопали
            else { return (int)eOptionShots.missed; }



            // Проверка на живой корабль у бота
            IsTheShipAlive(wreckedShip, bot);


            //Если корабль потоплен
            if (bot.ships[wreckedShip].Alive == false)
            {
                // Добавляем простреляние кординати
                AddShotCoordinatesShip(bot, wreckedShip);

                // Повторяем вистрел
                return (int)eOptionShots.destruction;
            }
            //Если не потоплен
            else { return (int)eOptionShots.hit; }


        }



        //placement of ships on the battle field
        public void Placement()
        {
            for (int ship = 0; ship < ships.Length; ship++)
            {

            //goto
            Again:

                //Generate x and y and DIRECTION 
                Random rand = new Random();

                int tempX = rand.Next(0, 10);
                int tempY = rand.Next(0, 10);

                int direction = rand.Next(0, 4);


                // Проверка на резервирование поля
                if (CheckReserveCordinate(tempX, tempY) == false) { goto Again; }



                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    if (deck == 0)
                    {
                        ships[ship].decks[deck].x = tempX;
                        ships[ship].decks[deck].y = tempY;
                    }
                    else
                    {
                        switch (direction)
                        {
                            case (int)eDirection.up:
                                tempY--;
                                break;
                            case (int)eDirection.down:
                                tempY++;
                                break;
                            case (int)eDirection.right:
                                tempX++;
                                break;
                            case (int)eDirection.left:
                                tempX--;
                                break;
                        }

                        if (CheckReserveCordinate(tempX, tempY) == false) { goto Again; }
                        else
                        {
                            ships[ship].decks[deck].x = tempX;
                            ships[ship].decks[deck].y = tempY;
                        }
                    }
                }


                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    // корабль
                    AddReserveCordinate(ships[ship].decks[deck].x, ships[ship].decks[deck].y);


                    // вспомогательние координати
                    AddReserveCordinate(ships[ship].decks[deck].x + 1, ships[ship].decks[deck].y);

                    AddReserveCordinate(ships[ship].decks[deck].x - 1, ships[ship].decks[deck].y);

                    AddReserveCordinate(ships[ship].decks[deck].x, ships[ship].decks[deck].y + 1);

                    AddReserveCordinate(ships[ship].decks[deck].x, ships[ship].decks[deck].y - 1);

                    AddReserveCordinate(ships[ship].decks[deck].x + 1, ships[ship].decks[deck].y + 1);

                    AddReserveCordinate(ships[ship].decks[deck].x + 1, ships[ship].decks[deck].y - 1);

                    AddReserveCordinate(ships[ship].decks[deck].x - 1, ships[ship].decks[deck].y + 1);

                    AddReserveCordinate(ships[ship].decks[deck].x - 1, ships[ship].decks[deck].y - 1);

                }

            }


        }






        //ADD reserve coordinate
        private void AddReserveCordinate(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return; }

            reserved[x, y] = true;
        }

        private bool CheckReserveCordinate(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return false; }

            if (reserved[x, y] == true) { return false; }
            else { return true; }
        }






        //Добавить кординати вистрела в наши даниє
        private void AddUsingCoordinate(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return; }

            myShot[x, y] = true;
        }

        // Проверка точки на совпадения с даними о стреляемих координатах + виход за придели
        private bool PointCheck(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return false; }

            if (myShot[x, y] == true) { return false; }

            return true;
        }

        //Проверка на негативний результат
        private bool CheckNegative(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return false; }

            if (myShot[x, y] == false) { return true; }
            else { return false; }
        }

        //Проверка на попадания во вражеский корабль + возвращения номера корабля
        private bool HitCheck(BattleField bot, int x, int y, ref int shipNumber)
        {
            for (int ship = 0; ship < bot.ships.Length; ship++)
            {
                if (bot.ships[ship].Alive == false) { continue; }

                for (int deck = 0; deck < bot.ships[ship].decks.Length; deck++)
                {
                    if (bot.ships[ship].decks[deck].Alive == false) { continue; }

                    // hit
                    if (x == bot.ships[ship].decks[deck].x && y == bot.ships[ship].decks[deck].y)
                    {
                        bot.ships[ship].decks[deck].Alive = false;

                        shipNumber = ship;
                        return true;
                    }
                }
            }

            return false;
        }

        // Проверка живой ли корабль у бота по номеру
        private void IsTheShipAlive(int currShip, BattleField bot)
        {
            bool resultat = false;

            for (int check = 0; check < bot.ships[currShip].decks.Length; check++)
            {
                resultat = resultat || bot.ships[currShip].decks[check].Alive;
            }
            bot.ships[currShip].Alive = resultat;
        }

        // проверка на подбитий корабль у бота проверка на дурака
        private void WreckedShipCheck(BattleField bot)
        {
            for (int ship = 0; ship < bot.ships.Length; ship++)
            {
                if (bot.ships[ship].Alive == false) { continue; }

                for (int deck = 0; deck < bot.ships[ship].decks.Length; deck++)
                {
                    if (bot.ships[ship].decks[deck].Alive == false) { continue; }

                    if (myShot[bot.ships[ship].decks[deck].x, bot.ships[ship].decks[deck].y] == true)
                    {
                        bot.ships[ship].decks[deck].Alive = false;
                    }
                }
            }

            CheckForVictory(bot);
        }

        //Проверка на нашу победу 
        private void CheckForVictory(BattleField bot)
        {
            bool finish = false;
            for (int i = 0; i < bot.ships.Length; i++) { finish = finish || bot.ships[i].Alive; }
            Win = !finish;
        }

        // Проверка на живность кораблей у бота
        private void ShipsCheck(BattleField bot)
        {
            for (int ship = 0; ship < bot.ships.Length; ship++)
            {
                bool res = false;
                for (int check = 0; check < bot.ships[ship].decks.Length; check++)
                {
                    res = res || bot.ships[ship].decks[check].Alive;
                }
                bot.ships[ship].Alive = res;
            }
        }

        // Добавить простреляние кординати корабля у бота
        private void AddShotCoordinatesShip(BattleField bot, int currShip)
        {
            for (int i = 0; i < ships[currShip].decks.Length; i++)
            {
                // вспомогательние координати
                AddUsingCoordinate(bot.ships[currShip].decks[i].x, bot.ships[currShip].decks[i].y);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x + 1, bot.ships[currShip].decks[i].y);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x - 1, bot.ships[currShip].decks[i].y);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x, bot.ships[currShip].decks[i].y + 1);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x, bot.ships[currShip].decks[i].y - 1);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x + 1, bot.ships[currShip].decks[i].y + 1);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x + 1, bot.ships[currShip].decks[i].y - 1);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x - 1, bot.ships[currShip].decks[i].y + 1);

                AddUsingCoordinate(bot.ships[currShip].decks[i].x - 1, bot.ships[currShip].decks[i].y - 1);
            }
        }

    }
}

