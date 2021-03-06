using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    class BaseShip
    {
        public struct Deck
        {
            public int x;
            public int y;

            public bool Alive;
        }

        public Deck[] decks;
        public bool Alive;

        public BaseShip() { Alive = true; }
    }



    class FourDeckShip : BaseShip
    {
        public int SIZE = 4;

        public FourDeckShip()
        {
            decks = new Deck[SIZE];
            for (int i = 0; i < decks.Length; i++) { decks[i].Alive = true; }
        }
    }
    class ThreeDeckShip : BaseShip
    {
        public int SIZE = 3;

        public ThreeDeckShip()
        {
            decks = new Deck[SIZE];
            for (int i = 0; i < decks.Length; i++) { decks[i].Alive = true; }
        }
    }
    class DoubleDeckShip : BaseShip
    {
        public int SIZE = 2;

        public DoubleDeckShip() 
        { 
            decks = new Deck[SIZE];
            for (int i = 0; i < decks.Length; i++) { decks[i].Alive = true; }
        }
    }
    class SingleDeckShip : BaseShip
    {
        public int SIZE = 1;

        public SingleDeckShip() 
        { 
            decks = new Deck[SIZE];
            for (int i = 0; i < decks.Length; i++) { decks[i].Alive = true; }
        }
    }


}
