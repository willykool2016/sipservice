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
        btnAnswer = New Button()
        btnCallIntercom = New Button()
        TableLayoutPanel1 = New TableLayoutPanel()
        TableLayoutPanel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' btnHangUp
        ' 
        btnHangUp.Dock = DockStyle.Fill
        btnHangUp.Location = New Point(13, 171)
        btnHangUp.Margin = New Padding(3, 2, 3, 2)
        btnHangUp.Name = "btnHangUp"
        btnHangUp.Size = New Size(220, 155)
        btnHangUp.TabIndex = 1
        btnHangUp.Text = "Hang Up"
        btnHangUp.UseVisualStyleBackColor = True
        ' 
        ' lblStatus
        ' 
        lblStatus.Anchor = AnchorStyles.None
        lblStatus.AutoSize = True
        lblStatus.Location = New Point(317, 82)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(64, 15)
        lblStatus.TabIndex = 2
        lblStatus.Text = "Status: idle"
        lblStatus.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' btnAnswer
        ' 
        btnAnswer.Dock = DockStyle.Fill
        btnAnswer.Enabled = False
        btnAnswer.Location = New Point(239, 172)
        btnAnswer.Name = "btnAnswer"
        btnAnswer.Size = New Size(220, 153)
        btnAnswer.TabIndex = 4
        btnAnswer.Text = "Answer Call"
        btnAnswer.UseVisualStyleBackColor = True
        ' 
        ' btnCallIntercom
        ' 
        btnCallIntercom.Dock = DockStyle.Fill
        btnCallIntercom.Location = New Point(465, 171)
        btnCallIntercom.Margin = New Padding(3, 2, 3, 2)
        btnCallIntercom.Name = "btnCallIntercom"
        btnCallIntercom.Size = New Size(222, 155)
        btnCallIntercom.TabIndex = 5
        btnCallIntercom.Text = "Make Call"
        btnCallIntercom.UseVisualStyleBackColor = True
        ' 
        ' TableLayoutPanel1
        ' 
        TableLayoutPanel1.ColumnCount = 3
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.3333321F))
        TableLayoutPanel1.Controls.Add(btnCallIntercom, 2, 1)
        TableLayoutPanel1.Controls.Add(btnAnswer, 1, 1)
        TableLayoutPanel1.Controls.Add(btnHangUp, 0, 1)
        TableLayoutPanel1.Controls.Add(lblStatus, 1, 0)
        TableLayoutPanel1.Dock = DockStyle.Fill
        TableLayoutPanel1.Location = New Point(0, 0)
        TableLayoutPanel1.Name = "TableLayoutPanel1"
        TableLayoutPanel1.Padding = New Padding(10)
        TableLayoutPanel1.RowCount = 2
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.RowStyles.Add(New RowStyle(SizeType.Percent, 50F))
        TableLayoutPanel1.Size = New Size(700, 338)
        TableLayoutPanel1.TabIndex = 6
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(700, 338)
        Controls.Add(TableLayoutPanel1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Form1"
        Text = "Form1"
        TableLayoutPanel1.ResumeLayout(False)
        TableLayoutPanel1.PerformLayout()
        ResumeLayout(False)
    End Sub
    Friend WithEvents btnHangUp As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents btnAnswer As Button
    Friend WithEvents btnCallIntercom As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel

End Class
