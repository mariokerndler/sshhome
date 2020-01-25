using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SSHUI : MonoBehaviour
{
    #region Singleton
    private static SSHUI instance = null;
    private static readonly object padlock = new object();

    SSHUI() { }

    public static SSHUI Instance {
        get {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SSHUI();
                    }
                }
            }

            return instance;
        }
    }
    #endregion

    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField ip;

    public TMP_InputField command;
    public TMP_Text response;
    public Color successColor = Color.green;
    public Color failureColor = Color.red;

    private SSHCredentials credentials;

    private void Start()
    {
        credentials = new SSHCredentials();

        LoadCredentials();
    }

    public void SendRequest()
    {
        if (string.IsNullOrEmpty(username?.text.Trim()) && string.IsNullOrEmpty(username?.text.Trim()) && string.IsNullOrEmpty(username?.text.Trim())) return;

        credentials = SSHManager.Instance.SaveCredentials(command.text.Trim(), Guid.NewGuid().ToString(), username.text.Trim(), password.text.Trim(), ip.text.Trim());

        SSHResponse sshResponse = SSHManager.Instance.SendCommand(command.text.Trim(), credentials);
        if(sshResponse.Success)
        {
            response.color = successColor;
        }
        else
        {
            response.color = failureColor;
        }
            response.text = sshResponse.Message;
    }

    private void LoadCredentials()
    {
        (SSHCredentials, string) result = SSHManager.Instance.LoadCredentials();
        username.text = result.Item1.Username;
        password.text = result.Item1.Password;
        ip.text = result.Item1.Ip;
        command.text = result.Item2;

        credentials = result.Item1;
    }

    private void OnApplicationQuit()
    {
        credentials = SSHManager.Instance.SaveCredentials(command.text.Trim(), Guid.NewGuid().ToString(), username.text.Trim(), password.text.Trim(), ip.text.Trim());
    }

}
