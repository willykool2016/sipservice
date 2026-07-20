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

    'Testing something
    'Private img As Image = Image.FromFile("C:\Users\Glade Robertson\Downloads\New folder\mic.png")


    '


    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        'MAKE SURE TO GET AN ACTUAL IMAGE PNG
        'Using img As Image = Image.FromFile("C:\Users\Glade Robertson\Downloads\New folder\mic2.png")
        'Dim img As Image = System.Drawing.Bitm.Resources.mic2



        Using img As Image = My.Resources.microphone
            e.Graphics.DrawImage(img, 0, 0, CSng(Me.ClientSize.Width / 2), CSng(Me.ClientSize.Height / 2))

        End Using
        '-------------------------------
        'Testing something below

        'e.Graphics.CompositingQuality = CompositingQuality.HighQuality

        'Dim opacity As Single = 0.5F

        'Dim matrixItems As Single()() = {
        '   New Single() {1, 0, 0, 0, 0},
        '  New Single() {0, 1, 0, 0, 0},
        ' New Single() {0, 0, 1, 0, 0},
        'New Single() {0, 0, 0, opacity, 0},
        ' New Single() {0, 0, 0, 0, 1}
        '}

        '  Dim colorMatrix As New ColorMatrix(matrixItems)

        '  Using attributes As New ImageAttributes()

        '  attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

        '  Dim destRect As New Rectangle(50, 50, img.Width, img.Height)


        'e.Graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes)
        ' e.Graphics.DrawImage(img, 0, 0, CSng(Me.ClientSize.Width / 2), CSng(Me.ClientSize.Height / 2))
        'End Using

    End Sub




    'All stuff for the image #manifesting good things


    'Private formImage As Image

    'Public Sub New()

    '    ' This call is required by the designer.
    '    InitializeComponent()

    '    ' Add any initialization after the InitializeComponent() call.


    '    Me.DoubleBuffered = True

    '    Me.ResizeRedraw = True

    'End Sub





    Private Sub CallNotify_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        '    formImage = Image.FromFile("C:\Users\Glade Robertson\Downloads\New folder\mic.png")


    End Sub

    'Private Sub CallNotify_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Load

    '    If formImage IsNot Nothing Then

    '        Dim opacity As Single = 0.5F

    '        Dim matrix As New ColorMatrix()
    '        matrix.Matrix33 = opacity

    '        Dim attributes As New ImageAttributes()
    '        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)

    '        Dim destRect As New Rectangle(0, 0, CSng(Me.ClientSize.Width / 2), CSng(Me.ClientSize.Height / 2))

    '        e.Graphics.DrawImage(formImage, destRect, 0, 0, formImage.Width, formImage.Height, GraphicsUnit.Pixel, attributes)


    '    End If

    'End Sub








End Class