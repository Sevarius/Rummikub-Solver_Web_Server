using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using MechanicsModel;
using RumExceptions;

namespace MechanicsModel
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
        /// Список индекс -> кобмбинация
        /// </summary>
        private readonly List<CombinationModel> _combinationList;

        /// <summary>
        /// Словарь фишка -> индекс фишки
        /// </summary>
        private readonly Dictionary<Card, int> _cardMap;

        /// <summary>
        /// Список индекс -> фишка
        /// </summary>
        private readonly List<Card> _cardList;

        /// <summary>
        /// массив [индекс комбинации, индекс фишки] -> количество фишек в комбинации
        /// </summary>
        private int[,] _combinationCardMap;

        /// <summary>
        /// Были ли сгенерированы словари для модели
        /// </summary>
        public bool Generated { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public CombinationsMap()
        {
            _cardMap = new Dictionary<Card, int>();
            _cardList = new List<Card>();
            _combinationMap = new Dictionary<CombinationModel, int>();
            _combinationList = new List<CombinationModel>();
        }

        /// <summary>
        /// Генерирует словари соответсвия фишек, комбинаций и индексов
        /// </summary>
        public void GenerateMaps()
        {
            if (Generated)
            {
                return;
            }

            foreach (var pair in Card.AllPossibleCards().Select((card, i) => new {Card = card, Index = i}))
            {
                _cardList.Add(pair.Card);
                _cardMap.Add(pair.Card, pair.Index);
            }

            foreach (var pair in GenerateCombinations().Select((model, i) => new {Comb = model, Index = i}))
            {
                _combinationList.Add(pair.Comb);
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
        /// Получить индекс фишки
        /// </summary>
        /// <param name="card">Фишка</param>
        /// <returns>int -> индекс фишки</returns>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public int GetCardIndex(Card card)
        {
            if (!Generated)
            {
                throw new RumException(ExceptionType.CombinationMapError01, "Модель не былв сгенерирована");
            }

            return _cardMap[card];
        }

        /// <summary>
        /// Получить список коэффициентов Vj для фишки, где j - индекс комбинации, а Vj - Сколько раз данная фишка встречается в комбинации j
        /// </summary>
        /// <param name="card">Фишка</param>
        /// <returns>List_double -> список коэффициентов для данной фишка</returns>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public List<double> GetCoefficientsForCard(Card card)
        {
            if (!Generated)
            {
                throw new RumException(ExceptionType.CombinationMapError01, "Модель не былв сгенерирована");
            }
            var result = new List<double>();
            int cardIndex = _cardMap[card];
            for (int i = 0; i < _combinationMap.Count; i++)
            {
                result.Add(_combinationCardMap[i, cardIndex]);
            }

            return result;
        }

        /// <summary>
        /// Получить комбинацию по её индексу
        /// </summary>
        /// <param name="combinationIndex">Индекс комбинации</param>
        /// <returns>CombinationModel -> комбинация</returns>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public CombinationModel GetCombinationModelByIndex(int combinationIndex)
        {
            if (!Generated)
            {
                throw new RumException(ExceptionType.CombinationMapError01, "Модель не была сгенерирована");
            }

            return _combinationList[combinationIndex];
        }

        /// <summary>
        /// Получить фишку по индексу
        /// </summary>
        /// <param name="cardIndex">Индекс фишки</param>
        /// <returns>Card -> фишка</returns>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public Card GetCardByIndex(int cardIndex)
        {
            if (!Generated)
            {
                throw new RumException(ExceptionType.CombinationMapError01, "Модель не былв сгенерирована");
            }

            return _cardList[cardIndex];
        }

        /// <summary>
        /// IEnumerable фишек
        /// </summary>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public IEnumerable<Card> Cards
        {
            get
            {
                if (!Generated)
                {
                    throw new RumException(ExceptionType.CombinationMapError01, "Модель не былв сгенерирована");
                }

                return _cardList;
            }
        }

        /// <summary>
        /// IEnumerable комибнаций фишек
        /// </summary>
        /// <exception cref="RumException">Исключение, если словари не были сформированы</exception>
        public IEnumerable<CombinationModel> Combinations
        {
            get
            {
                if (!Generated)
                {
                    throw new RumException(ExceptionType.CombinationMapError01, "Модель не былв сгенерирована");
                }

                return _combinationList;
            }
        }

        /// <summary>
        /// Количество комбинаций
        /// </summary>
        public int CombinationCount => _combinationMap.Count;

        /// <summary>
        /// Количество фишек
        /// </summary>
        public int CardCount => _cardMap.Count;

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
