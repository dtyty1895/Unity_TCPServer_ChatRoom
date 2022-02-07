using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using System.Threading;

public class ChatManagers : MonoBehaviour
{
    public string ipaddress = "172.20.10.5";
    public int port = 7878;
    public InputField inputField;
    public Text txt;


    private Socket clientSocket;
    private Thread t;
    private byte[] data = new byte[1024];
    private string message = "";
    string value;
    // Start is called before the first frame update
    void Start()
    {
        ConnectedToServer();
    }

    // Update is called once per frame
    void Update()
    {
        //print(value);
   
        //print(message+" "+ value);
        if (message != null && message != "")
        {
            txt.text += "\n" + message;
            message = "";     
        }
    }
    void ConnectedToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));

        t = new Thread(ReceeiveMessage);
        t.Start();
    }
    void ReceeiveMessage()
    {
        while (true)
        {
            if (clientSocket.Connected == false)
            {
                break;
            }
            int length = clientSocket.Receive(data);
            message = Encoding.UTF8.GetString(data, 0, length);
        }
    }
    void SendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(data);
    }

    public void OnSendButtonClick()
    {
        inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
        txt = GameObject.Find("Canvas/txt").GetComponent<Text>();
        value = inputField.text.ToString();
       // print(value);
        //print(value);
        SendMessage(value);
        inputField.text = "";
    }
    void OnDestroy()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
    }
}
