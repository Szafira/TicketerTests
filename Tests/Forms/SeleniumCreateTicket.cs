using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TicketerTests
{
    class SeleniumCreateTicket
    {
        private IWebDriver driver;
        private string url;

        [OneTimeTearDown]
        public void tearDown()
        {
            driver.Close();
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

            driver.FindElement(By.CssSelector("a[href*='tickets']")).Click();
            System.Threading.Thread.Sleep(1000);
        }
        [Test,Order(0)]
        public void Tickets_LinkTest()
        {
            driver.FindElement(By.CssSelector("a[href*='tickets/Create']")).Click();
            Assert.That(driver.Url, Is.EqualTo(url + "tickets/Create"));
        }
        [Test,Order(1)]
        public void Tickets_NewTicketTest()
        {
            IWebElement category = driver.FindElement(By.Id("category"));
            var selectCategory = new SelectElement(category);
            selectCategory.SelectByValue("Administrative");

            IWebElement titleInput = driver.FindElement(By.Id("title"));
            titleInput.SendKeys("Test");

            IWebElement commentInput = driver.FindElement(By.Id("comment"));
            commentInput.SendKeys("Test from Selenium");

            commentInput.Submit();

            System.Threading.Thread.Sleep(1000);
            Assert.That(driver.Url, Is.EqualTo(url + "tickets"));
        }
        [Test,Order(2)]
        public void Tickets_Edit()
        {
            IWebElement editElement = driver.FindElement(By.XPath("//*[contains(text(),'Test from Selenium')]"));
            editElement.FindElement(By.XPath("./following-sibling::td/*[contains(text(),'Edit')]")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();

            Assert.That(driver.Url, Is.EqualTo(url + "tickets"));
        }
        [Test,Order(3)] 
        public void Tickets_Details()
        {
            IWebElement editElement = driver.FindElement(By.XPath("//*[contains(text(),'Test from Selenium')]"));
            editElement.FindElement(By.XPath("./following-sibling::td/*[contains(text(),'Details')]")).Click();
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("a[href*='tickets']")).Click();
            Assert.That(driver.Url, Is.EqualTo(url + "tickets"));

        }
        [Test, Order(4)]
        public void Tickets_Delete()
        {
            IWebElement editElement = driver.FindElement(By.XPath("//*[contains(text(),'Test from Selenium')]"));
            editElement.FindElement(By.XPath("./following-sibling::td/*[contains(text(),'Delete')]")).Click();
            System.Threading.Thread.Sleep(1000);

            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            System.Threading.Thread.Sleep(1000);
            Assert.That(driver.Url, Is.EqualTo(url + "tickets"));
        }
    }
}
