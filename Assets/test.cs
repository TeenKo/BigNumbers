using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{
    public string fullNumber;
    public string shorNumber;
    
    private readonly BigNumber _bigNumber = new BigNumber();

    [ContextMenu("ShowCount")]
    public void ShowCount()
    {
        var orders = BigNumberConverter.ParseNumberString(fullNumber);
        _bigNumber.ChangeNumber(orders);
        shorNumber = BigNumberConverter.ConvertToNormalForm(_bigNumber.GetNumber());
        Debug.Log(shorNumber);
    }
}

public class BigNumber
{
    private List<short> _orders;

    public void ChangeNumber(List<short> newOrders)
    {
        if(newOrders == null) return;
        
        _orders = newOrders;
    }

    public List<short> GetNumber()
    {
        return _orders;
    }
}

public static class BigNumberConverter
{
    private const int CharsCount = 4;
    private static readonly string[] Suffixes =
        { "", "K", "M", "B", "T", "Q", "Qt", "Sx", "Sp", "Oc", "No", "De", "Un", "Du", "Tr", "Qu", "Qua" };

    public static string ConvertToNormalForm(List<short> orders)
    {
        if (orders.Count == 0) return "orders count == 0";

        var result = new StringBuilder();
        for (var i = 0; i < orders.Count; i++)
        {
            result.Append(orders[i]);
            if (i < orders.Count - 1)
            {
                result.Append('.');
            }
        }
        
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