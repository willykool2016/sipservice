<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        btnConnect = New Button()
        btnDisconnect = New Button()
        lblStatus = New Label()
        txtLog = New TextBox()
        SuspendLayout()
        ' 
        ' btnConnect
        ' 
        btnConnect.Location = New Point(137, 153)
        btnConnect.Name = "btnConnect"
        btnConnect.Size = New Size(100, 183)
        btnConnect.TabIndex = 0
        btnConnect.Text = "btnConnect"
        btnConnect.UseVisualStyleBackColor = True
        ' 
        ' btnDisconnect
        ' 
        btnDisconnect.Location = New Point(524, 152)
        btnDisconnect.Name = "btnDisconnect"
        btnDisconnect.Size = New Size(108, 193)
        btnDisconnect.TabIndex = 1
        btnDisconnect.Text = "btnDisconnect"
        btnDisconnect.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(298, 61)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(36, 20)
        lblStatus.TabIndex = 2
        lblStatus.Text = "N/A"
        ' 
        ' txtLog
        ' 
        txtLog.Location = New Point(240, 102)
        txtLog.Name = "txtLog"
        txtLog.Size = New Size(240, 27)
        txtLog.TabIndex = 3
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(txtLog)
        Controls.Add(lblStatus)
        Controls.Add(btnDisconnect)
        Controls.Add(btnConnect)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents btnConnect As Button
    Friend WithEvents btnDisconnect As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtLog As TextBox

End Class
