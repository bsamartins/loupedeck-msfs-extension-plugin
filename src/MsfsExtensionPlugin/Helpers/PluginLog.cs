namespace Loupedeck.MsfsExtensionPlugin
{
    using System;
    using System.Runtime.Serialization;

    internal static class PluginLog
    {
        private static PluginLogFile _pluginLogFile;

        public static void Init(PluginLogFile pluginLogFile)
        {
            pluginLogFile.CheckNullArgument(nameof(pluginLogFile));
            PluginLog._pluginLogFile = pluginLogFile;
        }

        public static void Verbose(String text) => PluginLog._pluginLogFile?.Verbose(format(text));

        public static void Verbose(Exception ex, String text) => PluginLog._pluginLogFile?.Verbose(ex, format(text));

        public static void Info(String text) => PluginLog._pluginLogFile?.Info(format(text));

        public static void Info(Exception ex, String text) => PluginLog._pluginLogFile?.Info(ex, format(text));

        public static void Warning(String text) => PluginLog._pluginLogFile?.Warning(format(text));

        public static void Warning(Exception ex, String text) => PluginLog._pluginLogFile?.Warning(ex, format(text));

        public static void Error(String text) => PluginLog._pluginLogFile?.Error(format(text));

        public static void Error(Exception ex, String text) => PluginLog._pluginLogFile?.Error(ex, format(text));

        public static void WriteOpenLine() => PluginLog._pluginLogFile?.WriteOpenLine();

        public static void WriteCloseLine() => PluginLog._pluginLogFile?.WriteCloseLine();

        private static String format(String text) { 
            var formattedTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff");
            return $"[{formattedTimestamp}] {text}";
        }
    }
}
