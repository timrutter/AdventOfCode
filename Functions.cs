using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class Functions
    {
        #region Methods

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> create)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key,create.Invoke());
            return dict[key];

        }
        public static (List<int> indeces, List<int> values) FindSum(this IList<int> arr, int target, int count = 2)
        {
            foreach (var t in arr.Combinations(count))
            {
                if (t.Sum(i => arr[i]) == target)
                    return (t, t.Select(j => arr[j]).ToList());
            }

            return (null,null);
        }
        public static (List<int> indeces, List<long> values) FindSum(this IList<long> arr, long target, int count = 2)
        {
            foreach (var t in arr.Combinations(count))
            {
                if (t.Sum(i => arr[i]) == target)
                    return (t, t.Select(j => arr[j]).ToList());
            }

            return (null,null);
        }
        public static (List<long> values, int index1, int index2) FindContiguousSum(this IList<long> arr, long target, int count = 2)
        {
            for (int j = 0; j < arr.Count - count; j++)
            {
                var l = arr.Skip(j).Take(count).ToList();
                if (l.Sum() == target)
                    return (l,j,j+count);
            }

            return (null,-1,-1);
        }
        public static (List<int> values, int index1, int index2) FindContiguousSum(this IList<int> arr, int target, int count = 2)
        {
            for (int j = 0; j < arr.Count - count; j++)
            {
                var l = arr.Skip(j).Take(count).ToList();
                if (l.Sum() == target)
                    return (l,j,j+count);
            }

            return (null,-1,-1);
        }

        public static IEnumerable<List<int>> Combinations(this IList<int> arr, int count = 2)
        {
            var indeces = Enumerable.Range(0, count).ToList();

            IEnumerable<List<int>> Increment(int index, int start)
            {
                if (index == count) yield break;
                for (indeces[index] = start; indeces[index] < arr.Count; indeces[index]++)
                {
                    if (index == count - 1)
                        yield return indeces.ToList();
                    foreach (var inc in Increment(index + 1, indeces[index] + 1))
                        yield return inc;
                }
            }

            foreach (var inc in Increment(0, 0))
                yield return inc;
        }

        public static IEnumerable<List<int>> Combinations(this IList<long> arr, int count = 2)
        {
            var indeces = Enumerable.Range(0, count).ToList();

            IEnumerable<List<int>> Increment(int index, int start)
            {
                if (index == count) yield break;
                for (indeces[index] = start; indeces[index] < arr.Count; indeces[index]++)
                {
                    if (index == count - 1)
                        yield return indeces.ToList();
                    foreach (var inc in Increment(index + 1, indeces[index] + 1))
                        yield return inc;
                }
            }

            foreach (var inc in Increment(0, 0))
                yield return inc;
        }

        public static IEnumerable<int> Accumulate(this IEnumerable<int> arr)
        {
            var sum = 0;
            foreach (var i in arr)
            {
                sum += i;
                yield return sum;
            }
        }
        public static IEnumerable<long> Accumulate(this IEnumerable<long> arr)
        {
            long sum = 0;
            foreach (var i in arr)
            {
                sum += i;
                yield return sum;
            }
        }

        #endregion
    }

    public static class Characters
    {
        public const string LowerAlphabet = "abcdefghijklmnopqrstuvwxyz";
        public const string UpperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string UpperAndLowerAlphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string LowerVowels = "aeiou";
        public const string UpperVowels = "AEIOU";
        public const string UpperAndLowerVowels = "aeiouAEIOU";
        public const string LowerConsonants = "bcdfghjklmnpqrstvwxyz";
        public const string UpperConsonants = "BCDFGHJKLMNPQRSTVWXYZ";
        public const string UpperAndLowerConsonants = "bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ";
        public const string DecimalNumbers = "0123456789";

        public static bool IsVowel(this char c)
        {
            return LowerVowels.Contains(char.ToLower(c));
        }
        public static bool IsConsonant(this char c)
        {
            return LowerConsonants.Contains(char.ToLower(c));
        }

    }
    public static class StringOperations
    {
        public static IEnumerable<string> RemoveBlankLines(this IEnumerable<string> strs)
        {
             return strs.Where(s => !string.IsNullOrWhiteSpace(s));
        }

        public static IEnumerable<T> SplitToType<T>(this string str, string separator)
        {
            var bits = str.Split("x");
            return bits.Select(b => (T)Convert.ChangeType(b, typeof(T)));
        }
        
        public static string ToHexString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
    public static class FileOperations
    {
        /// <summary>
        /// key: value
        /// key: value
        ///
        /// or other separator
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static IEnumerable<(string key, string value)> ReadAllKeyValuePairs(this string fileName,
            string separator = ":")
        {
            var strings = File.ReadAllLines(fileName);
            foreach (var s in strings)
            {
                var bits = s.Split(separator);
                yield return (bits[0], bits[1]);
            }
        }
        public static IEnumerable<(T1 key, T2 value)> ReadAllKeyValuePairs<T1, T2>(this string fileName,
            string separator = ":")
        {
            var strings = File.ReadAllLines(fileName);
            foreach (var s in strings)
            {
                var bits = s.Split(separator);
                yield return ((T1)Convert.ChangeType(bits[0],typeof(T1)) , (T2)Convert.ChangeType(bits[1],typeof(T2)));
            }
        }

        public static T[] ReadAll<T>(this string fileName)
        {
            var str = File.ReadAllLines(fileName);
            return str.Select(s =>(T) Convert.ChangeType(s,typeof(T))).ToArray();
        }

        
        /// <summary>
        /// Like this:
        /// A record
        /// 
        /// A record
        /// A record
        /// A record
        ///
        /// A record
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="collapseNewLines"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadAllBlankLineSeparatedRecords(this string fileName,
            bool collapseNewLines = false)
        {
            var lines = File.ReadAllLines(fileName);
            var record = "";
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    record += collapseNewLines ? $" {line}" : $"{line}\r\n";
                    continue;
                }

                yield return record;
                record = "";
            }

            yield return record;
        } 
        public static string RemoveAllWhiteSpace(this string str)
        {
            return Regex.Replace(str, @"\s+", "");
        }
        public static IEnumerable<string> SplitLines(this string str, string lineEnding ="\r\n")
        {
            return str.Split(lineEnding, StringSplitOptions.RemoveEmptyEntries);
            
        }
    }
}
