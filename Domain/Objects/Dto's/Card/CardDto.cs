namespace Domain.Objects.Dto_s.Card
{
    public record CardData
    {
        public required int Day { get; set; }
        public IEnumerable<CardContent>? Contents { get; set; }
    }

    public record CardContent
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int StudyTime { get; set; }
    }
}
