using System.Collections.Generic;
using MechanicsModel;
using Xunit;

namespace EmptyMVC.Tests
{
    public class CombinationModelTests
    {
        [Fact]
        public void CombinationModelToSetTest()
        {
            var combinationList = new List<CombinationModel>()
            {
                new CombinationModel(new List<CardModel>()
                {
                    new CardModel(CardColor.Red, 1),
                    new CardModel(CardColor.Red, 2),
                    new CardModel(CardColor.Red, 3),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                },
                new CombinationModel(new List<CardModel>()
                {
                    new CardModel(CardColor.Red, 1),
                    new CardModel(CardColor.Red, 2),
                    new CardModel(CardColor.Red, 3),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                }
            };

            var combinationSet = new HashSet<CombinationModel>(combinationList);
            Assert.Single(combinationSet);

            combinationList = new List<CombinationModel>()
            {
                new CombinationModel(new List<CardModel>()
                {
                    new CardModel(CardColor.Red, 1),
                    new CardModel(CardColor.Red, 2),
                    new CardModel(CardColor.Red, 3),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                },
                new CombinationModel(new List<CardModel>()
                {
                    new CardModel(CardColor.Red, 2),
                    new CardModel(CardColor.Red, 3),
                    new CardModel(CardColor.Red, 4),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                }
            };

            combinationSet = new HashSet<CombinationModel>(combinationList);
            Assert.Equal(2, combinationSet.Count);
        }
    }
}
