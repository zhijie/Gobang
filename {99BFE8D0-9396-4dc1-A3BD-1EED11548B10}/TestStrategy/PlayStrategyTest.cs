using _99BFE8D0_9396_4dc1_A3BD_1EED11548B10_;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrapeCity.Competition.Gomoku.Core;

namespace TestStrategy
{
    
    
    /// <summary>
    ///这是 PlayStrategyTest 的测试类，旨在
    ///包含所有 PlayStrategyTest 单元测试
    ///</summary>
    [TestClass()]
    public class PlayStrategyTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试属性
        // 
        //编写测试时，还可使用以下属性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///UpdateScoreTable 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("{99BFE8D0-9396-4dc1-A3BD-1EED11548B10}.dll")]
        public void UpdateScoreTableTest()
        {
            PlayStrategy_Accessor target = new PlayStrategy_Accessor(); // TODO: 初始化为适当的值
            IChessBoard chessBoard = null; // TODO: 初始化为适当的值
            IChessBoard chessBoardExpected = null; // TODO: 初始化为适当的值
            Position pos = null; // TODO: 初始化为适当的值
            PieceTypeEnum pieceType = new PieceTypeEnum(); // TODO: 初始化为适当的值
            target.UpdateScoreTable(ref chessBoard, pos, pieceType);
            Assert.AreEqual(chessBoardExpected, chessBoard);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///IsFobidden 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("{99BFE8D0-9396-4dc1-A3BD-1EED11548B10}.dll")]
        public void IsFobiddenTest()
        {
            PlayStrategy_Accessor target = new PlayStrategy_Accessor(); // TODO: 初始化为适当的值
            IChessBoard chessBoard = null; // TODO: 初始化为适当的值
            IChessBoard chessBoardExpected = null; // TODO: 初始化为适当的值
            Position point = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.IsFobidden(ref chessBoard, point);
            Assert.AreEqual(chessBoardExpected, chessBoard);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///tupleNum 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("{99BFE8D0-9396-4dc1-A3BD-1EED11548B10}.dll")]
        public void tupleNumTest()
        {
            PlayStrategy_Accessor target = new PlayStrategy_Accessor(); // TODO: 初始化为适当的值
            int row = 0; // TODO: 初始化为适当的值
            int col = 0; // TODO: 初始化为适当的值
            int expected = 3; // TODO: 初始化为适当的值
            int actual;
            actual = target.tupleNum(row, col);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
