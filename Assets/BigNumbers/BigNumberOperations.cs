using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BigNumberOperations
{
    public static BigNumber SumBigNumber(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("BigNumber: Null value in + operator");

        var fistList = first.GetNumber();
        var secondList = second.GetNumber();
        var sumList = new List<short>();
        var maxListCount = fistList.Count >= secondList.Count ? fistList.Count : secondList.Count;
        var nextCellIncrease = 0;

        if (fistList.Count != secondList.Count) EqualizeCountOfList(fistList, secondList);

        for (var i = 0; i < maxListCount; i++)
        {
            var firstNumber = fistList[i];
            var secondNumber = secondList[i];

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

            sumList.Add(Convert.ToInt16(Math.Abs(cellValue)));
        }

        if (nextCellIncrease > 0)
        {
            sumList.Add(Convert.ToInt16(nextCellIncrease));
        }

        return new BigNumber(sumList);
    }

    public static BigNumber SubBigNumber(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("BigNumber: Null value in - operator");

        List<short> fistList;
        List<short> secondList;
        var shouldBeNegative = false;

        if (first.IsGreaterThen(second) || first.IsEqual(second))
        {
            fistList = first.GetNumber();
            secondList = second.GetNumber();
        }
        else
        {
            secondList = first.GetNumber();
            fistList = second.GetNumber();
            shouldBeNegative = true;
        }

        var subList = new List<short>();
    
        for (var i = fistList.Count; i > 0; i--)
        {
            var currentIndex = i - 1;
            
            var firstDigit = currentIndex >= 0 ? fistList[currentIndex] : 0;
            var secondDigit = currentIndex >= 0 ? secondList[currentIndex] : 0;

            if (firstDigit < secondDigit)
            {
                fistList[currentIndex] += 1000;
                fistList[i - 2]--;
            }
            
            var value = Math.Abs(fistList[currentIndex] - secondList[currentIndex]);
            subList.Insert(0, (short)value);
        }

        if (subList.All(item => item == 0)) subList = new List<short> { 0 };
        
        var result = new BigNumber(subList);
        if (shouldBeNegative) result.IsNegative = true;
        return result;
    }

    #region Opreations

    #region == != < > <= >=

    private static void EqualizeCountOfList(IList<short> first, IList<short> second)
    {
        var firstMoreSecond = first.Count > second.Count;
        var difference = firstMoreSecond
            ? first.Count - second.Count
            : second.Count - first.Count;

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

    #endregion

    #endregion
}