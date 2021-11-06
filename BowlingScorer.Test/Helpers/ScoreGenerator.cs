using BowlingScorer.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScorer.Test.Helpers
{
    public class ScoreGenerator
    {
        private static Random _rand = new Random();

        public static BowlingFrameRollsDto GenerateRandomScoresForAGame()
        {
            List<FrameRolls> frameScoresList = new List<FrameRolls>();

            for (int i=1; i<11; i++)
            {
                frameScoresList.Add(GenerateRandomFrameScores(i));
            }

            return new BowlingFrameRollsDto
            {
                Scores = frameScoresList
            };
        }

        public static BowlingFrameRollsDto GenerateRandomScores(int numberOfFrames)
        {
            List<FrameRolls> frameScoresList = new List<FrameRolls>();

            for (int i = 1; i < numberOfFrames + 1; i++)
            {
                frameScoresList.Add(GenerateRandomFrameScores(i));
            }

            return new BowlingFrameRollsDto
            {
                Scores = frameScoresList
            };
        }

        private static FrameRolls GenerateRandomFrameScores(int i)
        {
            int roll1Score = _rand.Next(0, 11);
            int roll2Score = 0;
            int roll3Score = 0;

            if (i == 10 && roll1Score == 10)
                roll2Score = _rand.Next(0, 11);
            else if (roll1Score != 10)
                roll2Score = _rand.Next(0, 10 - roll1Score + 1);
            
            if (i == 10 && (roll1Score + roll2Score == 10 || (roll1Score + roll2Score == 20))) 
                roll3Score = _rand.Next(0, 11);
            else if (i == 10 && roll1Score == 10 && roll2Score != 10)
                roll3Score = _rand.Next(0, 10 - roll2Score + 1);

            return new FrameRolls
            {
                FrameNumber = i,
                Roll1Score = roll1Score,
                Roll2Score = roll2Score,
                Roll3Score = roll3Score
            };
        }
    }
}
