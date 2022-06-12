using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Reflection;

namespace TicketerTests
{
    public class SeleniumAccountTest
    {
        private IWebDriver driver;
        private string url;

        [OneTimeTearDown]
        public void tearDown()
        {
            driver.Close();
        }
        [TearDown]
        public void accPageTeardown()
        {
            driver.FindElement(By.CssSelector("a[href*='users']")).Click();
            System.Threading.Thread.Sleep(500);
        }
        [OneTimeSetUp]
        public void Setup()
        {
            url = "https://ticketerapp.azurewebsites.net/";
            driver = new FirefoxDriver();

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
        [Test, Order(1)]
        public void Account_LinkTest()
        {
            driver.FindElement(By.CssSelector("a[href*='users/CreateAccount']")).Click();
            Assert.That(driver.Url, Is.EqualTo(url + "users/CreateAccount"));
        }

        [Test, Order(2)]
        public void Account_NewAccountTest_CheckPasswordValidation()
        {
            driver.FindElement(By.CssSelector("a[href*='users/CreateAccount']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("Kotdev");

            IWebElement nameInput = driver.FindElement(By.Id("name"));
            nameInput.SendKeys("Anna Kot");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("12345");

            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("Anna.Kot31@outlook.com");

            IWebElement roleInput = driver.FindElement(By.Id("role"));
            roleInput.SendKeys("Support");

            roleInput.Submit();
            System.Threading.Thread.Sleep(500);
            try
            {
                IWebElement error = driver.FindElement(By.XPath("//*[contains(text(),'Hasło')]"));
                System.Threading.Thread.Sleep(2000);
            }
             catch (NotFoundException)
            {
                Assert.Fail("Warning not shown");
            }
        }
        [Test, Order(3)]
        public void Account_NewAccountTest()
        {
            driver.FindElement(By.CssSelector("a[href*='users/CreateAccount']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("Kotdev");

            IWebElement nameInput = driver.FindElement(By.Id("name"));
            nameInput.SendKeys("Anna Kot");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("SeleniumTest*12^");

            IWebElement emailInput = driver.FindElement(By.Id("email"));
            emailInput.SendKeys("Anna.Kot31@outlook.com");

            IWebElement roleInput = driver.FindElement(By.Id("role"));
            roleInput.SendKeys("Support");

            roleInput.Submit();

            System.Threading.Thread.Sleep(2000);
            Assert.That(driver.Url, Is.EqualTo(url + "users"));
        }
        
        [Test, Order(4)]
        public void Account_UnsufficientInput()
        {
            try
            {
                driver.FindElement(By.CssSelector("a[href*='users/CreateAccount']")).Click();
                System.Threading.Thread.Sleep(500);
                IWebElement roleInput = driver.FindElement(By.Id("role"));

                roleInput.Submit();
                System.Threading.Thread.Sleep(2000);

                IWebElement usernameError = driver.FindElement(By.Id("username-error"));
                
            }
            catch (NotFoundException)
            {
                Assert.Fail("Warning not shown");
            }
        }
        [Test, Order(5)]
        public void Account_LoginInNewAccount()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignOut']")).Click();
            driver.FindElement(By.CssSelector("a[href*='/']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("Kotdev");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("SeleniumTest*12^");

            passwordInput.Submit();
            System.Threading.Thread.Sleep(2000);
            Assert.That(driver.Url, Is.EqualTo(url + "users"));
        }
        [Test, Order(6)]
        public void CreateAccount_DeleteAccount()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a[href*='users/SignOut']")).Click();
            driver.FindElement(By.CssSelector("a[href*='/']")).Click();
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.CssSelector("a[href*='users/SignIn']")).Click();

            IWebElement loginInput = driver.FindElement(By.Id("username"));
            loginInput.SendKeys("Kotdev");

            IWebElement passwordInput = driver.FindElement(By.Id("password"));
            passwordInput.SendKeys("SeleniumTest*12^");

            passwordInput.Submit();
            System.Threading.Thread.Sleep(2000);
            IWebElement editElement = driver.FindElement(By.XPath("//*[contains(text(),'Kotdev')]"));
            editElement.FindElement(By.XPath("./following-sibling::td/*[contains(text(),'Delete')]")).Click();
            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            System.Threading.Thread.Sleep(1000);
            Assert.That(driver.Url, Is.EqualTo(url + "users"));
        }
    }
}
