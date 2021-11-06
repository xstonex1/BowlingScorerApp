using BowlingScorer.Domain.Domain;
using BowlingScorer.Domain.DTO;
using BowlingScorer.Domain.Shared;
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
        public BowlingScorerService()
        {
        }

        public List<BowlingFrame> CalculateScoresForEachFrameOfBowlingGame(BowlingFrameRollsDto gameScores)
        {
            if (gameScores.Scores.Count > GlobalConstants.MaxNumberFrames)
                throw new ValidationException($"A game of bowling cannot have more than {GlobalConstants.MaxNumberFrames} frames");

            List<BowlingFrame> fullGameBowlingFrames = new List<BowlingFrame>();
            var orderedFrames = gameScores.Scores.OrderBy(a => a.FrameNumber).ToList();

            for (int i = 0; i < orderedFrames.Count; i++)
            {
                int followUpRollScore = -1;
                int secondFollowUpRollScore = -1;

                if (i == orderedFrames.Count - 1)
                {
                    followUpRollScore = 0;
                    secondFollowUpRollScore = 0;
                }
                else if (i == orderedFrames.Count - 2)
                {
                    followUpRollScore = orderedFrames[orderedFrames.Count - 1].Roll1Score;
                    secondFollowUpRollScore = orderedFrames[orderedFrames.Count - 1].Roll2Score;
                }
                else
                {
                    followUpRollScore = orderedFrames[i + 1].Roll1Score;
                    secondFollowUpRollScore = orderedFrames[i + 1].Roll1Score == 10 ? orderedFrames[i + 2].Roll1Score : orderedFrames[i + 1].Roll2Score;
                }

                BowlingFrame frame = new BowlingFrame(orderedFrames[i], followUpRollScore, secondFollowUpRollScore);
                frame.CalculateFrameScore();
                fullGameBowlingFrames.Add(frame);
            }

            return fullGameBowlingFrames;
        }

        public FinalCalculatedGameScoresDto BuildFinalResultsDto(List<BowlingFrame> fullGameBowlingFrames)
        {
            FinalCalculatedGameScoresDto resultsDto = new FinalCalculatedGameScoresDto();
            resultsDto.Frame1Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 1)?.FrameScore ?? 0;
            resultsDto.Frame2Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 2)?.FrameScore ?? 0;
            resultsDto.Frame3Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 3)?.FrameScore ?? 0;
            resultsDto.Frame4Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 4)?.FrameScore ?? 0;
            resultsDto.Frame5Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 5)?.FrameScore ?? 0;
            resultsDto.Frame6Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 6)?.FrameScore ?? 0;
            resultsDto.Frame7Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 7)?.FrameScore ?? 0;
            resultsDto.Frame8Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 8)?.FrameScore ?? 0;
            resultsDto.Frame9Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 9)?.FrameScore ?? 0;
            resultsDto.Frame10Score = fullGameBowlingFrames.FirstOrDefault(a => a.FrameNumber == 10)?.FrameScore ?? 0;
            resultsDto.GameFinalScore = resultsDto.Frame1Score + resultsDto.Frame2Score + resultsDto.Frame3Score +
                resultsDto.Frame4Score + resultsDto.Frame5Score + resultsDto.Frame6Score + resultsDto.Frame7Score +
                resultsDto.Frame8Score + resultsDto.Frame9Score + resultsDto.Frame10Score;

            return resultsDto;
        }
    }
}