using System.IO;

namespace Solidry.Extensions
{
    public static class Assembly
    {
        /// <summary>
        /// Read embedded resource from assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="path">For instance: assemblyname.directory.filename</param>
        /// <returns></returns>
        public static string ReadEmbeddedResource(this System.Reflection.Assembly assembly, string path)
        {
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }
    }
}