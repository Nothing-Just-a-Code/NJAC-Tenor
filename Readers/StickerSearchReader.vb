Imports Newtonsoft.Json
Public Class StickerSearchReader
    ''' <summary>
    ''' A Collection of Searched Stickers.
    ''' </summary>
    ''' <returns></returns>
    Public Property Stickers As System.Collections.ObjectModel.ReadOnlyCollection(Of StickerItem)

    ''' <summary>
    ''' Token to Open the Next Page of Searched Images.
    ''' </summary>
    ''' <remarks>Token will be <c>null</c> if there is no more page.</remarks>
    ''' <returns></returns>
    Public Property NextPageToken As NextPageToken
End Class
