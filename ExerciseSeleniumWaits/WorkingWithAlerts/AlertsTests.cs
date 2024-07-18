using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace WorkingWithAlerts
{
    public class AlertTests
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test, Order(1)]
        public void HandleBasicAlert()
        {
            // Click on the "Click for JS Alert" button
            driver.FindElement(By.XPath("//button[contains(text(), 'Click for JS Alert')]")).Click();

            // Switch to the alert
            IAlert alert = driver.SwitchTo().Alert();

            // Verify the alert text
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Alert text is not as expected.");

            // Accept the alert
            alert.Accept();

            // Verify the result message
            IWebElement resultElement = driver.FindElement(By.Id("result"));
            Assert.That(resultElement.Text, Is.EqualTo("You successfully clicked an alert"),
                "Result message is not as expected.");
        }

        [Test, Order(2)]
        public void HandleConfirmAlert()
        {
            // Click on the "Click for JS Confirm" button
            driver.FindElement(By.XPath("//button[contains(text(), 'Click for JS Confirm')]")).Click();

            // Switch to the alert
            IAlert alert = driver.SwitchTo().Alert();

            // Verify the alert text
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Alert text is not as expected.");

            // Accept the alert
            alert.Accept();

            // Verify the result message
            IWebElement resultElement = driver.FindElement(By.Id("result"));
            Assert.That(resultElement.Text, Is.EqualTo("You clicked: Ok"),
                "Result message is not as expected after accepting the alert.");

            // Trigger the alert again
            driver.FindElement(By.XPath("//button[contains(text(), 'Click for JS Confirm')]")).Click();

            // Switch to the alert
            alert = driver.SwitchTo().Alert();

            // Dismiss the alert
            alert.Dismiss();

            // Verify the result message
            resultElement = driver.FindElement(By.Id("result"));
            Assert.That(resultElement.Text, Is.EqualTo("You clicked: Cancel"),
                "Result message is not as expected after dismissing the alert.");
        }

        [Test, Order(3)]
        public void HandlePromptAlert()
        {
            // Click on the "Click for JS Prompt" button
            driver.FindElement(By.XPath("//button[contains(text(), 'Click for JS Prompt')]")).Click();

            // Switch to the alert
            IAlert alert = driver.SwitchTo().Alert();

            // Verify the alert text
            Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Alert text is not as expected.");

            // Send text to the alert
            string inputText = "Hello there!";
            alert.SendKeys(inputText);

            // Accept the alert
            alert.Accept();

            // Verify the result message
            IWebElement resultElement = driver.FindElement(By.Id("result"));
            Assert.That(resultElement.Text, Is.EqualTo("You entered: " + inputText),
                "Result message is not as expected after entering text in the prompt.");
        }
    }
}