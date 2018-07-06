using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Murlan.Models
{
    public class Deck
    {
        static List<String> hand1 = new List<String>();
        static List<String> hand2 = new List<String>();
        static List<String> hand3 = new List<String>();
        static List<String> hand4 = new List<String>();
        public String GroupName;
        private Stack<String[]> theHand = new Stack<String[]>();
        private List<String[]> OpponentHands = new List<String[]>();

        public Deck(String group)
        {
            this.GroupName = group;
            shuffleCards();
            fillHands();
            getHands();
        }
        
        private static Stack<Card> deck = new Stack<Card>();
        String[] suit = { "spades", "hearts", "clubs", "diamonds" };
        String[] value = { "14", "15", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13" };
        Random rnd = new Random();

        public Stack<string[]> TheHand { get => theHand; set => theHand = value; }
        public List<string[]> OpponentHands1 { get => OpponentHands; set => OpponentHands = value; }

        public void shuffleCards()
        {
            int id = 54;
            int r = 1;
            foreach (String x in suit)
            {
                foreach (String y in value)
                {
                    deck.Push(new Card(y, x, id--, "rank" + r++));
                }
                r = 1;
            }
            deck.Push(new Card("16", "joker", id--, "rank1"));
            deck.Push(new Card("17", "joker", --id, "rank3"));
            Shuffle<Card>(deck);
        }
        public void Shuffle<T>(Stack<T> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => rnd.Next()))
                stack.Push(value);
        }
        public void fillHands()
        {
            hand1.Clear();
            hand2.Clear();
            hand3.Clear();
            hand4.Clear();
            while (deck.Count != 2)
            {
                var a = deck.Pop();
                hand1.Add(a.getId() + "|" + a.getVal() + "|" + a.getSuit() + "|" + a.getRank());
                var b = deck.Pop();
                hand2.Add(b.getId() + "|" + b.getVal() + "|" + b.getSuit() + "|" + b.getRank());
                var c = deck.Pop();
                hand3.Add(c.getId() + "|" + c.getVal() + "|" + c.getSuit() + "|" + c.getRank());
                var d = deck.Pop();
                hand4.Add(d.getId() + "|" + d.getVal() + "|" + d.getSuit() + "|" + d.getRank());
            }
            var e = deck.Pop();
            hand1.Add(e.getId() + "|" + e.getVal() + "|" + e.getSuit() + "|" + e.getRank());
            var f = deck.Pop();
            hand2.Add(f.getId() + "|" + f.getVal() + "|" + f.getSuit() + "|" + f.getRank());
        }
        

        public void getHands()
        {
            String[] Player1Hand = hand1.OrderByDescending(o => int.Parse(o.Split('|')[1])).ToList().ToArray();
            String[] Player2Hand = hand2.OrderByDescending(o => int.Parse(o.Split('|')[1])).ToList().ToArray();
            String[] Player3Hand = hand3.OrderByDescending(o => int.Parse(o.Split('|')[1])).ToList().ToArray();
            String[] Player4Hand = hand4.OrderByDescending(o => int.Parse(o.Split('|')[1])).ToList().ToArray();
            theHand.Push(Player1Hand);
            theHand.Push(Player2Hand);
            theHand.Push(Player3Hand);
            theHand.Push(Player4Hand);

            OpponentHands.Add(Player1Hand);
            OpponentHands.Add(Player2Hand);
            OpponentHands.Add(Player3Hand);
            OpponentHands.Add(Player4Hand);
        }
    }
}