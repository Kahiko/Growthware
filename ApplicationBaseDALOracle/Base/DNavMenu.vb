Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Imports ApplicationBase.Common
Imports ApplicationBase.Model.Group
Imports ApplicationBase.Interfaces
Imports ApplicationBaseDALOracle.SharedOracle

Public Class DNavMenu
	Implements INavMenu

	Private _ConnectionString As String = String.Empty
	Private MenuWidth As Integer = 100

	'*********************************************************************
	'
	' GetNavType Method
	'
	' Returns all of the navigation types from the database.
	'
	'*********************************************************************
	Public Function GetNavType(ByVal dsNavType As System.Data.DataSet) As System.Data.DataSet Implements INavMenu.GetNavType
		Try
			dsNavType = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Nav_Types")
		Catch ex As Exception
			Throw ex
		End Try
		Return dsNavType
	End Function	  'GetNavType

	'*********************************************************************
	' GetNavLinks function
	' Represents all left hand menu items
	'*********************************************************************
	Public Function GetNavLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet Implements INavMenu.GetNavLinks
		Try
			MenuDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Left_NavLinks", New OracleParameter() { _
			New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
		Catch ex As Exception
			Throw ex
		End Try
		Return MenuDataSet
	End Function	' GetNavLinks

	'*********************************************************************
	' GetLineMenuLinks function
	' Represents all left hand menu items
	'*********************************************************************
	Public Function GetLineMenuLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet Implements INavMenu.GetLineMenuLinks
		Try
			MenuDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_LineMenu_NavLinks", New OracleParameter() { _
			New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID), _
			New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID)})
		Catch ex As Exception
			Throw ex
		End Try
		Return MenuDataSet
	End Function	' GetLineMenuLinks

	Public Function GetHierarchicalMenuData(ByRef retDataset As System.Data.DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal ACCOUNT_SEQ_ID As Integer) As System.Data.DataSet Implements INavMenu.GetHierarchicalMenuData
		Try
			retDataset = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Hierarchical_Menu_Data", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID)})
		Catch ex As Exception
			Throw ex
		End Try
		Return retDataset
	End Function

	Public Sub GetRootLinks(ByRef YourDataSet As System.Data.DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) Implements INavMenu.GetRootLinks
		Try
			YourDataSet = OracleHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "ZB_SECURITY_PKG.ZBP_get_Root_Links", New OracleParameter() {New OracleParameter("@P_BUSINESS_UNIT_SEQ_ID", BUSINESS_UNIT_SEQ_ID), New OracleParameter("@P_ACCOUNT_SEQ_ID", ACCOUNT_SEQ_ID)})
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	'*********************************************************************
	'
	' ConnectionString Method
	'
	' Get the connection string from the web.config file.
	'
	'*********************************************************************
	Private Function ConnectionString() As String
		If _ConnectionString = String.Empty Then
			' try to decrypt the connection string
            _ConnectionString = ConnectionHelper.GetConnectionString("ApplicationBaseDALOracle")
		End If
		Return _ConnectionString
	End Function
End Class