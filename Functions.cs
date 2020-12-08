using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static (int index1, int index2) FindSum(this IList<int> arr, int target)
        {
            foreach (var t in arr.Combinations())
                if (arr[t[0]] + arr[t[1]] == target)
                    return (t[0], t[1]);

            return (-1, -1);
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

        public static IEnumerable<int> Accumulate(this IEnumerable<int> arr)
        {
            var sum = 0;
            foreach (var i in arr)
            {
                sum += i;
                yield return sum;
            }
        }

        #endregion
    }

    public static class StringOperations
    {
        public static IEnumerable<string> RemoveBlankLines(this IEnumerable<string> strs)
        {
             return strs.Where(s => !string.IsNullOrWhiteSpace(s));
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

        public static int[] ReadAllInts(this string fileName)
        {
            var str = File.ReadAllLines(fileName);
            return str.Select(int.Parse).ToArray();
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
