using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Competition.Gomoku.Core;

namespace _99BFE8D0_9396_4dc1_A3BD_1EED11548B10_
{
    /// <summary>
    /// PlayStrategy class
    /// </summary>
    public partial class PlayStrategy : IPlayStrategy
    {
        // scores of every kind of TupleTypes
        static private int[] tupleScoreTable;
        //scores of every position
        private int[,] scoreTable;
        //which side we are
        //PieceTypeEnum playSide;
        // constructor
        public PlayStrategy()
        {
            // scores of every TupleType in offensive order
            tupleScoreTable = new int[11] {
                7,35,800,15000,800000,
                15,400,1800,100000,0,0
            };

            scoreTable = new int[15, 15];
            //initialize score table
            for (int row = 0; row < 15;row ++ )
            {
                for (int col = 0; col < 15;col  ++ )
                {
                    scoreTable[row, col] = tupleNum(row, col) * tupleScoreTable[(int)TupleType.Blank];
                }
            }
        }

        /// <summary>
        /// calculate number of tuples which contains Position(col,row)
        /// </summary>
        /// <param name="row">row index of the point</param>
        /// <param name="col">column index of the point</param>
        /// <returns>number of tuples which contains Position(col,row)</returns>
        private int tupleNum(int row,int col)
        {
            //consider symmetry,to 1/4
            if (row>7)
            {
                row =14-row;
            }
            if (col>7)
            {
                col =14-col;
            }
            //consider symmetry,continue to 1/2 
            if (row<col)
            {
                int t = row;
                row = col;
                col = t;
            }

            if (col>=4)
            {
                return 20;
            } 
            else if (row>=4)
            {
                return 3 * (col + 1) + 5;
            }
            else
            {
                return 2 * (col + 1) + row + 1;
            }

        }

        #region ---IPlayStrategy
        /// <summary>
        /// 获得当前五子棋策略的下一步落棋位置。
        /// </summary>
        /// <param name="chessBoard">五子棋棋盘对象。</param>
        /// <param name="pieceType">棋子的类型。</param>
        /// <param name="stepIndex">下一步落子操作的序号。</param>
        /// <param name="prevStep">包含对方上一步落子操作信息的走步对象。</param>
        /// <returns>下一步落棋的位置。</returns>
        public ChessBoardPoint GetNextStep(IChessBoard chessBoard, PieceTypeEnum pieceType, int stepIndex, PlayStep? prevStep)
        {
            //change the frame of axes to our(   |_  to |-  )      
            //判断是否为黑方的第一手棋
            if (stepIndex == 0&&pieceType == PieceTypeEnum.Black )
            {
                // update score table
                UpdateScoreTable(ref chessBoard,new Position(7,7),PieceTypeEnum.Black);

                //如果是黑方的第一手棋，则总是返回天元
                return ChessBoardPoint.TENGEN;
            }else if(stepIndex==1&&pieceType==PieceTypeEnum.White){
                //change scoreTable to defensive order
                for (int i = 1; i < 5;i++ )
                {
                    int tmp = tupleScoreTable[i];
                    tupleScoreTable[i] = tupleScoreTable[i + 4];
                    tupleScoreTable[i + 4] = tmp;
                }
            }          

            Position preStepPos = new Position(14 - prevStep.Value.Location.RowIndex, prevStep.Value.Location.ColumnIndex);

            //update score table for changes caused by previous step
            UpdateScoreTable(ref chessBoard, preStepPos,prevStep.Value.PieceType);

            // position to play chess
            Position bestPos = FindBestPos(ref chessBoard, prevStep.Value.PieceType);

            //update score table for changes caused by this step
            UpdateScoreTable(ref chessBoard,bestPos,prevStep.Value.PieceType==PieceTypeEnum.Black?PieceTypeEnum.White:PieceTypeEnum.Black);

            return new ChessBoardPoint(14-bestPos.Row,bestPos.Col);

        }
        #endregion
               
        #region --- enum
        /// <summary>
        /// type enum of tuple
        /// </summary>
        /// <remarks> if a tuple contains at lease one white chess
        /// and one black tuple,we call it "polluted",and such tuple 
        /// is of no use to both attacker and defender.So score of 
        /// such tuple should be zero.So we ignore this type   
        /// </remarks>
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

        enum ExtendedPointStateEnum
        {
            Blank=PointStateEnum.Blank,
            Black=PointStateEnum.Black,
            White=PointStateEnum.White,
            Virtual //not exist
        };

        #endregion
    }

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
        public Position(int row,int col)
        {
            this.col=col;
            this.row=row;
        }
     }

    
}
