using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace Loglet
{
    public class Logger : ExceptionLogger, IDisposable
    {
        private static StreamWriter Writer;

        public Logger()
        {
            try
            {
                Directory.CreateDirectory("C:/Apollo Asset Manager/Asset Manager Logs");
                Writer = new StreamWriter($"C:/Apollo Asset Manager/Asset Manager Logs/Log_{DateTime.Now.ToString("dd-MM-yyyy")}.txt", true);

                Writer.WriteLine();
                Writer.WriteLine($"<<<<<<<<<< Asset Manager Log {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} >>>>>>>>>>");
                Writer.WriteLine();
            }
            catch (Exception e)
            {
                Writer = StreamWriter.Null;
                Out("Error creating log file: " + e.Message);
            }
        }

        ~Logger()
        {
            Dispose(false);
        }

        public void UpdateWriter()
        {
            Writer = new StreamWriter($"C:/Apollo Asset Manager/Asset Manager Logs/Log_{DateTime.Now.ToString("dd-MM-yyyy")}.txt", true);

            Writer.WriteLine();
            Writer.WriteLine($"<<<<<<<<<< Asset Manager Log {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} >>>>>>>>>>");
            Writer.WriteLine();
        }

        public void Out(string str = "")
        {
            Trace.WriteLine(str);

            if (str == "")
                Writer.WriteLine(str);
            else
                Writer.WriteLine(string.Format("{0}  {1}", DateTime.Now.ToString("HH:mm:ss:fff"), str));

            Writer.Flush();
        }

        public void Out(object obj)
        {
            Out(JsonConvert.SerializeObject(obj));
        }

        public void Error(string str)
        {
            Out("ERROR: " + str);
        }

        public void Error(string message, string stacktrace)
        {
            string str = $"ERROR: Message: {message} Stack Trace: {stacktrace}";

            Out(str);
        }

        public void Error(Exception ex)
        {
            string str = $"ERROR: Message: {ex.Message} Method: {ex.TargetSite.Name}";

            Out(str);
            Out(ex.StackTrace);
        }

        public override void Log(ExceptionLoggerContext Context)
        {
            //Error(Context.ExceptionContext.Exception.Message, Context.ExceptionContext.Exception.TargetSite.Name);
            Error(Context.ExceptionContext.Exception);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Writer.Flush();
                    Writer.Close();

                    Writer = StreamWriter.Null;

                    //Writer.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}
