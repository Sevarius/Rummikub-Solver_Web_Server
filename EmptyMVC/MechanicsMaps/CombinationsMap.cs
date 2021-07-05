﻿using System.Collections.Generic;
using System.Linq;
using MechanicsModel;

namespace MechanicsMaps
{
    /// <summary>
    /// Модель соответствия фишек и доступных комбинаций
    /// </summary>
    public class CombinationsMap
    {
        /// <summary>
        /// Словарь комбинация -> индекс комбинации
        /// </summary>
        private readonly Dictionary<CombinationModel, int> _combinationMap;
        /// <summary>
        /// Словарь фишка -> индекс фишки
        /// </summary>
        private readonly Dictionary<Card, int> _cardMap;
        /// <summary>
        /// массив [индекс комбинации, индекс фишки] -> количество фишек в комбинации
        /// </summary>
        private int[,] _combinationCardMap;

        /// <summary>
        /// Были ли сгенерированы словари для модели
        /// </summary>
        public bool Generated { get; private set; }

        public CombinationsMap()
        {
            _cardMap = new Dictionary<Card, int>();
            _combinationMap = new Dictionary<CombinationModel, int>();
        }

        public void GenerateMaps()
        {
            if (Generated)
            {
                return;
            }

            foreach (var pair in Card.AllPossibleCards().Select((card, i) => new {Card = card, Index = i}))
            {
                _cardMap.Add(pair.Card, pair.Index);
            }

            foreach (var pair in GenerateCombinations().Select((model, i) => new {Comb = model, Index = i}))
            {
                _combinationMap.Add(pair.Comb, pair.Index);
            }

            _combinationCardMap = new int[_combinationMap.Count, _cardMap.Count];

            foreach (var model in _combinationMap.Keys)
            {
                foreach (var card in model)
                {
                    _combinationCardMap[_combinationMap[model], _cardMap[card]] = model.CountCard(card);
                } 
            }

            Generated = true;
        }

        /// <summary>
        /// Генерирует список всех валидных комбинаций длины 3, 4 и 5
        /// </summary>
        /// <returns>Список валидных комбинаций длины 3, 4 и 5 без повторений</returns>
        internal List<CombinationModel> GenerateCombinations()
        {
            var all3LengthValidCombinations = GetOnlyValidCombinations(GenerateAll3LengthVariations());

            var all4LengthValidCombinations = GetOnlyValidCombinations(GetAllVariations(all3LengthValidCombinations));

            var all5LengthValidCombinations = GetOnlyValidCombinations(GetAllVariations(all4LengthValidCombinations));

            var allCombinations = new List<CombinationModel>();
            allCombinations.AddRange(all3LengthValidCombinations);
            allCombinations.AddRange(all4LengthValidCombinations);
            allCombinations.AddRange(all5LengthValidCombinations);

            allCombinations = RemoveColorDuplicationCombinations(allCombinations);

            return allCombinations;
        }

        /// <summary>
        /// Создаёт список всех возможных комбинаций из текущей комбинации путём добавления одной фишки
        /// </summary>
        /// <param name="combinations">текущая комбинация (может быть пустой)</param>
        /// <returns>Список всех возможных комбинаций</returns>
        internal List<CombinationModel> GetAllVariations(List<CombinationModel> combinations)
        {
            var result = new List<CombinationModel>();

            foreach (CombinationModel combination in combinations)
            {
                for (var number = 1; number <= 13; number++)
                {
                    foreach (CardColor color in new[] { CardColor.Red, CardColor.Yellow, CardColor.Black, CardColor.Blue })
                    {
                        List<Card> newList = combination.Cards.ToList();
                        newList.Add(new Card(color, number));
                        result.Add(new CombinationModel(newList));
                    }
                }

                List<Card> newListForJoker = combination.Cards.ToList();
                newListForJoker.Add(new Card(CardColor.Joker, 0));
                result.Add(new CombinationModel(newListForJoker));
            }

            return result;
        }

        /// <summary>
        /// Генерация всех возможных комбинаций длины 3
        /// </summary>
        /// <returns>Список всех возможных комбинаций длины 3</returns>
        internal List<CombinationModel> GenerateAll3LengthVariations()
        {
            var firstCombination = new List<CombinationModel>() {new CombinationModel(new List<Card>())};
            return GetAllVariations(GetAllVariations(GetAllVariations(firstCombination)));
        }

        /// <summary>
        /// Фильтрация только валидных комбинаций
        /// </summary>
        /// <param name="combinations">Список комбинаций</param>
        /// <returns>Новый список с только валидными комбинациями</returns>
        internal List<CombinationModel> GetOnlyValidCombinations(List<CombinationModel> combinations)
        {
            var checker = new CombinationChecker();

            var validCombinations = new List<CombinationModel>();
            foreach (CombinationModel combination in combinations)
            {
                try
                {
                    (combination.isValid, combination.Type) = checker.CheckCombination(combination);
                }
                catch
                {
                    continue;
                }

                if (combination.isValid)
                {
                    validCombinations.Add(combination);
                }
            }

            return validCombinations;
        }

        /// <summary>
        /// Убирает повторения из списка комбинаций. Комбинации по значению сортирует по цвету и убирает одинаковые
        /// </summary>
        /// <param name="combinations">Список комбинаций</param>
        /// <returns>Новый список комбинаций без повторений</returns>
        internal List<CombinationModel> RemoveColorDuplicationCombinations(List<CombinationModel> combinations)
        {
            foreach (CombinationModel combination in combinations)
            {
                if (combination.Type == CombinationType.Value)
                {
                    combination.Cards.Sort();
                }
            }

            return new List<CombinationModel>(new HashSet<CombinationModel>(combinations));
        }
    }
}
