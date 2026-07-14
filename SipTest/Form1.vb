Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Security
Imports System.Net.WebSockets
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports SIPSorcery.Media
Imports SIPSorcery.SIP
Imports SIPSorcery.SIP.App
Imports SIPSorceryMedia.Windows
'Imports System.Text
'Imports System.Net.Http
'Imports Net
'Imports System.Net.Security.Cryptography

Public Class Form1
    Dim client As New HttpClient()
    Private webSocket As ClientWebSocket
    Private cts As CancellationTokenSource

    Private sipTransport As SIPTransport
    Private userAgent As SIPUserAgent
    Private windowsAudio As WindowsAudioEndPoint

    Private activeCallAgent As SIPUserAgent
    Private activeServerAgent As SIPServerUserAgent

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Start the SIP Server to listen for incoming calls right away
        StartListening()
        ' 2. Connect the WebSocket to send API commands
        Try
            lblStatus.Text = "Status: Connecting..."
            webSocket = New ClientWebSocket()
            cts = New CancellationTokenSource()
            ' Apply certificate bypass and credentials
            webSocket.Options.RemoteCertificateValidationCallback = AddressOf AcceptAllCertificates
            ' Make sure to put your actual device password here!
            webSocket.Options.Credentials = New System.Net.NetworkCredential("willTestCam", "root")
            Dim deviceIp As String = "192.168.0.208"
            Dim serverUri As New Uri($"wss://{deviceIp}/vapix/intercomws")
            ' Connect!
            Await webSocket.ConnectAsync(serverUri, cts.Token)
            lblStatus.Text = "Status: Connected & Listening"
            lblStatus.ForeColor = Color.Green
        Catch ex As Exception
            lblStatus.Text = "Status: Connection Failed"
            lblStatus.ForeColor = Color.Red
            MessageBox.Show($"Failed to connect to the intercom: {ex.Message}")
        End Try
    End Sub

    'Makes sure that if the user closes the app, the ports are freed
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' First hang up any active calls
        HangUp()

        ' NOW it is safe to completely shut down the network listener
        If sipTransport IsNot Nothing Then
            sipTransport.Shutdown()
        End If

        ' Force all background threads to die immediately so Port 5060 is released!
        Environment.Exit(0)
    End Sub

