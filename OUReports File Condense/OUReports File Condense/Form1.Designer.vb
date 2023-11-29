<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        FileUploadButton = New Button()
        SelectedFileLabel = New Label()
        Title = New Label()
        FileTypeDropdown = New ComboBox()
        Description = New Label()
        FileTypeLabel = New Label()
        ProgressBar1 = New ProgressBar()
        ChooseFolder = New Button()
        Label1 = New Label()
        Label2 = New Label()
        SuspendLayout()
        ' 
        ' FileUploadButton
        ' 
        FileUploadButton.Location = New Point(103, 418)
        FileUploadButton.Name = "FileUploadButton"
        FileUploadButton.Size = New Size(298, 23)
        FileUploadButton.TabIndex = 0
        FileUploadButton.Text = "Upload File"
        FileUploadButton.UseVisualStyleBackColor = True
        ' 
        ' SelectedFileLabel
        ' 
        SelectedFileLabel.ForeColor = SystemColors.Highlight
        SelectedFileLabel.Location = New Point(103, 444)
        SelectedFileLabel.Name = "SelectedFileLabel"
        SelectedFileLabel.Size = New Size(298, 47)
        SelectedFileLabel.TabIndex = 2
        ' 
        ' Title
        ' 
        Title.Font = New Font("Segoe UI", 24.0F, FontStyle.Regular, GraphicsUnit.Point)
        Title.Location = New Point(69, 24)
        Title.Name = "Title"
        Title.Size = New Size(343, 148)
        Title.TabIndex = 3
        Title.Text = "OUReports Condense File Application"
        Title.TextAlign = ContentAlignment.TopCenter
        ' 
        ' FileTypeDropdown
        ' 
        FileTypeDropdown.DropDownStyle = ComboBoxStyle.DropDownList
        FileTypeDropdown.FormattingEnabled = True
        FileTypeDropdown.Items.AddRange(New Object() {"CSV", "XML", "JSON"})
        FileTypeDropdown.Location = New Point(103, 283)
        FileTypeDropdown.Name = "FileTypeDropdown"
        FileTypeDropdown.Size = New Size(134, 23)
        FileTypeDropdown.TabIndex = 4
        ' 
        ' Description
        ' 
        Description.Font = New Font("Segoe UI", 12.0F, FontStyle.Regular, GraphicsUnit.Point)
        Description.Location = New Point(69, 126)
        Description.Name = "Description"
        Description.Size = New Size(343, 128)
        Description.TabIndex = 5
        Description.Text = "This Application allows you to upload a either csv, xml, or json files and will condense the file into 4MB chunks and store them into a new folder on your computer."
        Description.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' FileTypeLabel
        ' 
        FileTypeLabel.AutoSize = True
        FileTypeLabel.Location = New Point(250, 286)
        FileTypeLabel.Name = "FileTypeLabel"
        FileTypeLabel.Size = New Size(55, 15)
        FileTypeLabel.TabIndex = 6
        FileTypeLabel.Text = "File Type:"
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Location = New Point(103, 494)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(298, 23)
        ProgressBar1.TabIndex = 7
        ' 
        ' ChooseFolder
        ' 
        ChooseFolder.Location = New Point(103, 344)
        ChooseFolder.Name = "ChooseFolder"
        ChooseFolder.Size = New Size(134, 23)
        ChooseFolder.TabIndex = 8
        ChooseFolder.Text = "Choose Folder"
        ChooseFolder.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.Location = New Point(250, 318)
        Label1.Name = "Label1"
        Label1.Size = New Size(204, 36)
        Label1.TabIndex = 9
        Label1.Text = "Choose a Folder where the reduced files will be saved"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 8.0F, FontStyle.Regular, GraphicsUnit.Point)
        Label2.Location = New Point(250, 354)
        Label2.Name = "Label2"
        Label2.Size = New Size(224, 13)
        Label2.TabIndex = 10
        Label2.Text = "(Default to where uploaded file is located)"
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        AutoValidate = AutoValidate.EnablePreventFocusChange
        ClientSize = New Size(502, 549)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(ChooseFolder)
        Controls.Add(ProgressBar1)
        Controls.Add(FileTypeLabel)
        Controls.Add(Description)
        Controls.Add(FileTypeDropdown)
        Controls.Add(Title)
        Controls.Add(SelectedFileLabel)
        Controls.Add(FileUploadButton)
        Name = "Form1"
        Text = "OUReports Condense File"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents FileUploadButton As Button
    Friend WithEvents SelectedFileLabel As Label
    Friend WithEvents Title As Label
    Friend WithEvents FileTypeDropdown As ComboBox
    Friend WithEvents Description As Label
    Friend WithEvents FileTypeLabel As Label
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents ChooseFolder As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
