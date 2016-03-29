using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/// <summary>
/// The application namespace.
/// </summary>
namespace ComServer
{
	/// <summary>
	/// A simple server / socket handler class.
	/// </summary>
	public class Server
	{
		/// <summary>
		/// Handler for when a connection is being established.
		/// </summary>
		private ManualResetEvent mConnectionResetEvent = new ManualResetEvent(false);
		/// <summary>
		/// Handler for when client information is being processed.
		/// </summary>
		private ManualResetEvent mProcessClientData = new ManualResetEvent(false);
		/// <summary>
		/// The connection listener used for client connection
		/// </summary>
		private Thread ConnectionListener;
		private Thread MessageDataHandler;
		/// <summary>
		/// The TCP socket.
		/// </summary>
		TcpListener mTCPServer;

		/// <summary>
		/// The port which the server executes from.
		/// </summary>
		public int port { get; set; } = 27015;
		/// <summary>
		/// Initializes a new instance of the <see cref="ComServer.Server"/> class.
		/// </summary>
		public Server ()
		{
			// I was always warned about using constructors for anything but initialising variables.
		}
		~Server()
		{
			mTCPServer.Stop ();
		}

		/// <summary>
		/// Start the server with the specified arguments.
		/// </summary>
		/// <param name="arguments">Arguments.</param>
		public bool StartServer( string arguments = "" )
		{
			try
			{
				// Initialise the listener.
				mTCPServer = new TcpListener (IPAddress.Any,port);
				mTCPServer.Start();
				// Begin listening on a thread.
				ConnectionListener = new Thread(() => WaitingThread(mTCPServer));

				// Start our new thread.
				ConnectionListener.Start();
			}
			catch(System.Exception e)
			{
				Console.WriteLine (e.Message);
				// Raises an error
				return true;
			}

			// Unhandled exception, this shouldn't happen.
			return mTCPServer == null;
		}

		/// <summary>
		/// Client waiting thread function.
		/// </summary>
		/// <param name="listener">Listener.</param>
		private void WaitingThread( TcpListener sockListener )
		{
			// Blocking threads from executing.
			mConnectionResetEvent.Reset ();
			Console.WriteLine ("Waiting for a connection!");

			// Process the incomming connection.
			sockListener.BeginAcceptTcpClient (new AsyncCallback (ProcessIncomingConnection), sockListener);

			// Wait until connection is established. (e.g. when mConnectionResetEvent.Set is called)
			mConnectionResetEvent.WaitOne();
		}

		/// <summary>
		/// Processes the incoming connection on a thread.
		/// </summary>
		/// <param name="ar">Ar.</param>
		private void ProcessIncomingConnection( IAsyncResult ar )
		{
			// Read the async object.
			TcpListener listener = (TcpListener) ar.AsyncState;

			// End accepting the connection
			TcpClient client = listener.EndAcceptTcpClient (ar);

			// Show the information in the console.
			Console.WriteLine ("Accepted client connection");
			//mProcessClientData.Reset ();

			// Message data handler - process network message stream.
			MessageDataHandler = new Thread(() => ProcessNetworkMessage( client ));
			MessageDataHandler.Start ();
			MessageDataHandler.Join ();
			//mProcessClientData.WaitOne ();
			Console.WriteLine ("Handling network data");

		}

		private void ProcessNetworkMessage(TcpClient client)
		{
			// Open network stream reference
			NetworkStream ns = client.GetStream ();

			string data = "";
			// If data is available.
			while (ns.DataAvailable) 
			{
				// Append byte
				data += Convert.ToChar(ns.ReadByte());
			}
			// REMEMEBR NO ENCODING WAS CHECKED.
			Console.WriteLine ("Data recieved: " + data);
		}

	}
}

