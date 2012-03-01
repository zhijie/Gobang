using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GrapeCity.Competition.Gomoku.Core;


namespace _99BFE8D0_9396_4dc1_A3BD_1EED11548B10_
{
     /// <summary>
    /// 玩家类
    /// </summary>
    /// <remarks>用于对外提供玩家信息。</remarks>
    /// <history>
    /// [onezeros]               2010/3/23 9:54    创建
    /// </history>
    public class Player : IPlayer
    {
        #region ---IPlayer
        /// <summary>
        /// 获得玩家编号。
        /// </summary>
        /// <value>玩家编号。</value>
        /// <remarks></remarks>
        /// <history>
        /// [onezeros]               2010/3/23 9:55    创建
        /// </history>
        public Guid ID
        {
            get
            {
                //此处返回参赛小组的编号，建议返回用作工程命名的GUID
                return PLAYER_ID;
            }
        }
        /// <summary>
        /// 返回玩家名称
        /// </summary>
        /// <value>玩家名称</value>
        /// <remarks></remarks>
        /// <history>
        /// [onezeros]               2010/3/23 9:56    创建
        /// </history>
        public string Name
        {
            get
            {
                //此处返回参赛小组的名称
                return "onezeros";
            }
        }
        /// <summary>
        /// 获得五子棋算法策略的实例。
        /// </summary>
        /// <value>五子棋算法策略的实例。</value>
        /// <remarks></remarks>
        /// <history>
        /// [onezeros]               2010/3/23 10:12    创建
        /// </history>
        public IPlayStrategy PlayStrategy
        {
            get
            {
                //此处返回参赛小组提供的五子棋算法策略实例
                return this.m_PlayStrategy;
            }
        }
        #endregion
        #region ---Field
        /// <summary>
        /// 五子棋算法策略，此处创建的为上一步创建的随机算法策略
        /// </summary>
        private IPlayStrategy m_PlayStrategy = new PlayStrategy_v1();
        /// <summary>
        /// 玩家编号，此处返回的是用于工程命名的GUID
        /// </summary>
        private static readonly Guid PLAYER_ID = new Guid("{99BFE8D0-9396-4dc1-A3BD-1EED11548B10}");
        #endregion
    }
}
