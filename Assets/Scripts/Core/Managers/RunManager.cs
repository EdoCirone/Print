using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    //public static event UnityAction OnHiringState;
    //public static event UnityAction OnNewsSelectionState;
    //public static event UnityAction OnAssignmentState;
    //public static event UnityAction OnReportState;

    //public static event UnityAction OnDay1;

    [Header("Initial Run Settings")]
    [SerializeField] int _initialReaders = 1000;
    [SerializeField] int _initialCredibility = 50;

    [Header("Journalist Run")]
    [SerializeField] private List<JournalistData> _allJournalists = new List<JournalistData>();
    [SerializeField] int _hiringCandidatesCount = 5;
    [SerializeField] private int _numberOfHiringJournalists = 3;

    [Header("News Run")]
    [SerializeField] private List<NewsData> _allNews = new List<NewsData>();
    [SerializeField] private int _newsPerDay = 5;

    [Header("Debug")]
    [SerializeField] private JournalistData[] _debugSelectedJournalists;
    [SerializeField] private GameSession _session = new GameSession();

    private void Awake()
    {
        GameManager.instance.RegisterRunManager(this);
    }

    private void GoToState(GameState state)
    {
        switch (state)
        {

            case GameState.Hiring:
                Debug.Log("Stato: Hiring");
                break;
            case GameState.NewsSelection:
                Debug.Log("Stato: NewsSelection");
                break;
            case GameState.Assignment:
                Debug.Log("Stato: Assignment");
                break;
            case GameState.Report:
                Debug.Log("Stato: Report");
                break;
        }

    }

    private void StartDay()
    {
        ResetDayState();
        AddDayNews();
        GoToState(GameState.NewsSelection);

    }

    private void EndDay()
    {
        _session.RunState.CurrentDay++;
    }
    private void ResetDayState()
    {
        foreach (var journalist in _session.RunState.HiredJournalists)
        {
            journalist.IsAssignedToday = false;
        }
        _session.DayState.Assignments.Clear();
        _session.DayState.CurrentDayNews.Clear();
    }

    private void AddDayNews()
    {
        _session.DayState.CurrentDayNews.Clear();
        List<NewsData> newsPool = new List<NewsData>(_allNews);
        ShuffleList(newsPool);
        int newsToSelect = Mathf.Min(_newsPerDay, newsPool.Count);
        for (int i = 0; i < newsToSelect; i++)
        {
            _session.DayState.CurrentDayNews.Add(newsPool[i]);
        }
    }

    //private void AssignNewsToJournalist()
    //{
    //    List<JournalistRuntime> availableJournalists = _session.RunState.HiredJournalists.Where(j => !_session.DayState.Assignments.Any(a => a.Journalist == j)).ToList();
    //}

    private void GenerateHiringCandidates()
    {
        _session.RunState.CurrentHiringCandidates.Clear();
        List<JournalistData> candidatesPool = new List<JournalistData>(_allJournalists);
        ShuffleList(candidatesPool);
        int candidatesToSelect = Mathf.Min(_hiringCandidatesCount, candidatesPool.Count);
        for (int i = 0; i < candidatesToSelect; i++)
        {
            _session.RunState.CurrentHiringCandidates.Add(candidatesPool[i]);
        }

    }

    private void ConfirmHiring(JournalistData[] selectedJournalists)
    {
        List<JournalistData> validSelections = new List<JournalistData>(); //questa lista mi serve per controllare che non ci siano duplicati e che non ci siano null, se tutto è ok allora prendo i dati da questa lista per creare i JournalistRuntime da aggiungere alla runState.HiredJournalists

        foreach (var selected in selectedJournalists)
        {
            if (selected == null)
            {
                continue;
            }

            if (validSelections.Contains(selected))
            {
                Debug.Log("Hai selezionato lo stesso giornalista più di una volta. Seleziona giornalisti diversi.");
                return;
            }

            if (!_session.RunState.CurrentHiringCandidates.Contains(selected))
            {
                Debug.Log($"Il giornalista {selected.displayName} non è tra i candidati disponibili. Seleziona solo giornalisti disponibili.");
                return;
            }

            validSelections.Add(selected);
        }

        int actualNumberOfHiringJournalists = Mathf.Min(_numberOfHiringJournalists, _session.RunState.CurrentHiringCandidates.Count); //Quando avrò più SO non seriverà più questa limitazione, ma per ora è per evitare che il giocatore debba selezionare 3 giornalisti quando ne vengono generati solo 2 ad esempio

        if (validSelections.Count != actualNumberOfHiringJournalists)
        {
            Debug.Log($"Devi selezionare esattamente {actualNumberOfHiringJournalists} giornalisti. Selezionati: {validSelections.Count}");
            return;
        }

        foreach (var selected in validSelections)
        {
            bool alreadyHired = _session.RunState.HiredJournalists.Any(j => j.SourceData == selected); //Controllo che non stiano nella lista dello stato
            if (alreadyHired)
            {
                Debug.Log($"Il giornalista {selected.displayName} è già stato assunto. Seleziona giornalisti diversi.");
                return;
            }
            JournalistRuntime newHire = new JournalistRuntime(selected);

            _session.RunState.HiredJournalists.Add(newHire);

        }

        StartDay();
    }

    private void StartRun()
    {
        _session = new GameSession();
        _session.RunState.CurrentDay = 1;

        _session.RunState.Readers = _initialReaders;
        _session.RunState.Credibility = _initialCredibility;

        GenerateHiringCandidates();

        GoToState(GameState.Hiring);
    }

    //Utility Da spostare in una static appena è utile
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    // *************** DEBUG METHODS *****************

    [ContextMenu("DEBUG/Start Run")]
    private void DebugStartRun()
    {
        StartRun();
    }

    [ContextMenu("DEBUG/Confirm Hiring")]
    private void DebugConfirmHiring()
    {
        ConfirmHiring(_debugSelectedJournalists);
    }

    [ContextMenu("DEBUG/Assignment test")]

    private void DebugAssignmentTest()
    {
        if (_session.RunState.HiredJournalists.Count == 0)
        {
            Debug.Log("Non ci sono giornalisti assunti. Assumi almeno un giornalista per testare le assegnazioni.");
            return;
        }
        if (_session.DayState.CurrentDayNews.Count == 0)
        {
            Debug.Log("Non ci sono notizie per il giorno corrente. Avvia un giorno per testare le assegnazioni.");
            return;
        }


        // Assegna la prima notizia al primo giornalista con un angolo di Sensazionalismo
        var journalist = _session.RunState.HiredJournalists[0];
        var news = _session.DayState.CurrentDayNews[0];

        if (journalist.IsAssignedToday) { Debug.Log("Il giornalista è già impegnato"); return; }

        ArticleAssignment assignment = new ArticleAssignment
        {
            Journalist = journalist,
            News = news,
            AngleType = EditorialAngleType.Sensationalism
        };


        _session.DayState.Assignments.Add(assignment);
        journalist.IsAssignedToday = true;
    }

    [ContextMenu("DEBUG/Test All Avaible Assignment")]

    private void DebugAssignAllAvailable()
    {
        if (_session.RunState.HiredJournalists.Count == 0)
        {
            Debug.Log("Non ci sono giornalisti assunti.");
            return;
        }

        if (_session.DayState.CurrentDayNews.Count == 0)
        {
            Debug.Log("Non ci sono notizie del giorno.");
            return;
        }

        int assignmentsToCreate = Mathf.Min(_session.RunState.HiredJournalists.Count, _session.DayState.CurrentDayNews.Count) - _session.DayState.Assignments.Count;
        foreach (var journalist in _session.RunState.HiredJournalists)
        {
            if (assignmentsToCreate <= 0) break;
            if (journalist.IsAssignedToday) continue;
            var news = _session.DayState.CurrentDayNews.FirstOrDefault(n => !_session.DayState.Assignments.Any(a => a.News == n));
            if (news == null) break;
            ArticleAssignment assignment = new ArticleAssignment
            {
                Journalist = journalist,
                News = news,
                AngleType = EditorialAngleType.Truth
            };
            _session.DayState.Assignments.Add(assignment);
            journalist.IsAssignedToday = true;
            assignmentsToCreate--;
        }
    }

    [ContextMenu("DEBUG/Reset Assignments")]
    private void DebugResetAssignments()
    {
        foreach (var assignment in _session.DayState.Assignments)
        {
            assignment.Journalist.IsAssignedToday = false;
        }
        _session.DayState.Assignments.Clear();
    }

    [ContextMenu("DEBUG/End Day")]

    private void DebugEndDay()
    {
        if (_session.RunState.HiredJournalists.Count == 0)
        {
            Debug.Log("Non ci sono giornalisti assunti.");
            return;
        }

        if (_session.DayState.CurrentDayNews.Count == 0)
        {
            Debug.Log("Non ci sono notizie del giorno.");
            return;
        }

        if (_session.DayState.Assignments.Count == 0)
        {
            Debug.Log("Non ci sono assegnazioni per il giorno corrente. Assegna almeno una notizia a un giornalista per terminare il giorno.");
            return;
        }
        Report report = DayResolutionService.ResolveDay(_session);
        Debug.Log($"Giorno {_session.RunState.CurrentDay} risolto. Cambio Lettori: {report.deltaReaders}, Cambio Credibilità: {report.deltaCredibility} Lettori totali: {_session.RunState.Readers}, Credibilità totale: {_session.RunState.Credibility}");
        EndDay();
        StartDay();
    }
}


