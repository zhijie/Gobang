using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Competition.Gomoku.Core;

namespace _99BFE8D0_9396_4dc1_A3BD_1EED11548B10_
{
    public partial class PlayStrategy : IPlayStrategy
    {
        /// <summary>
        /// search score table to find the best place to put chess
        /// </summary>
        /// <returns>best position</returns>
        
        //record invalid positions
        List<Position> invalidBestPos = new List<Position>();

        private Position FindBestPos(ref IChessBoard chessBoard,PieceTypeEnum pieceType)
        {
            //generally con not happen,all the position is not Okay
            if (invalidBestPos.Capacity>=15*15)
            {
                return invalidBestPos[(new Random()).Next(invalidBestPos.Count)];
            }

            List<Position> bestPosList = new List<Position>();
            bestPosList.Add(new Position(0, 0));
            //find the point whose score is highest
            for (int row = 0; row < 15; row++)
            {
                for (int col = 0; col < 15; col++)
                {
                    if (!invalidBestPos.Contains(new Position(row, col)))
                    {
                        if (scoreTable[col,row] > scoreTable[bestPosList[0].Col,bestPosList[0].Row])
                        {
                            bestPosList.Clear();
                            bestPosList.Add(new Position(row,col));
                        }
                        else if (scoreTable[col,row] == scoreTable[bestPosList[0].Col,bestPosList[0].Row])
                        {
                            bestPosList.Add(new Position(row, col));
                        }
                    }
                }
            }

            //generally connot happen,there is no bestPos
            if (bestPosList.Count==0)
            {
                return new Position(-1,-1);
            }

            Random rnd=new Random();
            int index=rnd.Next(bestPosList.Count);
            while (IsFobidden(ref chessBoard, bestPosList[index],pieceType))
            {
                invalidBestPos.Add(bestPosList[index]);
                bestPosList.RemoveAt(index);
                index = rnd.Next(bestPosList.Count);
                if (index == 0)
                {
                    FindBestPos(ref chessBoard, pieceType);
                }               
            }


            invalidBestPos.Clear();
            return bestPosList[index];
        }

        /// <summary>
        /// check whether the position is forbidden
        /// </summary>
        /// <returns>if it is forbidden ,true;otherwise ,false
        /// </returns>
        private bool IsFobidden(ref IChessBoard chessBoard,Position point,PieceTypeEnum pieceType)
        {
            Referee platformCheck = new Referee();
            PlayStepResultEnum stepResult=platformCheck.Check(chessBoard, new ChessBoardPoint(point.Row, point.Col), PieceTypeEnum.Black);            
            //it seems the platform does not check four-three-three 
            
                if (stepResult == PlayStepResultEnum.Four_Four ||
                   stepResult == PlayStepResultEnum.Three_Three ||
                   stepResult == PlayStepResultEnum.Overline)
                {
                    if (pieceType == PieceTypeEnum.Black)
                    {
                        return true;
                    }
                    else
                    {
                        stepResult = platformCheck.Check(chessBoard, new ChessBoardPoint(point.Row, point.Col), PieceTypeEnum.White);
                        if (stepResult == PlayStepResultEnum.Four_Four ||
                            stepResult == PlayStepResultEnum.Three_Three||
                            stepResult==PlayStepResultEnum.Win)
                        {
                            return false;
                        }
                    }
                }           
                

            return false;
        }
    }
}
