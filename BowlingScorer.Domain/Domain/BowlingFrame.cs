using BowlingScorer.Domain.DTO;
using BowlingScorer.Domain.Shared.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingScorer.Domain.Domain
{
    public class BowlingFrame
    {
        public int FrameNumber { get; private set; }
        public int Roll1Score { get; private set; }
        public int Roll2Score { get; private set; }
        public int Roll3Score { get; private set; }
        public int FollowUpRollScore { get; private set; }
        public int SecondFollowUpRollScore { get; private set; }
        public int FrameScore { get; private set; }

        private bool isSpare { get; set; } = false;
        private bool isStrike { get; set; } = false;
        private bool hasScoreBeenCalculated { get; set; } = false;

        public BowlingFrame(FrameRolls frameInfo, int followUpRollScore, int secondFollowUpRollScore)
        {
            if (frameInfo == null) throw new ArgumentNullException("frameInfo");
            ValidateInputParameters(frameInfo, followUpRollScore, secondFollowUpRollScore);

            FrameNumber = frameInfo.FrameNumber;
            Roll1Score = frameInfo.Roll1Score;
            Roll2Score = frameInfo.Roll2Score;
            Roll3Score = frameInfo.Roll3Score;
            FollowUpRollScore = followUpRollScore;
            SecondFollowUpRollScore = secondFollowUpRollScore;
        }

        private void ValidateInputParameters(FrameRolls frameInfo, int followUpRollScore, int secondFollowUpRollScore)
        {
            if (frameInfo.Roll1Score < 0 || frameInfo.Roll2Score < 0 || frameInfo.Roll3Score < 0)
                throw new ValidationException($"Frame {frameInfo.FrameNumber}: A roll cannot be scored for negative points");

            if (frameInfo.Roll1Score > 10 || frameInfo.Roll2Score > 10 || frameInfo.Roll3Score > 10)
                throw new ValidationException($"Frame {frameInfo.FrameNumber}: A roll cannot be scored for more than 10 points");

            if (followUpRollScore > 10 || secondFollowUpRollScore > 10)
                throw new ValidationException("A follow-up roll cannot count for more than 10 points");

            if (followUpRollScore < 0 || secondFollowUpRollScore < 0)
                throw new ValidationException("A follow-up roll cannot count for negative points");

            if (frameInfo.FrameNumber != 10 && (frameInfo.Roll1Score + frameInfo.Roll2Score > 10))
                throw new ValidationException($"Frame {frameInfo.FrameNumber}: The sum of 2 rolls outside the 10th frame cannot count for more than 10 points");

            if (frameInfo.FrameNumber != 10 && frameInfo.Roll3Score > 0)
                throw new ValidationException("There cannot be a third roll outside of the 10th frame");

            if (frameInfo.FrameNumber == 10 && frameInfo.Roll3Score > 0 && frameInfo.Roll1Score + frameInfo.Roll2Score != 10 && frameInfo.Roll1Score != 10)
                throw new ValidationException("Frame 10: A player cannot roll a third time if they did not bowl a strike or spare within that frame");
        }

        public int CalculateFrameScore()
        {
            if (!hasScoreBeenCalculated)
            {
                if (FrameNumber != 10)
                {
                    if (Roll1Score == 10)
                        isStrike = true;
                    else if (Roll1Score + Roll2Score == 10)
                        isSpare = true;
                }

                FrameScore = isStrike ? Roll1Score + FollowUpRollScore + SecondFollowUpRollScore :
                    isSpare ? Roll1Score + Roll2Score + FollowUpRollScore :
                    Roll1Score + Roll2Score + Roll3Score;
                hasScoreBeenCalculated = true;
            }

            return FrameScore;
        }
    }
}
