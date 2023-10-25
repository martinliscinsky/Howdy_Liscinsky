using System.Text.Json;

namespace Howdy_Liscinsky
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var pathToFile = Path.Combine(currentDirectory, "answers.json");
            var sessions = ReadSessionsFromFile(pathToFile);
            if (sessions != null)
            {
                var evaluator = new HowdySessionsEvaluator(sessions);

                Console.WriteLine("Evaluation of all groups:");
                evaluator.EvaluateAllGroups();

                Console.WriteLine("\n\nEvaluation of group:");
                evaluator.EvaluateSessionsOfGroup(3);
            }
        }

        private static Session[] ReadSessionsFromFile(string pathToFile)
        {
            Session[]? sessions = default;
            try
            {
                var jsonString = File.ReadAllText(pathToFile);
                sessions = JsonSerializer.Deserialize<Session[]>(jsonString);
            }
            catch (IOException e)
            {
                Console.WriteLine($"An error occurred while reading the file: {e.Message}");
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Error deserializing JSON: {e.Message}");
            }
            return sessions;
        }
    }
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
    public class Session
    {
        public int groupId { get; set; }
        public int employeeId { get; set; }
        public DateTime answeredOn { get; set; }
        public int answer1 { get; set; }
        public int answer2 { get; set; }
        public int answer3 { get; set; }
        public int answer4 { get; set; }
        public int answer5 { get; set; }

        public double SessionScore { get { return (answer1 + answer2 + answer3 + answer4 + answer5) / 5; } }
        public int Month { get { return answeredOn.Month; } }
        public int Year { get { return answeredOn.Year; } }
    }
    public class EvaluationResult
    {
        public int GroupId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Score { get; set; }
    }
}