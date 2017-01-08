using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GooglePlayMusicAPI;
using System.Collections.Generic;

namespace GooglePlayMusicClientTest
{
    [TestClass]
    public class GooglePlayMusicClientTests
    {
        GooglePlayMusicClient client;

        [TestInitialize]
        private void TestSetup()
        {
            client = new GooglePlayMusicClient(new MockRequestClient());
        }

        [TestCleanup]
        private void TestCleanup()
        {

        }

        [TestMethod]
        public async void TestGetLibrary()
        {

        }
    }
}
