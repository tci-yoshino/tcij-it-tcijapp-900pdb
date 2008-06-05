Public Partial Class CountryList
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcCountry.SelectCommand = "SELECT [CountryCode], [CountryName], [DefaultQuoLocationName], './CountrySetting.aspx?Action=Edit&Code=' + [CountryCode] AS Url FROM [v_Country]"
    End Sub

End Class