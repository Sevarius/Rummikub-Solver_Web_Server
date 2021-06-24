using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MechanicsModel;

namespace MechanicsMaps
{
    public class CombinationsMap
    {
        private Dictionary<CombinationModel, int> _map;

        public bool Generated { get; private set; }

        public CombinationsMap()
        {
            this._map = new Dictionary<CombinationModel, int>();
        }

        public void GenerateCombinations()
        {
            var all3LengthValidCombinations = GetOnlyValidCombinations(this.GenerateAll3LengthVariations());

            all3LengthValidCombinations = RemoveColorDuplicationCombinations(all3LengthValidCombinations);

            var all4LengthValidCombinations = GetOnlyValidCombinations(GetAllVariations(all3LengthValidCombinations));

            all4LengthValidCombinations = RemoveColorDuplicationCombinations(all4LengthValidCombinations);

            var all5LengthValidCombinations = GetOnlyValidCombinations(GetAllVariations(all4LengthValidCombinations));

            all5LengthValidCombinations = RemoveColorDuplicationCombinations(all5LengthValidCombinations);
        }

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

        internal List<CombinationModel> GenerateAll3LengthVariations()
        {
            var firstCombination = new List<CombinationModel>() {new CombinationModel(new List<Card>())};
            return GetAllVariations(GetAllVariations(GetAllVariations(firstCombination)));
        }

        public List<CombinationModel> GetOnlyValidCombinations(List<CombinationModel> combinations)
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

        public List<CombinationModel> RemoveColorDuplicationCombinations(List<CombinationModel> combinations)
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
