Public Partial Class CountryList
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcCountry.SelectCommand = "SELECT [CountryCode], [CountryName], [DefaultQuoLocationName] FROM [v_Country]"
    End Sub

End Class