using UnityEngine;

[CreateAssetMenu(fileName = "JournalistData", menuName = "PRINT/JournalistData")]
public class JournalistData : ScriptableObject
{
    [Header("Journalist Info")]
    public string displayName;
    //public Sprite journalistPortrait;
    public string id;
    [TextArea]public string bio;

    [Header("Journalist Attributes")]
    public PoliticalLean politicalLean;
    public NewsTopic specialization;

    [Header("Journalist Stats")]
    [Range(0, 100)]
    public int persuasion;
    [Range(0, 100)]
    public int integrity;
}
