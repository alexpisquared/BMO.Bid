using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace BMO.OLP.Common.Common
{
	public class FtpHelper // derived from BBG soxified tezka to work by default with ISSUER RISK
	{
		const string _reqSampl = @"C:\temp\IssuerRiskFileSamples\Request\UEI_TO_ISSUER_RISK_ENTITY_MAPPING_20150507.txt";
		static string _ftpHost, _ftpLogn, _ftpPswd;

		public static void TestFtpComm()
		{
			var rvL = FtpHelper.ListFiles();
			var rvF = FtpHelper.GetFile(@"zTest.Reply", "", "", "");
			var rvS = FtpHelper.UploadFile2(_reqSampl);
			var rvZ = FtpHelper.PutFile(_reqSampl);
		}

		public static FtpStatusCode UploadString(string req, string remoteFile)
		{
			var rv = FtpStatusCode.Undefined;

			if (remoteFile.ToLower().EndsWith(".req") == false && remoteFile.ToLower().EndsWith(".req.enc") == false) remoteFile += ".req";
			if (remoteFile.Length > 25) throw new ArgumentOutOfRangeException("remoteFile", "The request file name (including the four characters used for .req) must not exceed 25 characters."); // if (remoteFile.ToLower().EndsWith(".copied")) throw new ArgumentOutOfRangeException("remoteFile", "The only accepted extensions are .req and .req.enc (see encryption note below). Requests sent in with the .copied extension in the file name will not be processed.");

			try
			{
				byte[] reqContents = Encoding.UTF8.GetBytes(req);

				var request = createGetRequest(string.Format("ftp://{0}/{1}", _ftpHost, remoteFile), WebRequestMethods.Ftp.UploadFile);
				request.ContentLength = reqContents.Length;

				using (var requestStream = request.GetRequestStream())
				{
					requestStream.Write(reqContents, 0, reqContents.Length);
					requestStream.Close();
				}

				using (var response = (FtpWebResponse)request.GetResponse())
				{
					Trace.WriteLine(string.Format("    Upload File Complete, status: {0} - {1}", rv = response.StatusCode, response.StatusDescription));
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return rv;
		}
		public static FtpStatusCode PutFile(string localFilePath)
		{
			var rv = FtpStatusCode.Undefined;
			if (!File.Exists(localFilePath)) throw new ArgumentOutOfRangeException(localFilePath, "!File.Exists");

			var remoteFile = Path.GetFileName(localFilePath);
			if (remoteFile.Length > 25) throw new ArgumentOutOfRangeException("remoteFile", "The request file name (including the four characters used for .req) must not exceed 25 characters.");
			if (remoteFile.ToLower().EndsWith(".copied")) throw new ArgumentOutOfRangeException("remoteFile", "The only accepted extensions are .req and .req.enc (see encryption note below). Requests sent in with the .copied extension in the file name will not be processed.");

			try
			{
				var request = createGetRequest(string.Format("ftp://{0}/{1}", _ftpHost, remoteFile), WebRequestMethods.Ftp.UploadFile);

				using (var sourceStream = new StreamReader(localFilePath))
				{
					byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
					sourceStream.Close();
					request.ContentLength = fileContents.Length;

					using (var requestStream = request.GetRequestStream())
					{
						requestStream.Write(fileContents, 0, fileContents.Length);
						requestStream.Close();
					}
				}

				using (var response = (FtpWebResponse)request.GetResponse())
				{
					Trace.WriteLine(string.Format("    Upload File Complete, status: {0} - {1}", rv = response.StatusCode, response.StatusDescription));
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return rv;
		}
		public static string GetFile(string remoteFile, string ftpHost, string ftpLogn, string ftpPswd)
		{
			_ftpHost = ftpHost;
			_ftpLogn = ftpLogn;
			_ftpPswd = ftpPswd;
			string rv = null;

			if (remoteFile.ToLower().EndsWith(".out") == false) remoteFile += ".out";
			if (remoteFile.Length > 25) throw new ArgumentOutOfRangeException("remoteFile", "The request file name (including the four characters used for .req) must not exceed 25 characters.");
			if (remoteFile.ToLower().EndsWith(".copied")) throw new ArgumentOutOfRangeException("remoteFile", "The only accepted extensions are .req and .req.enc (see encryption note below). Requests sent in with the .copied extension in the file name will not be processed.");

			try
			{
				var request = createGetRequest(string.Format("ftp://{0}/{1}", _ftpHost, remoteFile), WebRequestMethods.Ftp.DownloadFile);

				using (var response = (FtpWebResponse)request.GetResponse())
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var reader = new StreamReader(responseStream))
						{
							rv = reader.ReadToEnd();
							Trace.Write(string.Format("   Download Complete, status: {0} : {1}", response.StatusCode, response.StatusDescription));
						}
					}
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return rv;
		}
		public static string WaitGetFile(string remoteFile, TimeSpan timeout)
		{
			string content = null;

			for (var until = DateTime.Now.Add(timeout); DateTime.Now < until; )
			{
				var files = ListFiles();
				if (files.Contains(remoteFile))
					break;

				System.Threading.Thread.Sleep(10000);
			}

			try
			{
				var request = createGetRequest(string.Format("ftp://{0}/{1}", _ftpHost, remoteFile), WebRequestMethods.Ftp.DownloadFile);

				using (var response = (FtpWebResponse)request.GetResponse())
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var reader = new StreamReader(responseStream))
						{
							content = reader.ReadToEnd();
							Trace.Write(string.Format("   Download Complete, status: {0} : {1}", response.StatusCode, response.StatusDescription));
						}
					}
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return content;
		}
		public static string ListFiles()
		{
			string rv = null;
			try
			{
				var request = createGetRequest(string.Format("ftp://{0}", _ftpHost), WebRequestMethods.Ftp.ListDirectoryDetails);

				using (var response = (FtpWebResponse)request.GetResponse())
				{
					using (var responseStream = response.GetResponseStream())
					{
						using (var reader = new StreamReader(responseStream))
						{
							rv = reader.ReadToEnd();
							Trace.Write(string.Format("   Download Complete, status: {0} : {1}", response.StatusCode, response.StatusDescription));
						}
					}
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return rv;
		}

		static FtpWebRequest createGetRequest(string ftp, string method)
		{
			Trace.Write(string.Format("  {0,-6}{1,-48} ", method, ftp));
			var request = (FtpWebRequest)WebRequest.Create(ftp);
			request.Method = method;
			request.KeepAlive = false;

#if FTP_NEEDED_AND_APPSCAN_APPEASED //todo: if needed - resolve AppScan issues and uncomment (but currently this is not used, thus, left out ...but kept in for future references)
			request.Credentials = new NetworkCredential(_ftpLogn, _ftpPswd);

			if (true) // method != WebRequestMethods.Ftp.UploadFile)
				request.Proxy = null; //doproxy_NOGO(request);//tu: The requested FTP command is not supported when using HTTP proxy
			else
			{
				request.Proxy = WebRequest.DefaultWebProxy; //TU: 1/2
				request.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
			}
#endif

			return request;
		}

		private static WebProxy doproxy_NOGO(FtpWebRequest request)
		{
			WebProxy myProxy = new WebProxy();
			Console.WriteLine("\nThe actual default Proxy settings are {0}", myProxy.Address);
			try
			{
				var proxyAddress = "172.30.40.198:1080";

				Uri newUri = new Uri(proxyAddress);
				myProxy.Address = newUri;
				myProxy.Credentials = CredentialCache.DefaultNetworkCredentials; 
				request.Proxy = myProxy;

				Console.WriteLine("\nThe Address of the  new Proxy settings are {0}", myProxy.Address);
			}
			catch (UriFormatException e) { Console.WriteLine("\nUriFormatException is thrown.Message is {0}", e.Message); }

			return myProxy;
		}

		public static bool UploadFile2(string localFilePath = @"C:\1\Obligor_SOI_Run1.0b.req", string remoteDirectory = @"")
		{
			var proxyHost = new IPAddress[] { IPAddress.Parse("172.30.40.198"), IPAddress.Parse("172.30.40.199"), IPAddress.Parse("172.30.40.200"), IPAddress.Parse("172.21.129.198"), IPAddress.Parse("172.21.129.199"), IPAddress.Parse("172.21.129.200"), IPAddress.Parse("172.21.129.204") };
			var proxyPort = 1080;



			try
			{
				//Socket client = SocksProxy.ConnectToSocks5Proxy("172.30.40.198", 1080, "www.microsoft.com", 80, "U$er", "Pa$$word!");
				//string strGet = "GET //\r\n";
				//client.Send(System.Text.Encoding.ASCII.GetBytes(strGet));



				var fileName = Path.GetFileName(localFilePath);
				string content;
				using (var reader = new StreamReader(localFilePath))
					content = reader.ReadToEnd();

				//var proxyAuthB64Str = Convert.ToBase64String(Encoding.ASCII.GetBytes(_proxyUserName + ":" + _proxyPassword));
				var sendStr = "GET ftp://" + _ftpLogn + ":" + _ftpPswd
					//						+ "@" + _ftpHost + "/" + remoteDirectory + fileName + " HTTP/1.1\n"
						+ "@" + _ftpHost + "/zTest.Reply.out HTTP/1.1\n"
						+ "Host: " + _ftpHost + "\n"
						+ "User-Agent: Mozilla/4.0 (compatible; Eradicator; dotNetClient)\n"
					//+ "Proxy-Authorization: Basic " + proxyAuthB64Str + "\n"
						+ "Content-Type: application/octet-stream\n"
						+ "Content-Length: " + content.Length + "\n"
						+ "Connection: close\n\n" + content;
				;


				sendStr = @"GET ftp://" + _ftpHost + @"/ HTTP/1.1  
     User-Agent: Mozilla/4.0 (compatible; MSIE 8.0)  
     Host: " + _ftpHost + @":1080
     Proxy-Connection: Keep-Alive";

				var sendBytes = Encoding.ASCII.GetBytes(sendStr);

				using (var proxySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
				{
					proxySocket.Connect(proxyHost, proxyPort);
					if (!proxySocket.Connected)
						throw new SocketException();

					Debug.WriteLine("Send = {0}", proxySocket.Send(sendBytes));

					int recvSize = 65536;
					var recvBytes = new byte[recvSize];
					Debug.WriteLine("Rcvd = {0}", proxySocket.Receive(recvBytes, recvSize, SocketFlags.Partial));

					var responseFirstLine = new string(Encoding.ASCII.GetChars(recvBytes)).Split("\n".ToCharArray()).Take(1).ElementAt(0);
					var httpResponseCode = Regex.Replace(responseFirstLine, @"HTTP/1\.\d (\d+) (\w+)", "$1");
					var httpResponseDescription = Regex.Replace(responseFirstLine, @"HTTP/1\.\d (\d+) (\w+)", "$2");
					return httpResponseCode.StartsWith("2");
				}
			}
			catch (Exception ex) { Trace.WriteLine(string.Format("    Exn: {0}", ex.Message)); }
			return false;
		}
	}
}
