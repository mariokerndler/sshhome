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

    private SSHCredentials credentials;

    private void Start()
    {
        credentials = new SSHCredentials();

        LoadCredentials();
    }

    public void SendRequest()
    {
        if (string.IsNullOrEmpty(username.text.Trim()) && string.IsNullOrEmpty(username.text.Trim()) && string.IsNullOrEmpty(username.text.Trim())) return;

        credentials = new SSHCredentials()
        {
            Username = username.text.Trim(),
            Password = password.text.Trim(),
            Ip = ip.text.Trim(),
            EncryptionKey = Guid.NewGuid().ToString()
        };

        PlayerPrefs.SetString("command", command.text.Trim());
        PlayerPrefs.SetString("encryptionKey", credentials.EncryptionKey);
        PlayerPrefs.SetString("username", username.text.Trim());
        PlayerPrefs.SetString("password", password.text.Trim().Encrypt(credentials.EncryptionKey));
        PlayerPrefs.SetString("ip", ip.text.Trim());
        PlayerPrefs.Save();

        SSHResponse sshResponse = SSHManager.Instance.SendCommand(command.text.Trim(), credentials);
        response.text = sshResponse.Message;
    }

    private void LoadCredentials()
    {
        string encryptionkey = PlayerPrefs.GetString("encryptionKey");
        string loadedUsername = PlayerPrefs.GetString("username");
        string loadedPassword = PlayerPrefs.GetString("password").Decrypt(encryptionkey);
        string loadedIp = PlayerPrefs.GetString("ip");
        string loadedCommand = PlayerPrefs.GetString("command");

        username.text = loadedUsername;
        password.text = loadedPassword;
        ip.text = loadedIp;
        command.text = loadedCommand;

        credentials = new SSHCredentials()
        {
            Username = loadedUsername,
            Password = loadedPassword,
            Ip = loadedIp,
            EncryptionKey = encryptionkey
        };
    }
}
