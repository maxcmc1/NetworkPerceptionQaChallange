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
        private IWebDriver Driver;
        private By AvailableExamples(string tab)
        {
            return By.XPath("//a[text()='" + tab + "']");
        }

        private By Content => By.Id("content");
        private IWebElement UserNameInput => Driver.FindElement(By.Id("username"));
        private IWebElement PasswordInput => Driver.FindElement(By.Id("password"));
        private IWebElement LoginButton => Driver.FindElement(By.XPath("//button[@type='submit']"));
        private string GetUrl => Driver.Url;
        private IWebElement WarningMessage => Driver.FindElement(By.Id("flash-messages"));
        private IWebElement DynamicallyLoadedElementExample(string exampleNumber)
        {
            return Driver.FindElement(By.XPath("//a[contains(text(),'" + exampleNumber + "')]"));
        }
        private IWebElement StartButton => Driver.FindElement(By.XPath("//button[text()='Start']"));
        private By LoadingMessage => By.Id("loading");
        private IWebElement Finish => Driver.FindElement(By.Id("finish"));

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

        [DataRow("Drag and Drop")]
        [DataTestMethod]
        public void DragAndDrop(string tab)
        {
            NavigatingIntoAvailableExample(tab);
            By CubesLocator = By.XPath("//div[@id='columns']//header"); // storing locator for both cubes - A and B in order to be able to put it into the list
            IList<IWebElement> AandB = Driver.FindElements(CubesLocator);
            Assert.IsTrue(AandB[0].Text.Equals("A"), "First cube didn't contain letter A"); // verifying that cube A is on the left side
            IWebElement CubeA = Driver.FindElement(By.XPath("//header[text()='A']/.."));
            IWebElement CubeB = Driver.FindElement(By.XPath("//header[text()='B']/.."));
            JavascriptDragAndDrop(CubeA, CubeB, Driver); // JavaScript executer method to drag and drop cube A on cube B side
            IList<IWebElement> BandA = Driver.FindElements(CubesLocator); //srting new position of the both cubes locators
            Assert.IsTrue(BandA[1].Text.Equals("A"), "Cube A was not swapped with Cube B"); // verifying that cube A is on the right side after been dragged and dropped
            JavascriptDragAndDrop(CubeB, CubeA, Driver); //JavaScript executer to swap back two cubes into their initial position
            AandB = Driver.FindElements(CubesLocator);
            Assert.IsTrue(AandB[0].Text.Equals("A"), "First cube didn't contain letter A"); // verifying that cube A is back on the left side again
        }

        [DataRow("Form Authentication", "tomsmith", "SuperSecretPassword!", "johnDhoe", "helloWorld")]
        [DataTestMethod]
        public void FormAuthentication(string tab, string userName, string password, string invalidUserName, string invalidPass)
        {
            NavigatingIntoAvailableExample(tab);

            // the first test below to verify that user is successfully able to login with valid creds
            UserNameInput.SendKeys(userName);
            PasswordInput.SendKeys(password);
            LoginButton.Click();
            Assert.IsTrue(GetUrl.EndsWith("secure"), "The User was not able to login");

            IWebElement LogoutButon = Driver.FindElement(By.XPath("//i[contains(text(),'Logout')]"));
            LogoutButon.Click();
            Assert.IsTrue(GetUrl.EndsWith("login"), "The User didn't logout successfully"); // verifying that user was able to logout successfully

            // the second test below to verify user is not able to login with invalid username and valid password
            GetWait(Driver).Until(ExpectedConditions.ElementIsVisible(Content));
            UserNameInput.SendKeys(invalidUserName);
            PasswordInput.SendKeys(password);
            LoginButton.Click();
            Assert.IsTrue(WarningMessage.Text.Contains("username is invalid"), "The warning was not displayed");

            // the third test below to verify user is not able to login with valid username and invalid password
            UserNameInput.SendKeys(userName);
            PasswordInput.SendKeys(invalidPass);
            LoginButton.Click();
            Assert.IsTrue(WarningMessage.Text.Contains("password is invalid"), "The warning was not displayed");
        }

        [DataRow("Dynamic Loading", "Hello World")]
        [DataTestMethod]
        public void DynamicallyLoadedPageElements(string tab, string loadingMessage)
        {
            NavigatingIntoAvailableExample(tab);

            // the first test below to verify that user is able to load 'Hello World' using first exmaple link
            GetActions(Driver).MoveToElement(DynamicallyLoadedElementExample("1")) // opening first example link in separate tab
                    .KeyDown(Keys.LeftControl)
                    .Click(DynamicallyLoadedElementExample("1"))
                    .KeyUp(Keys.LeftControl)
                    .Build()
                    .Perform();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]); // switching to the tab with the first example
            StartButton.Click();
            GetWait(Driver).Until(ExpectedConditions.InvisibilityOfElementLocated(LoadingMessage));
            Assert.IsTrue(Finish.Text.Contains(loadingMessage), "The message was not displayed");
            Driver.Close(); // closing first example link in separate tab

            // the second test below to verify that user is able to load 'Hello World' using second exmaple link
            Driver.SwitchTo().Window(Driver.WindowHandles[0]); // switching back to the parent tab
            GetActions(Driver).MoveToElement(DynamicallyLoadedElementExample("2"))
                    .KeyDown(Keys.LeftControl)
                    .Click(DynamicallyLoadedElementExample("2"))
                    .KeyUp(Keys.LeftControl)
                    .Build()
                    .Perform();
            Driver.SwitchTo().Window(Driver.WindowHandles[1]); // switching to the tab with the second example
            StartButton.Click();
            GetWait(Driver).Until(ExpectedConditions.InvisibilityOfElementLocated(LoadingMessage));
            Assert.IsTrue(Finish.Text.Contains(loadingMessage), "The message was not displayed");
        }

        public void NavigatingIntoAvailableExample(string tab)
        {
            Driver.FindElement(AvailableExamples(tab)).Click();
            GetWait(Driver).Until(ExpectedConditions.ElementIsVisible(Content));
        }

    }
}
