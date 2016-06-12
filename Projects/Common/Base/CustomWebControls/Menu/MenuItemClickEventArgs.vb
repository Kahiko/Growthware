'/ <summary>
   '/ Provides the EventArgs class for the MenuItemClick event.
   '/ This EventArgs provides a single string parameter: CommandName
   '/ </summary>
   
   Public Class MenuItemClickEventArgs
      Inherits EventArgs
		Private _commandName As String
      
      
      '/ <summary>
      '/ Describes which menuitem was clicked by returning the command name property.
      '/ </summary>
      Public Sub New(name As String)
			_commandName = name
      End Sub 'New
      
      '/ <summary>
      '/ Readonly access to commandName parameter of EventArgs class
      '/ </summary>
      
      Public ReadOnly Property CommandName() As String
         Get
				Return _commandName
         End Get
      End Property
   End Class 'MenuItemClickEventArgs