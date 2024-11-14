Imports Newtonsoft.Json
Imports System.Text
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports NJACTenor.Language
Imports Newtonsoft.Json.Linq
Imports System.Collections.ObjectModel
Imports NJACTenor.TenorClient
Namespace Interfaces

    Public Interface ITenorClient
        Property GIFSearch As Search
        Property Featured As Featured

        Property APIKey As String
        ReadOnly Property BaseUrl As String
        Property ClientKey As String
        Property Locale As String
        Property UserAgent As String

        Function GetCategories(Optional categorytype As CategoryType = CategoryType.Featured) As Task(Of ReadOnlyCollection(Of CategoryItem))
        Function AutoCompleteSuggestions(ByVal term As String, ByVal Optional limit As Integer = 5) As Task(Of ReadOnlyCollection(Of String))
    End Interface
End Namespace

