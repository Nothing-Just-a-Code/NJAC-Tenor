Imports System.Collections.ObjectModel

Public Class ImageItem
    Public Property TinyGIF As String
    Public Property NanoPreview As String
    Public Property WebM As String
    Public Property GIFPreview As String
    Public Property WebP As String
    Public Property GIF As String
    Public Property MediumSizeGIF As String
    Public Property Tags As ReadOnlyCollection(Of String)

    ''' <summary>
    ''' Get the Tenor Web Page URL of this Image.
    ''' </summary>
    ''' <returns></returns>
    Public Property PageURL As String
End Class
