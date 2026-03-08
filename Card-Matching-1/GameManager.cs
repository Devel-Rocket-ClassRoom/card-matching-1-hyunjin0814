using System;
using System.Threading;

class GameManager
{
    Card card;
    bool[] hiddenArray;
    bool IsSecondCard;
    int maxTry = 20;
    int tryCount;
    int pairCount;
    int totalRow;
    int totalCol;
    int FirstRow;
    int FirstCol;
    int index;

    public GameManager(Card card)
    {
        this.card = card;
    }

    public void MakeArray(int row, int col)
    {
        tryCount = 0;
        pairCount = 0;
        totalRow = row;
        totalCol = col;
        card.MakeCards(row, col);
        card.Shuffle();
        hiddenArray = new bool[row * col];
        for (int i = 0; i < hiddenArray.Length; i++)
        {
            hiddenArray[i] = true;
        }
    }

    public void PrintScreen()
    {
        Console.Clear();
        Console.Write("     ");
        for (int i = 0; i < totalCol; i++)
        {
            Console.Write($"{i + 1}열  ");
        }
        Console.WriteLine();

        for (int j = 0; j < totalRow; j++)
        {
            Console.Write($"{j + 1}행  ");
            for (int k = 0; k < totalCol; k++)
            {
                if (hiddenArray[j * totalCol + k])
                {
                    Console.Write($"**  ");
                }
                else
                {
                    Console.Write($"[{card.ReturnCard(j * totalCol + k)}] ");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine($"시도 횟수: {tryCount}/{maxTry} | 찾은 쌍: {pairCount}/{card.totalPair}");
        Console.WriteLine();
    }

    //public int InputRowCol(string text)
    //{
    //    while (true)
    //    {
    //        Console.Write($"{text} 번째 카드를 선택하세요 (행 열): ");
    //        string input = Console.ReadLine();

    //        string[] numbers = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // 연속된 공백 처리
    //        if (numbers.Length != 2)
    //        {
    //            Console.WriteLine("행과 열을 공백으로 구분하여 입력하세요. (예: 1 3)");
    //            continue;
    //        }

    //        int row = int.Parse(numbers[0]);
    //        int col = int.Parse(numbers[1]);
    //        if (row > _row || row < 1 || col > _col || col < 1)
    //        {
    //            Console.WriteLine("행은 1~4, 열은 1~4 범위로 입력하세요.");
    //            continue;
    //        }
            
    //        int index = (row - 1) * _col + col - 1;
    //        hiddenArray[index] = false;
    //        return index;
    //    }
    //}

    public int CardFlip_2()
    {
        while (true)
        {
            string input;

            if (!IsSecondCard)
            {
                Console.Write("첫 번째 카드를 선택하세요 (행 열): ");
                input = Console.ReadLine();
            }
            else
            {
                Console.Write("두 번째 카드를 선택하세요 (행 열): ");
                input = Console.ReadLine();
            }

            if (Validation(input))
            {
                hiddenArray[index] = false;
                return index;
            }
        }
    }

    public bool Validation(string input)
    {
        string[] numbers = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // 연속된 공백 처리

        // 행과 열로 분리해서 숫자 2개가 나오는지 검사
        if (numbers.Length != 2)
        {
            Console.WriteLine("행과 열을 공백으로 구분하여 입력하세요. (예: 1 3)");
            return false;
        }

        int row = int.Parse(numbers[0]);
        int col = int.Parse(numbers[1]);

        // 행과 열의 범위 검사
        if (row > totalRow || row < 1 || col > totalCol || col < 1)
        {
            Console.WriteLine("행은 1~4, 열은 1~4 범위로 입력하세요.");
            return false;
        }

        // 첫번째 입력값은 통과
        if (!IsSecondCard)
        {
            FirstRow = row;
            FirstCol = col;
            index = (row - 1) * totalCol + col - 1;
            IsSecondCard = !IsSecondCard;
            return true;
        }
        // 두번째 입력값은 첫번째 입력값과 비교
        else
        {
            if (FirstRow == row && FirstCol == col)
            {
                Console.WriteLine("같은 카드를 선택할 수 없습니다. 다른 카드를 선택하세요.");
                return false;
            }
        }

        index = (row - 1) * totalCol + col - 1;
        IsSecondCard = !IsSecondCard;
        return true;
    }

    public void CompareCards()
    {
        int firstIndex = CardFlip_2();
        PrintScreen();
        int secondIndex = CardFlip_2();
        tryCount++;
        if (card.ReturnCard(firstIndex) == card.ReturnCard(secondIndex))
        {
            pairCount++;
            PrintScreen();
            Console.WriteLine();
            Console.WriteLine("짝을 찾았습니다!");
        }
        else
        {
            PrintScreen();
            Console.WriteLine();
            Console.WriteLine("짝이 맞지 않습니다!");
            Thread.Sleep(1500);
            hiddenArray[firstIndex] = true;
            hiddenArray[secondIndex] = true;
        }
    }

    public void GameStart()
    {
        MakeArray(4, 4);
        string exit = string.Empty;
        while (exit != "N")
        {
            PrintScreen();
            if (pairCount == card.totalPair)
            {
                Console.WriteLine("=== 게임 클리어! ===");
                Console.WriteLine($"총 시도 횟수: {tryCount}");

                Console.WriteLine();
                Console.Write("새 게임을 하시겠습니까? (Y/N): ");
                exit = Console.ReadLine().ToUpper();

                MakeArray(4, 4);
            }
            else if (tryCount == maxTry)
            {
                Console.WriteLine("=== 게임 오버! ===");
                Console.WriteLine("시도 횟수를 모두 사용했습니다.");
                Console.WriteLine($"찾은 쌍: {pairCount}/{card.totalPair}");

                Console.WriteLine();
                Console.Write("새 게임을 하시겠습니까? (Y/N): ");
                exit = Console.ReadLine().ToUpper();

                MakeArray(4, 4);
            }
            else
            {
                CompareCards();
            }
        }
    }
}