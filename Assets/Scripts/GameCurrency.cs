using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameCurrency
{
    int[] _currencyUnits;
    string[] _currencyNames = {"", "K", "M", "B", "T", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
    public GameCurrency()
    {
        _currencyUnits = new int[31];
    }
    public GameCurrency(int[] currencyEntry)
    {
        _currencyUnits = new int[31];
        for (int i = 0; i<_currencyUnits.Length; i++)
        {
            _currencyUnits[i] = currencyEntry[i];
        }
    }
    public GameCurrency(int units)
    {
        _currencyUnits = new int[31];
        _currencyUnits[0] = units;
        ConvertUnits();
    }
    public int[] GetAmount()
    {
        return _currencyUnits;
    }
    public void SetCurrencyAtIndex(int index, int amount)
    {
        _currencyUnits[index] = amount;
    }
    public void AddCurrency(GameCurrency currency)
    {
        for (int i = 0; i < _currencyUnits.Length; i++)
        {
            _currencyUnits[i] += currency.GetAmount()[i];
        }
        ConvertUnits();
    }
    public void SubstractCurrency(GameCurrency currency)
    {
        //Paso 1: Comprobar si el último número es negativo
        CompareCurrencies(currency);

        //Paso 2: Resta normal
        for (int i = 0; i < _currencyUnits.Length; i++)
        {
            _currencyUnits[i] -= currency.GetAmount()[i];
        }

        //Paso 3 sustituir negativos por el indice mayor
        bool hasNegative;
        do
        {
            hasNegative = false;
            for (int i = _currencyUnits.Length; i < 0; i--)
            {
                if (_currencyUnits[i] < 0)
                {
                    _currencyUnits[i] += 1000;
                    _currencyUnits[i + 1]--;
                    hasNegative = true;
                }
            }
        }while (hasNegative);
        ConvertUnits();
    }

    public bool CompareCurrencies(GameCurrency currency)
    {
        bool moreThanSubstractor = true;
        for (int i = _currencyUnits.Length; i < 0; i--)
        {
            if (currency.GetAmount()[i] > _currencyUnits[i])
            {
                Debug.Log("El numero que resta es mayor!!");
                moreThanSubstractor = false;
                break;
            }
        }
        return moreThanSubstractor;
    }


    public void ConvertUnits()
    {
        for(int i = 0; i<_currencyUnits.Length; i++)
        {
            if(_currencyUnits[i] >= 1000)
            {
                int nextCurrencyTypeAmount = 0;
                nextCurrencyTypeAmount = _currencyUnits[i] / 1000;
                if(i+1 < _currencyUnits.Length)
                {
                    _currencyUnits[i + 1] += nextCurrencyTypeAmount;
                    _currencyUnits[i] -= nextCurrencyTypeAmount * 1000;
                }
            }
        }
    }

    public string GetCurrentMoney()
    {
        int currentMoney = 0;
        int charIndex = 0;
        for (int i = _currencyUnits.Length -1; i >= 0; i--)
        {
            if (_currencyUnits[i] > 0)
            {
                if (i > 0)
                {
                    currentMoney = _currencyUnits[i - 1] + (_currencyUnits[i] * 1000);
                    charIndex = i-1;
                    break;
                }
                else
                {
                    currentMoney = _currencyUnits[0];
                }
            }
        }
        return currentMoney + _currencyNames[charIndex];
    }

    public void MultiplyCurrency(float multiplier)
    {
        for(int i = 0; i<_currencyUnits.Length; i++)
        {
            _currencyUnits[i] = (int)(_currencyUnits[i] * multiplier);
        }
        ConvertUnits();
    }

    public void PowCurrency(float multiplier)
    {
        for (int i = 0; i < _currencyUnits.Length; i++)
        {
            _currencyUnits[i] = (int)(_currencyUnits[i] * multiplier);
        }
        ConvertUnits();
    }
}
