using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Collections;

public class NonMutexx : MonoBehaviour
{

    public string path = "TEST/test.txt";
    public string append1 = "hello";
    public string append2 = "there";

    private static readonly object padlock = new object();

    Mutex mut = new Mutex();
    
   
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("startLogging");



        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startLogging()
    {


        //yield return null;

        for (int i = 0; i < 10; i++)
        {


            Task.Factory.StartNew(() =>
            {
                //lock(padlock)

                if(mut.WaitOne(1000))

                {

                    try
                        {
                            Console.WriteLine(File.ReadAllText(@path));
                            File.AppendAllText(path, append1);
                            Thread.Sleep(1000);
                            File.AppendAllText(path, append2);
                        }
                    finally
                        {
                            mut.ReleaseMutex();
                        }

                }

                else
                {
                    Console.WriteLine("Mutex has not been released in 1 sec");

                }


            });
        }

        Console.ReadKey();

        yield return new WaitForEndOfFrame();
    }


    
}