#Region "Helper Methods"
    'Helper Method to accept certificates before connecting
    Private Function AcceptAllCertificates(sender As Object,
                                       certificate As System.Security.Cryptography.X509Certificates.X509Certificate,
                                       chain As System.Security.Cryptography.X509Certificates.X509Chain,
                                       sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        ' Always return True to ignore certificate errors (for testing only)
        Return True
    End Function

    'Creates Two-way Connection between Intercom and PC
    Public Async Function StartCallAsync() As Task
        Try
            ' 1. Set up the SIP Transport (listens for SIP traffic)
            sipTransport = New SIPTransport()
            ' 2. Set up the Windows Audio Endpoint (Mic and Speakers)
            windowsAudio = New WindowsAudioEndPoint(New SIPSorcery.Media.AudioEncoder())
            ' 3. Create the VoIP Media Session
            Dim voipMediaSession As New VoIPMediaSession(windowsAudio.ToMediaEndPoints())
            ' 4. Create the SIP User Agent (Your Softphone)
            userAgent = New SIPUserAgent(sipTransport, Nothing)
            ' --- Event Handlers to track the call state ---
            AddHandler userAgent.ClientCallRinging, Sub(ua, res)
                                                        Console.WriteLine("Intercom is ringing...")
                                                    End Sub
            AddHandler userAgent.ClientCallAnswered, Sub(ua, res)
                                                         Console.WriteLine("Call answered! Audio streaming started.")
                                                     End Sub
            AddHandler userAgent.ClientCallFailed, Sub(ua, err, res)
                                                       Console.WriteLine($"Call failed: {err}")
                                                   End Sub
            ' ----------------------------------------------
            ' 5. The Intercom's SIP Address
            ' Format: sip:username@ip_address
            Dim intercomSipUri As String = "sip:192.168.0.208:5060"
            ' 6. Start the Call!
            ' We pass the SIP URI and the Media Session to handle the audio
            Dim callTask = userAgent.Call(intercomSipUri, Nothing, Nothing, voipMediaSession)
            Await callTask
        Catch ex As Exception
            MessageBox.Show($"SIP Error: {ex.Message}")
        End Try
    End Function

    'Where the code to make the hang up button and form closing hangup occur
    'Where the code to make the hang up button occur
    Public Sub HangUp()
        Try
            ' 1. Hang up only the active call, do NOT shut down the sipTransport!
            If activeCallAgent IsNot Nothing Then
                activeCallAgent.Hangup()
            End If

            ' 2. Clean up the audio devices
            If windowsAudio IsNot Nothing Then
                windowsAudio.CloseAudio()
            End If

            ' 3. Reset our variables for the next call
            activeCallAgent = Nothing
            activeServerAgent = Nothing

            ' 4. Reset the UI
            lblStatus.Text = "Status: Connected & Listening"
            lblStatus.ForeColor = Color.Green
            btnAnswer.Enabled = False

        Catch ex As Exception
            MessageBox.Show($"Error hanging up: {ex.Message}")
        End Try
    End Sub

    'When pressed, should hang up the call with the intercom
    Private Sub btnHangUp_Click(sender As Object, e As EventArgs) Handles btnHangUp.Click
        HangUp()
    End Sub
#End Region

#Region "Prepare for Incoming Calls"
    'Automatically listens for incoming traffic
    Public Sub StartListening()
        Try
            ' 1. Set up the SIP Transport
            sipTransport = New SIPTransport()
            ' --- THE MISSING PIECE ---
            ' Explicitly tell SIPSorcery to open and listen on UDP Port 5060
            Dim sipChannel As New SIPUDPChannel(New Net.IPEndPoint(Net.IPAddress.Any, 5060))
            sipTransport.AddSIPChannel(sipChannel)
            ' -------------------------
            ' 2. Create the SIP User Agent (Your Softphone)
            userAgent = New SIPUserAgent(sipTransport, Nothing)
            ' 3. Tell the agent what to do when an incoming call arrives
            AddHandler userAgent.OnIncomingCall, AddressOf Intercom_Ringing
        Catch ex As Exception
            MessageBox.Show($"Failed to start listening: {ex.Message}")
        End Try
    End Sub

    ' Update the Ringing event to HOLD the call instead of answering it
    Private Sub Intercom_Ringing(ua As SIPUserAgent, req As SIPRequest)
        Try
            ' 1. Accept the incoming call (This tells the intercom to play the ringing sound outside)
            activeCallAgent = ua
            activeServerAgent = ua.AcceptCall(req)

            ' 2. Visually show that the call is incoming and unlock the Answer button
            Me.Invoke(Sub()
                          lblStatus.Text = "Status: INCOMING CALL! (Ringing...)"
                          lblStatus.ForeColor = Color.Orange
                          btnAnswer.Enabled = True
                      End Sub)
        Catch ex As Exception
            Me.Invoke(Sub()
                          lblStatus.Text = $"Status: Ring Error - {ex.Message}"
                      End Sub)
        End Try
    End Sub

    ' Create the click event for your new Answer button
    Private Async Sub btnAnswer_Click(sender As Object, e As EventArgs) Handles btnAnswer.Click
        ' Safety check
        If activeCallAgent Is Nothing OrElse activeServerAgent Is Nothing Then Return
        Try
            ' Lock the button so it can't be double-clicked
            btnAnswer.Enabled = False
            lblStatus.Text = "Status: Connecting Audio..."
            ' 1. Set up the Audio endpoints (Mic and Speakers)
            windowsAudio = New WindowsAudioEndPoint(New AudioEncoder)
            Dim voipMediaSession As New VoIPMediaSession(windowsAudio.ToMediaEndPoints)
            ' 2. Formally Answer the call and open the 2-way audio!
            Dim answered = Await activeCallAgent.Answer(activeServerAgent, voipMediaSession)
            If answered Then
                lblStatus.Text = "Status: Call Active! (Audio Live)"
                lblStatus.ForeColor = Color.Green
            Else
                lblStatus.Text = "Status: Failed to answer call."
                lblStatus.ForeColor = Color.Red
            End If
        Catch ex As Exception
            MessageBox.Show($"Error answering call: {ex.Message}")
            lblStatus.Text = "Status: Error Answering"
            lblStatus.ForeColor = Color.Red
        End Try
    End Sub
#End Region

#Region "Send Call to Intercom"
    'When pressed, should send a call out to the intercom
    Private Sub btnMakeCall_Click_1(sender As Object, e As EventArgs) Handles btnMakeCall.Click
        MessageBox.Show("Hello? Hello hello!")
    End Sub
#End Region
End Class
