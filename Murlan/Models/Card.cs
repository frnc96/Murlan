using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Murlan
{
    public class Card
    {
        String Rank;
        String Value;
        String Suit;
        int Id;
        public Card(String value, String suit, int id, String rank)
        {
            this.Value = value;
            this.Id = id;
            this.Suit = suit;
            this.Rank = rank;
        }
        
        public String getVal()
        {
            return this.Value;
        }

        public String getSuit()
        {
            return this.Suit;
        }

        public int getId()
        {
            return this.Id;
        }

        public String getRank()
        {
            return this.Rank;
        }
    }
}