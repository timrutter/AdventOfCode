using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Helpers;

namespace AdventOfCode.Advent2016
{
    public class Advent2016Day05 : Solution
    {
        public Advent2016Day05()
        {
            Answer1 = "4543C154";
            Answer2 = "1050CBBD";
        }
        public string CreateMD5Hash(string input)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
                if (sb.Length == 6 && !sb.ToString().StartsWith("00000")) return null;
            }
            return sb.ToString();
        }
        public override object ExecutePart1()
        {
            string input = "ojvtpuvg";
            int index  = 0;
            var password = "";
            while (true)
            {
                var hash  = CreateMD5Hash(input + index);
                if (hash != null)
                {
                    password += hash[5];
                    if (password.Length == 8) return password;
                }

                index++;
            }
        }

        public override object ExecutePart2()
        {
            string input = "ojvtpuvg";
            int index = 0;
            var password = new[]{'-', '-', '-', '-', '-', '-', '-', '-', };
            int found = 0;
            while (true)
            {
                index++;
                var hash = CreateMD5Hash(input + index);
                if (hash != null)
                {
                    if (!int.TryParse(hash[5].ToString(),out var i)) continue;
                    if (i > 7 || password[i] != '-') continue;
                    password[i] = hash[6];
                    if (++found == 8) return new string(password);
                }
            }
        }
    }
}