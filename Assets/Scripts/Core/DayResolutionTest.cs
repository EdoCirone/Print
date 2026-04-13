using UnityEngine;

public class DayResolutionTest : MonoBehaviour
{
    [SerializeField] private NewsData _news1;
    [SerializeField] private NewsData _news2;
    [SerializeField] private JournalistData _journalistData1;
    [SerializeField] private JournalistData _journalistData2;
    [SerializeField] private EditorialAngleType _angle1;
    [SerializeField] private EditorialAngleType _angle2;

    GameSession _session;
    ArticleAssignment _assignment1;
    ArticleAssignment _assignment2;
    Report _report;

    void Start()
    {
        //Check per i null
        if (_journalistData1 == null ||  _journalistData2 == null) { Debug.Log("Assegna i giornalisti"); return; }
        if (_news1 == null || _news2 == null ){ Debug.LogError("Assegna le notizie"); return; }
        
        // creo la sessione di gioco e ci attacco i primi dati 
        _session = new GameSession();

        _session.RunState.Readers = 1000;
        _session.RunState.Credibility = 50;

        JournalistRuntime _juornalist1 = new JournalistRuntime(_journalistData1);
        JournalistRuntime _juornalist2 = new JournalistRuntime(_journalistData2);

        _assignment1 = new ArticleAssignment();
        _assignment2 = new ArticleAssignment();

        _assignment1.Journalist = _juornalist1;
        _assignment2.Journalist = _juornalist2;

        _assignment1.News = _news1;
        _assignment2.News = _news2;

        _assignment1.AngleType = _angle1;
        _assignment2.AngleType = _angle2;

        DayState dayState = _session.DayState;

        dayState.Assignments.Add(_assignment1);
        dayState.Assignments.Add(_assignment2);

        _report = DayResolutionService.ResolveDay(_session);

        Debug.Log($"cambio di lettori{ _report.deltaReaders} cambio di credibilità {_report.deltaCredibility} lettori finali {_session.RunState.Readers} credibilità finale {_session.RunState.Credibility}" );

    }

}
