using System;
using System.Collections.Generic;
using System.Text;
using GrapeCity.Competition.Gomoku.Core;

namespace _9F5D91B6_123D_4de2_B26D_B59041CEB07E_
{
    /// <summary>
    /// 五子棋游戏策略类
    /// </summary>
    /// <remarks></remarks>
    /// <history>
    /// [zanezeng]               2010/3/23 9:40    创建
    /// </history>
    public class PlayStrategyDemo : IPlayStrategy
    {
        #region ---IPlayStrategy

        /// <summary>
        /// 获得当前五子棋策略的下一步落棋位置。
        /// </summary>
        /// <param name="chessBoard">五子棋棋盘对象。</param>
        /// <param name="pieceType">棋子的类型。</param>
        /// <param name="stepIndex">下一步落子操作的序号。</param>
        /// <param name="prevStep">包含对方上一步落子操作信息的走步对象。</param>
        /// <returns>下一步落棋的位置。</returns>
        /// <remarks></remarks>
        /// <history>
        /// [zanezeng]               2010/3/23 9:40    创建
        /// </history>
        public ChessBoardPoint GetNextStep( IChessBoard chessBoard, PieceTypeEnum pieceType, int stepIndex, PlayStep? prevStep )
        {
            //判断是否为黑方的第一手棋
            if (pieceType == PieceTypeEnum.Black && stepIndex == 0)
            {
                //如果是黑方的第一手棋，则总是返回天元
                return ChessBoardPoint.TENGEN;
            }
            //用于保存落子位置的行序号
            int rowIndex = 0;
            //用于保存落子位置的列序号
            int columnIndex = 0;

            //创建随机数生成器
            Random random = new Random();

            //循环直到获得可以落子的位置
            while (true)
            {
                //生成随机行坐标序号（有效的行序号为0~14）
                rowIndex = random.Next() % 15;

                //生成随机列坐标序号（有效的列序号为0~14）
                columnIndex = random.Next() % 15;

                //判断随机落子点是否已经存在棋子
                if (chessBoard.GetPointState( rowIndex, columnIndex ) == PointStateEnum.Blank)
                {
                    //如果随机落子点为空白点，则返回随机落子点
                    return new ChessBoardPoint( rowIndex, columnIndex );
                }
            }
        }

        #endregion
    }
}
