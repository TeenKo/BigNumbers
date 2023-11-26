using System;
using System.Collections.Generic;
using System.Linq;

public class BigNumber
{
    private List<short> _orders;

    public bool IsNegative = false;

    public BigNumber(List<short> value)
    {
        _orders = value;
    }

    public BigNumber()
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
    
    public bool IsEqual(BigNumber other)
    {
        if (IsNegative != other.IsNegative) return false;
        if (_orders.Count != other.GetNumber().Count) return false;
        
        var otherArray = other.GetNumber();
        
        return !_orders.Where((t, i) => t != otherArray[i]).Any();
    }
    
    public bool IsGreaterThen(BigNumber other)
    {
        return IsNegative switch
        {
            false when !other.IsNegative => IsFirstGreaterThenSecond(this, other),
            true when !other.IsNegative => false,
            false when other.IsNegative => true,
            true when other.IsNegative => !IsFirstGreaterThenSecond(this, other),
            _ => false
        };
    }
    
    private bool IsFirstGreaterThenSecond(BigNumber first, BigNumber second)
    {
        var firstList = first.GetNumber();
        var secondList = second.GetNumber();

        var firstSize = firstList.Count;
        var secondSize = secondList.Count;

        if (firstSize > secondSize) return true;
        if (firstSize < secondSize) return false;
        
        for (var i = 0; i < firstSize; i++)
        {
            var currentIndex = i > 0 ? i - 1 : 0;
            
            if (firstList[currentIndex] > secondList[currentIndex]) return true;
            if (firstList[currentIndex] < secondList[currentIndex]) return false;
        }
        
        return false;
    }
    
    public static bool operator ==(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in == operator");
        return first.IsEqual(second);
    }

    public static bool operator !=(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in != operator");
        return !(first == second);
    }

    public static bool operator >(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in > operator");
        return first.IsGreaterThen(second);
    }

    public static bool operator <(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in < operator");
        return !first.IsGreaterThen(second);
    }

    public static bool operator >=(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in >= operator");
        return first.IsGreaterThen(second) || first.IsEqual(second);
    }

    public static bool operator <=(BigNumber first, BigNumber second)
    {
        if (first is null || second is null)
            throw new Exception("IdleNumber: Null value in <= operator");
        return !first.IsGreaterThen(second) || first.IsEqual(second);
    }
}