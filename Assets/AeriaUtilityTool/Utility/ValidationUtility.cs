using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace AeriaUtil.Validation
{
    /// <summary>
    /// 
    /// TODO: IMPROVEMENT REQUIRED FOR VALIDATION
    /// CREATE VALIDATION RESULT:
    /// -> Include Validity, Item itself, ErrorMessage
    /// -> Either ValidityResult or ValidityException
    /// </summary>
    public static class ValidationUtility
    {
        public static T NotEmpty<T>(this T item, Action<T> onError = null)
        {
            if (item == null ||
                (item is string str && string.IsNullOrWhiteSpace(str)) ||
                (item is ICollection collection && collection.Count == 0))
            {
                onError?.Invoke(item);
                return default;
            }

            return item;
        }
        public static string NotEmpty(this string item, Action<string> onError = null)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                onError?.Invoke("Field is empty!");
                return default;
            }

            return item;
        }

        public static bool NotEmpty<T>(this T item)
        {
            if (item == null ||
                (item is string str && string.IsNullOrWhiteSpace(str)) ||
                (item is ICollection collection && collection.Count == 0))
            {
                return false;
            }

            return true;
        }


        public static string MaxLength(this string item, int length, Action<string> onError = null)
        {
            if (!item.NotEmpty())
            {
                onError?.Invoke("Field is empty!");
                return default;
            }
            else if (item is string str && str.Length > length)
            {
                onError?.Invoke($"Maximum Character Limit: {length}");
                return default;
            }
            return item;
        }

        public static string MinLength(this string item, int length, Action<string> onError = null)
        {
            if (!item.NotEmpty())
            {
                onError?.Invoke("Field is empty!");
                return default;
            }
            else if (item is string str && str.Length < length)
            {
                onError?.Invoke($"Minimum Character Limit: {length}");
                return default;
            }
            return item;
        }

        public static string Matches(this string item, string pattern, Action<string> onError = null)
        {
            if (!item.NotEmpty())
            {
                onError?.Invoke("Field is empty!");
                return default;
            }
            else if (!Regex.IsMatch(item, pattern))
            {
                onError?.Invoke($"Please try again!");
                return default;
            }
            return item;
        }

        /// <summary>
        /// Add this only at the end of validity checks. This checks if the item is in default state or not. 
        /// Only way for a string to be default is invalidated by the validators.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsValid(this string item)
        {
            if (item == default(string)) return false;
            else return true;
        }
    }
}
