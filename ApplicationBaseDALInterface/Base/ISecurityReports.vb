Public Interface ISecurityReports
    Function SecurityByRole(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet
    Function Security4Module(ByVal Business_Unit_SEQ_ID As Integer, ByVal ENVIRONMENT As String) As DataSet
End Interface
