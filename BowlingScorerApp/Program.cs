using BowlingScorer.Domain;
using BowlingScorer.Domain.DTO;
using System;
using System.Linq;

namespace BowlingScorerApp
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

            var useTheseRolls = randomScoresDto;
            FrameRolls f1Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 1);
            FrameRolls f2Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 2);
            FrameRolls f3Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 3);
            FrameRolls f4Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 4);
            FrameRolls f5Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 5);
            FrameRolls f6Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 6);
            FrameRolls f7Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 7);
            FrameRolls f8Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 8);
            FrameRolls f9Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 9);
            FrameRolls f10Scores = useTheseRolls.Scores.FirstOrDefault(a => a.FrameNumber == 10);

            BowlingScorerService scorerService = new BowlingScorerService();
            FinalCalculatedGameScoresDto finalScores = scorerService.CalculateScoresForFullGameOfBowling(useTheseRolls);

            Console.Write("\n_______________________________________________________________________________________________________________________________"
                       + "\n|     F1    |     F2    |     F3    |     F4    |     F5    |     F6    |     F7    |     F8    |     F9    |        F10      |"
                       + "\n_______________________________________________________________________________________________________________________________"
                       + $"\n|  {f1Scores.Roll1Score}  |  {(f1Scores.Roll1Score == 10 ? string.Empty : f1Scores.Roll2Score.ToString())}  |" +
                         $"  {f2Scores.Roll1Score}  |  {(f2Scores.Roll1Score == 10 ? string.Empty : f2Scores.Roll2Score.ToString())}  |" +
                         $"  {f3Scores.Roll1Score}  |  {(f3Scores.Roll1Score == 10 ? string.Empty : f3Scores.Roll2Score.ToString())}  |" +
                         $"  {f4Scores.Roll1Score}  |  {(f4Scores.Roll1Score == 10 ? string.Empty : f4Scores.Roll2Score.ToString())}  |" +
                         $"  {f5Scores.Roll1Score}  |  {(f5Scores.Roll1Score == 10 ? string.Empty : f5Scores.Roll2Score.ToString())}  |" +
                         $"  {f6Scores.Roll1Score}  |  {(f6Scores.Roll1Score == 10 ? string.Empty : f6Scores.Roll2Score.ToString())}  |" +
                         $"  {f7Scores.Roll1Score}  |  {(f7Scores.Roll1Score == 10 ? string.Empty : f7Scores.Roll2Score.ToString())}  |" +
                         $"  {f8Scores.Roll1Score}  |  {(f8Scores.Roll1Score == 10 ? string.Empty : f8Scores.Roll2Score.ToString())}  |" +
                         $"  {f9Scores.Roll1Score}  |  {(f9Scores.Roll1Score == 10 ? string.Empty : f9Scores.Roll2Score.ToString())}  |" +
                         $"  {f10Scores.Roll1Score}  |  {f10Scores.Roll2Score}  |  {f10Scores.Roll3Score}  |"
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
