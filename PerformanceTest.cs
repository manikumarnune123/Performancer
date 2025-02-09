namespace Performancer
{
    public class PerformanceTest : CoreManager
    {
        #region Fixtures
        [SetUp]
        public void Setup()
        {
            Login();
        }

        [TearDown]
        public void Teardown()
        {
            webDriver.Quit();
            Logout();
        }
        #endregion

        #region Tests
        [Test]
        public void Test_InitialLoad_Performance()
        {
            Logger.Info($"Beginning Heap Total Size {Metrics.JSHeapTotalSize}");
            webDriver.Url = "https://example.com/";
            Thread.Sleep(5000);
            Logger.Info($"Heap Total Size after 5 seconds {Metrics.JSHeapTotalSize}");
        }
        #endregion

        #region Common Methods

        private static void Login()
        {

        }

        private static void Logout()
        {

        }

        private static void NavigateToPerformancePage()
        {

        }

        #endregion
    }
}