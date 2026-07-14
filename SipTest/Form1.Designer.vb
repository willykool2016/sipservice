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
        btnHangUp = New Button()
        lblStatus = New Label()
        txtLog = New TextBox()
        btnAnswer = New Button()
        btnMakeCall = New Button()
        SuspendLayout()
        ' 
        ' btnHangUp
        ' 
        btnHangUp.Location = New Point(173, 154)
        btnHangUp.Name = "btnHangUp"
        btnHangUp.Size = New Size(107, 183)
        btnHangUp.TabIndex = 1
        btnHangUp.Text = "HangUp"
        btnHangUp.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(319, 67)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(81, 20)
        lblStatus.TabIndex = 2
        lblStatus.Text = "Status: idle"
        ' 
        ' txtLog
        ' 
        txtLog.Location = New Point(240, 101)
        txtLog.Name = "txtLog"
        txtLog.PlaceholderText = "Console Status"
        txtLog.Size = New Size(239, 27)
        txtLog.TabIndex = 3
        ' 
        ' btnAnswer
        ' 
        btnAnswer.Enabled = False
        btnAnswer.Location = New Point(306, 154)
        btnAnswer.Margin = New Padding(3, 4, 3, 4)
        btnAnswer.Name = "btnAnswer"
        btnAnswer.Size = New Size(107, 183)
        btnAnswer.TabIndex = 4
        btnAnswer.Text = "Answer Call"
        btnAnswer.UseVisualStyleBackColor = True
        ' 
        ' btnMakeCall
        ' 
        btnMakeCall.Location = New Point(438, 154)
        btnMakeCall.Name = "btnMakeCall"
        btnMakeCall.Size = New Size(107, 183)
        btnMakeCall.TabIndex = 5
        btnMakeCall.Text = "Make Call"
        btnMakeCall.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 451)
        Controls.Add(btnMakeCall)
        Controls.Add(btnAnswer)
        Controls.Add(txtLog)
        Controls.Add(lblStatus)
        Controls.Add(btnHangUp)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents btnHangUp As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtLog As TextBox
    Friend WithEvents btnAnswer As Button
    Friend WithEvents btnMakeCall As Button

End Class
