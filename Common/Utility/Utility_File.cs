using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Utility
{
    public static class Utility_File
    {
        #region Identity
        public const String ClassName = nameof(Utility_File);
        #endregion

        #region Search
        /// <summary>
        /// This method will return an array of filenames of the files located in the given directory.
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <returns></returns>
        public static String[] GetAllNamedFilesOfTypeToArray(string saveDirectory, string extension)
        {
            return GetAllNamedFilesOfType(saveDirectory, extension).ToArray();
        }

        /// <summary>
        /// This method will return a collection of filenames of the files located in the given directory.
        /// </summary>
        /// <param name="saveDirectory"></param>
        /// <returns></returns>
        public static ICollection<String> GetAllNamedFilesOfType(string saveDirectory, string extension)
        {
            ICollection<string> allNames = new List<string>();
            IEnumerable<string> allFiles;
            try
            {
                allFiles = Directory.EnumerateFiles(saveDirectory, extension);
                if (allFiles.Count() > 0)
                {
                    foreach (string file in allFiles)
                    {
                        string nameSansExt = Path.GetFileName(file).Split('.')[0];
                        allNames.Add(nameSansExt);
                    }
                }
            }
            catch (DirectoryNotFoundException) { } // If the directroy doesn't exist then the files it contains must also not.
            return allNames;
        }
        #endregion /Search

        #region Validation
        /// <summary>
        /// This method checks to see if the file name is valid
        /// </summary>
        /// <param name="testName"></param>
        /// <returns></returns>
        public static bool IsValidFilename(string testName)
        {
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]");
            if (containsABadCharacter.IsMatch(testName) || testName.Contains('.'))
            {
                return false;
            }
            return true;
        }
        #endregion /Validation


    }
}
