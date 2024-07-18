using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

namespace WorkingWithWindows
{
    public class TestsSwitchingWindows
    {
        IWebDriver driver;
        WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/windows");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test, Order(1)]
        public void HandleMultipleWindows()
        {
            // Click on the "Click Here" link to open a new window
            driver.FindElement(By.LinkText("Click Here")).Click();

            // Get all window handles
            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            // Eshure there are at least two windows open
            Assert.That(windowHandles.Count(), Is.EqualTo(2), "There should be two windows open.");

            // Switch to the new window
            driver.SwitchTo().Window(windowHandles[1]);

            // Veify the content of the new window
            string newWindowContent = driver.PageSource;
            Assert.IsTrue(newWindowContent.Contains("New Window"),
                "The content of the new window is not as expected");

            // Log the content of the new window 
            string path = Path.Combine(Directory.GetCurrentDirectory(), "windows.txt");
            if (File.Exists(path))
            { 
                File.Delete(path);
            }
            File.AppendAllText(path, "Window handle for new window: " 
                + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "The page content: " + newWindowContent + "\n\n");

            // Close the new window
            driver.Close();

            // Switch back to the original window
            driver.SwitchTo().Window(windowHandles[0]);

            // Verify the content of the original window
            string originalWindowContent = driver.PageSource;
            Assert.IsTrue(originalWindowContent.Contains("Opening a new window"),
                "The content of the original window is not as expected.");

            // Log the content of the original window
            File.AppendAllText(path, "Window handle for new window: "
                + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "The page content: " + newWindowContent + "\n\n");
        }

        [Test, Order(2)]
        public void NoSuchWindowException()
        {
            // Click on the "Click Here" link to open a new window
            driver.FindElement(By.LinkText("Click Here")).Click();

            // Get all window handles
            ReadOnlyCollection<string> windowHandles = driver.WindowHandles;

            // Eshure there are at least two windows open
            Assert.That(windowHandles.Count(), Is.EqualTo(2), "There should be two windows open.");

            // Switch to the new window
            driver.SwitchTo().Window(windowHandles[1]);

                // Close the new window
            driver.Close();

            try
            {
                // Attempt to switch back to the closed window
                driver.SwitchTo().Window(windowHandles[1]);
            }
            catch (NoSuchWindowException ex)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "windows.txt");
                File.AppendAllText(path, "NoSuchWindowException caught: " + ex.Message + "\n\n");
                Assert.Pass("NoSuchWindowException was correctly handled.");
            }
            catch (Exception ex)
            { 
                Assert.Fail("An unexpected exception was thrown: " + ex.Message);
            }
            finally
            {
                // Switch back to the original window
                driver.SwitchTo().Window(windowHandles[0]);
            }
        }
    }
}