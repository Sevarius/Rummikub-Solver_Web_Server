using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicsMaps;
using MechanicsModel;
using Xunit;
using Xunit.Abstractions;

namespace EmptyMVC.Tests
{
    public class MechanicsMapsTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public MechanicsMapsTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CheckAllVariations()
        {
            var combMap = new CombinationsMap();
            var firstCombination = new List<CombinationModel>() { new CombinationModel(new List<Card>()) };
            var res = combMap.GetAllVariations(firstCombination);
            Assert.Equal(53, res.Count);
        }

        [Fact]
        public void GetAll3LenCombinations()
        {
            var combMap = new CombinationsMap();
            var check = new CombinationChecker();
            var res = combMap.RemoveColorDuplicationCombinations(combMap.GetOnlyValidCombinations(combMap.GenerateAll3LengthVariations()));

            using (var file = File.OpenWrite("seva.txt"))
            {
                using (var fileW = new StreamWriter(file))
                {
                    fileW.WriteLine(res.Count);
                    foreach (CombinationModel combination in res)
                    {
                        fileW.WriteLine(combination.ToString());
                    }
                }
            }

            Assert.Equal(188+143, res.Count);

        }
    }
}
