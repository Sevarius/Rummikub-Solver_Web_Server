using System.Collections.Generic;
using System.Linq;
using MechanicsModel;
using Xunit;

namespace EmptyMVC.Tests
{
    public class GameCheckerTest
    {
        private readonly GameChecker _gameChecker;
        private readonly StringToCombinationConverter _converter;
        
        public GameCheckerTest()
        {
            _converter = new StringToCombinationConverter();
            _gameChecker = new GameChecker();
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
            var combToSplit = _converter.StringToCombination(combinations[0], CombinationStringFormat.Short);
            var expectedResult = new List<CombinationModel>(combinations.Length - 1);
            expectedResult.AddRange(combinations.ToList().GetRange(1, combinations.Length - 1).Select(cmbStr => _converter.StringToCombination(cmbStr, CombinationStringFormat.Short)));
            var actualResult = _gameChecker.SplitCombination(combToSplit);
            Assert.Equal(expectedResult, actualResult);
        }
        
    }
}