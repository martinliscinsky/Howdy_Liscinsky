namespace Howdy_Liscinsky
{
    public class HowdySessionsEvaluator
    {
        public HowdySessionsEvaluator(Session[] sessions)
        {
            this.Sessions = sessions ?? (Array.Empty<Session>());
        }
        public Session[] Sessions { get; private set; }
        public void EvaluateAllGroups()
        {
            var results = GetEvaluationResults(this.Sessions);
            //...some additional processing of results
        }
        public void EvaluateSessionsOfGroup(int groupId)
        {
            var sessionsForEvaluation = this.Sessions.Where(a => a.groupId == groupId);
            var results = GetEvaluationResults(sessionsForEvaluation);
            //...some additional processing of results
        }
        private List<EvaluationResult> GetEvaluationResults(IEnumerable<Session> sessionsForEvaluation)
        {
            var results = new List<EvaluationResult>();
            var sessionsByYear = sessionsForEvaluation.GroupBy(a => a.Year).OrderBy(a => a.Key);
            foreach (var sessionsInYear in sessionsByYear)
            {
                Console.WriteLine($"=========================================");
                Console.WriteLine($"Year: {sessionsInYear.Key}");
                var sessionsByMonth = sessionsInYear.GroupBy(a => a.Month).OrderBy(a => a.Key);
                foreach (var sessionsInMonth in sessionsByMonth)
                {
                    Console.WriteLine($"---------------------------------------");
                    Console.WriteLine($"Month: {sessionsInMonth.Key}");
                    var sessionsbyGroup = sessionsInMonth.GroupBy(a => a.groupId).OrderBy(a => a.Key);
                    foreach (var sessionsOfGroup in sessionsbyGroup)
                    {
                        var groupScoreSum = sessionsOfGroup.Sum(a => a.SessionScore);
                        var numberOfSessions = sessionsOfGroup.Count();
                        var resultGroupScore = groupScoreSum / numberOfSessions;
                        var monthlyResultOfGroup = new EvaluationResult()
                        {
                            GroupId = sessionsOfGroup.Key,
                            Year = sessionsInYear.Key,
                            Month = sessionsInMonth.Key,
                            Score = resultGroupScore
                        };
                        results.Add(monthlyResultOfGroup);
                        Console.WriteLine($"    Group {sessionsOfGroup.Key}: \tScore: {resultGroupScore}\tSessions({numberOfSessions})");
                    }
                }
            }
            return results;
        }
    }
}