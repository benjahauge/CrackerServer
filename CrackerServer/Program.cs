using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CrackerServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Master Server has begun!");

			Cracker crack = new Cracker();
			crack.ReadDictionaryAndCreateChunks("webster-dictionary.txt");

			//listener.HandleClient(IPAddress.Any, 7777);

			TcpListener listener = new TcpListener(IPAddress.Any, 7777);
			listener.Start();

			while (true)
			{
				

				Task.Run(() =>
				{
					TcpClient socket = listener.AcceptTcpClient();
					crack.HandleClient(socket);
				});
			}
		}
	}
}
