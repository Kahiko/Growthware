Imports DALModel

Namespace Base.Interfaces
	Public Interface INavMenu
		Function GetNavLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		Function GetLineMenuLinks(ByVal MenuDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer) As DataSet
		Function GetNavType(ByVal dsNavType As DataSet) As DataSet
		Function GetHierarchicalMenuData(ByRef retDataset As DataSet, ByVal BUSINESS_UNIT_SEQ_ID As Integer, ByVal ACCOUNT_SEQ_ID As Integer) As DataSet
		Sub GetRootLinks(ByRef YourDataSet As DataSet, ByVal ACCOUNT_SEQ_ID As Integer, ByVal BUSINESS_UNIT_SEQ_ID As Integer)
	End Interface
End Namespace