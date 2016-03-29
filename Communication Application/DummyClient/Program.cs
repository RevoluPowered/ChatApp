using System;
using System.Net;
using System.Net.Sockets;

namespace DummyClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting the dummy client...");


			// SOCKET INFO
			int hostPort = 27015; // port to connect on.
			string hostIPAddress = "127.0.0.1"; // local unless otherwise specified.


			// ARGUMENT HANDLER
			// ARGS = 1 (IP)
			// ARGS = 2 (IP PORT)
			if (args.Length > 0 && args.Length < 3) {
				try 
				{
					if(args[0] == "help")
					{
						Console.WriteLine("syntax: DummyClient ip [port] - connects on the specified ip and port\n\tDummyClient - assumes localhost testing\n\tDummyClient ip - connects on the default port.");
					}
					// IP SUPPLIED.
					Console.WriteLine("Using ip override ip: " + args[0]);
					hostIPAddress = args[0];

					// PORT SUPPLIED?
					if(args.Length == 2)
					{
						if(Int32.TryParse(args[1],out hostPort))
						{
							Console.WriteLine("Using override port: " + hostPort);
						}
						else
						{
							Console.WriteLine("Error: incorrect format for port, try using 'help' command.");
						}
					}

				}
				catch(System.Exception e) {
					Console.WriteLine ("An unexpected error occoured:" + e.Message);

				}
			}
				
			//
			// Socket Debugging.
			//

			// Establish TCP client connection
			TcpClient client = new TcpClient();

			// Connection test.
			client.Connect (hostIPAddress, hostPort);

			// Data test.
			NetworkStream stream = client.GetStream ();
			// UTF8 data transfer test for decoding validity.
			byte[] testMessage = System.Text.Encoding.UTF8.GetBytes ("Hello Server... client version: dummy");
			// Write the message
			stream.Write (testMessage, 0, testMessage.Length);

			// Closing connection.
			Console.WriteLine ("Press any key to close connection.");
			Console.ReadLine ();

			// Close it down.
			client.Close ();
		}
	}
}
