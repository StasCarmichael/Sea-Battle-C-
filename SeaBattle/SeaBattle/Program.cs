using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    class Program
    {
        static void Main()
        {
            //Установка коректной кодировки
            Console.OutputEncoding = System.Text.Encoding.Unicode;


            //Установка коректной консоли
            Console.SetBufferSize(120, 60);

            //Установка задержки
            const int FREEZETIME = 500;


        //goto
        Again:

            Console.Clear();

            int mode = 0;
            bool options = true;

            //Вибор режима игри
            while (options)
            {
                try
                {
                    Console.SetCursorPosition(25, 0);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("GAME MODE SELECTION");

                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(15, 2);
                    Console.WriteLine("Press 1 and Enter 2 BOT");
                    Console.SetCursorPosition(15, 3);
                    Console.WriteLine("Press 2 and Enter 1 BOT and 1 PLAYER");
                    Console.SetCursorPosition(15, 4);
                    Console.WriteLine("Press 3 and Enter 2 PLAYER");
                    Console.ResetColor();

                    Console.SetCursorPosition(15, 6);
                    mode = int.Parse(Console.ReadLine());
                    options = false;
                }
                catch
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.SetCursorPosition(5, 7);
                    Console.Write("Enter data right!!! Press ENTER if you want play again .");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                }
            }


            Console.Clear();


            // Реализация режимов игри
            switch (mode)
            {
                case 1:
                    {

                        BattleField chuck = new BattleField();
                        BattleField morgan = new BattleField();


                        chuck.Placement();
                        morgan.Placement();


                        DrawInterfaceForTwoBots();

                        //Виводим названия битви
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(39, 1);
                            Console.WriteLine("BATTLE 2 BOTS");
                            Console.ResetColor();
                        }

                        while (!(chuck.Win || morgan.Win))
                        {

                            //Отрисовка кораблей
                            {
                                //корабли и вистрели по чаку
                                chuck.Draw(morgan, 5, 8);
                                chuck.DrawField(morgan, 25, 8);

                                //корабли и вистрели по моргану
                                morgan.Draw(chuck, 50, 8);
                                morgan.DrawField(chuck, 70, 8);
                            }

                            //Вистрел Чака по Моргану
                            while (chuck.ShotBot(morgan, FREEZETIME))
                            {
                                //корабли и вистрели по чаку
                                chuck.Draw(morgan, 5, 8);
                                chuck.DrawField(morgan, 25, 8);

                                //корабли и вистрели по моргану
                                morgan.Draw(chuck, 50, 8);
                                morgan.DrawField(chuck, 70, 8);
                            }


                            //Проверка на бистрий виграш
                            if (chuck.Win || morgan.Win) { break; }


                            //Отрисовка кораблей
                            {
                                //корабли и вистрели по чаку
                                chuck.Draw(morgan, 5, 8);
                                chuck.DrawField(morgan, 25, 8);

                                //корабли и вистрели по моргану
                                morgan.Draw(chuck, 50, 8);
                                morgan.DrawField(chuck, 70, 8);
                            }

                            //Вистрел Моргана по Чаку
                            while (morgan.ShotBot(chuck, FREEZETIME))
                            {
                                //корабли и вистрели по чаку
                                chuck.Draw(morgan, 5, 8);
                                chuck.DrawField(morgan, 25, 8);

                                //корабли и вистрели по моргану
                                morgan.Draw(chuck, 50, 8);
                                morgan.DrawField(chuck, 70, 8);
                            }


                            //Отрисоввка номера хода
                            {
                                Console.SetCursorPosition(42, 4);
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("Move " + (chuck.countOfMoves));
                                Console.ResetColor();
                            }

                        }

                        //Last shot
                        {
                            //корабли и вистрели по чаку
                            chuck.Draw(morgan, 5, 8);
                            chuck.DrawField(morgan, 25, 8);


                            //корабли и вистрели по моргану
                            morgan.Draw(chuck, 50, 8);
                            morgan.DrawField(chuck, 70, 8);
                        }


                        //Виводим результат стражения
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(35, 22);
                            Console.WriteLine("Количество ходов " + chuck.countOfMoves);
                            Console.SetCursorPosition(35, 23);
                            if (chuck.Win == true) { Console.WriteLine("CHUCK WINS"); }
                            else { Console.WriteLine("MORGAN WINS"); }
                            Console.ResetColor();
                        }


                        break;
                    }

                case 2:
                    {
                        BattleField bot = new BattleField();
                        BattleField player = new BattleField();

                        bot.Placement();
                        player.Placement();


                        DrawInterfaceForOnePlayer();
                        DrawInterfaceBattleSummary();

                        //Виводим названия битви
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(16, 1);
                            Console.WriteLine("BATTLE of AI and HUMAN");
                            Console.ResetColor();
                        }

                        int playerX = 0;
                        int playerY = 0;

                        int myResult = 0;

                        //Gebuging console
                        //bot.DrawShips(65, 8);


                        while (!(bot.Win || player.Win))
                        {

                            player.Draw(bot, 5, 8);
                            player.DrawField(bot, 35, 8);


                            //Отрисоввка номера хода
                            {
                                Console.SetCursorPosition(24, 4);
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("Move " + (bot.countOfMoves));
                                Console.ResetColor();
                            }


                        //goto
                        TryShot:


                            playerX = GetCorectX();
                            playerY = GetCorectY();

                            myResult = player.ShotMan(bot, playerX, playerY);

                            switch (myResult)
                            {
                                case (int)eOptionShots.hit:
                                    {
                                        player.Draw(bot, 5, 8);
                                        player.DrawField(bot, 35, 8);

                                        DrawBattleSummary("You hit the ship");

                                        goto TryShot;
                                    }

                                case (int)eOptionShots.repeatShot:
                                    {
                                        DrawBattleSummary("Нou already shot here", "Repeat shot please");

                                        goto TryShot;
                                    }

                                case (int)eOptionShots.destruction:
                                    {
                                        player.Draw(bot, 5, 8);
                                        player.DrawField(bot, 35, 8);

                                        DrawBattleSummary("You destroyed the ship", "Congratulations !!!");

                                        goto TryShot;
                                    }

                                case (int)eOptionShots.win:
                                    {
                                        DrawBattleSummary("You WIN !!!");

                                        break;
                                    }

                                case (int)eOptionShots.missed:
                                    {
                                        player.Draw(bot, 5, 8);
                                        player.DrawField(bot, 35, 8);

                                        DrawBattleSummary("You missed");

                                        break;
                                    }


                            }


                            //Проверка на бистрий виграш
                            if (bot.Win || player.Win) { break; }


                            //Вистрел бота
                            while (bot.ShotBot(player, FREEZETIME))
                            {
                                player.Draw(bot, 5, 8);
                                player.DrawField(bot, 35, 8);
                            }


                        }


                        player.Draw(bot, 5, 8);
                        player.DrawField(bot, 35, 8);

                        bot.DrawShips(65, 8);

                        //Отрисовка результата
                        {
                            if (player.Win == true) { DrawBattleSummary("YOU WIN"); }
                            else { DrawBattleSummary("YOU LOSE"); }
                        }


                        break;
                    }

                case 3:
                    {

                        BattleField player1 = new BattleField();
                        BattleField player2 = new BattleField();

                        player1.Placement();
                        player2.Placement();


                        DrawInterfaceForTwoPlayer();
                        DrawInterfaceBattleSummary();


                        //Виводим названия битви
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(18, 1);
                            Console.WriteLine("BATTLE of 2 PEOPLE");
                            Console.ResetColor();
                        }


                        int playerX = 0;
                        int playerY = 0;

                        int myResult = 0;


                        int move = 0;
                        while (!(player1.Win || player2.Win))
                        {

                            //Отрисовка номера хода
                            {
                                Console.SetCursorPosition(24, 4);
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                Console.ForegroundColor = ConsoleColor.Black;
                                Console.Write("Move " + (move));
                                Console.ResetColor();
                            }


                        TryShot1:

                            //Отрисовка имени
                            Console.SetCursorPosition(23, 20);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("Player 1");
                            Console.ResetColor();



                            player2.DrawField(player1, 5, 8);
                            player1.DrawField(player2, 35, 8);


                            playerX = GetCorectX();
                            playerY = GetCorectY();

                            myResult = player1.ShotMan(player2, playerX, playerY);

                            switch (myResult)
                            {
                                case (int)eOptionShots.hit:
                                    {
                                        DrawBattleSummary("Player 1", "You hit the ship", "");

                                        goto TryShot1;
                                    }

                                case (int)eOptionShots.repeatShot:
                                    {
                                        DrawBattleSummary("Player 1", "Нou already shot here", "Repeat shot please");

                                        goto TryShot1;
                                    }

                                case (int)eOptionShots.destruction:
                                    {
                                        DrawBattleSummary("Player 1", "You destroyed the ship", "Congratulations !!!");

                                        goto TryShot1;
                                    }

                                case (int)eOptionShots.win:
                                    {
                                        DrawBattleSummary("Player 1", "You WIN !!!", "");

                                        break;
                                    }

                                case (int)eOptionShots.missed:
                                    {
                                        DrawBattleSummary("Player 1", "You missed", "");

                                        break;
                                    }


                            }




                            //Проверка на бистрий виграш
                            if (player1.Win || player2.Win) { break; }


                        TryShot2:


                            //Отрисовка имени
                            Console.SetCursorPosition(23, 20);
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("Player 2");
                            Console.ResetColor();


                            player2.DrawField(player1, 5, 8);
                            player1.DrawField(player2, 35, 8);


                            playerX = GetCorectX();
                            playerY = GetCorectY();

                            myResult = player2.ShotMan(player1, playerX, playerY);

                            switch (myResult)
                            {
                                case (int)eOptionShots.hit:
                                    {
                                        DrawBattleSummary("Player 2", "You hit the ship", "");

                                        goto TryShot2;
                                    }

                                case (int)eOptionShots.repeatShot:
                                    {
                                        DrawBattleSummary("Player 2", "Нou already shot here", "Repeat shot please");

                                        goto TryShot2;
                                    }

                                case (int)eOptionShots.destruction:
                                    {
                                        DrawBattleSummary("Player 2", "You destroyed the ship", "Congratulations !!!");

                                        goto TryShot2;
                                    }

                                case (int)eOptionShots.win:
                                    {
                                        DrawBattleSummary("Player 2", "You WIN !!!", "");

                                        break;
                                    }

                                case (int)eOptionShots.missed:
                                    {
                                        DrawBattleSummary("Player 2", "You missed", "");

                                        break;
                                    }


                            }


                            move++;
                        }

                        player2.DrawField(player1, 5, 8);
                        player1.DrawField(player2, 35, 8);


                        // Узнать растановку кораблей
                        player1.DrawShips(60, 8);
                        player2.DrawShips(85, 8);


                        //Отрисовка результата
                        {
                            if (player1.Win == true) { DrawBattleSummary("PLAYER 1 WIN"); }
                            else { DrawBattleSummary("PLAYER 2 WIN"); }
                        }

                        break;
                    }

                default:
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.SetCursorPosition(25, 1);
                        Console.WriteLine("You made a wrong !!!");
                        Console.SetCursorPosition(15, 2);
                        Console.Write($"If you want to play again write AGAIN :");

                        string str = Console.ReadLine();
                        if (str == "AGAIN") { Console.ResetColor(); goto Again; }

                        Console.ResetColor();

                        break;
                    }

            }


            Console.SetCursorPosition(0, 22);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("If you want exit press any key : ");
            Console.ReadKey();
            Console.ResetColor();
        }





        //Отрисовка интерфейса для 2 ботов
        static void DrawInterfaceForTwoBots()
        {
            //Отрисовка верхней линии
            Console.SetCursorPosition(0, 3);
            for (int j = 0; j < 88; j++) { Console.Write("="); }

            //Отрисовка нижней линии
            Console.SetCursorPosition(0, 21);
            for (int j = 0; j < 88; j++) { Console.Write("="); }

            //Отрисовка Бокових линий
            for (int j = 0; j < 17; j++)
            {
                Console.SetCursorPosition(0, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(87, 4 + j);
                Console.Write("|");
            }

            //Отрисовка Центральние линий
            for (int j = 0; j < 15; j++)
            {
                Console.SetCursorPosition(44, 6 + j);
                Console.Write("|");
                Console.SetCursorPosition(45, 6 + j);
                Console.Write("|");
            }

            //Отрисовка окошка для ходов
            for (int j = 0; j < 12; j++)
            {
                Console.SetCursorPosition(39 + j, 5);
                Console.Write("=");
            }
            for (int j = 0; j < 1; j++)
            {
                Console.SetCursorPosition(39, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(50, 4 + j);
                Console.Write("|");
            }

            // Отрисовка имен ботов
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(8, 6);
            Console.Write("Bot CHUCK");
            Console.SetCursorPosition(52, 6);
            Console.Write("Bot MORGAN");

            Console.ResetColor();
        }
        //Отрисовка интерфейса для игрока і 1 бота
        static void DrawInterfaceForOnePlayer()
        {
            //Отрисовка верхней линии
            Console.SetCursorPosition(0, 3);
            for (int j = 0; j < 53; j++) { Console.Write("="); }

            //Отрисовка нижней линии
            Console.SetCursorPosition(0, 21);
            for (int j = 0; j < 53; j++) { Console.Write("="); }

            //Отрисовка Бокових линий
            for (int j = 0; j < 17; j++)
            {
                Console.SetCursorPosition(0, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(52, 4 + j);
                Console.Write("|");
            }



            //Отрисовка окошка для ходов
            for (int j = 0; j < 12; j++)
            {
                Console.SetCursorPosition(21 + j, 5);
                Console.Write("=");
            }
            for (int j = 0; j < 1; j++)
            {
                Console.SetCursorPosition(21, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(32, 4 + j);
                Console.Write("|");
            }

            // Отрисовка имен ботов
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(8, 6);
            Console.Write("Player 1");
            Console.SetCursorPosition(38, 6);
            Console.Write("Bot CHUCK");

            Console.ResetColor();
        }
        // Отрисовка интерфейса для 2 игроков
        static void DrawInterfaceForTwoPlayer()
        {
            //Отрисовка верхней линии
            Console.SetCursorPosition(0, 3);
            for (int j = 0; j < 53; j++) { Console.Write("="); }

            //Отрисовка нижней линии
            Console.SetCursorPosition(0, 21);
            for (int j = 0; j < 53; j++) { Console.Write("="); }

            //Отрисовка Бокових линий
            for (int j = 0; j < 17; j++)
            {
                Console.SetCursorPosition(0, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(52, 4 + j);
                Console.Write("|");
            }



            //Отрисовка окошка для ходов
            for (int j = 0; j < 12; j++)
            {
                Console.SetCursorPosition(21 + j, 5);
                Console.Write("=");
            }
            for (int j = 0; j < 1; j++)
            {
                Console.SetCursorPosition(21, 4 + j);
                Console.Write("|");
                Console.SetCursorPosition(32, 4 + j);
                Console.Write("|");
            }

            // Отрисовка имен ботов
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(8, 6);
            Console.Write("Player 1");
            Console.SetCursorPosition(38, 6);
            Console.Write("Player 2");

            Console.ResetColor();
        }



        //Отрисовка интерфейса сводки боя
        static void DrawInterfaceBattleSummary()
        {
            //Все прямие линии
            Console.SetCursorPosition(53, 21);
            for (int j = 0; j < 30; j++) { Console.Write("="); }

            Console.SetCursorPosition(53, 23);
            for (int j = 0; j < 30; j++) { Console.Write("="); }

            Console.SetCursorPosition(53, 27);
            for (int j = 0; j < 30; j++) { Console.Write("="); }


            //Боковие перегородки
            for (int j = 0; j < 5; j++)
            {
                Console.SetCursorPosition(53, 22 + j);
                Console.Write("|");
                Console.SetCursorPosition(82, 22 + j);
                Console.Write("|");
            }

            // Отрисовка имен ботов
            Console.SetCursorPosition(61, 22);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Battle Summary");
            Console.ResetColor();
        }



        //Отрисовка сводки боя
        static void DrawBattleSummary(string who, string message1, string message2)
        {
            Console.SetCursorPosition(54, 24);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 25);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 26);
            Console.Write("                        ");


            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(64, 24);
            Console.Write(who);
            Console.ResetColor();


            Console.SetCursorPosition(54, 25);
            Console.Write(message1);
            Console.SetCursorPosition(54, 26);
            Console.Write(message2);
        }
        static void DrawBattleSummary(string message1, string message2)
        {
            Console.SetCursorPosition(54, 24);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 25);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 24);
            Console.Write(message1);
            Console.SetCursorPosition(54, 25);
            Console.Write(message2);
        }
        static void DrawBattleSummary(string message1)
        {
            Console.SetCursorPosition(54, 24);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 25);
            Console.Write("                        ");
            Console.SetCursorPosition(54, 24);
            Console.Write(message1);
        }



        //Защита от ввода некоректних даних
        static int GetCorectX()
        {
            int tempX = 0;
            bool option = true;

            while (option)
            {
                try
                {
                    Console.SetCursorPosition(0, 22);
                    Console.WriteLine("                                                ");
                    Console.SetCursorPosition(0, 22);
                    Console.Write("Enter X coordinate : ");
                    tempX = int.Parse(Console.ReadLine());
                    option = false;

                    if (!(tempX >= 0 && tempX < 10)) { option = true; }
                }
                catch
                {
                    Console.SetCursorPosition(0, 22);
                    Console.WriteLine();
                }
            }

            return tempX;
        }
        static int GetCorectY()
        {
            int tempY = 0;

            bool option = true;

            while (option)
            {
                try
                {
                    Console.SetCursorPosition(0, 22);
                    Console.WriteLine("                                                ");
                    Console.SetCursorPosition(0, 22);
                    Console.Write("Enter Y coordinate : ");
                    tempY = int.Parse(Console.ReadLine());
                    option = false;

                    if (!(tempY >= 0 && tempY < 10)) { option = true; }
                }
                catch
                {
                    Console.SetCursorPosition(0, 22);
                    Console.WriteLine();
                }
            }


            return tempY;
        }
    }
}

