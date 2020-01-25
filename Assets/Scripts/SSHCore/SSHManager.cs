using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSHManager : MonoBehaviour
{
    #region Singleton
    private static SSHManager instance = null;
    private static readonly object padlock = new object();

    SSHManager() { }

    public static SSHManager Instance {
        get {
            if(instance == null)
            {
                lock (padlock)
                {
                    if(instance  == null)
                    {
                        instance = new SSHManager();
                    }
                }
            }

            return instance;
        } 
    }
    #endregion

    private SSHCredentials credentials;

    public void CreateCredentials(string username, string password, string ip)
    {
        credentials = new SSHCredentials()
        {
            Username = username,
            Password = password,
            Ip = ip
        };
    }

    public SSHResponse SendCommand(string command, SSHCredentials credentials)
    {
        if(String.IsNullOrEmpty(command))
        {
            return new SSHResponse(false, "Command was empty or null");
        }

        try
        {
            using (var client = new SshClient(credentials.Ip, credentials.Username, credentials.Password))
            {
                client.Connect();
                client.RunCommand(command);
                client.Disconnect();

                return new SSHResponse(true, "Successfully sent SSH command");
            }
        } catch(Exception e)
        {
            return new SSHResponse(false, e.Message, e);
        }
        
    }

    public SSHCredentials SaveCredentials(string command, string encryptionKey, string username, string password, string ip)
    {
        SSHCredentials credentials = new SSHCredentials()
        {
            Username = username,
            Password = password.Encrypt(encryptionKey),
            Ip = ip,
            EncryptionKey = encryptionKey
        };

        PlayerPrefs.SetString("command", command);
        PlayerPrefs.SetString("encryptionKey", encryptionKey);
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("password", password.Encrypt(encryptionKey));
        PlayerPrefs.SetString("ip", ip);
        PlayerPrefs.Save();

        return credentials;
    }

    public (SSHCredentials, string) LoadCredentials()
    {
        string encryptionkey = PlayerPrefs.GetString("encryptionKey");
        string loadedCommand = PlayerPrefs.GetString("command");

        SSHCredentials credentials = new SSHCredentials()
        {
            Username = PlayerPrefs.GetString("username"),
            Password = PlayerPrefs.GetString("password").Decrypt(encryptionkey),
            Ip = PlayerPrefs.GetString("ip"),
            EncryptionKey = encryptionkey
        };

        return (credentials, loadedCommand);
    }
}
