Imports System.Text
Imports OpenQA.Selenium
Imports OpenQA.Selenium.Chrome
Imports OpenQA.Selenium.Support.UI
Module Module1


    Sub Main()

        Dim Driver As IWebDriver
        Driver = New ChromeDriver
        Driver.Navigate().GoToUrl("https://monkeytype.com/")
        Dim Source As String = Driver.PageSource

        Dim Title As String = Driver.Title


        Dim Element As IWebElement = Driver.FindElement(By.XPath("//*[@id=""words""]/div[1]"))

        System.Threading.Thread.Sleep(5000)

        Console.WriteLine(Element)


    End Sub

End Module
