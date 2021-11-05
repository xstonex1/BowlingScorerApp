using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingScorer.Domain.DTO
{
    public class FinalCalculatedGameScoresDto
    {
        public int Frame1Score { get; set; }
        public int Frame2Score { get; set; }
        public int Frame3Score { get; set; }
        public int Frame4Score { get; set; }
        public int Frame5Score { get; set; }
        public int Frame6Score { get; set; }
        public int Frame7Score { get; set; }
        public int Frame8Score { get; set; }
        public int Frame9Score { get; set; }
        public int Frame10Score { get; set; }
        public int GameFinalScore { get; set; }
    }
}
