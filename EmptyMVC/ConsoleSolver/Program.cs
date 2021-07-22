using System;
using System.Collections.Generic;
using System.Linq;
using MathModel;
using MechanicsModel;

namespace ConsoleSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new CombinationsMap();
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);
            Console.WriteLine("Начало генерации модели");
            var start = DateTime.Now;
            map.GenerateMaps();
            Console.WriteLine($"Модель была сгенерирована за {(DateTime.Now - start).Seconds} секунд");
            Console.WriteLine("Если вы хотите прекратить выполнение программы, наберите строку \"exit\"");
            while (true)
            {
                bool exitFlag = false;
                Console.WriteLine("Введите комбинации, каждую на своей строке.\nПри окончании введите строку \"end\"");
                var tableCombinations = new List<CombinationModel>();
                string consoleString = Console.ReadLine();
                while (consoleString != "end")
                {
                    if (consoleString == "exit")
                    {
                        exitFlag = true;
                        break;
                    }
                    var currentCombination = converter.StringToCombination(consoleString);
                    tableCombinations.Add(currentCombination);
                    consoleString = Console.ReadLine();
                }

                if (exitFlag)
                {
                    break;
                }

                Console.WriteLine("Введите длинную строку из всех фишек, которые есть у вас на руке, через пробел");
                consoleString = Console.ReadLine();
                if (consoleString == "exit")
                {
                    break;
                }

                var handCards = consoleString.Split(" ").ToList();
                var hand = new List<Card>(handCards.Count);
                handCards.ForEach(c => hand.Add(converter.StringToCard(c)));

                var model = new MathProblem(new GameModel(){Hand = hand, Table = tableCombinations}, map);
                Console.WriteLine("Решение задачи");

                (var combinationsOnTable, var cardsToPutFromHand, var objValue) = model.Solve();
                if (combinationsOnTable is null || cardsToPutFromHand is null)
                {
                    Console.WriteLine("Задача не была решена корректно");
                }
                else
                {
                    Console.WriteLine("Задача была решена корректно");
                    Console.WriteLine($"Значение целевой функции: {objValue}");
                    Console.WriteLine($"{combinationsOnTable.Count} комбинаций на столе\n{cardsToPutFromHand.Count} фишек надо выложить с руки");
                    Console.WriteLine("Комбинации:");
                    combinationsOnTable.ForEach(c => Console.WriteLine(c.ToStringRaw()));
                    Console.WriteLine("Фишки:");
                    cardsToPutFromHand.ForEach(c => Console.WriteLine(c.ToString()));
                }

                Console.WriteLine("\n");
            }

            Console.WriteLine("Спасибо за использование программой");
        }
    }
}