using System.Collections.Generic;
using System.Diagnostics;
using MechanicsModel;
using Xunit;
using Xunit.Sdk;

namespace EmptyMVC.Tests
{
    public sealed class CardModelTests
    {
        [Theory]
        [ClassData(typeof(GoodCombinations))]
        public void CheckGoodCombinations(CombinationType combinationType, CombinationModel combination)
        {
            var checker = new CombinationChecker();
            var (res, type) = checker.CheckCombination(combination);
            try
            {
                Assert.True(res);
                Assert.Equal(combinationType, type);
            }
            catch (AssertActualExpectedException)
            {
                Trace.WriteLine(combination);
                Trace.WriteLine(combinationType);
                throw;
            }
        }

        [Theory]
        [ClassData(typeof(BadCombinations))]
        public void CheckBadCombinations(CombinationModel combination)
        {
            var checker = new CombinationChecker();
            bool res;
            var type = CombinationType.Unknown;

            try
            {
                (res, type) = checker.CheckCombination(combination);
            }
            catch
            {
                res = false;
            }

            Assert.False(res);
        }

        [Fact]
        public void CardSortTest()
        {
            var listToSort = new List<Card>()
            {
                new Card(CardColor.Red, 1),
                new Card(CardColor.Joker, 0),
                new Card(CardColor.Blue, 1),
                new Card(CardColor.Joker, 0)
            };

            listToSort.Sort();

            Assert.Equal(CardColor.Red, listToSort[0].Color);
            Assert.Equal(CardColor.Blue, listToSort[1].Color);
            Assert.Equal(CardColor.Joker, listToSort[2].Color);
            Assert.Equal(CardColor.Joker, listToSort[3].Color);
        }
    }

    public class GoodCombinations : TheoryData<CombinationType, CombinationModel>
    {
        public GoodCombinations()
        {
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);

            var testsList = new List<(CombinationType, string)>()
            {
                (CombinationType.Color, "1r 2r 3r"),
                (CombinationType.Color, "1bb 2bb 3bb 4bb 5bb 6bb 7bb 8bb 9bb 10bb 11bb 12bb 13bb"),
                (CombinationType.Color, "2r j 4r"),
                (CombinationType.Color, "j 2r 3r 4r j"),

                (CombinationType.Value, "3r 3y 3b 3bb"),
                (CombinationType.Value, "1r j j"),
                (CombinationType.Value, "j j 3b"),
                (CombinationType.Value, "11y j j"),
                (CombinationType.Value, "j 12b j"),
                (CombinationType.Value, "j j 13y"),
                (CombinationType.Value, "13r 13y j j"),
                (CombinationType.Value, "1y 1bb j"),
                (CombinationType.Value, "j j 10y 10b"),
                (CombinationType.Value, "j 10y 10b"),
                (CombinationType.Value, "j 10b j 10bb"),
                (CombinationType.Value, "j 13b 13y 13bb"),
                (CombinationType.Value, "j j 3r")
            };

            foreach (var test in testsList)
            {
                Add(test.Item1, converter.StringToCombination(test.Item2));
            }

            ;
        }
    }

    public sealed class BadCombinations : TheoryData<CombinationModel>
    {
        public BadCombinations()
        {
            var converter = new StringToCombinationConverter(CombinationStringFormat.Short);

            var testsList = new List<string>()
            {
                "1r 2r 3r 4r 5r 6r 7r 8r 9r 10r 11r 12r 13r 13r",
                "1bb 2bb 3bb 3bb",
                "j j 1y 2y 3y",
                "j 1y 2y 3y 4y j 6y 7y 8y j",
                "j 1b 2b 3b",
                "2b 3b 1b",
                "1bb 2bb",
                "j j",
                "j",
                "1b",
                "4b 5b 6b 6b",
                "3y 3r 3b 3y",
                "1y 1r 4r 1bb",
                "13r 13y 13b 13bb j",
                "13b 13bb j 13bb"
            };

            foreach (var test in testsList)
            {
                Add(converter.StringToCombination(test));
            }
        }
    }
}
