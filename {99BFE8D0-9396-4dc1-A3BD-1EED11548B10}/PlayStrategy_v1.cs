using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Competition.Gomoku.Core;

namespace _99BFE8D0_9396_4dc1_A3BD_1EED11548B10_
{
    class PlayStrategy_v1:IPlayStrategy
    {
        public ChessBoardPoint GetNextStep(IChessBoard chessBoard, PieceTypeEnum pieceType, int stepIndex, PlayStep? prevStep)
        {
            int[] tupleScoreTable=new int[11]{7,0,0,0,0,0,0,0,0,0,0};
            //initialize tuple score table
            if (stepIndex%2==0)//even step is first hand,i.e. the black side
            {
                if (stepIndex==0)
                {
                    return ChessBoardPoint.TENGEN;
                }
                tupleScoreTable[1]=35;
                tupleScoreTable[2]=800;
                tupleScoreTable[3]=15000;
                tupleScoreTable[4]=800000;
                tupleScoreTable[5]=15;
                tupleScoreTable[6]=400;
                tupleScoreTable[7]=1800;
                tupleScoreTable[8]=100000;
            }
            else//odd step is back hand,i.e. the white side
            {
                tupleScoreTable[1]=15;
                tupleScoreTable[2]=400;
                tupleScoreTable[3]=1800;
                tupleScoreTable[4]=100000;
                tupleScoreTable[5]=35;
                tupleScoreTable[6]=800;
                tupleScoreTable[7]=15000;
                tupleScoreTable[8]=800000;
            }

            //extended board,with virtual points 
            ExtendedPointStateEnum[,] exPointStates = new ExtendedPointStateEnum[23, 23];
            for (int row = 0; row < 23;row++ )
            {
                for (int col = 0; col < 23;col++ )
                {
                    if (row<4||row>18||col<4||col>18)
                    {
                        exPointStates[row, col] = ExtendedPointStateEnum.Virtual;
                    }
                    else
                    {
                        exPointStates[row, col] = (ExtendedPointStateEnum)chessBoard.GetPointState(14-(row - 4), col - 4);
                    }
                }
            }

            int[,] scoreTable = new int[15, 15];

            /// <summary>calculate type of every tuple</summary>
            /// <description>In order to give a clear train of thought,
            /// I used an intuitionistic method to do this work.
            /// But this results in not efficient.
            /// Every tuple is calculated for twice.
            /// But it's easy to modify it:two ends of a tuple own the same tuple
            /// I'll modify it in next version ,where efficiency becomes bottleneck.
            /// </description>
            //every point indexs 8 tuples around it
            TupleType[, ,] tupleTable = new TupleType[15, 15, 8];			
            for (int row = 4; row < 19;row++ )
            {            
                for (int col = 4; col < 19;col++ )
                {										
                    int[] white = new int[8];//white points in a tuple
                    int[] black = new int[8];//black points in a tuple
                    #region ---check tuples of every direction
                    //left ,index 0
                    for (int i = 0; i < 5;i++ )
                    {
                        if (exPointStates[row,col-i]==ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 0] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row, col - i] == ExtendedPointStateEnum.Black)
                        {
                            black[0]++;
                        }
                        else if (exPointStates[row, col - i] == ExtendedPointStateEnum.White)
                        {
                            white[0]++;
                        }
                    }
                    //top left,index 1
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row-i, col - i] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 1] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row-i, col-i] == ExtendedPointStateEnum.Black)
                        {
                            black[1]++;
                        }
                        else if (exPointStates[row-i, col - i] == ExtendedPointStateEnum.White)
                        {
                            white[1]++;
                        }
                    }
                    //up ,index 2
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row-i, col] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 2] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row-i, col] == ExtendedPointStateEnum.Black)
                        {
                            black[2]++;
                        }
                        else if (exPointStates[row-i, col] == ExtendedPointStateEnum.White)
                        {
                            white[2]++;
                        }
                    }
                    //top right,index 3
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row-i, col + i] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 3] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row-i, col + i] == ExtendedPointStateEnum.Black)
                        {
                            black[3]++;
                        }
                        else if (exPointStates[row-i, col + i] == ExtendedPointStateEnum.White)
                        {
                            white[3]++;
                        }
                    }
                    //right,index 4
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row, col + i] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 4] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row, col + i] == ExtendedPointStateEnum.Black)
                        {
                            black[4]++;
                        }
                        else if (exPointStates[row, col + i] == ExtendedPointStateEnum.White)
                        {
                            white[4]++;
                        }
                    }
                    //bottom right,index 5
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row+i, col + i] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 5] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row+i, col + i] == ExtendedPointStateEnum.Black)
                        {
                            black[5]++;
                        }
                        else if (exPointStates[row+i, col + i] == ExtendedPointStateEnum.White)
                        {
                            white[5]++;
                        }
                    }
                    //bottom,index 6
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row+i, col ] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 6] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row+i, col ] == ExtendedPointStateEnum.Black)
                        {
                            black[6]++;
                        }
                        else if (exPointStates[row+i, col ] == ExtendedPointStateEnum.White)
                        {
                            white[6]++;
                        }
                    }
                    //bottom left,index 7
                    for (int i = 0; i < 5; i++)
                    {
                        if (exPointStates[row+i, col - i] == ExtendedPointStateEnum.Virtual)
                        {
                            tupleTable[row - 4, col - 4, 7] = TupleType.Virtual;
                            break;
                        }
                        else if (exPointStates[row+i, col - i] == ExtendedPointStateEnum.Black)
                        {
                            black[7]++;
                        }
                        else if (exPointStates[row+i, col - i] == ExtendedPointStateEnum.White)
                        {
                            white[7]++;
                        }
                    }
                    #endregion //check tuples of every direction
					
                    //decide tuple type
                    for (int i = 0; i < 8;i++ )
                    {
						//already assigned
						if(tupleTable[row-4,col-4,i]==TupleType.Virtual)
						{
							continue;
						}
                        if (white[i]>0&&black[i]>0)
                        {
                            tupleTable[row - 4, col - 4,i] = TupleType.Polluted;
                        }
                        else if (white[i]==0&&black[i]==0)
                        {
                            tupleTable[row-4,col-4,i]=TupleType.Blank;
                        }
                        else if(white[i]==0)
                        {
                            tupleTable[row-4,col-4,i]=(TupleType)black[i];
                        }
                        else
                        {
                            tupleTable[row - 4, col - 4,i] = (TupleType)(white[i] + 4);
                        }
                    }
                }
            }
			#region ---scoreTable calculate
			//calculate score table . using symmetry             
			//top left corner
			for(int row=0;row<8;row++)
			{
				for(int col=0;col<8;col++)
				{	
					if(exPointStates[row+4,col+4]!=ExtendedPointStateEnum.Blank)
					{
						//this situation has been considered
						//scoreTable[row,col]=0;
						continue;
					}
					for(int m=0;m<5;m++)
					{   
                        if (row>=m)//top right
                        {
                            scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col+m, 7]];
                        }
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col, 2]];//bottom
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col + m, 1]];//bottom right
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row, col + m, 0]];//right
					}
				}
			}
			//top right corner
			for(int row=0;row<8;row++)
			{				
				for(int col=8;col<15;col++)
				{	
					if(exPointStates[row+4,col+4]!=ExtendedPointStateEnum.Blank)
					{
						//this situation has been considered
						//scoreTable[row,col]=0;
						continue;
					}
					for(int m=0;m<5;m++)
					{
                        if (row>=m)//top left
                        {
                            scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col-m, 5]];
                        }
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col, 2]];//bottom
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col - m, 3]];//bottom left
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row , col-m , 4]];//left
					}
				}
			}
			//bottom left corner
			for(int row=8;row<15;row++)
			{
				for(int col=0;col<8;col++)
				{	
					if(exPointStates[row+4,col+4]!=ExtendedPointStateEnum.Blank)
					{
						//this situation has been considered
						//scoreTable[row,col]=0;
						continue;
					}
					for(int m=0;m<5;m++)
					{
                        if (row+m<15)//bottom right
                        {
                            scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col+m, 1]];
                        }
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col, 6]];//top
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col + m, 7]];//top right
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row, col + m, 0]];//right
					}
				}
			}
			//bottom right corner
			for(int row=8;row<15;row++)
			{
				for(int col=8;col<15;col++)
				{	
					if(exPointStates[row+4,col+4]!=ExtendedPointStateEnum.Blank)
					{
						//this situation has been considered
						//scoreTable[row,col]=0;
						continue;
					}
					for(int m=0;m<5;m++)
					{
                        if (row+m<15)//bottom left
                        {
                            scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row + m, col-m, 3]];
                        }
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col, 6]];//top
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row - m, col - m, 5]];//top left
                        scoreTable[row, col] += tupleScoreTable[(int)tupleTable[row, col - m, 4]];//left
					}
				}
			}
			#endregion //scoreTable
			
			//select best position
			List<Position> bestList=new List<Position>();

            //select first valid point               
            Position first=new Position(0,0);
            //all the point is forbidden.connot happen
            //while (IsFobidden(first))
            //{                            
            //    while (IsFobidden(first))
            //    {
            //        if (first.Col<14)
            //        {
            //            first.Col++;
            //        }
            //        else
            //        {
            //            break;
            //        }                                
            //    }
            //    if (first.Row<14)
            //    {
            //        first.Row++;
            //    }
            //    else
            //    {
            //        break;
            //    } 
            //}
            while (IsFobidden(ref chessBoard,first,pieceType))
            {
                if (first.Col<14)
                {
                    first.Col++;
                }
                else if(first.Row<14)
                {
                    first.Row++;
                    first.Col=0;
                }
                else
                {
                    return new ChessBoardPoint(-1,-1);
                }
            }
            bestList.Add(first);

            Referee checkWin = new Referee();
            //select best points
			for(int row=0;row<15;row++)
			{
				for(int col=0;col<15;col++)
				{                                        
				    if(scoreTable[row,col]>scoreTable[bestList[0].Row,bestList[0].Col])
				    {
                        Position best = new Position(row, col);
                        if (!IsFobidden(ref chessBoard, best, pieceType))
                        {
                            bestList.Clear();
                            bestList.Add(best);
                        }
                      
    			    }
				    else if(scoreTable[row,col]==scoreTable[bestList[0].Row,bestList[0].Col])
				    {
                        Position best=new Position(row,col);
                        if (!IsFobidden(ref chessBoard,best,pieceType))
                        {
                            bestList.Add(best);
                        }
				    }	
				}
			}
            //there is no best .connot happen
            if (bestList.Count==0)
            {
                return new ChessBoardPoint(-1,-1);
            }
            Position ret = bestList[(new Random()).Next(bestList.Count)];
            return new ChessBoardPoint(14-ret.Row,ret.Col);
        }
		//is forbidden
        private bool IsFobidden(ref IChessBoard chessBoard, Position point, PieceTypeEnum pieceType)
        {
            Referee platformCheck = new Referee();
            PlayStepResultEnum stepResult = platformCheck.Check(chessBoard, new ChessBoardPoint(14-point.Row, point.Col), PieceTypeEnum.Black);
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
                    //white priority vs. black forbidden
                    //if this position is black-forbidden,generally white should not be put here
                    //but if is advantaged enough for white,it should   
                    if (PlayStepResultEnum.Win==platformCheck.Check(chessBoard, new ChessBoardPoint(14-point.Row, point.Col), PieceTypeEnum.White))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
		
        enum ExtendedPointStateEnum
        {
            Blank = PointStateEnum.Blank,
            Black = PointStateEnum.Black,
            White = PointStateEnum.White,
            Virtual //not exist
        };
        enum TupleType
        {
            // tuple is empty
            Blank,
            // tuple contains a black chess
            B,
            // tuple contains two black chesses
            BB,
            // tuple contains three black chesses
            BBB,
            // tuple contains four black chesses
            BBBB,
            // tuple contains a white chess
            W,
            // tuple contains two white chesses
            WW,
            // tuple contains three white chesses
            WWW,
            // tuple contains four white chesses
            WWWW,
            // tuple does not exist
            Virtual,
            // tuple contains at least one black and at least one white
            Polluted
        };
        public class Position
        {
            private int col;

            public int Col
            {
                get { return col; }
                set { col = value; }
            }
            private int row;

            public int Row
            {
                get { return row; }
                set { row = value; }
            }
            public Position()
            {
                col = 0;
                row = 0;
            }
            public Position(int row, int col)
            {
                this.col = col;
                this.row = row;
            }
        }
    }    
}
