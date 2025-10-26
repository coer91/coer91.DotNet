﻿using Microsoft.AspNetCore.JsonPatch;
using System.Data;
using System.Globalization;
using System.Text;

namespace coer91.Tools
{
    public static class Strings
    {  
        /// <summary>
        /// Removes extra spaces between words
        /// </summary> 
        public static string CleanUpBlanks(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string[] words = value.Split(' ');
            words = words.Where(x => x.Length > 0).ToArray();
            return string.Join(' ', words);
        }


        /// <summary>
        /// 
        /// </summary> 
        public static string FirstCharToUpper(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return $"{char.ToUpper(value[0]) + value[1..]}";
        }


        /// <summary>
        /// 
        /// </summary>
        public static string FirstCharToLower(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return $"{char.ToLower(value[0]) + value[1..]}";
        }


        /// <summary>
        /// Word word word => wordWordWord
        /// </summary>
        public static string ToCamelCase(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string[] worlds = value.Split(' ');
            worlds = worlds.Where(x => x.Length > 0).Select(x => x.FirstCharToUpper()).ToArray();
            value = string.Join("", worlds);
            return value.FirstCharToLower();
        }


        /// <summary>
        /// Word word word => WordWordWord
        /// </summary>
        public static string ToPascalCase(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            string[] worlds = value.Split(' ');
            worlds = worlds.Where(x => x.Length > 0).Select(x => x.FirstCharToUpper()).ToArray();
            value = string.Join("", worlds);
            return value.FirstCharToUpper();
        }


        /// <summary>
        /// Word word word => word_word_word
        /// </summary>
        public static string ToSnakeCase(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.CleanUpBlanks();
            value = value.Replace(" ", "_");
            return value.ToLower();
        }


        /// <summary>
        /// Word word word => WORD_WORD_WORD
        /// </summary>
        public static string ToScreamingSnakeCase(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.CleanUpBlanks();
            value = value.Replace(" ", "_");
            return value.ToUpper();
        }


        /// <summary>
        /// Word word word => word-word-word
        /// </summary>
        public static string ToKebabCase(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.CleanUpBlanks();
            value = value.Replace(" ", "-");
            return value.ToLower();
        }


        /// <summary>
        /// 
        /// </summary>
        public static string RemoveLastChar(string value)
        {
            if (value is null)
                return null;

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            value = value.TrimEnd();
            return value.Length > 0 ? value[..^1] : string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        public static string RemoveAccents(string value)
        {
            string textForm = value.Normalize(NormalizationForm.FormD);

            StringBuilder textWithoutAccents = new();
            foreach (char character in textForm)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                    textWithoutAccents.Append(character);
            }

            value = textWithoutAccents.ToString().Normalize(NormalizationForm.FormC);
            return value;
        }


        #region ToString

        public static string ToString(string value)
            => value is null ? string.Empty : value;


        public static string ToString(JsonPatchDocument patch)
            => string.Join(", ", patch.Operations.ToArray().Select(x => new { x.op, x.path, x.value }));


        public static string ToString(DataRow dataRow, int column)
            => dataRow[column].ToString();

        #endregion 
    }
}