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
}