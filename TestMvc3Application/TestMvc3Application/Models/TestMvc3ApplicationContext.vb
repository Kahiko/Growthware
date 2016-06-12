Imports System.Data.Entity
Imports GrowthWare.Framework.Model.Profiles

Namespace Models
	Public Class TestMvc3ApplicationContext
		Inherits DbContext
		' You can add custom code to this file. Changes will not be overwritten.
		' 
		' If you want Entity Framework to drop and regenerate your database
		' automatically whenever you change your model schema, add the following
		' code to the Application_Start method in your Global.asax file.
		' Note: this will destroy and re-create your database with every model change.
		' 
		' System.Data.Entity.Database.SetInitializer(New System.Data.Entity.DropCreateDatabaseIfModelChanges(Of Models.TestMvc3ApplicationContext)())

		Public Property MFunctionProfiles As DbSet(Of MFunctionProfile)
	End Class


End Namespace


