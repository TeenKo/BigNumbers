using UnityEngine;

public enum Operation
{
    Sum,
    Sub
}
public class TestBigNumber : MonoBehaviour
{
    public Operation operation;
    
    public string firstNumber;
    public string secondNumber;

    public string shortNumber;
    public string bigNumber;

    public bool firstGreater;
    public bool equal;
    
    private BigNumber _firstBigNumber;
    private BigNumber _secondBigNumber;
    

    [ContextMenu("ShowCount")]
    public void ShowCount()
    {
        var firstOrdersList = BigNumberConverter.ParseNumberString(firstNumber);
        var secondOrdersList = BigNumberConverter.ParseNumberString(secondNumber);

        _firstBigNumber = new BigNumber(firstOrdersList);
        _secondBigNumber = new BigNumber(secondOrdersList);
        
        firstGreater = _firstBigNumber.IsGreaterThen(_secondBigNumber); 
        equal = _firstBigNumber.IsEqual(_secondBigNumber);
        
        var sum = BigNumberOperations.SumBigNumber(_firstBigNumber, _secondBigNumber);
        var sub = BigNumberOperations.SubBigNumber(_firstBigNumber, _secondBigNumber);
        
        var number = operation is Operation.Sub ? sub.GetNumber() : sum.GetNumber();
        var isNegative = operation is Operation.Sub ? sub.IsNegative : sum.IsNegative;
        
        bigNumber = BigNumberConverter.ConvertToViewForm(number, false, isNegative);
        shortNumber = BigNumberConverter.ConvertToViewForm(number, true, isNegative);
    }
}