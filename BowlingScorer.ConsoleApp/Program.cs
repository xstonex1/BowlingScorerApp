using BowlingScorer.Domain;
using BowlingScorer.Domain.Domain;
using BowlingScorer.Domain.DTO;
using BowlingScorer.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingScorer.ConsoleApp
{
    public class Program
    {
        /// <summary>
        /// Generate a random number of scores and output a rudimentary text scoreboard.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Console.WindowWidth = 150;
            Console.WriteLine("Lets Bowl!!!!");

            string doBowl;
            do
            {
                PlayBowling();

                Console.WriteLine("Enter 1 to bowl again!\nOr enter any other key to quit.");
                doBowl = Console.ReadLine();
            } while (doBowl == "1");

            Environment.Exit(0);
        }

        public static void PlayBowling()
        {
            BowlingFrameRollsDto randomScoresDto = ScoreGenerator.GenerateRandomScoresForAGame();

            #region Custom
            BowlingFrameRollsDto customScoresDto = new BowlingFrameRollsDto
            {
                Scores = new System.Collections.Generic.List<FrameRolls>
                {
                    new FrameRolls
                    {
                        FrameNumber = 1,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 2,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 3,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 4,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 5,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 6,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 7,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 8,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 9,
                        Roll1Score = 10
                    },
                    new FrameRolls
                    {
                        FrameNumber = 10,
                        Roll1Score = 10,
                        Roll2Score = 10,
                        Roll3Score = 10
                    }
                }
            };

            #endregion

            var useTheseRolls = randomScoresDto;

            //calculate all results
            BowlingScorerService scorerService = new BowlingScorerService();
            List<BowlingFrame> allFrameScores = scorerService.CalculateScoresForEachFrameOfBowlingGame(useTheseRolls);
            FinalCalculatedGameScoresDto finalScores = scorerService.BuildFinalResultsDto(allFrameScores);

            //extract individual roll scores for scoreboard
            BowlingFrame f1Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 1);
            BowlingFrame f2Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 2);
            BowlingFrame f3Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 3);
            BowlingFrame f4Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 4);
            BowlingFrame f5Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 5);
            BowlingFrame f6Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 6);
            BowlingFrame f7Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 7);
            BowlingFrame f8Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 8);
            BowlingFrame f9Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 9);
            BowlingFrame f10Info = allFrameScores.FirstOrDefault(a => a.FrameNumber == 10);

            //output scoreboard
            Console.Write("\n_______________________________________________________________________________________________________________________________"
                       + "\n|     F1    |     F2    |     F3    |     F4    |     F5    |     F6    |     F7    |     F8    |     F9    |        F10      |"
                       + "\n_______________________________________________________________________________________________________________________________"
                       + $"\n|  {f1Info.Roll1Score}  |  {(f1Info.Roll1Score == 10 ? string.Empty : f1Info.Roll2Score.ToString())}  |" +
                         $"  {f2Info.Roll1Score}  |  {(f2Info.Roll1Score == 10 ? string.Empty : f2Info.Roll2Score.ToString())}  |" +
                         $"  {f3Info.Roll1Score}  |  {(f3Info.Roll1Score == 10 ? string.Empty : f3Info.Roll2Score.ToString())}  |" +
                         $"  {f4Info.Roll1Score}  |  {(f4Info.Roll1Score == 10 ? string.Empty : f4Info.Roll2Score.ToString())}  |" +
                         $"  {f5Info.Roll1Score}  |  {(f5Info.Roll1Score == 10 ? string.Empty : f5Info.Roll2Score.ToString())}  |" +
                         $"  {f6Info.Roll1Score}  |  {(f6Info.Roll1Score == 10 ? string.Empty : f6Info.Roll2Score.ToString())}  |" +
                         $"  {f7Info.Roll1Score}  |  {(f7Info.Roll1Score == 10 ? string.Empty : f7Info.Roll2Score.ToString())}  |" +
                         $"  {f8Info.Roll1Score}  |  {(f8Info.Roll1Score == 10 ? string.Empty : f8Info.Roll2Score.ToString())}  |" +
                         $"  {f9Info.Roll1Score}  |  {(f9Info.Roll1Score == 10 ? string.Empty : f9Info.Roll2Score.ToString())}  |" +
                         $"  {f10Info.Roll1Score}  |  {f10Info.Roll2Score}  |  {f10Info.Roll3Score}  |"
                       + "\n_______________________________________________________________________________________________________________________________"
                       + $"\nFrame 1 Score: {finalScores.Frame1Score}"
                       + $"\nFrame 2 Score: {finalScores.Frame2Score}"
                       + $"\nFrame 3 Score: {finalScores.Frame3Score}"
                       + $"\nFrame 4 Score: {finalScores.Frame4Score}"
                       + $"\nFrame 5 Score: {finalScores.Frame5Score}"
                       + $"\nFrame 6 Score: {finalScores.Frame6Score}"
                       + $"\nFrame 7 Score: {finalScores.Frame7Score}"
                       + $"\nFrame 8 Score: {finalScores.Frame8Score}"
                       + $"\nFrame 9 Score: {finalScores.Frame9Score}"
                       + $"\nFrame 10 Score: {finalScores.Frame10Score}"
                       + $"\nFinal Score: {finalScores.GameFinalScore}" +
                       "\n\n");
        }
    }
}
