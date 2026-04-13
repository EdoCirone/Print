using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DayState
{
    [SerializeField] private List<NewsData> _currentDayNews = new List<NewsData>();
    [SerializeField] private List<ArticleAssignment> _assignments = new List<ArticleAssignment>();

    public List<NewsData> CurrentDayNews => _currentDayNews;
    public List<ArticleAssignment> Assignments => _assignments;

}
