using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SSHUI : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField ip;

    public TMP_InputField command;
    public TMP_Text response;

    private SSHCredentials credentials;

    private void Start()
    {
        credentials = new SSHCredentials();
    }

    public void SendRequest()
    {
        if (string.IsNullOrEmpty(username.text.Trim()) && string.IsNullOrEmpty(username.text.Trim()) && string.IsNullOrEmpty(username.text.Trim())) return;


        credentials = new SSHCredentials()
        {
            Username = username.text.Trim(),
            Password = password.text.Trim(),
            Ip = ip.text.Trim()
        };
        
        PlayerPrefs.SetString("username", username.text.Trim());
        PlayerPrefs.SetString("password", password.text.Trim());
        PlayerPrefs.SetString("ip", ip.text.Trim());

        SSHResponse sshResponse = SSHManager.Instance.SendCommand(command.text.Trim(), credentials);
        response.text = sshResponse.Message;
    }
}
