using NUnit.Framework;
using System;

namespace ComServer
{
	[TestFixture ()]
	/// <summary>
	/// A class to test the Server class.
	/// </summary>
	public class ServerTest
	{
		[Test ()]
		/// <summary>
		/// Does the server start without throwing an exception?
		/// </summary>
		public void ServerStartup ()
		{
			Server srv = new Server ();
			srv.StartServer ();
			Assert.That(srv.port, Is.EqualTo(27015)); 
		}
	}
}

