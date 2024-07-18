using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SearchWhithImplicitWait
{
    public class TestsSearchWhithImplicitWait
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test, Order(1)]
        public void SearchProduct_Keyboard_ShouldAddToCart()
        {
            // Fill in the search field textbox
            driver.FindElement(By.Name("keywords")).SendKeys("keyboard");

            // Click on the search icon 
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            try
            {
                // Click on Buy Now Link
                driver.FindElement(By.LinkText("Buy Now")).Click();

                // Verify text
                Assert.IsTrue(driver.PageSource.Contains("keyboard"),
                    "The product 'keyboard' was not found in the cart page.");
                Console.WriteLine("Scenario completed");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exeption: " + ex.Message);
            }
        }

        [Test, Order(2)]
        public void SearchProduct_Junc_ShouldThrowNoSuchElementExeption()
        {
            // Fill in the search field textbox
            driver.FindElement(By.Name("keywords")).SendKeys("junk");

            // Click on the search icon 
            driver.FindElement(By.XPath("//input[@title=' Quick Find ']")).Click();

            try
            {
                // Try to click on Buy Now Link
                driver.FindElement(By.LinkText("Buy Now")).Click();
            }
            catch (NoSuchElementException ex)
            {
                // Verify the exeption for non-existing product
                Assert.Pass("Expected NoSuchElementExeption was thrown.");
                Console.WriteLine("Timeout - " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unecpected exeption: " + ex.Message);
            }
        }
    }
}