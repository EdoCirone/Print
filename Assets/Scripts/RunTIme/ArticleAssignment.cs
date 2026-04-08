using UnityEngine;
[System.Serializable]
public class ArticleAssignment
{
    [SerializeField] private NewsData _news;
    [SerializeField] private JournalistRuntime _journalist;
    [SerializeField] private EditorialAngleType _angleType;

    public NewsData News
    {
        get => _news;
        set => _news = value;
    }
    public JournalistRuntime Journalist
    {
        get => _journalist;
        set => _journalist = value;
    }
    public EditorialAngleType AngleType
    {
        get => _angleType;
        set => _angleType = value;
    }
}
