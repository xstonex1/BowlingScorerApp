using BowlingScorer.Domain.Domain;
using BowlingScorer.Domain.DTO;
using BowlingScorer.Domain.Shared.CustomExceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BowlingScorer.Test
{
    [TestFixture, Category("Unit")]
    public class BowlingFrameTests
    {
        [Test]
        public void BowlingFrame_Ctor_ThrowsArgumentNull_WhenFrameInfoParameterIsNull()
        {
            // act
            var exception = Assert.Throws<ArgumentNullException>(() => { BowlingFrame bFrame = new BowlingFrame(null, 0, 0); });

            // assert
            Assert.AreEqual("Value cannot be null. (Parameter 'frameInfo')", exception.Message);
        }

        [Test, TestCaseSource(nameof(RollScoredForNegativePoints_DatasetsForValidation))]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfRollWasScoredForNegativePoints(FrameRolls frameInfo)
        {
            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, 0, 0); });

            // assert
            Assert.AreEqual($"Frame {frameInfo.FrameNumber}: A roll cannot be scored for negative points", exception.Message);
        }

        [Test, TestCaseSource(nameof(RollScoredForMoreThan10Points_DatasetsForValidation))]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfRollWasScoredForMoreThan10Points(FrameRolls frameInfo)
        {
            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, 0, 0); });

            // assert
            Assert.AreEqual($"Frame {frameInfo.FrameNumber}: A roll cannot be scored for more than 10 points", exception.Message);
        }

        [Test]
        [TestCase(11, 0)]
        [TestCase(0, 11)]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfFollowUpValuesScoredForMoreThan10Points(int firstFollowUp, int secondFollowUp)
        {
            // arrange
            FrameRolls frameInfo = new FrameRolls { FrameNumber = 1, Roll1Score = 5, Roll2Score = 3, Roll3Score = 0 };

            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, firstFollowUp, secondFollowUp); });

            // assert
            Assert.AreEqual("A follow-up roll cannot count for more than 10 points", exception.Message);
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, -1)]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfFollowUpValuesScoredForNegativePoints(int firstFollowUp, int secondFollowUp)
        {
            // arrange
            FrameRolls frameInfo = new FrameRolls { FrameNumber = 1, Roll1Score = 5, Roll2Score = 3, Roll3Score = 0 };

            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, firstFollowUp, secondFollowUp); });

            // assert
            Assert.AreEqual("A follow-up roll cannot count for negative points", exception.Message);
        }

        [Test]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfOutsideTenthFrame_AndRollsScoreForMoreThan10TotalPoints()
        {
            // arrange
            FrameRolls frameInfo = new FrameRolls { FrameNumber = 1, Roll1Score = 10, Roll2Score = 1, Roll3Score = 0 };

            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, 0, 0); });

            // assert
            Assert.AreEqual($"Frame {frameInfo.FrameNumber}: The sum of 2 rolls outside the 10th frame cannot count for more than 10 points", exception.Message);
        }

        [Test]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfOutsideTenthFrame_AndThirdRollWasScored()
        {
            // arrange
            FrameRolls frameInfo = new FrameRolls { FrameNumber = 1, Roll1Score = 5, Roll2Score = 4, Roll3Score = 1 };

            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, 0, 0); });

            // assert
            Assert.AreEqual("There cannot be a third roll outside of the 10th frame", exception.Message);
        }

        [Test, TestCaseSource(nameof(Roll3AttemptedIn10thFrameWithoutStrikeOrSpare_DatasetsForValidation))]
        public void BowlingFrame_Ctor_ThrowsValidationException_IfTenthFrame_AndThirdRollWasScored_WhenNoStrikeOrSpareRecordedWithinFrame(FrameRolls frameInfo)
        {
            // act
            var exception = Assert.Throws<ValidationException>(() => { BowlingFrame bFrame = new BowlingFrame(frameInfo, 0, 0); });

            // assert
            Assert.AreEqual($"Frame 10: A player cannot roll a third time if they did not bowl a strike or spare within that frame", exception.Message);
        }

        [Test]
        public void BowlingFrame_Ctor_StoresDataAsExpected()
        {
            // arrange
            FrameRolls frameInfo = new FrameRolls { FrameNumber = 10, Roll1Score = 5, Roll2Score = 5, Roll3Score = 1 };
            int firstFollowUp = 5;
            int secondFollowUp = 7;

            // act
            BowlingFrame bFrame = new BowlingFrame(frameInfo, firstFollowUp, secondFollowUp);

            // assert
            Assert.AreEqual(frameInfo.FrameNumber, bFrame.FrameNumber);
            Assert.AreEqual(frameInfo.Roll1Score, bFrame.Roll1Score);
            Assert.AreEqual(frameInfo.Roll2Score, bFrame.Roll2Score);
            Assert.AreEqual(frameInfo.Roll3Score, bFrame.Roll3Score);
            Assert.AreEqual(firstFollowUp, bFrame.FollowUpRollScore);
            Assert.AreEqual(secondFollowUp, bFrame.SecondFollowUpRollScore);
        }

        [Test, TestCaseSource(nameof(CalculateFrameScore_DatasetsForValidation))]
        public void BowlingFrame_CalculateFrameScore_CalculatesScoreAsExpected(FrameRolls frameInfo, int firstFollowUp, int secondFollowUp, int expectedScore)
        {
            // arrange
            BowlingFrame bFrame = new BowlingFrame(frameInfo, firstFollowUp, secondFollowUp);

            // act
            var result = bFrame.CalculateFrameScore();

            // assert
            Assert.AreEqual(expectedScore, result);
        }

        private static IEnumerable<FrameRolls> RollScoredForNegativePoints_DatasetsForValidation
        {
            get
            {
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = -1, Roll2Score = 0, Roll3Score = 0 };
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = -1, Roll3Score = 0 };
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = 0, Roll3Score = -1 };
            }
        }

        private static IEnumerable<FrameRolls> RollScoredForMoreThan10Points_DatasetsForValidation
        {
            get
            {
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = 11, Roll2Score = 0, Roll3Score = 0 };
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = 11, Roll3Score = 0 };
                yield return new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = 0, Roll3Score = 11 };
            }
        }

        private static IEnumerable<FrameRolls> Roll3AttemptedIn10thFrameWithoutStrikeOrSpare_DatasetsForValidation
        {
            get
            {
                yield return new FrameRolls() { FrameNumber = 10, Roll1Score = 5, Roll2Score = 4, Roll3Score = 1 };
                yield return new FrameRolls() { FrameNumber = 10, Roll1Score = 9, Roll2Score = 0, Roll3Score = 1 };
            }
        }

        private static IEnumerable<object[]> CalculateFrameScore_DatasetsForValidation
        {
            //FrameRolls frameInfo, int firstFollowUp, int secondFollowUp, int expectedScore
            get
            {
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 5, Roll2Score = 4, Roll3Score = 0 }, 5, 5, 9 };
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 2, Roll2Score = 2, Roll3Score = 0 }, 5, 5, 4 };
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = 0, Roll3Score = 0 }, 5, 5, 0 };
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 10, Roll2Score = 0, Roll3Score = 0 }, 5, 5, 20 };
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 5, Roll2Score = 5, Roll3Score = 0 }, 5, 5, 15 };
                yield return new object[] { new FrameRolls() { FrameNumber = 1, Roll1Score = 0, Roll2Score = 10, Roll3Score = 0 }, 5, 5, 15 };
                yield return new object[] { new FrameRolls() { FrameNumber = 10, Roll1Score = 5, Roll2Score = 5, Roll3Score = 7 }, 5, 5, 17 };
                yield return new object[] { new FrameRolls() { FrameNumber = 10, Roll1Score = 10, Roll2Score = 10, Roll3Score = 10 }, 5, 5, 30 };
                yield return new object[] { new FrameRolls() { FrameNumber = 10, Roll1Score = 10, Roll2Score = 6, Roll3Score = 4 }, 5, 5, 20 };
            }
        }
    }
}
