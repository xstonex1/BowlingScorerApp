using BowlingScorer.Domain;
using BowlingScorer.Domain.DTO;
using BowlingScorer.Domain.Shared;
using BowlingScorer.Domain.Shared.CustomExceptions;
using BowlingScorer.Test.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScorer.Test
{
    [TestFixture, Category("Unit")]
    public class BowlingScorerServiceTests
    {
        private static readonly Random _rand = new Random();

        [Test]
        public void BowlingScorerService_CalculateScoresForEachFrameOfBowlingGame_ThrowsValidationException_IfDtoFramesCountExceedsMaxAllowed()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(11);

            // act
            var exception = Assert.Throws<ValidationException>(() => { service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto); });

            // assert
            Assert.AreEqual($"A game of bowling cannot have more than {GlobalConstants.MaxNumberFrames} frames", exception.Message);
        }

        [Test]
        public void BowlingScorerService_CalculateScoresForEachFrameOfBowlingGame_Returns0ForFollowUpRolls_ForLastFrame()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(1);

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);

            // assert
            Assert.AreEqual(1, frameResults.Count);
            var frame = frameResults.FirstOrDefault();
            Assert.AreEqual(0, frame.FollowUpRollScore);
            Assert.AreEqual(0, frame.SecondFollowUpRollScore);
        }

        [Test]
        public void BowlingScorerService_CalculateScoresForEachFrameOfBowlingGame_ReturnsFinalFrameRollsAsFollowUps_ForNextToLastFrame()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(2);

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);

            // assert
            Assert.AreEqual(2, frameResults.Count);
            Assert.AreEqual(frameResults.FirstOrDefault(a => a.FrameNumber == 2).Roll1Score, frameResults.FirstOrDefault(a => a.FrameNumber == 1).FollowUpRollScore);
            Assert.AreEqual(frameResults.FirstOrDefault(a => a.FrameNumber == 2).Roll2Score, frameResults.FirstOrDefault(a => a.FrameNumber == 1).SecondFollowUpRollScore);
        }

        [Test]
        public void BowlingScorerService_CalculateScoresForEachFrameOfBowlingGame_ReturnsDataAsExpected_WithoutStrikes_ForFollowUpRolls()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(3);
            var firstFrame = bowlingFrameRollsDto.Scores.FirstOrDefault(a => a.FrameNumber == 1);
            firstFrame.Roll1Score = 3;
            firstFrame.Roll2Score = 3;
            var secondFrame = bowlingFrameRollsDto.Scores.FirstOrDefault(a => a.FrameNumber == 2);
            secondFrame.Roll1Score = 4;
            secondFrame.Roll2Score = 5;

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);

            // assert
            Assert.AreEqual(3, frameResults.Count);
            Assert.AreEqual(6, frameResults.FirstOrDefault(a => a.FrameNumber == 1).FrameScore);
            Assert.AreEqual(4, frameResults.FirstOrDefault(a => a.FrameNumber == 1).FollowUpRollScore);
            Assert.AreEqual(5, frameResults.FirstOrDefault(a => a.FrameNumber == 1).SecondFollowUpRollScore);
        }

        [Test]
        public void BowlingScorerService_CalculateScoresForEachFrameOfBowlingGame_ReturnsDataAsExpected_WithStrikes_ForFollowUpRolls()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(3);
            var firstFrame = bowlingFrameRollsDto.Scores.FirstOrDefault(a => a.FrameNumber == 1);
            firstFrame.Roll1Score = 10;
            firstFrame.Roll2Score = 0;
            var secondFrame = bowlingFrameRollsDto.Scores.FirstOrDefault(a => a.FrameNumber == 2);
            secondFrame.Roll1Score = 10;
            secondFrame.Roll2Score = 0;
            var thirdFrame = bowlingFrameRollsDto.Scores.FirstOrDefault(a => a.FrameNumber == 3);
            thirdFrame.Roll1Score = 9;
            thirdFrame.Roll2Score = 0;

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);

            // assert
            Assert.AreEqual(3, frameResults.Count);
            Assert.AreEqual(29, frameResults.FirstOrDefault(a => a.FrameNumber == 1).FrameScore);
            Assert.AreEqual(10, frameResults.FirstOrDefault(a => a.FrameNumber == 1).FollowUpRollScore);
            Assert.AreEqual(9, frameResults.FirstOrDefault(a => a.FrameNumber == 1).SecondFollowUpRollScore);
        }

        [Test]
        public void BowlingScorerService_BuildFinalResultsDto_BuildsDtoAsExpected()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScoresForAGame();

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);
            var finalGameResults = service.BuildFinalResultsDto(frameResults);

            // assert
            int f1Score = frameResults.FirstOrDefault(a => a.FrameNumber == 1).FrameScore;
            int f2Score = frameResults.FirstOrDefault(a => a.FrameNumber == 2).FrameScore;
            int f3Score = frameResults.FirstOrDefault(a => a.FrameNumber == 3).FrameScore;
            int f4Score = frameResults.FirstOrDefault(a => a.FrameNumber == 4).FrameScore;
            int f5Score = frameResults.FirstOrDefault(a => a.FrameNumber == 5).FrameScore;
            int f6Score = frameResults.FirstOrDefault(a => a.FrameNumber == 6).FrameScore;
            int f7Score = frameResults.FirstOrDefault(a => a.FrameNumber == 7).FrameScore;
            int f8Score = frameResults.FirstOrDefault(a => a.FrameNumber == 8).FrameScore;
            int f9Score = frameResults.FirstOrDefault(a => a.FrameNumber == 9).FrameScore;
            int f10Score = frameResults.FirstOrDefault(a => a.FrameNumber == 10).FrameScore;
            int finalScore = f1Score + f2Score + f3Score + f4Score + f5Score + f6Score + f7Score + f8Score + f9Score + f10Score;

            Assert.AreEqual(f1Score, finalGameResults.Frame1Score);
            Assert.AreEqual(f2Score, finalGameResults.Frame2Score);
            Assert.AreEqual(f3Score, finalGameResults.Frame3Score);
            Assert.AreEqual(f4Score, finalGameResults.Frame4Score);
            Assert.AreEqual(f5Score, finalGameResults.Frame5Score);
            Assert.AreEqual(f6Score, finalGameResults.Frame6Score);
            Assert.AreEqual(f7Score, finalGameResults.Frame7Score);
            Assert.AreEqual(f8Score, finalGameResults.Frame8Score);
            Assert.AreEqual(f9Score, finalGameResults.Frame9Score);
            Assert.AreEqual(f10Score, finalGameResults.Frame10Score);
            Assert.AreEqual(finalScore, finalGameResults.GameFinalScore);
        }

        [Test]
        public void BowlingScorerService_BuildFinalResultsDto_HandlesShorterGameScoresWithoutThrowingException()
        {
            // arrange
            BowlingScorerService service = new BowlingScorerService();
            BowlingFrameRollsDto bowlingFrameRollsDto = ScoreGenerator.GenerateRandomScores(1);

            // act
            var frameResults = service.CalculateScoresForEachFrameOfBowlingGame(bowlingFrameRollsDto);
            var finalGameResults = service.BuildFinalResultsDto(frameResults);

            // assert
            int f1Score = frameResults.FirstOrDefault(a => a.FrameNumber == 1).FrameScore;

            Assert.AreEqual(f1Score, finalGameResults.Frame1Score);
            Assert.AreEqual(0, finalGameResults.Frame2Score);
            Assert.AreEqual(0, finalGameResults.Frame3Score);
            Assert.AreEqual(0, finalGameResults.Frame4Score);
            Assert.AreEqual(0, finalGameResults.Frame5Score);
            Assert.AreEqual(0, finalGameResults.Frame6Score);
            Assert.AreEqual(0, finalGameResults.Frame7Score);
            Assert.AreEqual(0, finalGameResults.Frame8Score);
            Assert.AreEqual(0, finalGameResults.Frame9Score);
            Assert.AreEqual(0, finalGameResults.Frame10Score);
            Assert.AreEqual(f1Score, finalGameResults.GameFinalScore);
        }

        private List<FrameRolls> GenerateListOfRandomFrameRolls(int numberToGenerate)
        {
            List<FrameRolls> data = new List<FrameRolls>();
            for (int i = 0; i < numberToGenerate; i++)
            {
                data.Add(new FrameRolls
                {
                    FrameNumber = _rand.Next(1, 11),
                    Roll1Score = _rand.Next(1, 11),
                    Roll2Score = _rand.Next(1, 11),
                    Roll3Score = _rand.Next(1, 11)
                });
            }

            return data;
        }
    }
}
