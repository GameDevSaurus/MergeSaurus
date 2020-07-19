using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    List<DinosaurInstance> _dinosInGame;
    List<GameCurrency> _earningsByType = new List<GameCurrency>() { new GameCurrency(4), new GameCurrency(9), new GameCurrency(19), new GameCurrency(40), new GameCurrency(85), new GameCurrency(180), new GameCurrency(380), new GameCurrency(803), new GameCurrency(1697), new GameCurrency(3586), new GameCurrency(7578), new GameCurrency(16014), new GameCurrency(33841), new GameCurrency(71513), new GameCurrency(151121), new GameCurrency(319349), new GameCurrency(674848), new GameCurrency(1426677), new GameCurrency(3013439), new GameCurrency(6370677), new GameCurrency(13455677) };
    List<int> _initialCostList; 
    float _initialCostIncrementRatio = 3.092f;
    float _firstDinoIncrementRatio = 1.07f;
    float _dinoIncrementRatio = 1.157f;
    int initialCost0 = 100;
    int initialCost1 = 1500;

    private void Awake()
    {
        _initialCostList = new List<int>() {100, 1500};

        float firstDinoCost = _initialCostList[1];
        for (int i = 0; i < 10; i++)
        {
            firstDinoCost *= _initialCostIncrementRatio;
            _initialCostList.Add((int)firstDinoCost);
        }
    }
    public void EarnSoftCoins(GameCurrency softCoins)
    {
        UserDataController.AddSoftCoins(softCoins);
    }

    public void SetDinosInGame(List<DinosaurInstance> dinosInGame)
    {
        _dinosInGame = dinosInGame;
    }

    public void EarnHardCoins(int amount)
    {
        UserDataController.AddHardCoins(amount);
    }

    public bool SpendHardCoins(int amount)
    {
        bool canSpend = false;
        if(UserDataController.GetHardCoins() >= amount)
        {
            canSpend = true;
            UserDataController.SpendHardCoins(amount);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOHARDCOINS", ""));
        }
        return canSpend;
    }

    public bool SpendSoftCoins(GameCurrency amount)
    {
        bool canSpend = false;
        GameCurrency g = UserDataController.GetSoftCoins();
        g.SubstractCurrency(amount);
        int checkCurrency = int.Parse(g.GetIntAndCurrency()[0]);
        if (checkCurrency > 0)
        {
            canSpend = true;
            UserDataController.SpendSoftCoins(amount);
        }
        else
        {
            GameEvents.ShowAdvice.Invoke(new GameEvents.AdviceEventData("ADVICE_NOSOFTCOINS"));
        }
        return canSpend;
    }

    public string GetEarningsPerSecond()
    {
        GameCurrency earningsPerSecond = new GameCurrency();
        foreach(DinosaurInstance d in _dinosInGame)
        {
            if (d.IsWorking())
            {
                earningsPerSecond.AddCurrency(_earningsByType[d.GetDinosaur()]);
            }
        }
        earningsPerSecond.MultiplyCurrency(CurrentSceneManager.GetGlobalSpeed());
        return earningsPerSecond.GetCurrentMoney() + "/sec";
    }
    public GameCurrency GetTotalEarningsPerSecond()
    {
        GameCurrency earningsPerSecond = new GameCurrency();
        foreach (DinosaurInstance d in _dinosInGame)
        {
            earningsPerSecond.AddCurrency(_earningsByType[d.GetDinosaur()]);       
        }
        return earningsPerSecond;
    }

    public GameCurrency GetEarningsByType(int dinoType)
    {
        GameCurrency g = new GameCurrency(_earningsByType[dinoType].GetIntList());
        if (UserDataController.GetExtraEarningsLevel() > 0)
        {
            g.MultiplyCurrency(1f + (UpgradesManager.GetExtraEarnings() / 100f));
        }

        return g;
    }

    public GameCurrency GetDinoCost(int dinoType)
    {
        int ownedDinos = UserDataController.GetOwnedDinosByDinoType(dinoType);
        GameCurrency dinoCurrency = new GameCurrency();
        if(dinoType == 0)
        {
            dinoCurrency = GetInitialCost(dinoType);
            dinoCurrency.MultiplyCurrency(Mathf.Pow(_firstDinoIncrementRatio, ownedDinos));
        }
        else
        {
            if(dinoType >= 1)
            {
                dinoCurrency = GetInitialCost(dinoType);
                dinoCurrency.MultiplyCurrency(Mathf.Pow(_dinoIncrementRatio, ownedDinos));
            }
        }
        if(UserDataController.GetDiscountUpgradeLevel() > 0)
        {
            dinoCurrency.MultiplyCurrency(1f - (UpgradesManager.GetDiscount()/100f));
        }
        return dinoCurrency;
    }

    public GameCurrency GetInitialCost(int dinoType)
    {
        if(dinoType == 0)
        {
            return new GameCurrency(initialCost0);
        }
        else
        {
            if (dinoType == 1)
            {
                return new GameCurrency(initialCost1);
            }
            else
            {
                GameCurrency finalInitialCost = GetInitialCost(dinoType - 1);
                finalInitialCost.MultiplyCurrency(_initialCostIncrementRatio);
                return finalInitialCost;
            }
        }
    }
}
