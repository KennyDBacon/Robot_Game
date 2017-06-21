using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TClient : MonoBehaviour
{
	TcpClient client;

	Thread thread;
	bool threadState;

	KeyController keyController;

	void Start ()
	{
		keyController = this.transform.GetComponent<KeyController> ();

		Connect ();
	}

	void Connect ()
	{
		// Start a new TCPClient
		client = new TcpClient ();

		// For thread looping
		threadState = true;

		// Create a thread to run method that reads from BTBridge
		thread = new Thread (new ThreadStart (BTBridgeConnect));
		thread.Start ();
	}

	void BTBridgeConnect ()
	{
		while (threadState) {
			try {
				// Connect to localhost (127.0.0.1)
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

					// Read streams from BTBridge
					StreamReader sr = new StreamReader (client.GetStream ());

					while (threadState) {
						try {
							if (sr.Peek () > -1) {
								// Read and split the line read to multiple strings
								string btLine = sr.ReadLine ();
								string[] data = btLine.Split (',');

								if (!keyController.IsConnected) {
									keyController.Connect (data);
								} else if (keyController.CheckID (data [1])) {
									keyController.UpdateInput (data);
								}
							}
						} catch (SystemException e) {	
							Debug.Log ("Exception 1: " + e);
							sr.Close ();
							client.Close ();
							threadState = false;
						}
					}
				} else {
					Debug.Log ("Client is not connected!");
				}

			} catch (SystemException e) {
				//Debug.Log ("Exception 2: " + e);
			}

			Thread.Sleep (2000);
		}
	}

	void OnApplicationQuit ()
	{
		CloseClient ();
	}

	/// <summary>
	/// Clost thread and client.
	/// </summary>
	public void CloseClient ()
	{
		try {
			threadState = false;
			thread.Abort ();
		} catch (Exception e) {
			print ("[THREAD CLOSING ERROR] " + e);
		}

		try {
			client.Close ();
		} catch (Exception e) {
			print ("[TCPClient CLOSING ERROR] " + e);
		}
	}
}
