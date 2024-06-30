namespace PIEAPI.DataLayer.DataTransferObjects
{
    public class StatisticQuestionCountByLocation
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int LocationId { get; set; }
        public int Count { get; set; }
    }
}
