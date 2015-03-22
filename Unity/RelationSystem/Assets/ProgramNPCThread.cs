using System;
using System.Collections.Generic;
using System.Threading;

//Namespaces
using NRelationSystem;


public partial class Program
{
    List<Being> beings = new List<Being>();


    void NPCThreadFunc()
    {
        while (stopNPCLoop)
        {
            Thread.Sleep(1);
        }

        StopNPCThread();
    }


    void StopNPCThread()
    {

    }
}
