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
}
