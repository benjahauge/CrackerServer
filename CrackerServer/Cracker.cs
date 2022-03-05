using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using CrackerServer.model;
using CrackerServer.util;

namespace CrackerServer
{
	internal class Cracker
	{
		private BlockingCollection<List<string>> chunks = new BlockingCollection<List<string>>();
		public Cracker()
		{
			List<UserInfo> userInfos =
				PasswordFileHandler.ReadPasswordFile("passwords.txt");

			//ReadDictionaryAndCreateChunks("webster-dictionary.txt");

		}

		internal void HandleClient(TcpClient socket)
		{
			//TcpListener server = new TcpListener(loopback, port);
			//server.Start();
			//TcpClient socket = server.AcceptTcpClient();
			NetworkStream ns = socket.GetStream();
			StreamReader reader = new StreamReader(ns);
			StreamWriter writer = new StreamWriter(ns);

			//writer.AutoFlush = true;

			string request = reader.ReadLine();

			if (request == "password")
			{

			}
			else if (request == "chunk")
			{
				while (request != "bye")
				{
					List<string> chunk = chunks.Take();
					var data = JsonSerializer.Serialize(chunk);
					writer.WriteLine(data);
					writer.Flush();
				}
			}
			else
			{
				writer.WriteLine("ehm");
				writer.Flush();
			}
		}

		//public void Start(IPAddress loopback, int port)
		//{
		//	TcpListener server = new TcpListener(loopback, port);
		//	server.Start();
		//	TcpClient socket = server.AcceptTcpClient();
		//	NetworkStream ns = socket.GetStream();
		//	StreamReader reader = new StreamReader(ns);
		//	StreamWriter writer = new StreamWriter(ns);

		//	writer.AutoFlush = true;

		//	string request = reader.ReadLine();

		//	if (request == "password")
		//	{

		//	}
		//	else if (request == "chunk")
		//	{
		//		List<string> chunk = chunks.Take();
		//		var data = JsonSerializer.Serialize(chunk);
		//		writer.WriteLine(data);
		//	}
		//	else
		//	{
		//		writer.WriteLine("ehm");
		//	}

		//}


		internal void ReadDictionaryAndCreateChunks(string filename)
		{
			using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
			using (StreamReader dictionary = new StreamReader(fs))
			{
				int counter = 0;
				List<string> chunk = new List<string>();
				while (!dictionary.EndOfStream)
				{
					String word = dictionary.ReadLine();
					counter++;
					if (counter%10000 != 0)
					{
						chunk.Add(word);
					}
					else
					{
						chunks.Add(chunk);
						chunk = new List<string>();
					}
				}
				chunks.Add(chunk);
				List<string> lastList = new List<string>();
				lastList.Add("last chunk");
				chunks.Add(lastList);
			}

		}
	}
}