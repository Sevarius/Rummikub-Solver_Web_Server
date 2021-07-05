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
        public void CheckAllVariationsTest()
        {
            var combMap = new CombinationsMap();
            var firstCombination = new List<CombinationModel>() { new CombinationModel(new List<Card>()) };
            var res = combMap.GetAllVariations(firstCombination);
            Assert.Equal(53, res.Count);
        }

        [Fact]
        public void GetAll3LenCombinationsTest()
        {
            var combMap = new CombinationsMap();
            var check = new CombinationChecker();
            var res = combMap.GetOnlyValidCombinations(combMap.GenerateAll3LengthVariations());
            res = combMap.RemoveColorDuplicationCombinations(res);

            // using (var file = File.OpenWrite("seva.txt"))
            // {
            //     using (var fileW = new StreamWriter(file))
            //     {
            //         fileW.WriteLine(res.Count);
            //         foreach (CombinationModel combination in res)
            //         {
            //             fileW.WriteLine(combination.ToString());
            //         }
            //     }
            // }

            Assert.Equal(358, res.Count);
        }

        [Fact]
        public void GetAll5LenCombinationsTest()
        {
            var combMap = new CombinationsMap();
            var res = combMap.GenerateCombinations();

            using (var file = File.OpenWrite("seva.txt"))
            {
                using (var fileW = new StreamWriter(file))
                {
                    //fileW.WriteLine(res.Count);
                    foreach (CombinationModel combination in res)
                    {
                        fileW.WriteLine(combination.ToStringRaw());
                    }
                }
            }

            Assert.Equal(1517, res.Count);
        }
    }
}
