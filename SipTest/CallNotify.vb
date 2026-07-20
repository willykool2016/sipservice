Imports System.Drawing
Imports System.Windows.Forms

'-------------------------------
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports FFmpeg.AutoGen
'-------------------------------
Public Class CallNotify
    Private Const WS_EX_TRANSPARENT As Integer = &H20
    Private Const WS_EX_NOACTIVATE As Integer = &H8000000

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get

            Dim cp As CreateParams = MyBase.CreateParams

            cp.ExStyle = cp.ExStyle Or WS_EX_TRANSPARENT Or WS_EX_NOACTIVATE

            Return cp

        End Get
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
    End Sub

    Private Sub CallNotify_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

End Class