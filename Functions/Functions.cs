using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Functions
{
    public static class Functions
    {
        #region Methods

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> create)
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, create.Invoke());
            return dict[key];
        }

        public static (List<int> indeces, List<int> values) FindSum(this IList<int> arr, int target, int count = 2)
        {
            foreach (var t in arr.Combinations(count))
                if (t.Sum(i => arr[i]) == target)
                    return (t, t.Select(j => arr[j]).ToList());

            return (null, null);
        }

        public static (List<int> indeces, List<long> values) FindSum(this IList<long> arr, long target, int count = 2)
        {
            foreach (var t in arr.Combinations(count))
                if (t.Sum(i => arr[i]) == target)
                    return (t, t.Select(j => arr[j]).ToList());

            return (null, null);
        }

        public static (List<long> values, int index1, int index2) FindContiguousSum(this IList<long> arr, long target,
            int count = 2)
        {
            for (var j = 0; j < arr.Count - count; j++)
            {
                var l = arr.Skip(j).Take(count).ToList();
                if (l.Sum() == target)
                    return (l, j, j + count);
            }

            return (null, -1, -1);
        }

        public static (List<int> values, int index1, int index2) FindContiguousSum(this IList<int> arr, int target,
            int count = 2)
        {
            for (var j = 0; j < arr.Count - count; j++)
            {
                var l = arr.Skip(j).Take(count).ToList();
                if (l.Sum() == target)
                    return (l, j, j + count);
            }

            return (null, -1, -1);
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
        #region Fields

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

        #endregion

        #region Methods

        public static bool IsVowel(this char c)
        {
            return LowerVowels.Contains(char.ToLower(c));
        }

        public static bool IsConsonant(this char c)
        {
            return LowerConsonants.Contains(char.ToLower(c));
        }

        #endregion
    }

    public static class EnumerableExtensions
    {
        public static int IndexOfFirst<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        {
            var l = vals.ToList();
            for (int i = 0; i < l.Count; i++)
            {
                if (predicate(l[i]))
                    return i;
            }

            return -1;
        }
        public static int IndexOfLast<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        {
            var l = vals.ToList();
            for (int i = l.Count - 1; i >= 0; i--)
            {
                if (predicate(l[i]))
                    return i;
            }

            return -1;
        }
        // public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        // {
        //     var en = vals.GetEnumerator();
        //     while (en.MoveNext())
        //     {
        //         if (predicate.Invoke(en.Current))
        //             yield return en.Current;
        //         else break;
        //     }
        //     en.Dispose();
        // }
        public static IEnumerable<T> SkipFromEndWhile<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        {
            var valList = vals.ToList();
            var skipped = false;
            for (int i = valList.Count - 1; i >= 0; i--)
            {
                if (!skipped && predicate(valList[i]))
                    continue;
                skipped = true;
                yield return valList[i];
            }
        }
        public static IEnumerable<T> TakeFromEndWhile<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        {
            var valList = vals.ToList();
            for (int i = valList.Count - 1; i >= 0; i--)
            {
                if (predicate(valList[i]))
                    yield return valList[i];
            }
        }
        public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> vals, Func<T, bool> predicate)
        {
            var en = vals.GetEnumerator();
            while (en.MoveNext() && predicate.Invoke(en.Current))
            {
            }

            while (en.MoveNext())
                yield return en.Current;
            en.Dispose();
        }
    }
    public static class StringOperations
    {
        #region Methods

        public static string RemoveQuotes(this string s)
        {
            return s.TrimStart('\"').TrimEnd('\"');
        }
        
        public static IEnumerable<string> RemoveBlankLines(this IEnumerable<string> strs)
        {
            return strs.Where(s => !string.IsNullOrWhiteSpace(s));
        }

        public static IEnumerable<T> SplitToType<T>(this string str, string separator = "x")
        {
            var bits = str.Split(separator);
            return bits.Select(b => (T) Convert.ChangeType(b, typeof(T)));
        }

        public static string ToHexString(this byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        #endregion
    }

    public static class FileOperations
    {
        #region Methods

        public static Board ReadBoard(this string filename)
        {
            var read = File.ReadAllLines(filename);
            var board = new Board(read[0].Length, read.Length);
            for (var y = 0; y < read.Length; y++)
            {
                var s = read[y];
                for (var x = 0; x < s.Length && x < board.Width; x++)
                {
                    var ch = read[y][x];
                    board.SetValueAt(x, y, ch);
                }

            }

            return board;
        }
        /// <summary>
        ///     key: value
        ///     key: value
        ///     or other separator
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
                yield return ((T1) Convert.ChangeType(bits[0], typeof(T1)),
                    (T2) Convert.ChangeType(bits[1], typeof(T2)));
            }
        }

        public static T[] ReadAll<T>(this string fileName)
        {
            var str = File.ReadAllLines(fileName);
            return str.Select(s => (T) Convert.ChangeType(s, typeof(T))).ToArray();
        }
        public static IEnumerable<T> ReadAll<T>(this IEnumerable<string> strings)
        {
            return strings.Select(s => (T) Convert.ChangeType(s, typeof(T)));
        }


        /// <summary>
        ///     Like this:
        ///     A record
        ///     A record
        ///     A record
        ///     A record
        ///     A record
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

        public static IEnumerable<string> SplitLines(this string str, string lineEnding = "\r\n")
        {
            return str.Split(lineEnding, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion
    }
}