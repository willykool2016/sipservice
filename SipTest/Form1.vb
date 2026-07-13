Imports Net
Imports System.Net.Http
Imports System.Net.Security.Cryptography
Imports System.Text


Imports System.Net.WebSockets
Imports System.Threading
Imports System.Net
Imports System.IO
Imports System.Globalization
'Imports System.Text
'Imports System.Net.Http



Public Class Form1
    Dim client As New HttpClient()



    Private webSocket As ClientWebSocket
    Private cts As CancellationTokenSource





    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim username As String = "willTestCam"
        'Dim password As String = "root"
        'Dim credentials As String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"))
        'MessageBox.Show(credentials)

        'client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Basic", credentials)
        'MessageBox.Show(client.ToString)

        Dim cameraIp As String = "192.168.0.208"
        Dim handlerUrl As String = $"/axis-cgi/com/ptz.cgi?camera=1&continuouspantiltmove=50,-50"
        Dim fullUri As String = $"http://{cameraIp}{handlerUrl}"



        Dim handler As New HttpClientHandler()
        handler.Credentials = New NetworkCredential("willTestCam", "root")

        Dim client As New HttpClient(handler)



        'MessageBox.Show(client.ToString)

        'Try
        'Dim response As HttpResponseMessage = Await client.GetAsync(fullUri)
        'If response.IsSuccessStatusCode Then
        'MessageBox.Show("yippee")
        'Else
        'MessageBox.Show("ruh roh")
        'End If
        'Catch ex As Exception
        'MessageBox.Show("ruh roh raggy")
        'End Try

    End Sub

    Private Async Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click

        cts = New CancellationTokenSource()
        webSocket = New ClientWebSocket()
        MessageBox.Show(client.ToString)
        Dim uriString As String = "wss://192.168.0.208/vapix/" & client.ToString
        MessageBox.Show(uriString)
        Dim serverUri As New Uri(uriString)


        Try

            Await webSocket.ConnectAsync(serverUri, cts.Token)
            lblStatus.Text = "Connected"

            Await ListenForMessagesAsync(webSocket, cts.Token)



        Catch webEx As System.Net.WebException
            'Console.WriteLine("Status: " & webEx.Status.ToString())

            MessageBox.Show("Status: " & webEx.Status.ToString())

            If webEx.InnerException IsNot Nothing Then
                '   Console.WriteLine("Root Cause: " & webEx.InnerException.Message)

                MessageBox.Show("Root Cause: " & webEx.InnerException.Message)

                If TypeOf webEx.InnerException Is System.Net.Sockets.SocketException Then
                    Dim socketEx As System.Net.Sockets.SocketException = CType(webEx.InnerException, System.Net.Sockets.SocketException)

                    '       Console.WriteLine("Socket Error Code: " & socketEx.SocketErrorCode.ToString())
                    '      Console.WriteLine("Native Error Code: " & socketEx.NativeErrorCode)

                    MessageBox.Show("Socket Error Code: " & socketEx.SocketErrorCode.ToString())
                    MessageBox.Show("Native Error Code: " & socketEx.NativeErrorCode)

                End If

            End If
        Catch ex As Exception

            ' Console.WriteLine("General Error: " & ex.Message)

            MessageBox.Show("General Error: " & ex.Message)
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
