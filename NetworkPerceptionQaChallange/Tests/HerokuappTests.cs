using AutomationResources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using SeleniumExtras.WaitHelpers;
using static NetworkPerceptionQaChallange.Support.TestHelper;
using System.Threading;
using System.Drawing;

namespace NetworkPerceptionQaChallange.Tests
{
    [TestClass]
    public class HerokuappTests
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
        public void DragAndDrop()
        {
            Driver.FindElement(By.XPath("//a[text()='Drag and Drop']")).Click();
            GetWait(Driver).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='content']")));
            By CubesLocator = By.XPath("//div[@id='columns']//header");
            IList<IWebElement> AandB = Driver.FindElements(CubesLocator);
            Assert.IsTrue(AandB[0].Text.Equals("A"), "First cube didn't contain letter A");
            IWebElement CubeA = Driver.FindElement(By.XPath("//header[text()='A']/.."));
            IWebElement CubeB = Driver.FindElement(By.XPath("//header[text()='B']/.."));
            JavascriptDragAndDrop(CubeA, CubeB, Driver);
            IList<IWebElement> BandA = Driver.FindElements(CubesLocator);
            Assert.IsTrue(BandA[1].Text.Equals("A"), "Cube A was not swapped with Cube B");
            JavascriptDragAndDrop(CubeB, CubeA, Driver);
            AandB = Driver.FindElements(CubesLocator);
            Assert.IsTrue(AandB[0].Text.Equals("A"), "First cube didn't contain letter A");
        }
    }
}
