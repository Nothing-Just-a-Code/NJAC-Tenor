Imports Newtonsoft.Json
Public Class ImageSearchReader
    ''' <summary>
    ''' A Collection of Searched Images.
    ''' </summary>
    ''' <returns></returns>
    Public Property Images As System.Collections.ObjectModel.ReadOnlyCollection(Of ImageItem)

    ''' <summary>
    ''' Token to Open the Next Page of Searched Images.
    ''' </summary>
    ''' <remarks>Token will be <c>null</c> if there is no more page.</remarks>
    ''' <returns></returns>
    Public Property NextPageToken As NextPageToken
End Class

