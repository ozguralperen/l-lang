
using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace llang.Core.Server
{
    public class RequesterData
    {
        public string HostID;
        public DateTime Time;
        public string DocumentID;
    }

    class ServeHTTP
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static bool isAccepted;
        public static int requestCount = 0;

        public static string UniqueID = "";

        public static string HTML = "";
        public static string OpenHTML =
            "<!DOCTYPE>" +
            "<html>" +
            "  <head>" +
            "    <title>llang Document</title>" +
            "  </head>" +
            "  <body>";

        public static string CloseHTML =
            "    <p> [llang] IsAccepted? : {0}</p>" +
            "    <form method=\"post\" action=\"accept\">" +
            "      <input type=\"submit\" value=\"Accept\" {1}>" +
            "    </form>" +
            "  </body>" +
            "</html>";


        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;
            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;


                Console.WriteLine("[OK] Received any request #{0} :", ++requestCount);
                Console.WriteLine("\t" + req.Url.ToString());
                Console.WriteLine("\t" + req.HttpMethod);
                Console.WriteLine("\t" + req.UserHostName);
                Console.WriteLine("\t" + req.UserAgent);
                Console.WriteLine();


                // If Accepted , Add Database Who is accepted (ip , port) and change acceptance flag.

                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/accept"))
                {
                    var userData = new RequesterData() { HostID = req.UserHostAddress, Time = DateTime.Now , DocumentID = UniqueID };


                    using (var streamWriter = new System.IO.StreamWriter("AcceptData.xml"))
                    {
                        var xmlSerializer = new System.Xml.Serialization.XmlSerializer(userData.GetType());
                        xmlSerializer.Serialize(streamWriter, userData);
                    }

                    Console.WriteLine("[OK] Your document gets positive response.");
                    Console.WriteLine("---- Serving Ended. ---");
                    runServer = false;
                }

                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(String.Format(HTML, isAccepted, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }


        public static void StartHTTP()
        {

            // Document Serving To Anybody

            // First , read the output file genereted by llang compiler.
            // After than we will serve llang document to server.
            // Generate unique identifier to every file for database.
            // If the other party opens the site for  approve the document or sign the contract , (GET)
            // and clicks the button ->  (POST) Okey i signed / Okey i approved. 
            // Server Close and Generates [Successfully Responsed] message.

            string file = File.ReadAllText("../../Out.html").Replace("\r\n", " ");

            StringBuilder builder = new StringBuilder();

            Enumerable
                .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));

            UniqueID = builder.ToString();
            string id = "<span> File ID : " + UniqueID + " </span>";


            HTML = OpenHTML + id +  file + CloseHTML;


            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("[OK] Listening connections for your document on server. {0}", url);

            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();

            listener.Close();
        }
    }
}