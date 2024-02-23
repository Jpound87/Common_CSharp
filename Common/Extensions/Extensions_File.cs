using Common.Constant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Common.Extensions
{
    public static class Extensions_File
    {
        #region Identity
        public const String ClassName = nameof(Extensions_File);
        #endregion

        #region Path

        #region Constants
        public static readonly Char[] InvalidPathChars_Array = Path.GetInvalidFileNameChars();
        public static readonly HashSet<Char> InvalidPathChars_HashSet = InvalidPathChars_Array.ToHashSet();
        #endregion /Constants

        #region Methods

        #region Datam Address Dictionary 
        public static string GetDadPath(string saveName)
        {
            return Path.Combine(Tokens.Path_DatamConfigurationDirectory, $"{saveName}{Tokens.DATAM_CONFIG_EXT}");
        }
        #endregion

        #region Valididty
        public static bool IsValidPath(this String path)
        {
            return !(String.IsNullOrWhiteSpace(path) || path.CheckForInvalidCharacters());
        }

        public static bool IsValidExtension(this String path)
        {
            return !(String.IsNullOrWhiteSpace(path) || path.CheckForInvalidCharacters()) && path[0] == '.';
        }

        public static bool CheckForInvalidCharacters(this String path)
        {
            return path.ListContainedInvalidCharacters().Count() != 0;
        }

        public static IEnumerable<Char> ListContainedInvalidCharacters(this String path)
        {
            return path.Intersect(InvalidPathChars_HashSet);
        }
        #endregion /Validity

        #endregion /Methods

        #endregion /Path
    }
}
