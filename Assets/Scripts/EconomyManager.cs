using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    List<DinosaurInstance> _dinosInGame;
    List<int> _earningsByType = new List<int>() { 4, 9, 19, 40, 85, 180, 380, 803, 1697, 3586, 7578, 16014, 33841, 71513, 151121, 319349, 674848 };

    public void EarnSoftCoins(int softCoins)
    {
        UserDataController.AddSoftCoins(softCoins);
    }

    public void SetDinosInGame(List<DinosaurInstance> dinosInGame)
    {
        _dinosInGame = dinosInGame;
    }

    public int GetEarningsPerSecond()
    {
        int earningsPerSecond = 0;
        foreach(DinosaurInstance d in _dinosInGame)
        {
            if (d.IsWorking())
            {
                earningsPerSecond += _earningsByType[d.GetDinosaur()];
            }
        }
        return earningsPerSecond;
    }

    public int GetEarningsByType(int dinoType)
    {
        return _earningsByType[dinoType];
    }
}
