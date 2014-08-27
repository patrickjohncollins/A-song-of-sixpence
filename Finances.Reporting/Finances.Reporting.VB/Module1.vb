Module Module1

    Sub Main()

    End Sub

    Public Function BuildQS(ByVal MinYear As Integer, ByVal MaxYear As Integer, ByVal MinQuarter As Integer, ByVal MaxQuarter As Integer, ByVal MinMonth As Integer, ByVal MaxMonth As Integer, ByVal Category As String, ByVal MinAccount As Integer, ByVal MaxAccount As Integer) As String
        Return "http://local.finances.com/Transaction/?" & QS("year=", MinYear, MaxYear) & QS("&quarter=", MinQuarter, MaxQuarter) & QS("&month=", MinMonth, MaxMonth) & QS("&AccountID=", MinAccount, MaxAccount) & "&category=" & Category
    End Function

    Public Function QS(ByVal QueryString As String, ByVal Min As Integer, ByVal Max As Integer) As String
        If Min = Max Then
            Return QueryString & Min
        Else
            Return ""  ' If the values differ it means that the current selection spans multiple values in the range, for example several months, therefore don't filter by this criteria.
        End If
    End Function

End Module
