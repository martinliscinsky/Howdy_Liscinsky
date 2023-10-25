namespace Howdy_Liscinsky
{
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
}