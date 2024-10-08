﻿Imports MySql.Data.MySqlClient

Public Class Form1

    ' Cadena de conexión para MySQL
    Dim connectionString As String = "Server=localhost;Database=registropersonas;User ID='root';Password='';"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboComuna.Items.Add("Santiago")
        cboComuna.Items.Add("Cerrillos")
        cboComuna.Items.Add("Cerro Navia")
        cboComuna.Items.Add("Conchalí")
        cboComuna.Items.Add("El Bosque")
        cboComuna.Items.Add("Estación Central")
        cboComuna.Items.Add("Huechuraba")
        cboComuna.Items.Add("Independencia")
        cboComuna.Items.Add("La Cisterna")
        cboComuna.Items.Add("La Florida")
        cboComuna.Items.Add("La Granja")
        cboComuna.Items.Add("La Pintana")
        cboComuna.Items.Add("La Reina")
        cboComuna.Items.Add("Las Condes")
        cboComuna.Items.Add("Lo Barnechea")
        cboComuna.Items.Add("Lo Espejo")
        cboComuna.Items.Add("Lo Prado")
        cboComuna.Items.Add("Macul")
        cboComuna.Items.Add("Maipú")
        cboComuna.Items.Add("Ñuñoa")
        cboComuna.Items.Add("Pedro Aguirre Cerda")
        cboComuna.Items.Add("Peñalolén")
        cboComuna.Items.Add("Providencia")
        cboComuna.Items.Add("Pudahuel")
        cboComuna.Items.Add("Quilicura")
        cboComuna.Items.Add("Quinta Normal")
        cboComuna.Items.Add("Recoleta")
        cboComuna.Items.Add("Renca")
        cboComuna.Items.Add("San Joaquín")
        cboComuna.Items.Add("San Miguel")
        cboComuna.Items.Add("San Ramón")
        cboComuna.Items.Add("Vitacura")
        cboComuna.Items.Add("Puente Alto")
        cboComuna.Items.Add("Pirque")
        cboComuna.Items.Add("San José de Maipo")
        cboComuna.Items.Add("Colina")
        cboComuna.Items.Add("Lampa")
        cboComuna.Items.Add("Tiltil")
        cboComuna.Items.Add("San Bernardo")
        cboComuna.Items.Add("Buin")
        cboComuna.Items.Add("Calera de Tango")
        cboComuna.Items.Add("Paine")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim rut As String = txtRUT.Text
        Dim nombre As String = txtNombre.Text
        Dim apellido As String = txtApellido.Text
        Dim sexo As String

        ' Validar la selección del sexo
        If rbtnMasculino.Checked Then
            sexo = "Masculino"
        ElseIf rbtnFemenino.Checked Then
            sexo = "Femenino"
        ElseIf rbtnNoEspecifica.Checked Then
            sexo = "No especifica"
        Else
            MessageBox.Show("Por favor, seleccione el sexo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Obtener otros campos
        Dim comuna As String = cboComuna.SelectedItem?.ToString()
        Dim ciudad As String = txtCiudad.Text
        Dim observacion As String = txtObservacion.Text

        ' Validar campos obligatorios
        If String.IsNullOrWhiteSpace(rut) Or String.IsNullOrWhiteSpace(nombre) Or String.IsNullOrWhiteSpace(apellido) Or String.IsNullOrWhiteSpace(comuna) Then
            MessageBox.Show("Por favor, complete todos los campos que son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Guardar los datos en la base de datos
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim sql As String = "INSERT INTO Personas (RUT, Nombre, Apellido, Sexo, Comuna, Ciudad, Observacion) " &
                                    "VALUES (@rut, @nombre, @apellido, @sexo, @comuna, @ciudad, @observacion)"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@rut", rut)
                    cmd.Parameters.AddWithValue("@nombre", nombre)
                    cmd.Parameters.AddWithValue("@apellido", apellido)
                    cmd.Parameters.AddWithValue("@sexo", sexo)
                    cmd.Parameters.AddWithValue("@comuna", comuna)
                    cmd.Parameters.AddWithValue("@ciudad", ciudad)
                    cmd.Parameters.AddWithValue("@observacion", observacion)

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Datos guardados con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Limpiar el formulario después de guardar
                    LimpiarFormulario()
                End Using

            Catch ex As Exception
                MessageBox.Show("Error al guardar datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Método para limpiar el formulario
    Private Sub LimpiarFormulario()
        txtRUT.Clear()
        txtNombre.Clear()
        txtApellido.Clear()
        txtCiudad.Clear()
        txtObservacion.Clear()
        rbtnMasculino.Checked = False
        rbtnFemenino.Checked = False
        rbtnNoEspecifica.Checked = False
        cboComuna.SelectedIndex = -1
        txtRUT.Focus() ' Colocar el foco en el campo RUT
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If String.IsNullOrEmpty(txtRUT.Text) Then
            MessageBox.Show("Por favor ingresar un RUT.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Using conn As New MySqlConnection(connectionString)
                conn.Open()

                Dim query As String = "SELECT * FROM Personas WHERE RUT = @RUT"
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RUT", txtRUT.Text)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.HasRows Then
                            reader.Read()

                            txtNombre.Text = reader("Nombre").ToString()
                            txtApellido.Text = reader("Apellido").ToString()
                            Dim sexo As String = reader("Sexo").ToString()
                            If sexo = "Masculino" Then
                                rbtnMasculino.Checked = True
                            ElseIf sexo = "Femenino" Then
                                rbtnFemenino.Checked = True
                            Else
                                rbtnNoEspecifica.Checked = True
                            End If
                            cboComuna.SelectedItem = reader("Comuna").ToString()
                            txtCiudad.Text = reader("Ciudad").ToString()
                            txtObservacion.Text = reader("Observacion").ToString()
                        Else
                            MessageBox.Show("El RUT indicado no ha sido encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End Using
                End Using
            End Using

        Catch ex As MySqlException
            MessageBox.Show("Ha ocurrido un error al intentar buscar los datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If String.IsNullOrEmpty(txtRUT.Text) Then
            MessageBox.Show("Por favor, busque un usuario primero.")
            Exit Sub
        End If

        Dim query As String = "UPDATE personas SET Nombre = @Nombre, Apellido = @Apellido, Sexo = @Sexo, Comuna = @Comuna, Ciudad = @Ciudad, Observacion = @Observacion WHERE RUT = @RUT"

        Using conn As New MySqlConnection(connectionString)
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@RUT", txtRUT.Text)
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text)
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text)
                cmd.Parameters.AddWithValue("@Sexo", If(rbtnMasculino.Checked, "Masculino", "Femenino"))
                cmd.Parameters.AddWithValue("@Comuna", cboComuna.Text)
                cmd.Parameters.AddWithValue("@Ciudad", txtCiudad.Text)
                cmd.Parameters.AddWithValue("@Observacion", txtObservacion.Text)

                Try
                    conn.Open()
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("El usuario ha sido actualizado.")
                Catch ex As MySqlException
                    MessageBox.Show("Ha ocurrido un error al actualizar usuario: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If String.IsNullOrEmpty(txtRUT.Text) Then
            MessageBox.Show("Por favor, ingrese un RUT que desee eliminar.")
            Exit Sub
        End If

        If MessageBox.Show("¿Seguro de que deseas eliminar el usuario?", "Confirmar", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Dim query As String = "DELETE FROM personas WHERE RUT = @RUT"

            Using conn As New MySqlConnection(connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RUT", txtRUT.Text)

                    Try
                        conn.Open()
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("El usuario ha sido eliminado.")
                    Catch ex As MySqlException
                        MessageBox.Show("Ha ocurrido un error al eliminar el usuario: " & ex.Message)
                    End Try
                End Using
            End Using
        End If
    End Sub

    Private Sub btnVerDatos_Click(sender As Object, e As EventArgs) Handles btnVerDatos.Click
        Dim query As String = "SELECT RUT, Nombre, Apellido FROM personas"

        Using conn As New MySqlConnection(connectionString)
            Using cmd As New MySqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim reader As MySqlDataReader = cmd.ExecuteReader()
                    Dim usuarios As String = ""

                    While reader.Read()
                        usuarios &= "RUT: " & reader("RUT").ToString() & ", Nombre: " & reader("Nombre").ToString() & ", Apellido: " & reader("Apellido").ToString() & Environment.NewLine
                    End While

                    If usuarios <> "" Then
                        MessageBox.Show(usuarios)
                    Else
                        MessageBox.Show("No se han encontrado usuarios registrados.")
                    End If
                Catch ex As MySqlException
                    MessageBox.Show("Ha ocurrido un error al obtener usuarios: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
End Class


