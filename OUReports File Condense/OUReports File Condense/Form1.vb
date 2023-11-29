Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.VisualBasic.FileIO
Imports System.Xml
Imports Newtonsoft.Json.Linq

Public Class Form1
    Dim outputFolder As String = ""
    Private Sub ChoosesFolderButton_Click(sender As Object, e As EventArgs) Handles ChooseFolder.Click
        ' Create a new instance of FolderBrowserDialog
        Dim folderBrowserDialog As New FolderBrowserDialog()

        ' Show the FolderBrowserDialog and check if the user clicked OK
        If folderBrowserDialog.ShowDialog() = DialogResult.OK Then
            ' Get the selected folder path
            Dim selectedFolderPath As String = folderBrowserDialog.SelectedPath

            ' Ask the user for a new folder name
            Dim newFolderName As String = InputBox("Enter a new folder name:" & vbCrLf & "(Default to Uploaded File Name if left blank.)", "New Folder")

            ' Check if the user entered a folder name
            If Not String.IsNullOrEmpty(newFolderName) Then
                ' Combine the selected folder path with the new folder name
                outputFolder = Path.Combine(selectedFolderPath, newFolderName)

                ' Create the new folder
                Directory.CreateDirectory(outputFolder)
            End If
        End If
    End Sub

    Private Sub FileUploadButton_Click(sender As Object, e As EventArgs) Handles FileUploadButton.Click
        ' Resets the progress bar
        UpdateProgressBar(0)
        ' Create an instance of the OpenFileDialog
        Dim openFileDialog As New OpenFileDialog()

        ' Set the filter for the file types you want to allow
        Dim selectedValue As String = FileTypeDropdown.SelectedItem
        ' Check if an item is selected
        If selectedValue IsNot Nothing Then
            ' Convert the selected value to the appropriate data type
            Dim selectedText As String = selectedValue
            If selectedText = "CSV" Then
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            ElseIf selectedText = "XML" Then
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
            ElseIf selectedText = "JSON" Then
                openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            End If

            ' Show the dialog and get the result
            Dim result As DialogResult = openFileDialog.ShowDialog()

            ' Check if the user selected a file
            If result = DialogResult.OK Then
                ' Display the selected file in the Label
                SelectedFileLabel.Text = "File Uploaded: " & openFileDialog.FileName

                If selectedText = "CSV" Then
                    SplitCSV(openFileDialog.FileName)
                ElseIf selectedText = "XML" Then
                    SplitXML(openFileDialog.FileName)
                ElseIf selectedText = "JSON" Then
                    SplitJSON(openFileDialog.FileName)
                End If

            End If
        Else
            MessageBox.Show("File Type Was Not Selected.")
        End If

    End Sub

    Private Sub SplitCSV(ByVal inputFileName As String)
        Try
            ' Define constants and variables for splitting files
            Const MaxSize As Long = 4 * 1024 * 1024 ' 4 MB
            Dim FileNumber As Integer = 1
            Dim FileSize As Long = 0
            Dim folderBrowserDialog As New FolderBrowserDialog()
            Dim InputFileBaseName As String = Path.GetFileNameWithoutExtension(inputFileName)

            ' Creates a folder based on the input file name
            If outputFolder = "" Then
                outputFolder = Path.Combine(Path.GetDirectoryName(inputFileName), InputFileBaseName)
            End If
            Directory.CreateDirectory(outputFolder)

            Dim OutputName As String = Path.Combine(outputFolder, InputFileBaseName & FileNumber.ToString("D3") & ".csv")
            Dim OutputFile As New StreamWriter(OutputName)

            ' Initializing the progress bar
            ProgressBar1.Value = 0
            ProgressBar1.Maximum = 100

            Using parser As New TextFieldParser(inputFileName)
                parser.TextFieldType = FieldType.Delimited
                parser.SetDelimiters(",")

                ' Get the first line to get categories and write it to the first output file
                Dim categoryLine As String = parser.ReadLine()
                OutputFile.WriteLine(categoryLine)
                FileSize += categoryLine.Length + 2 ' Add 2 bytes for newline characters

                ' Calculating progress to update the progress bar
                Dim totalLines As Long = File.ReadAllLines(inputFileName).Length
                Dim linesProcessed As Long = 0

                ' Goes through the data and split it into smaller files
                While Not parser.EndOfData
                    Dim line As String = parser.ReadLine()

                    ' Check if adding this line would exceed the maximum size of the output file
                    If FileSize + line.Length + 2 > MaxSize Then
                        ' Close the current output file and increment the file number
                        OutputFile.Close()
                        FileNumber += 1

                        ' Create a new output file with the updated name and write the header line to it
                        OutputName = Path.Combine(outputFolder, InputFileBaseName & FileNumber.ToString("D3") & ".csv")
                        OutputFile = New StreamWriter(OutputName)
                        OutputFile.WriteLine(categoryLine)
                        FileSize = categoryLine.Length + 2 ' Reset the file size to include only the header line
                    End If

                    ' Write the current line to the output file and update the file size
                    OutputFile.WriteLine(line)
                    FileSize += line.Length + 2

                    linesProcessed += 1
                    Dim progress As Integer = CInt((linesProcessed / totalLines) * 100)
                    UpdateProgressBar(progress) ' Update the progress bar
                End While
            End Using

            ' Close the last output file
            OutputFile.Close()

            ' Set the progbar to 100% once the process is done
            UpdateProgressBar(100)

            MessageBox.Show("The CSV file was successfully split into " & FileNumber & " smaller files in folder " & outputFolder, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("An error occurred while splitting the CSV file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        outputFolder = ""
    End Sub

    Private Sub SplitXML(ByVal inputFileName As String)
        Try
            ' Define the maximum size of each chunk in bytes
            Const MaxSize As Long = 4 * 1024 * 1024 ' 4 MB
            Dim FileNumber As Integer = 1
            Dim FileSize As Long = 0
            Dim InputFileBaseName As String = Path.GetFileNameWithoutExtension(inputFileName)
            If outputFolder = "" Then
                outputFolder = Path.Combine(Path.GetDirectoryName(inputFileName), InputFileBaseName)
            End If
            Dim rootElementName As String = Nothing ' To store the root element name

            ' Create the output folder if it doesn't exist
            Directory.CreateDirectory(outputFolder)

            ' Create an XmlReader object to read the input file
            Using reader As XmlReader = XmlReader.Create(inputFileName)
                Dim writerSettings As New XmlWriterSettings()
                writerSettings.Indent = True
                writerSettings.Encoding = System.Text.Encoding.UTF8

                ' Create an XmlWriter object to write the output file
                Dim writer As XmlWriter = Nothing
                While reader.Read()
                    If reader.NodeType = XmlNodeType.Element And reader.Depth = 0 Then
                        ' Capture the root element name if it hasn't been captured already
                        If rootElementName Is Nothing Then
                            rootElementName = reader.Name
                        End If
                    End If


                    If reader.NodeType = XmlNodeType.Element And reader.Depth > 0 Then
                        If writer Is Nothing Then
                            ' Create a new writer for the output file
                            Dim outputFileName As String = Path.Combine(outputFolder, $"{InputFileBaseName}{FileNumber:D3}.xml")
                            writer = XmlWriter.Create(outputFileName, writerSettings)

                            ' Write the XML declaration for the output file
                            writer.WriteStartDocument()

                            ' Write the root element using the captured root element name
                            writer.WriteStartElement(rootElementName)
                        End If

                        ' Write the current element, including attributes and inner text
                        writer.WriteNode(reader, True)

                        ' Calculate the current size of the output file
                        Dim outputFileInfo As New FileInfo(Path.Combine(outputFolder, $"{InputFileBaseName}{FileNumber:D3}.xml"))
                        FileSize = outputFileInfo.Length

                        ' Check if the current size exceeds the maximum size
                        If FileSize >= MaxSize Then
                            ' Close and dispose the current writer
                            writer.WriteEndElement()
                            writer.WriteEndDocument()
                            writer.Close()
                            writer.Dispose()

                            ' Increment the file number
                            FileNumber += 1

                            ' Create a new writer for the next output file
                            Dim newOutputFileName As String = Path.Combine(outputFolder, $"{InputFileBaseName}{FileNumber:D3}.xml")
                            writer = XmlWriter.Create(newOutputFileName, writerSettings)

                            ' Write the XML declaration for the new output file
                            writer.WriteStartDocument()

                            ' Write the root element using the captured root element name
                            writer.WriteStartElement(rootElementName)
                        End If

                        ' Calculates the progress as a percentage
                        Dim totalChunks As Integer = FileNumber
                        Dim currentProgress As Integer = CInt((FileSize / MaxSize) * 100)
                        UpdateProgressBar(currentProgress) ' Calls to update the progbar
                    End If
                End While

                ' Close and dispose the last writer
                If writer IsNot Nothing Then
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                    writer.Close()
                    writer.Dispose()
                End If

                ' Close and dispose the last writer
                If writer IsNot Nothing Then
                    writer.WriteEndElement()
                    writer.WriteEndDocument()
                    writer.Close()
                    writer.Dispose()
                End If

                ' Set the progbar to 100% once the process is done
                UpdateProgressBar(100)
            End Using

            MessageBox.Show($"The XML file '{inputFileName}' was successfully split into {FileNumber} smaller files in folder '{outputFolder}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("An error occurred while splitting the XML file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        outputFolder = ""
    End Sub

    Private Sub SplitJSON(ByVal inputFileName As String)
        Try
            ' Define the maximum size of each chunk in bytes
            Dim MaxSize As Integer = 4 * 1024 * 1024 ' 4MB
            Dim FileNumber As Integer = 1
            Dim startIndex As Integer = 0
            Dim currentArray As JArray = New JArray()
            Dim InputFileBaseName As String = Path.GetFileNameWithoutExtension(inputFileName)
            If outputFolder = "" Then
                outputFolder = Path.Combine(Path.GetDirectoryName(inputFileName), InputFileBaseName)
            End If

            ' Create the output folder if it doesn't exist
            Directory.CreateDirectory(outputFolder)

            ' Read the entire input JSON file
            Dim jsonContent As String = File.ReadAllText(inputFileName)
            If Not jsonContent.StartsWith("[") Then
                jsonContent = "[" & jsonContent & "]"
            End If
            Dim TotalSize = jsonContent.Length
            ' Parse the input JSON
            Dim json As JArray = JArray.Parse(jsonContent)

            ' Calculate the total number of parts based on the chunk size
            Dim totalParts = CInt(Math.Ceiling(TotalSize / MaxSize))
            ' Calculate the progress step for each part
            Dim progressStep = 100 \ totalParts

            ' Split the JSON array into 4MB parts
            For Each item As JToken In json
                ' Add the current item to the current part
                currentArray.Add(item)

                ' Check if the part size exceeds the chunk size
                If currentArray.ToString().Length > MaxSize Then
                    ' Save the current part as a separate JSON file
                    Dim partContent = currentArray.ToString()
                    Dim outputFilePath = Path.Combine(outputFolder, $"{InputFileBaseName}{FileNumber:D3}.json")
                    File.WriteAllText(outputFilePath, partContent)

                    ' Update variables for the next part
                    currentArray = New JArray()
                    FileNumber += 1
                    startIndex = 0
                End If

                ' Calculates the progress as a percentage based on the current part
                Dim currentProgress As Integer = (FileNumber - 1) * progressStep + CInt((startIndex / TotalSize) * progressStep)
                UpdateProgressBar(currentProgress) ' Calls to update the progress bar

                ' Update the starting index for the next part
                startIndex += item.ToString().Length
            Next

            ' If there are remaining items in the current part, save it
            If currentArray.Count > 0 Then
                Dim partContent = currentArray.ToString()
                Dim outputFilePath = Path.Combine(outputFolder, $"{InputFileBaseName}{FileNumber:D3}.json")
                File.WriteAllText(outputFilePath, partContent)
            End If

            ' Set the progbar to 100% once the process is done
            UpdateProgressBar(100)

            MessageBox.Show($"The JSON file '{inputFileName}' was successfully split into {FileNumber} smaller files in folder '{outputFolder}'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("An error occurred while splitting the Json file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        outputFolder = ""
    End Sub


    Private Sub UpdateProgressBar(progress As Integer)
        ' Checks that the progress value is within the valid range (0-100)
        If progress < 0 Then
            progress = 0
        ElseIf progress > 100 Then
            progress = 100
        End If

        ' Update the progress bar
        ProgressBar1.Value = progress
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs) Handles ProgressBar1.Click

    End Sub

    Private Sub FileTypeLabel_Click(sender As Object, e As EventArgs) Handles FileTypeLabel.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class