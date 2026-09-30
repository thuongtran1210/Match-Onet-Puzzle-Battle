using BeastLinkBattle.Gameplay;
using UnityEngine;
using System;
using Unity.VisualScripting.FullSerializer;

public class ComboManager : IComboService
{
    public event Action<int> OnComboUpdated;
    public event Action<float> OnTimerUpdated;
    public event Action OnComboEnded;

    public bool IsActive { get; private set; }
    public int CurrentCombo { get; private set; }
    public float TimeLeft { get; private set; }


    public float MaxTimeLimit => _config.maxTimeLimit;
    private ComboConfig _config;

    public ComboManager(ComboConfig config)
    {
        _config = config;
    }
    public void RegisterMatch()
    {
        if (!IsActive)
        {
            IsActive = true;
            CurrentCombo = 1;
            TimeLeft = _config.initialTime; 
        }
        else
        {
            CurrentCombo++;
      
            TimeLeft = Mathf.Min(TimeLeft + _config.bonusTimePerMatch, _config.maxTimeLimit);
        }

        OnComboUpdated?.Invoke(CurrentCombo);
        OnTimerUpdated?.Invoke(TimeLeft);
    }

    public void Tick(float deltaTime)
    {
        if (!IsActive) return;

        TimeLeft -= deltaTime;
        OnTimerUpdated?.Invoke(TimeLeft);

        if (TimeLeft <= 0)
        {
            EndCombo();
        }
    }

    public void ForceEndCombo() => EndCombo();

    private void EndCombo()
    {
        IsActive = false;
        CurrentCombo = 0;
        TimeLeft = 0;
        OnComboEnded?.Invoke(); 
    }
}