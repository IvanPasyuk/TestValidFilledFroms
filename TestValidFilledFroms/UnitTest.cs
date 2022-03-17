using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProjectFieldValid
{
    [TestFixture]
    public class FormsValidTest
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private readonly By _emailInputButton = By.XPath("//input[@name='email']");

        [SetUp]
        public void SetupTest()
        {            
            driver = new ChromeDriver();
            baseURL = "http://hrmoscow.ru";
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void ValidEmailTest()
        {
            driver.Navigate().GoToUrl(baseURL);
            driver.Manage().Window.Maximize();

            var email = driver.FindElement(_emailInputButton);
            email.SendKeys("somebody@google.com");

            Thread.Sleep(10000);
         
            string inputText = email.GetAttribute("value");
            string pattern = "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$";
            Match isMatch = Regex.Match(inputText, pattern, RegexOptions.IgnoreCase);

            SaveResults(isMatch.Success);

            Assert.IsTrue(isMatch.Success);
        }

        private async void SaveResults(bool isEmailValid)
        {
            string path = @"C:\temp\Result.txt";
            string text;

            if (isEmailValid)
            {
                text = "The email format is correct";
            }
            else
            {
                text = "The email format is incorrect";
            }

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }         
        }
    }
}