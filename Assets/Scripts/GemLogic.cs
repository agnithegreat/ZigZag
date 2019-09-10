using UnityEngine;

public class GemLogic
{
    private bool _randomGems;
    private int _gemBlock;
    
    private int _currentGemCell;
    private int _cellsCounter;

    public void Init(int initialValue, int gemBlock, bool randomGems)
    {
        _cellsCounter = initialValue;
        _randomGems = randomGems;
        _gemBlock = gemBlock;
    }

    public bool Roll()
    {
        return _cellsCounter == _currentGemCell;
    }

    public void Step()
    {
        _cellsCounter++;
        
        if (_cellsCounter >= _gemBlock)
        {
            _cellsCounter = 0;
            if (_randomGems)
            {
                _currentGemCell = Random.Range(0, _gemBlock);
            }
            else
            {
                _currentGemCell = (_currentGemCell + 1) % _gemBlock;
            }
        }
    }
}