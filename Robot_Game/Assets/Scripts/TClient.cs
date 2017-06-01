using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.IO;

public class TClient : MonoBehaviour
{
	TcpClient client = new TcpClient ();
	Thread thread;

	List<string[]> controlChecker;
	List<string[]> newControlChecker;

	void Start ()
	{
		Connect ();
	}

	void Connect ()
	{
		thread = new Thread (new ThreadStart (BTBridgeConnect));
		thread.Start ();
	}

	public void CloseClient ()
	{
		try {
			thread.Abort ();
		} catch (Exception e) {
			print ("[THREAD CLOSING] " + e);
		}

		try {
			client.Close ();
		} catch (Exception e) {
			print ("[TCPClient CLOSING] " + e);
		}
	}

	void BTBridgeConnect ()
	{
		while (true) {
			try {
				IAsyncResult ar = client.BeginConnect ("127.0.0.1", 8000, null, null);
				System.Threading.WaitHandle wh = ar.AsyncWaitHandle;  
				try {  
					if (!ar.AsyncWaitHandle.WaitOne (TimeSpan.FromSeconds (5), false)) {  
						client.Close ();  
						throw new TimeoutException ();  
					}

					client.EndConnect (ar);  
				} finally {  
					wh.Close ();  
				}

				if (client.Connected) {
					Debug.Log ("Client is connected!");

					StreamReader sr = new StreamReader (client.GetStream ());

					while (true) {
						try {
							if (sr.Peek () > -1) {
								
							}
						} catch (SystemException e) {	
							Debug.Log ("Exception 1: " + e);
							sr.Close ();
							client.Close ();
						}
					}
				} else {
					Debug.Log ("Client is not connected!");
				}

			} catch (SystemException e) {
				Debug.Log ("Exception 2: " + e);
			}

			Thread.Sleep (2000);
		}
	}

	void OnApplicationQuit ()
	{
		CloseClient ();
	}
}
