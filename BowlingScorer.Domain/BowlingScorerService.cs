using BowlingScorer.Domain.Domain;
using BowlingScorer.Domain.DTO;
using BowlingScorer.Domain.Shared.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingScorer.Domain
{
    /// <summary>
    /// This service is used to calculate the score of a bowling game once all rolls have been completed
    /// </summary>
    public class BowlingScorerService
    {
        internal const int _maxNumberFrames = 10;

        public BowlingScorerService()
        {
        }

        public FinalCalculatedGameScoresDto CalculateScoresForFullGameOfBowling(BowlingFrameRollsDto gameScores)
        {
            if (gameScores.Scores.Count > _maxNumberFrames)
                throw new ValidationException($"A game of bowling cannot have more than {_maxNumberFrames} frames");

            List<BowlingFrame> fullGameBowlingFrames = new List<BowlingFrame>();
            var orderedFrames = gameScores.Scores.OrderBy(a => a.FrameNumber).ToList();

            for (int i = 0; i < _maxNumberFrames; i++)
            {
                int followUpRollScore = -1;
                int secondFollowUpRollScore = -1;

                if (i == _maxNumberFrames - 1)
                {
                    followUpRollScore = 0;
                    secondFollowUpRollScore = 0;
                }
                else if (i == _maxNumberFrames - 2)
                {
                    followUpRollScore = orderedFrames[_maxNumberFrames - 1].Roll1Score;
                    secondFollowUpRollScore = orderedFrames[_maxNumberFrames - 1].Roll2Score;
                }
                else
                {
                    followUpRollScore = orderedFrames[i + 1].Roll1Score;
                    secondFollowUpRollScore = orderedFrames[i + 1].Roll1Score == 10 ? orderedFrames[i + 2].Roll1Score : orderedFrames[i + 1].Roll2Score;
                }

                BowlingFrame frame = new BowlingFrame(orderedFrames[i], followUpRollScore, secondFollowUpRollScore);
                fullGameBowlingFrames.Add(frame);
            }

            FinalCalculatedGameScoresDto resultsDto = BuildFinalResultsDto(fullGameBowlingFrames);
            return resultsDto;
        }

        private FinalCalculatedGameScoresDto BuildFinalResultsDto(List<BowlingFrame> fullGameBowlingFrames)
        {
            FinalCalculatedGameScoresDto resultsDto = new FinalCalculatedGameScoresDto();
            resultsDto.Frame1Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 1).FrameScore;
            resultsDto.Frame2Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 2).FrameScore;
            resultsDto.Frame3Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 3).FrameScore;
            resultsDto.Frame4Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 4).FrameScore;
            resultsDto.Frame5Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 5).FrameScore;
            resultsDto.Frame6Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 6).FrameScore;
            resultsDto.Frame7Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 7).FrameScore;
            resultsDto.Frame8Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 8).FrameScore;
            resultsDto.Frame9Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 9).FrameScore;
            resultsDto.Frame10Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 10).FrameScore;
            resultsDto.GameFinalScore = resultsDto.Frame1Score + resultsDto.Frame2Score + resultsDto.Frame3Score +
                resultsDto.Frame4Score + resultsDto.Frame5Score + resultsDto.Frame6Score + resultsDto.Frame7Score +
                resultsDto.Frame8Score + resultsDto.Frame9Score + resultsDto.Frame10Score;

            return resultsDto;
        }
    }
}