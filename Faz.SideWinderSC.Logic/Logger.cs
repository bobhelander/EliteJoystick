using System.Diagnostics;

namespace Faz.SideWinderSC.Logic
{
    /// <summary>
    /// Provides the available trace sources and the extension methods for
    /// the <see cref="TraceSource"/> class.
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// The default trace source.
        /// </summary>
        public readonly static TraceSource Default = new TraceSource("SideWinderSC", SourceLevels.Information);

        /// <summary>
        /// Logs a message with a <see cref="TraceEventType.Verbose"/> level.
        /// </summary>
        /// <param name="source">The associated trace.</param>
        /// <param name="format">The message or the format string.</param>
        /// <param name="args">The arguments of the format string.</param>
        public static void Verbose(this TraceSource source, string format, params object[] args)
        {
            source.TraceEvent(TraceEventType.Verbose, 0, format, args);
        }

        /// <summary>
        /// Checks whether a trace source logs <see cref="TraceEventType.Verbose"/> messages.
        /// </summary>
        /// <param name="source">The associated trace.</param>
        /// <returns>
        /// <c>true</c> if Verbose message are displayed by the <paramref name="source"/>;
        /// <c>false</c> otherwise.
        /// </returns>
        public static bool IsVerbose(this TraceSource source)
        {
            return source.Switch.Level <= SourceLevels.Verbose;
        }
    }
}
