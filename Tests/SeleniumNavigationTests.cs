using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Reflection;

namespace TicketerTests
{
    public class SeleniumNavigationTests
    {
        private IWebDriver driver;
        private string url;

        [TearDown]
        public void tearDown()
        {
            driver.Close();
        }

        [SetUp]
        public void Setup()
        {
            url = "https://ticketerapp.azurewebsites.net/";
            driver = new FirefoxDriver();
        }

        [Test, Order(1)]
        public void ClickOnNavbar_Login()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            Assert.That(driver.Url, Is.EqualTo(url + "users/SignIn"));
        }
        [Test, Order(2)]
        public void ClickOnNavbar_Home()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='/']")).Click();

            Assert.That(driver.Url, Is.EqualTo(url));
        }
        [Test, Order(4)]
        public void Login_SendCorrect()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("wikdev");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("12345");

            passwordInput.Submit();
            System.Threading.Thread.Sleep(500);
            Assert.That(driver.Url, Is.EqualTo(url + "users"));

        }
        [Test, Order(3)]
        public void Login_SendIncorrectPassword()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("wikdev");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("12345");

            passwordInput.Submit();
            System.Threading.Thread.Sleep(500);
            Assert.That(driver.Url, Is.EqualTo(url + "users"));
        }
        public void Login_SendIncorrectLogin()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("wikdev");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("123");

            passwordInput.Submit();

            try
            {
                IWebElement error = driver.FindElement(By.XPath("//*[contains(text(),'Niepoprawne')]"));
                System.Threading.Thread.Sleep(500);
            }
            catch (NotFoundException)
            {
                Assert.Fail("Warning not shown");
            }
        }
    }
}