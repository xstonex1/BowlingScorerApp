using System;
using System.Collections.Generic;

namespace BowlingScorer.Domain.DTO
{
    public class BowlingFrameRollsDto
    {
        public List<FrameRolls> Scores { get; set; }
    }

    public class FrameRolls
    {
        public int FrameNumber { get; set; }
        public int Roll1Score { get; set; }
        public int Roll2Score { get; set; }
        public int Roll3Score { get; set; }
    }
}
