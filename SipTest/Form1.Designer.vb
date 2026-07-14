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
        btnDisconnect = New Button()
        lblStatus = New Label()
        txtLog = New TextBox()
        btnAnswer = New Button()
        SuspendLayout()
        ' 
        ' btnDisconnect
        ' 
        btnDisconnect.Location = New Point(210, 114)
        btnDisconnect.Margin = New Padding(3, 2, 3, 2)
        btnDisconnect.Name = "btnDisconnect"
        btnDisconnect.Size = New Size(94, 137)
        btnDisconnect.TabIndex = 1
        btnDisconnect.Text = "btnDisconnect"
        btnDisconnect.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(279, 50)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(64, 15)
        lblStatus.TabIndex = 2
        lblStatus.Text = "Status: idle"
        ' 
        ' txtLog
        ' 
        txtLog.Location = New Point(210, 76)
        txtLog.Margin = New Padding(3, 2, 3, 2)
        txtLog.Name = "txtLog"
        txtLog.PlaceholderText = "Console Status"
        txtLog.Size = New Size(210, 23)
        txtLog.TabIndex = 3
        ' 
        ' btnAnswer
        ' 
        btnAnswer.Enabled = False
        btnAnswer.Location = New Point(326, 114)
        btnAnswer.Name = "btnAnswer"
        btnAnswer.Size = New Size(94, 137)
        btnAnswer.TabIndex = 4
        btnAnswer.Text = "Answer Call"
        btnAnswer.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(700, 338)
        Controls.Add(btnAnswer)
        Controls.Add(txtLog)
        Controls.Add(lblStatus)
        Controls.Add(btnDisconnect)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents btnDisconnect As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtLog As TextBox
    Friend WithEvents btnAnswer As Button

End Class
