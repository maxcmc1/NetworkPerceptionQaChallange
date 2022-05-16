using AutomationResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace NetworkPerceptionQaChallange.Tests
{
    [TestClass]
    public class HerokuappDragAndDropTests
    {
        public IWebDriver Driver { get; private set; }

        [TestInitialize]
        public void SetupForEverySingleTestMethod()
        {
            var factory = new WebDriverFactory();
            Driver = factory.Create(BrowserType.Chrome);
            Driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/");
        }


        [TestCleanup]
        public void TearDownForEverySingleTestMethod()
        {
            Driver.Quit();
        }

        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("The Page opened successfully");
        }
    }
}
