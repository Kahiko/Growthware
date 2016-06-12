Imports System.Collections.Generic
Imports GrowthWare.Framework.Model.Profiles.Base
Imports GrowthWare.Framework.Model.Profiles.Base.Interfaces
Imports System.Runtime.Serialization

Namespace Profiles.Base
	<Serializable()> _
	Public MustInherit Class MProfileCollection
		Inherits Hashtable

		Protected m_ByID As New Hashtable
		Protected m_ByName As New Hashtable

		Default Public Overloads Overrides Property Item(ByVal key As [Object]) As [Object]
			Set(ByVal Value As [Object])
				' Don't allow duplicate values
				If Contains(key) Then Exit Property
				Dim mProfile As MProfile = CType(Value, MProfile)
				If Not mProfile.Id = -1 Then m_ByID.Add(mProfile.Id, Value)
				If Not mProfile.Name.Trim.Length = 0 Then m_ByName.Add(mProfile.Name.ToLower, Value)
				MyBase.Item(key) = Value
			End Set
			Get
				Return MyBase.Item(key)
			End Get
		End Property

		Public Overrides Sub Add(ByVal key As [Object], ByVal value As [Object])
			' Don't allow duplicate values
			If Contains(key) Then Exit Sub
			Dim _Profile As MProfile = CType(value, MProfile)
			m_ByID.Add(_Profile.Id, value)
			m_ByName.Add(_Profile.Name.Trim.ToLower, value)
			MyBase.Add(key, value)
		End Sub

		Public Function GetByID(ByVal SEQ_ID As Integer) As IMProfile
			Return CType(m_ByID(SEQ_ID), IMProfile)
		End Function

		Public Function GetByString(ByVal yourString As String) As IMProfile
			Return CType(m_ByName(yourString.ToLower), IMProfile)
		End Function

		Protected Sub New()
			MyBase.New(New StringComparison())
		End Sub

		Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.New(info, context)
		End Sub

		Public Overrides Sub GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext)
			MyBase.GetObjectData(info, context)
		End Sub
	End Class
End Namespace