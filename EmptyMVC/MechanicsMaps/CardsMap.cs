using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechanicsModel;

namespace MechanicsMaps
{
    public class CardsMap
    {
        private Dictionary<Card, int> _map;

        public bool Generated { get; private set; }

        public CardsMap()
        {
            this._map = new Dictionary<Card, int>();
        }

        public void GenerateMap()
        {
            int id = 0;
            for (int number = 1; number <= 13; number++)
            {
                foreach (var color in new[] {CardColor.Red, CardColor.Yellow, CardColor.Black, CardColor.Blue})
                {
                    this._map.Add(new Card(color, number), id++);
                }
            }

            this._map.Add(new Card(CardColor.Joker, 0), id++);

            Generated = true;
        }

        public int this[Card card] => this._map[card];
    }
}
