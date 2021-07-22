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
                new CombinationModel(new List<Card>()
                {
                    new Card(CardColor.Red, 1),
                    new Card(CardColor.Red, 2),
                    new Card(CardColor.Red, 3),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                },
                new CombinationModel(new List<Card>()
                {
                    new Card(CardColor.Red, 1),
                    new Card(CardColor.Red, 2),
                    new Card(CardColor.Red, 3),
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
                new CombinationModel(new List<Card>()
                {
                    new Card(CardColor.Red, 1),
                    new Card(CardColor.Red, 2),
                    new Card(CardColor.Red, 3),
                })
                {
                    isValid = true,
                    Type = CombinationType.Color
                },
                new CombinationModel(new List<Card>()
                {
                    new Card(CardColor.Red, 2),
                    new Card(CardColor.Red, 3),
                    new Card(CardColor.Red, 4),
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
