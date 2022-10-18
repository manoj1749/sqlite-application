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
    public Button button1;
    public Button button2;
    public TextBox textInputTextBox;
    public Button textInputeButton;
    private static string dbCommand = "Data Source=DemoDB.db;Version=3;New=False;Compress=True;";
    private static SQLiteConnection dbConnection = new SQLiteConnection(dbCommand);
    private static SQLiteCommand Command = new SQLiteCommand("", dbConnection);
    //public bool license_valid;
    //public MainMenu Menu;
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


        var ip = "127.0.0.1";
        //MessageBox.Show("1");
        IPAddress address = IPAddress.Parse(ip);
        //MessageBox.Show(ip);
        //MessageBox.Show(address.ToString());
        //MessageBox.Show("2");
        IPEndPoint endPoint = new IPEndPoint(address, 8080);
        //MessageBox.Show(endPoint.ToString());
        //MessageBox.Show("3");
        Socket Sock = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        //MessageBox.Show("4");
        Sock.Connect(endPoint);
        //MessageBox.Show(endPoint.ToString());
        //MessageBox.Show("5");
        Console.WriteLine("Starting SQL");
        //dbConnection.Open();
        //Console.WriteLine("SQL Started");
        openConnection();
        using (var transaction = dbConnection.BeginTransaction())
        {
            var insertCmd = dbConnection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Person VALUES('LAGUNITAS','IPA')";
            insertCmd.ExecuteNonQuery();
            transaction.Commit();
        }
        var selectCmd = dbConnection.CreateCommand();

        selectCmd.CommandText = "SELECT FirstName FROM Person";
        string[] names;
        using (var reader = selectCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                var message = reader.GetString(0);
                names = new string[] { message };
                Console.WriteLine(message);
            }
        }
        Console.WriteLine(names);
        byte[] msg = Encoding.ASCII.GetBytes(names);
        Sock.Send(msg, msg.Length, 0);
        byte[] buffer = new byte[1024];
        int recieved = Sock.Receive(buffer);
        byte[] data = new byte[recieved];
        Array.Copy(buffer, data, recieved);
        MessageBox.Show(Encoding.ASCII.GetString(data));
        //MessageBox.Show("6");
        Sock.Close();
        closeConnection();
        return;

    }

    private void openConnection()
    {
        Console.WriteLine("Opening Connection");
        // TODO
        if (dbConnection.State == System.Data.ConnectionState.Closed)
        {
            dbConnection.Open();
            Console.WriteLine("Connection opened to:" + dbConnection.State.ToString());
        }
    }

    private void closeConnection()
    {
        // TODO
        if (dbConnection.State == System.Data.ConnectionState.Open)
        {
            dbConnection.Close();
            Console.WriteLine("Connection closed to:" + dbConnection.State.ToString());
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