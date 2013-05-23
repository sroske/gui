using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Diagnostics;

namespace SpintronicsGUI
{
    public class Logger
    {
        public enum LogLevel
        {
            Trivial,
            Debug,
            Error
        };

        private const string EventSourceName = "Application";
        private const string EventLogName = "Spintronics";

        public static void Log( string message, LogLevel level = LogLevel.Debug )
        {
            bool sourceExists = false;
            try
            {
                sourceExists = EventLog.SourceExists(EventSourceName);
            }
            catch (SecurityException e)
            {
                sourceExists = false;
            }
            if (!sourceExists)
            {
                EventLog.CreateEventSource(EventSourceName, EventLogName);
            }

            string formattedMessage = string.Format( "{0}: '{1}'", GetStringForLogLevel(level), message );
            
            //Console.WriteLine( formattedMessage );
            
            System.Diagnostics.Debug.WriteLine( formattedMessage );

            EventLog.WriteEntry(EventSourceName, formattedMessage);
        }

        private static string GetStringForLogLevel( LogLevel level )
        {
            switch (level)
            {
                    case LogLevel.Debug:
                        return "DEBUG";
                    case LogLevel.Trivial:
                        return "TRIVIAL";
                    case LogLevel.Error:
                        return "ERROR";
            }
            return "Unknown";
        }

        public static void Trivial( string message)
        {
            Log( message, LogLevel.Trivial);
        }
        public static void Debug( string message )
        {
            Log( message, LogLevel.Debug );
        }
        public static void Error(string message)
        {
            Log( message, LogLevel.Error );
        }
    }
}
