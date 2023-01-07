Imports System
Imports PuppeteerSharp


Module Program
    Sub Main()


        ExecuteAsync().GetAwaiter().GetResult()


    End Sub

    Private Async Function ExecuteAsync() As Task


        Dim browser = Await Puppeteer.LaunchAsync(New LaunchOptions With {.Headless = False, .ExecutablePath = "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"})
        Dim page = Await browser.NewPageAsync()


        Await page.GoToAsync("https://monkeytype.com/")

        Await Task.Delay(10000)

        Dim words = Await GetWordsAsync(Await page.QuerySelectorAllAsync("div.word"))

        Await page.TypeAsync("#wordsInput", words, New Input.TypeOptions With {.Delay = 0})

        Dim activeWords = Await page.QuerySelectorAsync("div.word.active")

        While activeWords IsNot Nothing
            For Each letter In Await activeWords.QuerySelectorAllAsync("letter")
                Dim innerText = Await letter.GetPropertyAsync("innerText")
                Dim value = Await innerText.JsonValueAsync()

                Await page.TypeAsync("#wordsInput", value)
            Next

            Await page.TypeAsync("#wordsInput", " ")

            activeWords = Await page.QuerySelectorAsync("div.word.active")
        End While

        Await StoreDataOntoFile(page)

    End Function

    Private Async Function GetWordsAsync(words As ElementHandle()) As Task(Of String)
        Dim wordList As New List(Of String)

        For Each word In words
            Dim letters = Await word.QuerySelectorAllAsync("letter")

            For Each letter In letters
                Dim innerText = Await letter.GetPropertyAsync("innerText")
                Dim value = Await innerText.JsonValueAsync()

                wordList.Add(value)
            Next
            wordList.Add(" ")
        Next

        Return String.Join("", wordList)
    End Function

    Private Async Function StoreDataOntoFile(page As Page) As Task


        Dim RawDiv = Await page.WaitForSelectorAsync("div.group.raw")
        Dim Rawresult = Await RawDiv.QuerySelectorAsync("div.bottom")

        Dim Raw = Await Rawresult.EvaluateFunctionAsync(Of String)("e => e.getAttribute('aria-label')")

        '===================================

        Dim ConsDiv = Await page.WaitForSelectorAsync("div.group.consistency")
        Dim Consresult = Await ConsDiv.QuerySelectorAsync("div.bottom")

        Dim Cons = Await Consresult.EvaluateFunctionAsync(Of String)("e => e.getAttribute('aria-label')")

        '===================================

        Dim Wpm As String = ("")




        Console.WriteLine("Raw typing speed = " & Raw & " WPM")
        Console.WriteLine("=====================================")
        Console.WriteLine("Consistency while typing = " & Cons)

        Console.ReadLine()
        'next write to file

        'C:\Users\sebcl\OneDrive\Desktop\A-LVL\CS\Experiments and practice\AutoTyper\RecordList

    End Function
End Module
