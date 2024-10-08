﻿using Domain.Models.Enums.Task;

namespace Domain.Objects.Responses.Card
{
    public record GetCardResultsResponse
    {
        public int CardId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required ECardStatus TaskStatus { get; set; }
        public double Order { get; set; }
        public required string StudyTime { get; set; }
    }
}