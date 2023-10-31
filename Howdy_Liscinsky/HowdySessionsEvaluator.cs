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
        private IEnumerable<EvaluationResult> GetEvaluationResults(IEnumerable<Session> sessionsForEvaluation)
        {
            var results = new List<EvaluationResult>();
            var evaluationResults = sessionsForEvaluation.GroupBy(a => new { a.Year, a.Month, a.groupId })
                .Select(a =>
            {
                //var score = a.Average(d=>d.SessionScore);
                var score = a.GroupBy(session => session.employeeId).Select(employeeSessions => employeeSessions.Average(f => f.SessionScore)).Average();
                return new EvaluationResult()
                {
                    Year = a.Key.Year,
                    Month = a.Key.Month,
                    GroupId = a.Key.groupId,
                    Score = score
                };
            }).OrderBy(a => a.Year).ThenBy(b => b.Month).ThenBy(c => c.GroupId);

            foreach (var res in evaluationResults)
            {
                Console.WriteLine($"Year: {res.Year}, Month: {res.Month}, GroupId: {res.GroupId}, Score: {res.Score}");
            }

            return evaluationResults;
        }
    }
}