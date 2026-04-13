using UnityEngine;
[System.Serializable]
public class GameSession
{
    [SerializeField] private DayState _dayState = new DayState();
    [SerializeField] private RunState _runState = new RunState();

    public DayState DayState => _dayState;
    public RunState RunState => _runState;
}
