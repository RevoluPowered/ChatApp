using System;

namespace ComServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");
			Server mServer = new Server ();
			mServer.StartServer ();
		}
	}
}
