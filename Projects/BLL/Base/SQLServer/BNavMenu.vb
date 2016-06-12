Imports DALFactory.Base.Application
Imports DALInterface.Base.Interfaces
Imports System.Runtime.InteropServices

Namespace Base.SQLServer
    Public Class BNavMenu
		'Private Shared iBaseDAL As INavMenu = FNavMenu.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"))
		Private Shared iBaseDAL As INavMenu = AbstractFactory.Create(Configuration.ConfigurationSettings.AppSettings("BaseDAL"), "DNavMenu")

        Public Shared Function GetHierarchicalMenuData(ByRef retDataset As DataSet, ByVal BUSINESS_SEQ_ID As Integer, ByVal ACCOUNT_SEQ_ID As Integer) As DataSet
			Return iBaseDAL.GetHierarchicalMenuData(retDataset, BUSINESS_SEQ_ID, ACCOUNT_SEQ_ID)
        End Function

        Public Shared Function GetLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_SEQ_ID As Integer) As DataSet
			Return iBaseDAL.GetNavLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_SEQ_ID)
        End Function

        Public Shared Function GetLineMenuLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_SEQ_ID As Integer) As DataSet
			Return iBaseDAL.GetLineMenuLinks(MenuDataSet, ACCOUNT_SEQ_ID, BUSINESS_SEQ_ID)
        End Function
        Public Shared Function GetNavType(ByVal dsNavType As DataSet) As DataSet
			Return iBaseDAL.GetNavType(dsNavType)
        End Function

        Public Shared Function GetRootLinks(ByRef YourDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_SEQ_ID As Integer)
			iBaseDAL.GetRootLinks(YourDataSet, ACCOUNT_SEQ_ID, BUSINESS_SEQ_ID)
        End Function
    End Class
End Namespace