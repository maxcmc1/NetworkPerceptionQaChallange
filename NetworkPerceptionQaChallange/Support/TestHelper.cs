using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace NetworkPerceptionQaChallange.Support
{
    public class TestHelper
    {

        public static WebDriverWait GetWait(IWebDriver driver, int timeout)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
        }
        public static WebDriverWait GetWait(IWebDriver driver)
        {
            return GetWait(driver, 10);
        }

        public static Actions GetActions(IWebDriver driver)
        {
            return new Actions(driver);
        }

        public static IJavaScriptExecutor GetJsExecutor(IWebDriver driver)
        {
            return (IJavaScriptExecutor)driver;
        }

        public static void JavascriptDragAndDrop(IWebElement source, IWebElement target, IWebDriver driver)
        {
            string script = System.IO.File.ReadAllText(@"C:\Users\maksym.seliukov\source\repos\NetworkPerceptionQaChallange\Script1.js");
            script += "simulateHTML5DragAndDrop(arguments[0], arguments[1])";
            GetJsExecutor(driver).ExecuteScript(script, source, target);
        }

    }
}
