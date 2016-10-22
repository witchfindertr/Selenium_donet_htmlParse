using System;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenQA.Selenium.Remote;
using NUnit;
using OpenQA.Selenium.Firefox;

namespace PhantomJSCrawler
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {

            Control.CheckForIllegalCrossThreadCalls = false;
            Thread s = new Thread(new ParameterizedThreadStart(GetIframeResult));
            s.IsBackground = true;
            s.Start(urlTextBox.Text);
            Console.WriteLine("线程已启动");
            //while (Process.GetProcessesByName("PhantomJS").Length > 0)
            //{

            //    WindowChange.ToChange(Process.GetProcessesByName("PhantomJS")[0].MainWindowHandle, false);

            //}


        }


        private void Log(string detail)
        {
            this.logTextBox.AppendText(detail + "\r\n");
        }

        private void GetIframeResult(object uri)
        {

            //IWebDriver driver = new ChromeDriver();+
            /*phantomjs*/
            //var phantomJSDriverService = PhantomJSDriverService.CreateDefaultService();
            //phantomJSDriverService.ProxyType = "none";
            //PhantomJSOptions options = new PhantomJSOptions();
            ////options.AddAdditionalCapability("cssSelectorsEnabled",false);
            //IWebDriver driver = new PhantomJSDriver(phantomJSDriverService);

            /*firefox*/

            //IWebDriver driver = new FirefoxDriver();

            /*chrome*/
            ChromeDriverService cds = ChromeDriverService.CreateDefaultService();
            cds.HideCommandPromptWindow = true;
            IWebDriver driver = new ChromeDriver(cds);



            driver.Navigate().GoToUrl(uri as string);
            //string pageInfo = driver.PageSource;
           
            //var driver = new PhantomJSDriver
            //{
            //    Url = uri as string,
            //};

            //driver.Navigate();

            //var source = driver.PageSource;
            ReadOnlyCollection<IWebElement> elem = driver.FindElements(By.TagName("iframe"));
            foreach (IWebElement item in elem)
            {
                Log(item.GetAttribute("src"));

            }
           // Screenshot ss = driver.GetScreenshot();
            //ss.SaveAsFile(driver.Title + ".png", System.Drawing.Imaging.ImageFormat.Png);
            driver.Close();
            driver.Dispose();
            driver.Quit();


        }

        private void logTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }


        private void Mainform_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }



    }


    public class WindowChange
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;          //常量，隐藏
        private const int SW_SHOWNORMAL = 1;    //常量，显示，标准状态
        private const int SW_SHOWMINIMIZED = 2; //常量，显示，最小化
        private const int SW_SHOWMAXIMIZED = 3; //常量，显示，最大化
        private const int SW_SHOWNOACTIVATE = 4;//常量，显示，不激活
        private const int SW_RESTORE = 9;       //常量，显示，回复原状
        private const int SW_SHOWDEFAULT = 10;  //常量，显示，默认

        public static void ToChange(IntPtr p, bool isboolean)
        {
            if (isboolean)
            {
                ShowWindowAsync(p, SW_SHOWNORMAL);
            }
            else
            {
                ShowWindowAsync(p, SW_HIDE);
            }
        }
    }
}
