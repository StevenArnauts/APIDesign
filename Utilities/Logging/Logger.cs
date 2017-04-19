using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Utilities.Logging {

	public class Sender {

		public string Name { get; set; }
		public int Id { get; set; }

	}

	public interface IAppender {
		void Append(Sender sender, Level level, string line);
	}

	public class TraceAppender : IAppender {

		private readonly object _locker = new object();

		public void Append(Sender sender, Level level, string line) {
			lock(_locker) {
				Trace.WriteLine(string.Format("{0} [{4}] [{1}] {2}: {3}", DateTime.Now.ToString("HH:mm:ss.fff"), sender, level, line, Thread.CurrentThread.GetHashCode()));
			}
		}

	}

	public class ConsoleAppender : IAppender {

		private static readonly ConsoleColor defaultColor;
		private readonly object _locker = new object();

		static ConsoleAppender() {
			defaultColor = Console.ForegroundColor;
		}

		public void Append(Sender sender, Level level, string line) {
			lock(_locker) {
				Console.ForegroundColor = DetermineColorForLevel(level);
				Console.WriteLine("{0} [{4,2}|{5,8}|{2,5}] {1} : {3}", DateTime.Now.ToString("HH:mm:ss.fff"), sender.Name, level.ToString().ToUpper(), line, Thread.CurrentThread.GetHashCode(), sender.Id);
				Console.ResetColor();
			}
		}

		private ConsoleColor DetermineColorForLevel(Level level) {
			switch(level) {
				case Level.Info:
					return ( ConsoleColor.White );
				case Level.Warn:
					return ( ConsoleColor.Yellow );
				case Level.Error:
					return ( ConsoleColor.Red );
				default:
					return ( defaultColor );
			}
		}

	}

	public class FileAppender : IAppender, IDisposable {

		private readonly object _locker = new object();
		private readonly string _file;
		private FileStream _stream;
		private TextWriter _writer;

		public FileAppender(string file) {
			this._file = file;
		}

		public void Append(Sender sender, Level level, string line) {
			lock(_locker) {
				EnsureFile();
				this._writer.WriteLine("{0} [{4}] [{1}] {2}: {3}", DateTime.Now.ToString("HH:mm:ss.fff"), sender, level, line, Thread.CurrentThread.GetHashCode());
				this._writer.Flush();
			}
		}

		private void EnsureFile() {
			if(this._stream == null) {
				if(!Directory.Exists(Path.GetDirectoryName(this._file))) Directory.CreateDirectory(Path.GetDirectoryName(this._file));
				this._stream = File.Open(this._file, FileMode.Append, FileAccess.Write, FileShare.Read);
				this._writer = new StreamWriter(this._stream);
			}
		}

		public void Dispose() {
			if(this._writer != null) {
				this._writer.Close();
				this._writer = null;
				this._stream = null;
			}
		}

	}

	public enum Level {
		Debug = 1,
		Info = 2,
		Warn = 3,
		Error = 4
	}

	/// <summary>
	/// This maps a type name or a (partial) namespace to a level.
	/// </summary>
	public class Source {

		private static readonly Source root = new Source(string.Empty); // this is the root source, has no name
		private readonly List<Source> _children = new List<Source>(); // the next generation

		public static Source Root {
			get { return (root); }
		}

		public Source(string name) {
			this.Name = name;
		}

		public string Name { get; private set; }
		public Level Level { get; set; }

		public Source Add(string source, Level level) {
			string[] parts = source.Split('.');
			return ( Add(parts, level) );
		}

		public Level Get(string source) {
			string[] parts = source.Split('.');
			return (Get(parts));
		}

		private Level Get(string[] parts) {
			Source child = this._children.FirstOrDefault(c => c.Name == parts[0]);
			if (child == null) return (this.Level);
			if (parts.Length > 1) {
				return (child.Get(parts.Take(parts.Length - 1).ToArray()));
			} else {
				return (child.Level);
			}
		}

		private Source Add(string[] parts, Level level) {
			Source child;
			if (!this._children.Any(c => c.Name == parts[0])) {
				child = new Source(parts[0]);
				this._children.Add(child);
			} else {
				child = this._children.First(c => c.Name == parts[0]);
			}
			if (parts.Count() > 1) {
				child.Add(parts.Take(parts.Count() - 1).ToArray(), level);
			} else {
				child.Level = level;
			}
			return (child);
		}

		

	}

	public static class Logger {

		public static Level Level {
			get { return (Source.Root.Level); }
			set { Source.Root.Level = value; }
		}

		public static List<IAppender> Appenders = new List<IAppender>();

		public static void Add(string logger, Level level) {
			Source.Root.Add(logger, level);
		}

		public static Level GetLevel(string source) {
			return (Source.Root.Get(source));
		}

		public static void Debug(Func<string> message) {
			Log(message, Level.Debug);
		}

		public static void Debug(object sender, Func<string> message) {
			Log(sender, message, Level.Debug);
		}

		public static void Debug(object sender, string message) {
			Log(sender, message, Level.Debug);
		}

		public static void Debug(string message) {
			Log(message, Level.Debug);
		}

		public static void Info(Func<string> message) {
			Log(message, Level.Info);
		}

		public static void Info(string message) {
			Log(message, Level.Info);
		}

		public static void Info(object source, Func<string> message) {
			Log(message, Level.Info);
		}

		public static void Info(object source, string message) {
			Log(source, message, Level.Info);
		}

		public static void Warn(Func<string> message) {
			Log(message, Level.Warn);
		}

		public static void Warn(string message) {
			Log(message, Level.Warn);
		}

		public static void Warn(object source, string message) {
			Log(source, message, Level.Warn);
		}

		public static void Error(Func<string> message) {
			Log(message, Level.Error);
		}

		public static void Error(object source, string message) {
			Log(source, message, Level.Error);
		}

		public static void Error(object source, Exception exception) {
			Log(source, FormatException(exception), Level.Error);
		}

		public static void Error(Exception exception) {
			Log(() => FormatException(exception), Level.Error);
		}

		public static void Error(Func<string> message, Exception exception) {
			Log(() => message + Environment.NewLine + FormatException(exception), Level.Error);
		}

		public static void Error(object sender, string message, Exception exception) {
			Log(sender, message + Environment.NewLine + FormatException(exception), Level.Error);
		}

		public static void Log(object sender, Func<string> message, Level level) {
			if(IsLevelEnabled(sender, level)) WriteLine(sender, message, level);
		}

		public static void Log(Func<string> message, Level level) {
			object sender = GetSender();
			if(IsLevelEnabled(sender, level)) WriteLine(sender, message, level);
		}

		public static void Log(object sender, string message, Level level) {
			if(IsLevelEnabled(sender, level)) WriteLine(sender, message, level);
		}

		public static void Log(string message, Level level) {
			object sender = GetSender();
			if(IsLevelEnabled(sender, level)) WriteLine(sender, message, level);
		}

		private static void WriteLine(object source, Func<string> message, Level level) {
			foreach(IAppender appender in Appenders) appender.Append(CreateSender(source), level, message.Invoke());
		}

		private static Sender CreateSender(object source) {
			Sender sender = new Sender { Id = source.GetHashCode(), Name = FormatSender(source) };
			return sender;
		}

		private static void WriteLine(object source, string message, Level level) {
			foreach(IAppender appender in Appenders) appender.Append(CreateSender(source), level, message);
		}

		private static bool IsLevelEnabled(object sender, Level level) {
			string source = GetLoggerName(sender);
			return (level >= GetLevel(source));
		}

		private static string GetLoggerName(object sender) {
			if(sender is Type) {
				return ( ( (Type)sender ).FullName );
			} else if(sender is string) {
				return ( (string)sender );
			} else {
				return ( sender.GetType().FullName );
			}
		}

		/// <summary>
		/// Formats the sender
		/// </summary>
		/// <returns>The caller of the log functionality</returns>
		private static string FormatSender(object sender) {
			if(sender is Type) {
				return ( ( (Type)sender ).Name );
			} else if(sender is string) {
				return ( (string)sender );
			} else {
				return ( sender.GetType().Name );
			}
		}

		/// <summary>
		/// Returns the calling method, i.e. the first method that does not belong to the
		/// Logger class.
		/// </summary>
		/// <returns>The caller of the log functionality</returns>
		private static Type GetSender() {
			StackTrace t = new StackTrace();
			for(int i = 0; i < t.FrameCount; i++) {
				MethodBase m = t.GetFrame(i).GetMethod();
				if(m.ReflectedType != typeof(Logger)) {
					return m.ReflectedType;
				}
			}
			return null;
		}

		/// <summary>
		/// Format an exception to a nicely readable string representation.
		/// </summary>
		/// <param name="ex">The exception data.</param>
		/// <returns>A formatted string of the exception.</returns>
		public static string FormatException(Exception ex) {
			Exception e = ex;
			StringBuilder bldr = new StringBuilder();
			while(e != null) {
				bldr.Append(string.Format("Type: {0}", e.GetType()) + Environment.NewLine);
				bldr.Append(string.Format("Message: {0}", e.Message) + Environment.NewLine);
				bldr.Append("Stack Trace: " + Environment.NewLine);
				bldr.Append(e.StackTrace);
				e = e.InnerException;
				if(e != null) {
					bldr.Append(Environment.NewLine);
					bldr.Append("Caused by: " + Environment.NewLine);
				}
			}
			return ( bldr.ToString() );
		}

	}

}
