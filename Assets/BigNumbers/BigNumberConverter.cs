using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Text;

public static class BigNumberConverter
{
    private const int CharsCount = 4;

    private static readonly string[] Suffixes =
        { "", "K", "M", "B", "T", "Q", "Qt", "Sx", "Sp", "Oc", "No", "De", "Un", "Du", "Tr", "Qu", "Qua" };

    public static string ConvertToViewForm(List<short> orders, bool shortForm = true, bool isNegative = false)
    {
        if (orders.Count == 0)
        {
            throw new ArgumentException("orders count == 0", nameof(orders));
        }

        var result = ZString.CreateStringBuilder();
        
        if (isNegative) result.Append('-');
        
        if (orders.Count == 1)
        {
            result.Append(orders[0]);
            if (!shortForm) return result.ToString();

            result.Append(Suffixes[0]);
            return result.ToString();
        }

        for (var i = 0; i < orders.Count; i++)
        {
            if (i > 0) result.Append('.');
            result.Append(i == 0 ? orders[i].ToString() : orders[i].ToString("D3"));
        }

        if (!shortForm) return result.ToString();
        if (result.Length > CharsCount) result.Remove(CharsCount, result.Length - CharsCount);
        if (result.ToString()[^1] == '.') result.Remove(result.Length - 1, 1);

        var suffixIndex = Math.Min(orders.Count - 1, Suffixes.Length - 1);
        var suffix = Suffixes[suffixIndex];
        result.Append(suffix);

        return result.ToString();
    }

    public static List<short> ParseNumberString(string numberString)
    {
        if (string.IsNullOrEmpty(numberString) || IsDigitsOnly(numberString) == false)
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

    private static bool IsDigitsOnly(string str)
    {
        return str.All(c => c is >= '0' and <= '9');
    }
}