using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    enum eDirection { up = 0, down, right, left };

    enum eOptionShots { missed = -1, repeatShot = 0, hit = 1, win = 2, destruction = 3 };

    enum eBotShots { missed = 0, hit = 1, destruction = 2 };

    enum eStatus { reserved = -1, empty = 0, shooted = 1, destroyed = 2 };


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

            public uint repeatShots;

            public int firstShotX;
            public int firstShotY;

            public int currShotX;
            public int currShotY;

            public bool rightDirection;
            public eDirection direction;

            public List<eDirection> invalidDirection;
        }
        private AI aI = new AI();


        //information about reserved coordinates
        private bool[,] reserved;


        //information about the coordinates of the shots
        private bool[,] myShot;

        //information about the ALL coordinates
        private int[,] myField;
        private int[,] enemyField;


        //information about the coordinates of the ship
        private BaseShip[] ships;


        //Bot state
        private int botStates;


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
            aI.repeatShots = 20000;

            Win = false;
            countOfMoves = 0;

            //initialize array
            reserved = new bool[SizeX, SizeY];
            myShot = new bool[SizeX, SizeY];
            myField = new int[SizeX, SizeY];
            enemyField = new int[SizeX, SizeY];

            botStates = (int)eBotShots.missed;
        }





        //Рисуем попадания по нам + наши все корабли включая вибитии
        public void Draw(BattleField bot, int startX, int startY)
        {
            AddFullOnMyMatrix(bot);

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Console.SetCursorPosition(2 + startX + x, 2 + startY + y);
                    if (myField[x, y] == (int)eStatus.destroyed)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("X");
                    }
                    else if (myField[x, y] == (int)eStatus.reserved)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("#");
                    }
                    else if (myField[x, y] == (int)eStatus.shooted)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(".");
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
            AddFullOnEnemyMatrix(bot);


            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Console.SetCursorPosition(2 + startX + x, 2 + startY + y);
                    if (enemyField[x, y] == (int)eStatus.destroyed)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("X");
                    }
                    else if (enemyField[x, y] == (int)eStatus.shooted)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(".");
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




        //Bot shoots another bot
        public bool ShotBot(BattleField bot , int freezeTime)
        {

            System.Threading.Thread.Sleep(freezeTime);


            //Проверка на потоплений корабль
            ShipsCheck(bot);

            //Проверка на подбитий корабль
            WreckedShipCheck(bot);

            //Проверка на победу
            CheckForVictory(bot);
            if (Win == true) { return false; }


            if (botStates == (int)eBotShots.missed)
            {
                //Подщет ходов
                countOfMoves++;

                int randomCount = 0;

            //goto
            TryAgain:

                randomCount++;

                //Вибирае точки
                Random rand = new Random();

                int tempX = rand.Next(0, 10);
                int tempY = rand.Next(0, 10);


                // Защита от вечного цикла
                if (randomCount > aI.repeatShots)
                {
                    WreckedShipCheck(bot);
                    if (Win == true) { return false; }
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
                else
                {
                    botStates = (int)eBotShots.missed;
                    return false;
                }


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


                    botStates = (int)eBotShots.destruction;
                }
                //Если не потоплен
                else
                {
                    aI.currentShip = true;

                    aI.firstShotX = tempX;
                    aI.firstShotY = tempY;

                    aI.currShotX = tempX;
                    aI.currShotY = tempY;


                    botStates = (int)eBotShots.hit;
                }

                return true;
            }
            else if(botStates == (int)eBotShots.destruction)
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
                if (randomCount > aI.repeatShots)
                {
                    WreckedShipCheck(bot);
                    if (Win == true) { return false; }
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
                else 
                {
                    botStates = (int)eBotShots.missed;
                    return false;
                }


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


                    botStates = (int)eBotShots.destruction;
                    return true;

                }
                //Если не потоплен
                else
                {
                    aI.currentShip = true;

                    aI.firstShotX = tempX;
                    aI.firstShotY = tempY;

                    aI.currShotX = tempX;
                    aI.currShotY = tempY;


                    botStates = (int)eBotShots.hit;
                    return true;
                }


            }
            else if (botStates == (int)eBotShots.hit)
            {
                if (aI.rightDirection == true)
                {
                    int tempX = aI.currShotX;
                    int tempY = aI.currShotY;


                    switch (aI.direction)
                    {
                        case eDirection.up:
                            tempY--;
                            break;
                        case eDirection.down:
                            tempY++;
                            break;
                        case eDirection.right:
                            tempX++;
                            break;
                        case eDirection.left:
                            tempX--;
                            break;
                    }


                    // Проверка на вистрел в уже стреляемоє + инвертация координат
                    if (!PointCheck(tempX, tempY))
                    {
                        //Улучения ИИ
                        aI.currShotX = aI.firstShotX;
                        aI.currShotY = aI.firstShotY;

                        switch (aI.direction)
                        {
                            case eDirection.up:
                                aI.direction = eDirection.down;
                                break;
                            case eDirection.down:
                                aI.direction = eDirection.up;
                                break;
                            case eDirection.right:
                                aI.direction = eDirection.left;
                                break;
                            case eDirection.left:
                                aI.direction = eDirection.right;
                                break;
                        }

                        botStates = (int)eBotShots.hit;
                        return true;
                    }


                    // Добавления простелених кординат
                    AddUsingCoordinate(tempX, tempY);


                    // Проверка всех кораблей на жизнепригодность
                    ShipsCheck(bot);


                    // Вистрел
                    int currentShip = 0;

                    if (HitCheck(bot, tempX, tempY, ref currentShip))
                    {
                        // проверка или живой корабль
                        IsTheShipAlive(currentShip, bot);

                        //Если корабль не живой
                        if (bot.ships[currentShip].Alive == false)
                        {
                            aI.currentShip = false;
                            aI.invalidDirection.Clear();

                            AddShotCoordinatesShip(bot, currentShip);

                            aI.rightDirection = false;
                            botStates = (int)eBotShots.destruction;
                            return true;
                        }
                        //Если живой
                        else
                        {
                            aI.rightDirection = true;

                            aI.currShotX = tempX;
                            aI.currShotY = tempY;

                            botStates = (int)eBotShots.hit;
                            return true;
                        }
                    }
                    else
                    {
                        //Улучения ИИ
                        aI.currShotX = aI.firstShotX;
                        aI.currShotY = aI.firstShotY;

                        switch (aI.direction)
                        {
                            case eDirection.up:
                                aI.direction = eDirection.down;
                                break;
                            case eDirection.down:
                                aI.direction = eDirection.up;
                                break;
                            case eDirection.right:
                                aI.direction = eDirection.left;
                                break;
                            case eDirection.left:
                                aI.direction = eDirection.right;
                                break;
                        }

                        botStates = (int)eBotShots.hit;
                        return false;
                    }
                }
                else
                {

                //goto
                NextDir:


                    //Вибор кординат и направления
                    Random random = new Random();

                    int myDir = random.Next(0, 4);

                    int tempX = aI.firstShotX;
                    int tempY = aI.firstShotY;


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


                    switch (aI.direction)
                    {
                        case eDirection.up:
                            tempY--;
                            break;
                        case eDirection.down:
                            tempY++;
                            break;
                        case eDirection.right:
                            tempX++;
                            break;
                        case eDirection.left:
                            tempX--;
                            break;
                    }


                    // Проверка на вистрел в уже стреляемоє + goto
                    if (!PointCheck(tempX, tempY))
                    {
                        aI.invalidDirection.Add(aI.direction);
                        goto NextDir;
                    }


                    // Добавления простелених кординат
                    AddUsingCoordinate(tempX, tempY);


                    // Проверка всех кораблей на жизнепригодность
                    ShipsCheck(bot);


                    // Вистрел
                    int currentShip = 0;

                    if (HitCheck(bot, tempX, tempY, ref currentShip))
                    {
                        // проверка или живой корабль
                        IsTheShipAlive(currentShip, bot);

                        //Если корабль не живой
                        if (bot.ships[currentShip].Alive == false)
                        {
                            aI.currentShip = false;
                            aI.invalidDirection.Clear();

                            AddShotCoordinatesShip(bot, currentShip);

                            aI.rightDirection = false;
                            botStates = (int)eBotShots.destruction;
                            return true;
                        }
                        //Если живой
                        else
                        {
                            aI.rightDirection = true;

                            aI.currShotX = tempX;
                            aI.currShotY = tempY;

                            botStates = (int)eBotShots.hit;
                            return true;
                        }
                    }
                    else { aI.invalidDirection.Add(aI.direction); }


                    botStates = (int)eBotShots.hit;
                    return false;

                }
            }


            //На всякий случай
            botStates = (int)eBotShots.missed;
            return false;
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



        //Занести корабли на матрицу 
        private void AddFullOnMyMatrix(BattleField bot)
        {

            // Живие коробли
            for (int ship = 0; ship < ships.Length; ship++)
            {
                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    if (ships[ship].decks[deck].Alive == true)
                    {
                        myField[ships[ship].decks[deck].x, ships[ship].decks[deck].y] = (int)eStatus.reserved;
                    }
                }
            }


            //Попадания
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (bot.myShot[x, y] == true)
                    {
                        myField[x, y] = (int)eStatus.shooted;
                    }
                }
            }


            // Мертвиє коробли
            for (int ship = 0; ship < ships.Length; ship++)
            {
                for (int deck = 0; deck < ships[ship].decks.Length; deck++)
                {
                    if (ships[ship].decks[deck].Alive == false)
                    {
                        myField[ships[ship].decks[deck].x, ships[ship].decks[deck].y] = (int)eStatus.destroyed;
                    }
                }
            }

        }
        private void AddFullOnEnemyMatrix(BattleField bot)
        {
            //Попадания
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (myShot[x, y] == true)
                    {
                        enemyField[x, y] = (int)eStatus.shooted;
                    }
                }
            }

            // Вибитие корабли
            for (int ship = 0; ship < bot.ships.Length; ship++)
            {
                for (int deck = 0; deck < bot.ships[ship].decks.Length; deck++)
                {
                    if (bot.ships[ship].decks[deck].Alive == false)
                    {
                        enemyField[bot.ships[ship].decks[deck].x, bot.ships[ship].decks[deck].y] = (int)eStatus.destroyed;
                    }
                }
            }

        }




        //ADD reserve coordinate
        private void AddReserveCordinate(int x, int y)
        {
            if (!(x < 10 && x >= 0 && y < 10 && y >= 0)) { return; }

            reserved[x, y] = true;
        }

        //Check reserve coordinate
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

