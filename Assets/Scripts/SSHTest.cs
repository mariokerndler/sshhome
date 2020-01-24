using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Renci.SshNet;

public class SSHTest : MonoBehaviour
{
    void Start()
    {
        SendTest();
    }

    private void SendTest()
    {
        using(var client = new SshClient("silvesterkoe.ddns.net", "pi", "Emma4thewinskdat3001"))
        {
            client.Connect();
            client.RunCommand("sudo bash ./wol.sh");
            client.Disconnect();
        }
    }
}
