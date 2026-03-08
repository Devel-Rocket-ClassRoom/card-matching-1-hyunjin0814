using System;
using System.Globalization;
using System.Threading;

class Card
{
    public int[] cards;
    public int totalPair;
    public void MakeCards(int row, int col)
    {
        cards = new int[row * col];
        int numbers = 0;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i % 2 == 0)
            {
                numbers++; 
            }
            cards[i] = numbers;
        }
        totalPair = numbers;
    }

    public void Shuffle()
    {
        System.Random random = new System.Random();
        random.Shuffle(cards);
        Console.Clear();
        Console.WriteLine("카드를 섞는 중...");
        Thread.Sleep(1000);
    }

    public int ReturnCard(int index)
    {
        return cards[index];
    }
}