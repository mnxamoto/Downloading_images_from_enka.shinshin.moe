using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Drawing;

FirefoxOptions firefoxOptions = new FirefoxOptions();
//firefoxOptions.AddArgument("--headless");
IWebDriver driver = new FirefoxDriver(@".\", firefoxOptions);

driver.Navigate().GoToUrl("https://enka.shinshin.moe/u/703940429");

List<string> urlsImage = new List<string>();
int timeSleep = 1000 * 5;

List<IWebElement> webElements = driver.FindElements(By.XPath($"//*[@class='avatar svelte-188i0pk']")).ToList();

IWebElement button = driver.FindElement(By.XPath("/html/body/main/content/div[3]/div[1]/button"));
button.Click();
Thread.Sleep(timeSleep);

foreach (IWebElement webElement in webElements)
{
    webElement.Click();
    Thread.Sleep(timeSleep);
    button.Click();
    Thread.Sleep(timeSleep);
}

webElements = driver.FindElements(By.XPath("/html/body/main/content/div[3]/img")).ToList();

foreach (IWebElement webElement in webElements)
{
    string url = webElement.GetAttribute("src");
    urlsImage.Add(url);
}

foreach (var url in urlsImage)
{
    string nameFile = url.Replace("blob:https://enka.shinshin.moe/", "");
    string path = @$".\Карточки\{nameFile}.png";
    ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
    driver.SwitchTo().Window(driver.WindowHandles.Last());
    driver.Navigate().GoToUrl(url);
    Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
    Bitmap bmpScreen = new Bitmap(new MemoryStream(screenshot.AsByteArray));
    IWebElement element = driver.FindElement(By.XPath("/html/body/img"));
    var cropArea = new Rectangle(element.Location, element.Size);
    bmpScreen = bmpScreen.Clone(cropArea, bmpScreen.PixelFormat);
    bmpScreen.Save(path);
}

driver.Quit();
