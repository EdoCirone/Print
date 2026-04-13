
public class Report 
{ 
    public int deltaReaders;
    public int deltaCredibility;
}

public static class DayResolutionService
{
    public static Report ResolveDay(GameSession session)
    {
        int totalReaders = 0;
        int totalCredibility = 0;

        var assignments = session.DayState.Assignments;

        foreach (var assignment in assignments)
        {
            // mi prendo i dati del giornalista assegnato e della notizia assegnatagli per non dover scrivere ogni volta assignment.Journalist.SourceData e per comodità
            JournalistData journalist = assignment.Journalist.SourceData;
            NewsData news = assignment.News;

            //Valuto se l'assegnazione è giusta
            NewsTopic journalistTopic = journalist.specialization;
            NewsTopic newsTopic = news.topic;

            float matchMultiplier = 1f;

            if (journalistTopic == newsTopic) matchMultiplier = 1.5f;

            //Considero l'angolo con cui affronto la notizia sia per readers che per credibility

            float readersAngleMultiplyer = 0;
            float credibilityBase = 0;

            switch (assignment.AngleType)
            {
                case EditorialAngleType.Truth:
                    readersAngleMultiplyer = 1f;
                    credibilityBase = 10f;
                    break;
                case EditorialAngleType.Sensationalism:
                    readersAngleMultiplyer = 1.5f;
                    credibilityBase = -8f;
                    break;
                case EditorialAngleType.Propaganda:
                    readersAngleMultiplyer = 1.3f;
                    credibilityBase = -12f;
                    break;

            }

            //Faccio i calcoli

            totalReaders += (int)(news.popularity * (journalist.persuasion / 50f) * readersAngleMultiplyer * matchMultiplier);
            totalCredibility += (int)( news.risk/50f * (journalist.integrity / 50f) * credibilityBase);
        }

        session.RunState.Readers += totalReaders;
        session.RunState.Credibility += totalCredibility;
        return new Report() { deltaReaders = totalReaders, deltaCredibility = totalCredibility };
    }
}