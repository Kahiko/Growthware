Imports System.Data.Entity
Imports GrowthWare.Framework.Model.Profiles
Imports GrowthWare.Framework.Web.Utilities
Imports GrowthWare.MvcCoreWeb.Models.GrowthWare

Namespace GrowthWare.MvcCoreWeb
	Public Class FunctionsController
		Inherits System.Web.Mvc.Controller

		Private db As MvcCoreWebContext = New MvcCoreWebContext

		'
		' GET: /Functions/

		Function Index() As ViewResult
			Dim mUtility As New FunctionUtility()
			Return View(mUtility.GetFunctions())
		End Function

		'
		' GET: /Functions/Details/5

		Function Details(id As Integer) As ViewResult
			Dim mUtility As New FunctionUtility()
			Return View(mUtility.GetFunction(id))
		End Function

		'
		' GET: /Functions/Create

		Function Create() As ViewResult
			Return View()
		End Function

		'
		' POST: /Functions/Create

		<HttpPost()>
		Function Create(mfunctionprofile As MFunctionProfile) As ActionResult
			If ModelState.IsValid Then
				db.MFunctionProfiles.Add(mfunctionprofile)
				db.SaveChanges()
				Return RedirectToAction("Index")
			End If

			Return View(mfunctionprofile)
		End Function

		'
		' GET: /Functions/Edit/5

		Function Edit(id As Integer) As ViewResult
			Dim mUtility As New FunctionUtility()
			Return View(mUtility.GetFunction(id))
		End Function

		'
		' POST: /Functions/Edit/5

		<HttpPost()>
		Function Edit(mfunctionprofile As MFunctionProfile) As ActionResult
			If ModelState.IsValid Then
				db.Entry(mfunctionprofile).State = EntityState.Modified
				db.SaveChanges()
				Return RedirectToAction("Index")
			End If

			Return View(mfunctionprofile)
		End Function

		'
		' GET: /Functions/Delete/5

		Function Delete(id As Integer) As ViewResult
			Dim mUtility As New FunctionUtility()
			Return View(mUtility.GetFunction(id))
		End Function

		'
		' POST: /Functions/Delete/5

		<HttpPost()>
		<ActionName("Delete")>
		Function DeleteConfirmed(id As Integer) As RedirectToRouteResult
			Dim mfunctionprofile As MFunctionProfile = db.MFunctionProfiles.Find(id)
			db.MFunctionProfiles.Remove(mfunctionprofile)
			db.SaveChanges()
			Return RedirectToAction("Index")
		End Function

		Protected Overrides Sub Dispose(disposing As Boolean)
			db.Dispose()
			MyBase.Dispose(disposing)
		End Sub

	End Class
End Namespace