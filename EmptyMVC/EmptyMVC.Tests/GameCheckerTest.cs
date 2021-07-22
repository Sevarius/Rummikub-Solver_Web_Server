using System.Collections.Generic;
using System.Linq;
using MechanicsModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Assert = Xunit.Assert;

namespace EmptyMVC.Tests
{
    public sealed class GameCheckerTest
    {
        private readonly GameChecker _gameChecker;
        private readonly StringToCombinationConverter _converter;
        private readonly PrivateObject _privateGameChecker;

        public GameCheckerTest()
        {
            _converter = new StringToCombinationConverter(CombinationStringFormat.Short);
            _gameChecker = new GameChecker();
            _privateGameChecker = new PrivateObject(_gameChecker);
        }

        [Theory]
        [InlineData("1r 2r 3r", "1r 2r 3r")]
        [InlineData("1r 2r 3r 4r", "1r 2r 3r 4r")]
        [InlineData("1r 2r 3r 4r 5r", "1r 2r 3r 4r 5r")]
        [InlineData("1r 2r 3r 4r 5r 6r", "1r 2r 3r", "4r 5r 6r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r", "1r 2r 3r", "4r 5r 6r 7r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r", "1r 2r 3r", "4r 5r 6r 7r 8r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r 9r", "1r 2r 3r", "4r 5r 6r", "7r 8r 9r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r 9r 10r", "1r 2r 3r", "4r 5r 6r", "7r 8r 9r 10r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r 9r 10r 11r", "1r 2r 3r", "4r 5r 6r", "7r 8r 9r 10r 11r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r 9r 10r 11r 12r", "1r 2r 3r", "4r 5r 6r", "7r 8r 9r", "10r 11r 12r")]
        [InlineData("1r 2r 3r 4r 5r 6r 7r 8r 9r 10r 11r 12r 13r", "1r 2r 3r", "4r 5r 6r", "7r 8r 9r", "10r 11r 12r 13r")]
        public void SplitCombinationTest(params string[] combinations)
        {
            var combToSplit = _converter.StringToCombination(combinations[0]);
            var expectedResult = new List<CombinationModel>(combinations.Length - 1);
            expectedResult.AddRange(combinations.ToList().GetRange(1, combinations.Length - 1).Select(cmbStr => _converter.StringToCombination(cmbStr)));
            var actualResult = (List<CombinationModel>) _privateGameChecker.Invoke("SplitCombination", combToSplit);
            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("1bb 2bb 3bb;1r 2r 3r 4r 5r 6r;1b 2b 3b", "1r 2r 3r", "4r 5r 6r")]
        [InlineData("1bb 2bb 3bb 4bb 5bb 6bb;1r 2r 3r 4r 5r 6r","1bb 2bb 3bb", "4bb 5bb 6bb", "1r 2r 3r", "4r 5r 6r")]
        [InlineData("1r 2r j 4r 5r 6r 7r 8r 9r 10r j 12r 13r", "1r 2r j", "4r 5r 6r", "7r 8r 9r", "10r j 12r 13r")]
        public void SplitTableCombinations(string table, params string[] combinations)
        {
            var tableCombinations = table.Split(';').Select(x => _converter.StringToCombination(x)).ToList();
            var game = new GameModel
            {
                Table = tableCombinations,
            };

            var expectedSplitCombinations = combinations.Select(x => _converter.StringToCombination(x)).ToList();

            var actualSplitCombinations = (List<CombinationModel>) _privateGameChecker.Invoke("SplitTableCombinations", game);

            Assert.Equal(expectedSplitCombinations, actualSplitCombinations);
        }

        [Theory]
        [InlineData("1r 2r 3r", "1b 2y 5r j")]
        [InlineData("1r 2r 3r;1r 2r 3r", "1b 2y 5r j")]
        [InlineData("1r 2r 3r 4r 5r 6r", "1b 2y 5r j")]
        [InlineData("j j 3r 4r 5r 6r;1bb 2bb 3bb;6bb 7bb 8bb", "1b 2y 5r 8y")]
        public void ValidateGoodGameTest(string table, string hand)
        {
            var tableCombinations = table.Split(';').Select(_converter.StringToCombination).ToList();
            var handCards = hand.Split(' ').Select(_converter.StringToCard).ToList();
            var game = new GameModel
            {
                Table = tableCombinations,
                Hand = handCards
            };
            (var vr, var combList, var cardList) = _gameChecker.ValidateGame(game);

            Assert.True(vr);
            Assert.False(combList.Any());
            Assert.False(cardList.Any());
            Assert.True(game.Table.All(x => x.isValid));
        }

        [Theory]
        [InlineData("1r 2r 3r", "1r 2r 3r 3r", "", "3r")]
        [InlineData("1r 2r 3r 4y", "1r 2r 3r", "1r 2r 3r 4y", "")]
        [InlineData("1r 2r 3r j 5r j 6r j 8r", "1r 2r 3r", "1r 2r 3r j 5r j 6r j 8r", "j")]
        [InlineData("1r j 3r;3r 4r 5r;3y 3bb j 3r", "j 10y", "", "3r j")]
        public void ValidateBadGameTest(string table, string hand, string badCombinations, string badCards)
        {
            var game = new GameModel
            {
                Table = table.Split(';').Select(_converter.StringToCombination).ToList(),
                Hand = hand.Split(' ').Select(_converter.StringToCard).ToList(),
            };
            (var vr, var combList, var cardList) = _gameChecker.ValidateGame(game);
            Assert.False(vr);

            if (badCombinations != string.Empty)
            {
                var badCombList = badCombinations.Split(';').Select(_converter.StringToCombination).ToList();
                Assert.Equal(badCombList, combList);
            }

            if (badCards != string.Empty)
            {
                var badCardsList = badCards.Split(' ').Select(_converter.StringToCard).ToList();
                badCardsList.Sort();
                cardList.Sort();
                Assert.Equal(badCardsList, cardList);
            }
        }
    }
}