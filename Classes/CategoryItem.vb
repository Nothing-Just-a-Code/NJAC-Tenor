Imports Newtonsoft.Json
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports System.Net.Http
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports NJACTenor.TenorClient
Imports System.Reflection
Public Class CategoryItem
    Private client As TenorClient
    Public Sub New(_c As TenorClient)
        Me.client = _c
    End Sub

    Public Property SearchTerm As String
    Public Property Query As String
    Public Property Image As String
    Public Property Name As String

    Public Async Function GetItems(Optional ByVal limit As Integer = 10) As Task(Of ImageSearchReader)
        If limit > 50 Or limit < 1 Then
            Throw New ArgumentException("Search limit cannot be less than 1 and more than 50.")
        Else
            Try
                Dim data As JObject
                Using http As New HttpClient()
                    http.DefaultRequestHeaders.Add("User-Agent", Me.client.UserAgent)
                    Dim content As String = $"https://tenor.googleapis.com{Query}&key={Me.client.APIKey}&limit={limit}"
                    Dim response As HttpResponseMessage = Await http.GetAsync(content)
                    response.EnsureSuccessStatusCode()
                    Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                    data = JObject.Parse(jsonResponse)
                End Using
                Dim l As New List(Of ImageItem)()
                If IsNothing(data("results")) Or data("results").Count = 0 Then Throw New ArgumentException("There are no results found.")
                Dim ISR As New ImageSearchReader()
                For Each result In data("results")
                    Dim imgitem As New ImageItem()
                    If Not IsEmpty(result("media_formats")("gif")("url").ToString()) Then imgitem.GIF = result("media_formats")("gif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("gifpreview")("url").ToString()) Then imgitem.GIFPreview = result("media_formats")("gifpreview")("url").ToString()
                    If Not IsEmpty(result("media_formats")("mediumgif")("url").ToString()) Then imgitem.MediumSizeGIF = result("media_formats")("mediumgif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("nanogifpreview")("url").ToString()) Then imgitem.NanoPreview = result("media_formats")("nanogifpreview")("url").ToString()
                    If Not IsEmpty(result("itemurl").ToString()) Then imgitem.PageURL = result("itemurl").ToString()
                    If Not IsEmpty(result("media_formats")("tinygif")("url").ToString()) Then imgitem.TinyGIF = result("media_formats")("tinygif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("webm")("url").ToString()) Then imgitem.WebM = result("media_formats")("webm")("url").ToString()
                    If Not IsEmpty(result("media_formats")("webp")("url").ToString()) Then imgitem.WebP = result("media_formats")("webp")("url").ToString()
                    Dim taglist As New List(Of String)()
                    For Each tag In result("tags")
                        taglist.Add(tag.ToString())
                    Next
                    imgitem.Tags = taglist.AsReadOnly()
                    l.Add(imgitem)
                Next
                ISR.Images = l.AsReadOnly()
                If data("next") IsNot Nothing AndAlso Not IsEmpty(data("next").ToString()) Then
                    ISR.NextPageToken = New NextPageToken() With {.ItemType = SearchItemType.GIF, .Query = Query, .NextToken = data("next").ToString()}
                End If
                Return ISR
            Catch ex As Exception
                Throw New ArgumentException(message:=ex.Message)
            End Try
        End If
    End Function

End Class
