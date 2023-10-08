using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string firstNumber;
    public string secondNumber;

    public string shorNumber;
    public string bigNumber;

    private BigNumber _bigNumber;

    [ContextMenu("ShowCount")]
    public void ShowCount()
    {
        var firstOrders = BigNumberConverter.ParseNumberString(firstNumber);
        var secondOrders = BigNumberConverter.ParseNumberString(secondNumber);

        _bigNumber = new BigNumber(firstOrders);
        _bigNumber.ChangeNumber(BigNumberOperations.SumBigNumber(firstOrders, secondOrders));
        bigNumber = BigNumberConverter.ConvertToViewForm(_bigNumber.GetNumber(), false);
        shorNumber = BigNumberConverter.ConvertToViewForm(_bigNumber.GetNumber(), true);
    }
}

public class BigNumber
{
    private List<short> _orders;

    public bool IsNegative;

    public BigNumber(List<short> value)
    {
        _orders = value;
    }

    private BigNumber()
    {
        _orders = new List<short>();
    }

    public void ChangeNumber(List<short> newOrders)
    {
        if (newOrders == null) return;

        _orders = newOrders;
    }

    public List<short> GetNumber()
    {
        return _orders;
    }

    public static BigNumber operator +(BigNumber first, BigNumber second)
    {
        return new BigNumber();
    }
}

public static class BigNumberOperations
{
    public static List<short> SumBigNumber(List<short> first, List<short> second)
    {
        if (first is null || second is null)
            throw new Exception("BigNumber: Null value in + operator");

        var sumList = new List<short>();
        var maxListCount = first.Count >= second.Count ? first.Count : second.Count;
        var nextCellIncrease = 0;

        if (first.Count != second.Count)
        {
            EqualizeCountOfList(first, second);
        }

        first.Reverse();
        second.Reverse();

        for (var i = 0; i < maxListCount; i++)
        {
            var firstNumber = first[i];
            var secondNumber = second[i];

            var cellValue = firstNumber + secondNumber + nextCellIncrease;
            if (cellValue >= 1000)
            {
                nextCellIncrease = 1;
                cellValue %= 1000;
            }
            else
            {
                nextCellIncrease = 0;
            }

            sumList.Insert(0, Convert.ToInt16(cellValue));
        }

        if (nextCellIncrease > 0)
        {
            sumList.Insert(0, Convert.ToInt16(nextCellIncrease));
        }

        return sumList;
    }

    private static void EqualizeCountOfList(List<short> first, List<short> second)
    {
        var firstMoreSecond = first.Count > second.Count;
        var difference = firstMoreSecond ? first.Count - second.Count : second.Count - first.Count;

        for (var i = 0; i < difference; i++)
        {
            if (firstMoreSecond)
            {
                second.Insert(0, 0);
                continue;
            }

            first.Insert(0, 0);
        }
    }
}

public static class BigNumberConverter
{
    private const int CharsCount = 4;

    private static readonly string[] Suffixes =
        { "", "K", "M", "B", "T", "Q", "Qt", "Sx", "Sp", "Oc", "No", "De", "Un", "Du", "Tr", "Qu", "Qua" };

    public static string ConvertToViewForm(List<short> orders, bool shortForm)
    {
        if (orders.Count == 0) return "orders count == 0";

        var result = new StringBuilder();

        for (var i = 0; i < orders.Count; i++)
        {
            if (i == 0) result.Append(orders[i]);
            else switch (orders[i])
            {
                case < 100 and > 10:
                    result.Append("0" + orders[i]);
                    break;
                case < 10:
                    result.Append("00" + orders[i]);
                    break;
                default:
                    result.Append(orders[i]);
                    break;
            }
            
            if (i < orders.Count - 1)
            {
                result.Append('.');
            }
        }

        if (!shortForm) return result.ToString();
        
        if (result.Length > CharsCount)
        {
            result.Length = CharsCount;
        }

        if (result[^1] == '.') result.Length -= 1;

        return result.ToString() + Suffixes[orders.Count - 1];

    }

    public static List<short> ParseNumberString(string numberString)
    {
        if (string.IsNullOrWhiteSpace(numberString))
        {
            throw new ArgumentException("Invalid number string", nameof(numberString));
        }

        var ordersCount = (int)Math.Ceiling(numberString.Length / 3f);
        var newOrders = new List<short>(ordersCount);
        var charIndex = numberString.Length - 1;

        while (charIndex >= 0)
        {
            var charsCount = Math.Min(3, charIndex + 1);
            var lastChars = numberString.Substring(charIndex - charsCount + 1, charsCount);
            charIndex -= charsCount;

            if (short.TryParse(lastChars, out var number))
            {
                newOrders.Add(number);
            }
            else
            {
                throw new ArgumentException("Invalid number string", nameof(numberString));
            }
        }

        newOrders.Reverse();
        return newOrders;
    }
}