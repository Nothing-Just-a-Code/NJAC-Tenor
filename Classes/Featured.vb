Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Threading
Imports System.Threading.Tasks
Imports NJACTenor.TenorClient
Imports Newtonsoft.Json.Linq
Imports System.Reflection
Public Class Featured
    Private client As TenorClient
    Sub New(ByVal _client As TenorClient)
        Me.client = _client
    End Sub

    ''' <summary>
    ''' Get Featured Images.
    ''' </summary>
    ''' <param name="limit">Search Items Limit. (Default is 10). Do Not Use Higher Limit if you Don't Need to. It May Takes Slightly Longer Time to Get Results.</param>
    ''' <returns><see cref="ImageSearchReader"/> An NJAC Class Which Contains the Searched Items and Informations.</returns>
    Public Async Function GetFeaturedGIFs(Optional ByVal limit As Integer = 10) As Task(Of ImageSearchReader)
        If limit > 50 Or limit < 1 Then
            Throw New ArgumentException("Search limit cannot be less than 1 and more than 50.")
        Else
            Try
                Dim data As JObject
                Dim content As String = $"{Me.client.BaseUrl}featured?key={Me.client.APIKey}&client_key={Me.client.ClientKey}&limit={limit}&locale={client.Locale}"
                Using http As New HttpClient()
                    http.DefaultRequestHeaders.Add("User-Agent", Me.client.UserAgent)
                    Dim response As HttpResponseMessage = Await http.GetAsync(content)
                    response.EnsureSuccessStatusCode()
                    Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                    data = JObject.Parse(jsonResponse)
                    If IsNothing(data("results")) Or data("results").Count = 0 Then Throw New ArgumentException("There are no results found.")
                End Using
                Dim l As New List(Of ImageItem)()
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
                    ISR.NextPageToken = New NextPageToken() With {.ItemType = SearchItemType.GIF, .Query = content, .NextToken = data("next").ToString()}
                End If
                Return ISR
            Catch ex As Exception
                Throw New ArgumentException(message:=ex.Message)
            End Try
        End If
    End Function

    ''' <summary>
    ''' Get Featured Stickers.
    ''' </summary>
    ''' <param name="limit">Search Items Limit. (Default is 10). Do Not Use Higher Limit if you Don't Need to. It May Takes Slightly Longer Time to Get Results.</param>
    ''' <param name="stickerfilter">Select Which Type of Stickers You Want to Search. 'Static' or 'Animated' or 'Both'
    ''' <list type="bullet"><item><see cref="StickerFilter.AnimatedAndStatic"/> - Search Both Type of Stickers. (Animated and Static).</item>
    ''' <item><see cref="StickerFilter.AnimatedOnly"/> - Search Only Animated Stickers.</item>
    ''' <item><see cref="StickerFilter.StaticOnly"/> - Search Only Static Stickers.</item></list></param>
    ''' <returns><see cref="StickerSearchReader"/> An NJAC Class Which Contains the Searched Items and Informations.</returns>
    Public Async Function GetFeaturedStickers(Optional ByVal limit As Integer = 10, Optional stickerfilter As StickerFilter = StickerFilter.AnimatedAndStatic) As Task(Of StickerSearchReader)
        If limit > 50 Or limit < 1 Then
            Throw New ArgumentException("Search limit cannot be less than 1 and more than 50.")
        Else
            Try
                Dim data As JObject
                Dim content As String
                If stickerfilter = StickerFilter.AnimatedAndStatic Then
                    content = $"{Me.client.BaseUrl}featured?key={Me.client.APIKey}&client_key={Me.client.ClientKey}&limit={limit}&searchfilter=sticker&locale={client.Locale}"
                ElseIf stickerfilter = StickerFilter.AnimatedOnly Then
                    content = $"{Me.client.BaseUrl}featured?key={Me.client.APIKey}&client_key={Me.client.ClientKey}&limit={limit}&searchfilter=sticker,-static&locale={client.Locale}"
                ElseIf stickerfilter = StickerFilter.StaticOnly Then
                    content = $"{Me.client.BaseUrl}featured?key={Me.client.APIKey}&client_key={Me.client.ClientKey}&limit={limit}&searchfilter=sticker,static&locale={client.Locale}"
                End If
                Using http As New HttpClient()
                    http.DefaultRequestHeaders.Add("User-Agent", Me.client.UserAgent)
                    Dim response As HttpResponseMessage = Await http.GetAsync(content)
                    response.EnsureSuccessStatusCode()
                    Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                    data = JObject.Parse(jsonResponse)
                End Using
                If IsNothing(data("results")) Or data("results").Count = 0 Then Throw New ArgumentException("There are no results found.")
                Dim l As New List(Of StickerItem)()
                Dim SSR As New StickerSearchReader()
                For Each result In data("results")
                    Dim imgitem As New StickerItem()
                    If Not IsEmpty(result("media_formats")("gif")("url").ToString()) Then imgitem.GIF = result("media_formats")("gif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("gifpreview")("url").ToString()) Then imgitem.GIFPreview = result("media_formats")("gifpreview")("url").ToString()
                    If Not IsEmpty(result("media_formats")("mediumgif")("url").ToString()) Then imgitem.MediumSizeGIF = result("media_formats")("mediumgif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("nanogifpreview")("url").ToString()) Then imgitem.NanoPreview = result("media_formats")("nanogifpreview")("url").ToString()
                    If Not IsEmpty(result("itemurl").ToString()) Then imgitem.PageURL = result("itemurl").ToString()
                    If Not IsEmpty(result("media_formats")("tinygif")("url").ToString()) Then imgitem.TinyGIF = result("media_formats")("tinygif")("url").ToString()
                    If Not IsEmpty(result("media_formats")("webm")("url").ToString()) Then imgitem.WebM = result("media_formats")("webm")("url").ToString()
                    If Not IsEmpty(result("media_formats")("webp")("url").ToString()) Then imgitem.WebP = result("media_formats")("webp")("url").ToString()
                    If Not IsEmpty(result("media_formats")("tinygif_transparent")("url").ToString()) Then imgitem.TinyGIFTransparent = result("media_formats")("tinygif_transparent")("url").ToString()
                    If Not IsEmpty(result("media_formats")("gif_transparent")("url").ToString()) Then imgitem.GIFTransparent = result("media_formats")("gif_transparent")("url").ToString()
                    Dim taglist As New List(Of String)()
                    For Each tag In result("tags")
                        taglist.Add(tag.ToString())
                    Next
                    imgitem.Tags = taglist.AsReadOnly()
                    l.Add(imgitem)
                Next
                SSR.Stickers = l.AsReadOnly()
                If data("next") IsNot Nothing AndAlso Not IsEmpty(data("next").ToString()) Then
                    SSR.NextPageToken = New NextPageToken() With {.ItemType = SearchItemType.Sticker, .Query = content, .NextToken = data("next").ToString()}
                End If
                Return SSR
            Catch ex As Exception
                Throw New ArgumentException(message:=ex.Message)
            End Try
        End If
    End Function

    ''' <summary>
    ''' Go to the Next Page (If any)
    ''' </summary>
    ''' <param name="nextpagetoken">Use <see cref="NextPageToken"/> Which Contains Values like Token and Query To Go to the Next Page.</param>
    ''' <returns>Return as <see cref="Object"/>. It Can be <see cref="ImageSearchReader"/> or <see cref="StickerSearchReader"/> Depend on Your Search Query.</returns>
    Public Async Function GoNextPage(ByVal nextpagetoken As NextPageToken) As Task(Of Object)
        If String.IsNullOrEmpty(nextpagetoken.NextToken) Or String.IsNullOrWhiteSpace(nextpagetoken.NextToken) Then
            Throw New ArgumentException($"'{NameOf(nextpagetoken.NextToken)}' cannot be empty.", NameOf(nextpagetoken.NextToken))
        End If
        If nextpagetoken.ItemType = SearchItemType.GIF Then
            Return Await GoToNextPageGIF(nextpagetoken)
        ElseIf nextpagetoken.ItemType = SearchItemType.Sticker Then
            Return Await GoToNextPageSticker(nextpagetoken)
        End If
    End Function

    Private Async Function GoToNextPageSticker(ByVal nextpagetoken As NextPageToken) As Task(Of StickerSearchReader)
        Try
            Dim data As JObject
            Dim query As String = $"{nextpagetoken.Query}&pos={nextpagetoken.NextToken}"
            Using http As New HttpClient()
                http.DefaultRequestHeaders.Add("User-Agent", Me.client.UserAgent)
                Dim response As HttpResponseMessage = Await http.GetAsync(query)
                response.EnsureSuccessStatusCode()
                Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                data = JObject.Parse(jsonResponse)
            End Using
            If IsNothing(data("results")) Or Not data("results").Any() Then Throw New ArgumentException("There are no results found.")
            Dim l As New List(Of StickerItem)()
            Dim SSR As New StickerSearchReader()
            For Each result In data("results")
                Dim imgitem As New StickerItem()
                If Not IsEmpty(result("media_formats")("gif")("url").ToString()) Then imgitem.GIF = result("media_formats")("gif")("url").ToString()
                If Not IsEmpty(result("media_formats")("gifpreview")("url").ToString()) Then imgitem.GIFPreview = result("media_formats")("gifpreview")("url").ToString()
                If Not IsEmpty(result("media_formats")("mediumgif")("url").ToString()) Then imgitem.MediumSizeGIF = result("media_formats")("mediumgif")("url").ToString()
                If Not IsEmpty(result("media_formats")("nanogifpreview")("url").ToString()) Then imgitem.NanoPreview = result("media_formats")("nanogifpreview")("url").ToString()
                If Not IsEmpty(result("itemurl").ToString()) Then imgitem.PageURL = result("itemurl").ToString()
                If Not IsEmpty(result("media_formats")("tinygif")("url").ToString()) Then imgitem.TinyGIF = result("media_formats")("tinygif")("url").ToString()
                If Not IsEmpty(result("media_formats")("webm")("url").ToString()) Then imgitem.WebM = result("media_formats")("webm")("url").ToString()
                If Not IsEmpty(result("media_formats")("webp")("url").ToString()) Then imgitem.WebP = result("media_formats")("webp")("url").ToString()
                If Not IsEmpty(result("media_formats")("tinygif_transparent")("url").ToString()) Then imgitem.TinyGIFTransparent = result("media_formats")("tinygif_transparent")("url").ToString()
                If Not IsEmpty(result("media_formats")("gif_transparent")("url").ToString()) Then imgitem.GIFTransparent = result("media_formats")("gif_transparent")("url").ToString()
                Dim taglist As New List(Of String)()
                For Each tag In result("tags")
                    taglist.Add(tag.ToString())
                Next
                imgitem.Tags = taglist.AsReadOnly()
                l.Add(imgitem)
            Next
            SSR.Stickers = l.AsReadOnly()
            If data("next") IsNot Nothing AndAlso Not IsEmpty(data("next").ToString()) Then
                SSR.NextPageToken = New NextPageToken() With {.ItemType = SearchItemType.Sticker, .Query = query, .NextToken = data("next").ToString()}
            End If
            Return SSR
        Catch ex As Exception
            Throw New ArgumentException(message:=ex.Message)
        End Try
    End Function

    Private Async Function GoToNextPageGIF(ByVal nextpage As NextPageToken) As Task(Of ImageSearchReader)
        Try
            Dim data As JObject
            Dim query As String = $"{nextpage.Query}&pos={nextpage.NextToken}"
            Using http As New HttpClient()
                http.DefaultRequestHeaders.Add("User-Agent", Me.client.UserAgent)
                Dim response As HttpResponseMessage = Await http.GetAsync(query)
                response.EnsureSuccessStatusCode()
                Dim jsonResponse As String = Await response.Content.ReadAsStringAsync()
                data = JObject.Parse(jsonResponse)
                If IsNothing(data("results")) Or data("results").Count = 0 Then Throw New ArgumentException("There are no results found.")
            End Using
            Dim l As New List(Of ImageItem)()
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
                ISR.NextPageToken = New NextPageToken() With {.ItemType = SearchItemType.GIF, .Query = query, .NextToken = data("next").ToString()}
            End If
            Return ISR
        Catch ex As Exception
            Throw New ArgumentException(message:=ex.Message)
        End Try
    End Function
End Class
