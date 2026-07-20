Imports System.Drawing.Text
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
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

Imports System.Drawing
Imports System.Windows.Forms
Imports Windows.Globalization.NumberFormatting



Public Class Form1
    Dim client As New HttpClient()
    Private webSocket As ClientWebSocket
    Private cts As CancellationTokenSource

    Private sipTransport As SIPTransport
    Private userAgent As SIPUserAgent
    Private windowsAudio As WindowsAudioEndPoint

    Private activeCallAgent As SIPUserAgent
    Private activeServerAgent As SIPServerUserAgent

    Private isAppInitiatingCall As Boolean = False

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 1. Start the SIP Server to listen for incoming calls right away
        StartListening()
        ' 2. Connect the WebSocket to send API commands
        Try
            lblStatus.Text = "Status: Connecting to API..."
            webSocket = New ClientWebSocket()
            cts = New CancellationTokenSource()
            ' Apply certificate bypass and credentials
            webSocket.Options.RemoteCertificateValidationCallback = AddressOf AcceptAllCertificates
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

        If Application.OpenForms.OfType(Of CallNotify)().Any() Then
            CallNotify.Close()
        End If


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

    'Where the code to make the hang up button and form closing hangup occur
    Public Sub HangUp()
        '-------------------------------
        If Application.OpenForms.OfType(Of CallNotify)().Any() Then
            Dim openForm As CallNotify = Application.OpenForms.OfType(Of CallNotify)().First()
            openForm.Close()
        End If
        '---------------------------------
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
            btnCallIntercom.Enabled = True

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

            ' --- NEW: Catch outbound call rejections here! ---
            AddHandler userAgent.ClientCallFailed, Sub(ua, err, res)
                                                       MessageBox.Show($"Call Failed! Reason: {err}")
                                                   End Sub

            ' 3. Tell the agent what to do when an incoming call arrives
            AddHandler userAgent.OnIncomingCall, AddressOf Intercom_Ringing
        Catch ex As Exception
            MessageBox.Show($"Failed to start listening: {ex.Message}")
        End Try
    End Sub

    ' Update the Ringing event to HOLD the call instead of answering it
    Private Sub Intercom_Ringing(ua As SIPUserAgent, req As SIPRequest)
        Try
            activeCallAgent = ua
            activeServerAgent = ua.AcceptCall(req)

            If isAppInitiatingCall Then
                ' We triggered this via HTTP, so auto-answer the audio immediately!
                isAppInitiatingCall = False

                Me.Invoke(Async Sub()
                              lblStatus.Text = "Status: Connecting Audio..."
                              windowsAudio = New WindowsAudioEndPoint(New AudioEncoder)
                              Dim voipMediaSession As New VoIPMediaSession(windowsAudio.ToMediaEndPoints)

                              Dim answered = Await activeCallAgent.Answer(activeServerAgent, voipMediaSession)
                              If answered Then
                                  lblStatus.Text = "Status: Call Active! (Audio Live)"
                                  lblStatus.ForeColor = Color.Green
                              Else
                                  lblStatus.Text = "Status: Failed to auto-answer."
                                  lblStatus.ForeColor = Color.Red
                                  btnCallIntercom.Enabled = True
                              End If
                          End Sub)
            Else
                ' Normal incoming call (someone physically pushed the button outside)
                Me.Invoke(Sub()
                              lblStatus.Text = "Status: INCOMING CALL! (Ringing...)"
                              lblStatus.ForeColor = Color.Orange
                              btnAnswer.Enabled = True
                          End Sub)
            End If
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
    Private Async Sub btnCallIntercom_Click(sender As Object, e As EventArgs) Handles btnCallIntercom.Click
        Try
            btnCallIntercom.Enabled = False
            lblStatus.Text = "Status: Waking up Intercom..."
            lblStatus.ForeColor = Color.Orange

            ' Set the flag so the incoming call event knows to auto-answer
            isAppInitiatingCall = True

            ' Fire the HTTP pulse to trigger the intercom's action rule
            Await ActivateVirtualInput()
        Catch ex As Exception
            MessageBox.Show($"Error triggering call: {ex.Message}")
            btnCallIntercom.Enabled = True
            isAppInitiatingCall = False
        End Try
    End Sub

    'Activate Led when call made by computer
    Private Async Function ActivateVirtualInput() As Task
        Dim deactivateUrl As String = "http://192.168.0.208/axis-cgi/virtualinput/deactivate.cgi?schemaversion=1&port=1"
        Dim activateUrl As String = "http://192.168.0.208/axis-cgi/virtualinput/activate.cgi?schemaversion=1&port=1"

        Dim handler As New HttpClientHandler()
        handler.Credentials = New NetworkCredential("willTestCam", "root")

        Using client As New HttpClient(handler)
            Try
                ' 1. Force the switch OFF just in case it got stuck ON previously
                Await client.GetAsync(deactivateUrl)

                ' Slight delay to let the Axis state machine process the transition
                Await Task.Delay(200)

                ' 2. Turn the switch ON (This triggers the Axis Action Rule to call us)
                Dim response = Await client.GetAsync(activateUrl)
                Dim body = Await response.Content.ReadAsStringAsync()

                If Not response.IsSuccessStatusCode Then
                    MessageBox.Show($"Failed to trigger intercom: {response.StatusCode} - {body}")
                End If

            Catch ex As Exception
                MessageBox.Show($"HTTP Request Error: {ex.Message}")
                ' Reset flag if the HTTP request failed so we don't accidentally auto-answer a real visitor
                isAppInitiatingCall = False
            End Try
        End Using
    End Function
#End Region

    'Informs the person if they are not muted/still on call when window resized




    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        'To let person know they are still on call

        If Me.WindowState = FormWindowState.Minimized Then
            'MessageBox.Show("The form has been minimized!")

            If lblStatus.Text.Contains("Status: Call Active! (Outbound)") Or lblStatus.Text.Contains("Status: Call Active! (Audio Live)") Then
                Dim userChoice As DialogResult = MessageBox.Show("You are still on call." & Environment.NewLine & "Would you like to hang up?", "Call Notification", MessageBoxButtons.YesNo)

                If userChoice = DialogResult.Yes Then
                    HangUp()
                    MessageBox.Show("No longer on call.", "Call Ended", MessageBoxButtons.OK)
                Else
                    Dim overlay As New CallNotify()

                    overlay.StartPosition = FormStartPosition.Manual
                    overlay.Location = New Point(25, 25)
                    overlay.Size = New Size(50, 150)

                    overlay.Show()
                End If

            End If

        End If

    End Sub

End Class
