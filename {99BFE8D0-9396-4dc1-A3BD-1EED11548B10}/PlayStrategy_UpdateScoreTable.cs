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
        /// update score table
        /// </summary>
        /// <param name="scoreTable"></param>
        /// <param name="col">columnIndex of new joined chess</param>
        /// <param name="row">rowIndex of new joined chess</param>
        private void UpdateScoreTable(ref IChessBoard chessBoard, Position pos,PieceTypeEnum pieceType)
        {
            //could not happen
/*            if (chessBoard.GetPointState(pos.Col,pos.Row)!=PointStateEnum.Blank)
            {
                return;
            }
*/
            #region ---partial board

            //get all the 32 piece states surrounding this position
            ExtendedPointStateEnum[,] pointState=new ExtendedPointStateEnum[4,9];
            
            int row,col;
            //horizontal
            for (col = pos.Col - 4; col < pos.Col + 5;col++)
            {
                if (col<0||col>14)
                {
                    pointState[0,col-pos.Col+4]=ExtendedPointStateEnum.Virtual;
                }
                else
                {
                    pointState[0, col - pos.Col + 4] = (ExtendedPointStateEnum)chessBoard.GetPointState(pos.Row,col);
                }
            }
            //vertical
            for (row = pos.Row - 4; row < pos.Row + 5;row++ )
            {
                if (row<0||row>14)
                {
                    pointState[1, row - pos.Row + 4] = ExtendedPointStateEnum.Virtual;
                }
                else
                {
                    pointState[1, row - pos.Row + 4] = (ExtendedPointStateEnum)chessBoard.GetPointState(row,pos.Col);
                }
            }
            //backslash
            for (row = pos.Row - 4, col = pos.Col - 4; row < pos.Row + 5;row++,col++ )
            {
                if (row < 0 || row > 14 || col < 0 || col > 14)
                {
                    pointState[2, row - pos.Row + 4] = ExtendedPointStateEnum.Virtual;
                }
                else
                {
                    pointState[2, row - pos.Row + 4] = (ExtendedPointStateEnum)chessBoard.GetPointState(row, col);
                }
            }
            //slash
            for (row = pos.Row + 4, col = pos.Col - 4; row > pos.Row - 5; row--, col++)
            {
                if (row < 0 || row > 14 || col < 0 || col > 14)
                {
                    pointState[3, col - pos.Col + 4] = ExtendedPointStateEnum.Virtual;
                }
                else
                {
                    pointState[3, col - pos.Col + 4] = (ExtendedPointStateEnum)chessBoard.GetPointState(col, col);
                }
            }
            //assume the center of partial board is empty
            for (int m = 0; m < 4;m++ )
            {
                pointState[m, 4] = ExtendedPointStateEnum.Blank;
            }

            #endregion

            
            #region ---recognize formerTuples
            //recognize types of all the 20 Tuples .
            TupleType[,] formerTuples = new TupleType[4, 5];
            TupleType[,] changedTuples = new TupleType[4, 5];
            
            int white, black, blank;
            for (int i = 0; i < 4;i++ )
            {                
                int start=0;
                int finish = 0;
                //deal with virtual tuples from front                
                for (int j = 0; j < 4;j++ )
                {
                    if (pointState[i,j]==ExtendedPointStateEnum.Virtual)
                    {
                        changedTuples[i,j] = formerTuples[i, j] = TupleType.Virtual;
                    }
                    else
                    {
                        start = j;
                        break;//jump inner layer loop
                    }

                }
                //deal with virtual tuples from back
                for (int j = 8; j >= 5;j-- )
                {
                    if (pointState[i, j] == ExtendedPointStateEnum.Virtual)
                    {
                        changedTuples[i, j-4] = formerTuples[i, j - 4] = TupleType.Virtual;
                    }
                    else
                    {
                        finish = j;
                        break;//jump inner layer loop
                    }
                }

                white = black = blank = 0;
                //start recognize general tuples,deal with first four points
                for (int j = start; j <start+4;j++ )
                {
                    if (pointState[i, j] == ExtendedPointStateEnum.Blank)
                    {
                        blank++;
                    }
                    else if (pointState[i, j] == ExtendedPointStateEnum.Black)
                    {
                        black++;
                    }
                    else if (pointState[i, j] == ExtendedPointStateEnum.White)
                    {
                        white++;
                    }                 
                }
                //tuples recognition sliding 
                for (int j = start + 4; j <= finish;j++ )
                {
                    if (pointState[i, j] == ExtendedPointStateEnum.Blank)
                    {
                        blank++;
                    }
                    else if (pointState[i, j] == ExtendedPointStateEnum.Black)
                    {
                        black++;
                    }
                    else if (pointState[i, j] == ExtendedPointStateEnum.White)
                    {
                        white++;
                    }
                    //deal with formerTuples
                    if (black > 0 && white > 0)
                    {
                        formerTuples[i, j - 4] = TupleType.Polluted;
                    }
                    else if (black == 0 && white == 0)
                    {
                        formerTuples[i, j - 4] = TupleType.Blank;
                    }
                    else if (black == 0)
                    {
                        formerTuples[i, j - 4] = (TupleType)(white + 4);
                    }
                    else
                    {
                        formerTuples[i, j - 4] = (TupleType)black;
                    }
                    //deal with changedTuples,increase for change
                    if (pieceType==PieceTypeEnum.Black)
                    {
                        black++;
                    }
                    else
                    {
                        white++;
                    }
                    //recognize
                    if (black > 0 && white > 0)
                    {
                        changedTuples[i, j - 4] = TupleType.Polluted;
                    }
                    else if (black == 0 && white == 0)
                    {
                        changedTuples[i, j - 4] = TupleType.Blank;
                    }
                    else if (black == 0)
                    {
                        changedTuples[i, j - 4] = (TupleType)(white + 4);
                    }
                    else
                    {
                        changedTuples[i, j - 4] = (TupleType)black;
                    }
                    //deal with changedTuples,decrease
                    if (pieceType == PieceTypeEnum.Black)
                    {
                        black--;
                    }
                    else
                    {
                        white--;
                    }

                    //slide to next tuple ,so the first of current tuple should be dropped
                    if (pointState[i, j-4] == ExtendedPointStateEnum.Blank)
                    {
                        blank--;
                    }
                    else if (pointState[i, j-4] == ExtendedPointStateEnum.Black)
                    {
                        black--;
                    }
                    else if (pointState[i, j-4] == ExtendedPointStateEnum.White)
                    {
                        white--;
                    }
                }
            }
            #endregion
            
            #region ---update scores of correlative points

            //score changed caused by this chess
            int[,] changedScore = new int[4, 5];
            for (int i = 0; i < 4;i++ )
            {
                for (int j = 0; j < 5;j++ )
                {
                    int score=tupleScoreTable[(int)changedTuples[i,j]]-tupleScoreTable[(int)formerTuples[i,j]];
                    changedScore[i, j] = score;
                }
            }

            //internal calculation
            int[,] changedScoreSum = new int[4, 9];
            for (int i = 0; i < 4;i++ )
            {
                for (int j = 0; j < 4;j++ )
                {
                    int sum=0;
                    for (int m=0;m<=j;m++)
                    {
                        sum+=changedScore[i,m];
                    }
                    changedScoreSum[i,j]=sum;
                }
                for (int j = 8; j > 4;j-- )
                {
                    int sum = 0;
                    for (int m=8;m>=j;m--)
                    {
                        sum += changedScore[i, m-4];
                    }
                    changedScoreSum[i, j] = sum;
                }
            }
            //update center point,occupied point's score is 0
            scoreTable[pos.Col, pos.Row] = 0;
            //update general points
            //horizontal
            for (col = (pos.Col >= 4?pos.Col-4:0); 
                col <= (pos.Col <=10?pos.Col+4:14); 
                col++)
            {
                if (scoreTable[col,pos.Row]!=0)//not occupied
                {
                    scoreTable[col, pos.Row] += changedScoreSum[0, col - pos.Col + 4]; 
                    if (scoreTable[col, pos.Row] < 0)
                    {
                        scoreTable[col, pos.Row] = 0;
                    }                  
                }               
            }
            //vertical
            for (row = (pos.Row >= 4 ? pos.Row - 4 : 0);
                row <= (pos.Row <= 10 ? pos.Row + 4 : 14);
                row++)
            {
                if (scoreTable[pos.Col, row] != 0)//not occupied
                {
                    scoreTable[pos.Col, row] += changedScoreSum[1, row - pos.Row + 4];
                    if (scoreTable[pos.Col, row] < 0)
                    {
                        scoreTable[pos.Col, row] = 0;
                    }
                }                
            }
            //backslash
            for (col = (pos.Col >= 4 ? pos.Col - 4 : 0),
                row = (pos.Row >= 4 ? pos.Row - 4 : 0);
                col <= (pos.Col <= 10 ? pos.Col + 4 : 14) &&
                row <= (pos.Row <= 10 ? pos.Row + 4 : 14);
                col++,row++)
            {
                if (scoreTable[col, row] != 0)//not occupied
                {
                    scoreTable[col, row] += changedScoreSum[2, row - pos.Row + 4];
                    if (scoreTable[col, row]<0)
                    {
                        scoreTable[col, row] = 0;
                    }
                }
            }
            //slash
            for (col = (pos.Col >= 4 ? pos.Col - 4 : 0),
                row = (pos.Row <= 10 ? pos.Row + 4 : 14);
                col <= (pos.Col <= 10 ? pos.Col + 4 : 14) &&
                row >= (pos.Row >= 4 ? pos.Row - 4 : 0);
                col++, row--)
            {
                if (scoreTable[col, row] != 0)//not occupied
                {
                    scoreTable[col, row] += changedScoreSum[3, col - pos.Col + 4];
                    if (scoreTable[col, row] < 0)
                    {
                        scoreTable[col, row] = 0;
                    }
                }
            }

            #endregion
        }
    }
}
