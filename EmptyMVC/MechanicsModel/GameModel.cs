using System.Collections.Generic;
using System.Linq;

namespace MechanicsModel
{
    public class GameModel
    {
        public List<CombinationModel> Table { get; set; }

        public List<Card> Hand { get; set; }

        public bool? IsValid = null;

        public int CountCardInHand(Card card)
        {
            return Hand.Count(x => x.Equals(card));
        }

        public int CountCardOnTable(Card card)
        {
            return Table.Select(x => x.CountCard(card)).Sum();
        }
    }
}