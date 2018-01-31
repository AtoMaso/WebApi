Imports System.Data.SqlClient
Imports System.IO
Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports LottoStuff.BusinessDocuments
Public Class PictureHelper

    Protected WithEvents imgUpload As Global.System.Web.UI.WebControls.FileUpload

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles btnSubmit.Click

        'get relatives
        'Dim relatives As String = GetSelectedRelatives()
        Dim connection As SqlConnection = Nothing
        Try
            Dim user As System.Security.Principal.IPrincipal = HttpContext.Current.User
            Dim username As String = user.Identity.Name

            'imgUpload is the upload control

            Dim img As FileUpload = CType(imgUpload, FileUpload)
            Dim imgByte As Byte() = Nothing
            If img.HasFile AndAlso Not img.PostedFile Is Nothing Then
                'To create a PostedFile
                Dim File As HttpPostedFile = imgUpload.PostedFile
                'Create byte Array with file len
                imgByte = New Byte(File.ContentLength - 1) {}
                'force the control to load data in array
                File.InputStream.Read(imgByte, 0, File.ContentLength)

                'resize the photo to 400x300 px
                imgByte = ResizeImageFile(imgByte, 400)

                ' Insert the picture tag and image into db
                Dim conn As String = ConfigurationManager.ConnectionStrings("PictureAlbumConnectionString").ConnectionString
                connection = New SqlConnection(conn)
                connection.Open()

                Dim cmd As SqlCommand = New SqlCommand()
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "sptStoreImage"
                cmd.Connection = connection
                'cmd.Parameters.AddWithValue("@tag", txtTags.Text)
                cmd.Parameters.AddWithValue("@pic", imgByte)
                cmd.Parameters.AddWithValue("@username", username)
                'cmd.Parameters.AddWithValue("@visibility", relatives)
                cmd.Parameters.AddWithValue("@upload_date", DateTime.Today)
                Dim id As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                Dim errorText As String = String.Format("Picture with ID is {0} was successfuly uploaded.", id)
                'Me.DisplayMessages(errorText, "Info")
            Else
                'Me.DisplayMessages("No image to upload selected. Please select an image to upload.", "Error")
            End If

        Catch ex As SqlException
            Dim errorText As String = String.Format("There was an error: " + ex.Message)
            'Me.DisplayMessages(errorText, "Error")
        Finally
            If Not connection Is Nothing Then
                connection.Close()
            End If
        End Try
    End Sub


    Public Function ResizeImageFile(ByVal imageFile As Byte(), ByVal targetSize As Integer) As Byte()

        Dim original As Image = Image.FromStream(New MemoryStream(imageFile))
        Dim targetH, targetW As Integer
        With original
            If (.Height > .Width) Then
                targetH = targetSize
                targetW = CType(.Width * (CType(targetSize, Decimal) / CType(.Height, Decimal)), Integer)
            Else
                targetW = targetSize
                targetH = CType(.Height * (CType(targetSize, Decimal) / CType(.Width, Decimal)), Integer)
            End If
        End With

        Dim imgPhoto As Image = Image.FromStream(New MemoryStream(imageFile))
        ' Create a new blank canvas.  The resized image will be drawn on this canvas.
        Dim bmPhoto As Bitmap = New Bitmap(targetW, targetH, PixelFormat.Format24bppRgb)
        With (bmPhoto)
            .SetResolution(400, 300)
        End With

        'new image
        Dim grPhoto As Graphics = Graphics.FromImage(bmPhoto)
        With grPhoto
            .SmoothingMode = SmoothingMode.AntiAlias
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .DrawImage(imgPhoto, New Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel)
        End With

        ' Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.      
        Dim mm As MemoryStream = New MemoryStream()
        bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg)
        original.Dispose()
        imgPhoto.Dispose()
        bmPhoto.Dispose()
        grPhoto.Dispose()
        Return mm.GetBuffer()

    End Function


End Class
