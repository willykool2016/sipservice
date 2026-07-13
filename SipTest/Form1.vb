Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text


Imports System.Net.WebSockets
Imports System.Threading
'Imports System.Text
'Imports System.Net.Http



Public Class Form1
    Dim client As New HttpClient()



    Private webSocket As ClientWebSocket
    Private cts As CancellationTokenSource





    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim username As String = "willTestCam"
        Dim password As String = "root"
        Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"))
        MessageBox.Show(credentials)

        client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Basic", credentials)
        MessageBox.Show(client.ToString)
    End Sub

    Private Async Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click

        cts = New CancellationTokenSource()
        webSocket = New ClientWebSocket()
        MessageBox.Show(client.ToString)
        Dim uriString As String = "wss://192.168.0.195/vapix/" & client.ToString
        Dim serverUri As New Uri(uriString)


        Try

            Await webSocket.ConnectAsync(serverUri, cts.Token)
            lblStatus.Text = "Connected"

            Await ListenForMessagesAsync(webSocket, cts.Token)



        Catch ex As Exception

            MessageBox.Show($"Connection Error: {ex.Message}")

        End Try

    End Sub


    Private Async Function ListenForMessagesAsync(socket As ClientWebSocket, ct As CancellationToken) As Task

        Dim buffer As Byte() = New Byte(1023) {}

        While socket.State = WebSocketState.Open

            Dim result = Await socket.ReceiveAsync(New ArraySegment(Of Byte)(buffer), ct)

            If result.MessageType = WebSocketMessageType.Close Then

                Await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None)
                Exit While

            End If

            Dim messageString As String = Encoding.UTF8.GetString(buffer, 0, result.Count)

            Me.Invoke(Sub()
                          txtLog.AppendText(messageString & vbCrLf)
                      End Sub)

        End While

    End Function

    Private Async Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click

        If webSocket IsNot Nothing AndAlso webSocket.State = WebSocketState.Open Then

            cts.Cancel()
            Await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "User disconnected", CancellationToken.None)
            lblStatus.Text = "Disconnected"

        End If

    End Sub


End Class
