namespace Domain.Objects.Responses.Asset
{
    public record GetBoardResultsResponse
    {
        public int BoardId { get; set; }
        public required string Name { get; set; }
        public required string Theme { get; set; }
        public required DateTime ExamDateTime { get; set; }
    }
}

