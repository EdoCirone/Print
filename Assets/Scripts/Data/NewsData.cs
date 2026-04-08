using UnityEngine;

[CreateAssetMenu(fileName = "NewsData", menuName = "PRINT/NewsData")]
public class NewsData : ScriptableObject
{

    [Header("News Info")]
    public string id;
    public string newsTitle;
    [TextArea] public string description;

    [Header("News Attributes")]
    public NewsTopic topic;

    [Header("News Stats")]
    [Range(0, 100)]
    public int popularity;
    [Range(0, 100)]
    public int politicalWeight;
    [Range(0, 100)]
    public int risk;


}
