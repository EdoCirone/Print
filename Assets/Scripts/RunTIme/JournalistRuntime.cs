using UnityEngine;
[System.Serializable]
public class JournalistRuntime
{
    [SerializeField]private JournalistData sourceData;
    [SerializeField]private bool isAssignedToday = false;

    public JournalistData SourceData => sourceData;
    public bool IsAssignedToday { get => isAssignedToday; set => isAssignedToday = value; }
}
