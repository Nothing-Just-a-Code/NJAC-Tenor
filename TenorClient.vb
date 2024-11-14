Imports Newtonsoft.Json
Imports System.Text
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports NJACTenor.Language
Imports Newtonsoft.Json.Linq
Imports System.Collections.ObjectModel
Imports NJACTenor.Interfaces


''' <summary>
''' NJAC Tenor Client
''' </summary>
Public NotInheritable Class TenorClient : Implements Interfaces.ITenorClient
    ''' <summary>
    ''' A Class to Search GIF Images from Tenor.
    ''' </summary>
    ''' <returns></returns>
    Public Property GIFSearch As Search Implements ITenorClient.GIFSearch

    ''' <summary>
    ''' A Class to Get Featured Images.
    ''' </summary>
    ''' <returns></returns>
    Public Property Featured As Featured Implements ITenorClient.Featured

    ''' <summary>
    ''' Your Tenor API Key.
    ''' </summary>
    ''' <returns></returns>
    Public Property APIKey As String Implements ITenorClient.APIKey

    ''' <summary>
    ''' Base URL for all Tenor HTTP Requests.
    ''' </summary>
    ''' <remarks>https://tenor.googleapis.com/v2/</remarks>
    ''' <returns></returns>
    Public ReadOnly Property BaseUrl As String = "https://tenor.googleapis.com/v2/" Implements ITenorClient.BaseUrl

    ''' <summary>
    ''' Your Tenor Client Key
    ''' </summary>
    '''<example>You can use any value if you don't have any Client Key.</example> 
    ''' <returns></returns>
    Public Property ClientKey As String Implements ITenorClient.ClientKey

    ''' <summary>
    ''' Locale (Default is en_US). Don't modify it if you don't want to change Locale from English to another Language.
    ''' ''' </summary>
    ''' <returns></returns>
    Public Property Locale As String = "en_US" Implements ITenorClient.Locale

    ''' <summary>
    ''' User Agent (Don't change it if you don't know need to.)
    ''' </summary>
    ''' <returns></returns>
    Public Property UserAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:132.0) Gecko/20100101 Firefox/132.0" Implements ITenorClient.UserAgent

    ''' <summary>
    ''' Create a new instance by using Tenor API Key.
    ''' </summary>
    ''' <param name="API">Your Tenor API Key</param>
    Sub New(ByVal API As String)
        Me.APIKey = API
        Me.GIFSearch = New Search(Me)
        Me.Featured = New Featured(Me)
    End Sub
    Enum SearchItemType
        GIF
        Sticker
    End Enum
    Enum StickerFilter
        AnimatedOnly
        StaticOnly
        AnimatedAndStatic
    End Enum
    Enum CategoryType
        ''' <summary>
        ''' Get the 'Trending' Category of GIF Images.
        ''' </summary>
        Trending
        ''' <summary>
        ''' Get the 'Featured' Category of GIF Images.
        ''' </summary>
        Featured
    End Enum

    Friend Shared Function IsEmpty(ByVal text As String) As Boolean
        Return String.IsNullOrEmpty(text) Or String.IsNullOrWhiteSpace(text)
    End Function

    ''' <summary>
    ''' Get Categories of Tenor Images.
    ''' </summary>
    ''' <param name="categorytype">Select of which type the category you need to grab.</param>
    ''' <returns></returns>
    Public Async Function GetCategories(Optional categorytype As CategoryType = CategoryType.Featured) As Task(Of ReadOnlyCollection(Of CategoryItem)) Implements ITenorClient.GetCategories
        Dim content As String
        If categorytype = CategoryType.Featured Then
            content = $"{BaseUrl}categories?key={Me.APIKey}&type=featured&client_key={Me.ClientKey}&locale={Me.Locale}&contentfilter=off"
        ElseIf categorytype = CategoryType.Trending Then
            content = $"{BaseUrl}categories?key={Me.APIKey}&type=trending&client_key={Me.ClientKey}&locale={Me.Locale}&contentfilter=off"
        End If
        Try
            Dim data As JObject
            Using http As New HttpClient()
                http.DefaultRequestHeaders.Add("User-Agent", Me.UserAgent)
                Dim Res As HttpResponseMessage = Await http.GetAsync(content)
                Res.EnsureSuccessStatusCode()
                Dim jsonResponse As String = Await Res.Content.ReadAsStringAsync()
                data = JObject.Parse(jsonResponse)
            End Using
            If IsNothing(data) Or IsNothing(data("tags")) Or Not data("tags").Any() Then Throw New ArgumentException("There are no results found.")
            Dim l As New List(Of CategoryItem)()
            For Each item In data("tags")
                Dim CI As New CategoryItem(Me)
                If Not IsEmpty(item("searchterm").ToString()) Then CI.SearchTerm = item("searchterm").ToString()
                If Not IsEmpty(item("path").ToString()) Then CI.Query = item("path").ToString()
                If Not IsEmpty(item("image").ToString()) Then CI.Image = item("image").ToString()
                If Not IsEmpty(item("name").ToString()) Then CI.Name = item("name").ToString()
                l.Add(CI)
            Next
            Return l.AsReadOnly()
        Catch ex As Exception
            Throw New ArgumentException(ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Get the AutoComplete Suggestion. (Helpful if you want to make your application's search system more easy)
    ''' </summary>
    ''' <param name="term">A Short Text to Get the List of AutoComplete Search Items.</param><example>If you want to get the list of all search items starts with 'good' like 'good day' or 'good morning' then type 'good' in Term.</example>
    ''' <param name="limit">Search Items Limit. (Default is 10). Do not use Higher Limit if You Don't Need to.</param>
    ''' <returns>A <see cref="ReadOnlyCollection(Of String)"/> which contains the AutoComplete Suggestions.</returns>
    Public Async Function AutoCompleteSuggestions(ByVal term As String, ByVal Optional limit As Integer = 5) As Task(Of ReadOnlyCollection(Of String)) Implements ITenorClient.AutoCompleteSuggestions
        If limit > 50 Or limit < 1 Then
            Throw New ArgumentException("Search limit cannot be less than 1 and more than 50.")
        Else
            Try
                Dim data As JObject
                Using http As New HttpClient()
                    http.DefaultRequestHeaders.Add("User-Agent", Me.UserAgent)
                    Dim content As String = $"{Me.BaseUrl}autocomplete?q={term}&key={Me.APIKey}&client_key={Me.ClientKey}&limit={limit}&locale={Me.Locale}"
                    Dim response As HttpResponseMessage = Await http.GetAsync(content)
                    response.EnsureSuccessStatusCode()
                    Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                    data = JObject.Parse(jsonResponse)
                End Using
                Dim l As New List(Of String)()
                If IsNothing(data) Or IsNothing(data("results")) Or Not data("results").Any() Then Throw New ArgumentException("There are no results found.")
                For Each item In data("results")
                    l.Add(item.ToString())
                Next
                Return l.AsReadOnly()
            Catch ex As Exception
                Throw New ArgumentException(ex.Message)
            End Try
        End If
    End Function
End Class
