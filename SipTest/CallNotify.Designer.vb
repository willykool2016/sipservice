<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CallNotify
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        SuspendLayout()
        ' 
        ' CallNotify
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Red
        ClientSize = New Size(56, 51)
        FormBorderStyle = FormBorderStyle.None
        Name = "CallNotify"
        Opacity = 0.6R
        ShowIcon = False
        ShowInTaskbar = False
        Text = "CallNotify"
        TopMost = True
        TransparencyKey = Color.Red
        ResumeLayout(False)
    End Sub
End Class
