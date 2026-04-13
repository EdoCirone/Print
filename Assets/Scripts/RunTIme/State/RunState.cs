
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RunState
{
    [SerializeField] private int _dayNumber;
    [SerializeField] private int _readers;
    [SerializeField] private int _credibility;

    [SerializeField] private List<JournalistRuntime> _hiredJournalists = new List<JournalistRuntime>();
    [SerializeField] private List<JournalistData> _currentHiringCandidates = new List<JournalistData>() ;


    public int CurrentDay { get => _dayNumber; set => _dayNumber = value; }
    public int Readers { get => _readers; set => _readers = value; }
    public int Credibility { get => _credibility; set => _credibility = value; }

    public List<JournalistRuntime> HiredJournalists => _hiredJournalists;
    public List<JournalistData> CurrentHiringCandidates => _currentHiringCandidates;


}
