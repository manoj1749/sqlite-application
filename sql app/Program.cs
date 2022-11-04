using System;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Data.SQLite;
namespace appgui;
public class Form1 : Form
{
    public Button button2;
    public TextBox textInputTextBox;
    private static string dbCommand = "Data Source=DemoDB.db;Version=3;New=False;Compress=True;";
    private static SQLiteConnection dbConnection = new SQLiteConnection(dbCommand);
    private static SQLiteCommand Command = new SQLiteCommand("", dbConnection);
    public Form1()
    {
        Label ip_label = new Label();
        ip_label.Text = "License Key";
        ip_label.Location = new Point(25, 30);
        ip_label.AutoSize = true;
        ip_label.Font = new Font("Calibri", 10);
        ip_label.Padding = new Padding(6);
        this.Controls.Add(ip_label);
        textInputTextBox = new TextBox();
        textInputTextBox.Location = new Point(110, 30);
        textInputTextBox.Size = new Size(120, 20);
        this.Controls.Add(textInputTextBox);
        Size = new Size(300, 150);
        button2 = new Button();
        button2.Size = new Size(60, 25);
        button2.Location = new Point(120, 60);
        button2.Text = "Run";
        this.Controls.Add(button2);
        button2.Click += new EventHandler(license_click);
    }
    private void license_click(object sender, EventArgs e)
    {
        MessageBox.Show("Starting SQL");
        openConnection();
        using (var transaction = dbConnection.BeginTransaction())
        {
            var insertCmd = dbConnection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Person VALUES('LAGUNITAS','IPA')";
            insertCmd.ExecuteNonQuery();
            transaction.Commit();
        }
        var selectCmd = dbConnection.CreateCommand();
        var selectCmd1 = dbConnection.CreateCommand();
        selectCmd.CommandText = "SELECT FirstName FROM Person";
        selectCmd1.CommandText = "SELECT LastName FROM Person";
        string msg1;
        using (var reader = selectCmd.ExecuteReader())
        {
            using (var reader1 = selectCmd1.ExecuteReader())
            {
                while (reader.Read() && reader1.Read())
                {
                    var message = reader.GetString(0);
                    var message1 = reader1.GetString(0);
                    msg1 = message + " " + message1;
                    MessageBox.Show(msg1);
                }         
            }
        }
        //var reader = selectCmd.ExecuteReader(); 
        //reader.Read();
        closeConnection();
        return;
    }
    private void openConnection()
    {
        MessageBox.Show("Opening Connection");
        if (dbConnection.State == System.Data.ConnectionState.Closed)
        {
            dbConnection.Open();
            MessageBox.Show("Connection opened to:" + dbConnection.State.ToString());
        }
    }
    private void closeConnection()
    {
        if (dbConnection.State == System.Data.ConnectionState.Open)
        {
            dbConnection.Close();
            MessageBox.Show("Connection closed to:" + dbConnection.State.ToString());
        }
    }
}
static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}