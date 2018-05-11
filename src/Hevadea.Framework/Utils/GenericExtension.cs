﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Hevadea.Framework.Utils
{
    public static class GenericExtension
    {
        public static IList<T> Clone<T>(this IList<T> listToClone)
        {
            var result = new List<T>();

            foreach (var item in listToClone)
            {
                result.Add(item);
            }

            return result;
        }

        public static byte WriteBit(this byte value, byte selectedBit, bool bit)
        {
            if (bit)
            {
                return (byte)(value | (1 << selectedBit));
            }

            return (byte)(value & ~(1 << selectedBit));
        }

        public static int WriteBit(this int value, byte selectedBit, bool bit)
        {
            if (bit)
            {
                return (value | (1 << selectedBit));
            }

            return (value & ~(1 << selectedBit));
        }

        public static bool ReadBit(this byte value, int selectedBit)
        {
            return (value & (1 << selectedBit)) != 0;
        }

        public static bool ReadBit(this int value, int selectedBit)
        {
            return (value & (1 << selectedBit)) != 0;
        }

        public static void SaveToBin(object obj, string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static T LoadFromBin<T>(string path)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            T obj = (T)formatter.Deserialize(stream);
            stream.Close();

            return obj;
        }

        // Fast Random utils ---------------------------------------------------

        public static float NextFloat(this FastRandom rnd, float max)
        {
            return (float)rnd.NextDouble() * max;
        }

        public static float NextFloat(this FastRandom rnd)
        {
            return (float)rnd.NextDouble();
        }

        public static float NextFloatRange(this FastRandom rnd, float max)
        {
            return (rnd.NextFloat() - 0.5f) * 2f * max;
        }

        public static float NextFloatRange(this FastRandom rnd)
        {
            return (rnd.NextFloat() - 0.5f) * 2f;
        }

        public static T NextValue<T>(this FastRandom rnd, params T[] values)
        {
            return values[rnd.Next(values.Length)];
        }

        public static void Shuffle<T>(this FastRandom rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        // Random utils --------------------------------------------------------

        public static float NextFloat(this Random rnd, float max)
        {
            return (float)rnd.NextDouble() * max;
        }

        public static float NextFloat(this Random rnd)
        {
            return (float)rnd.NextDouble();
        }

        public static float NextFloatRange(this Random rnd, float max)
        {
            return (rnd.NextFloat() - 0.5f) * 2f * max;
        }

        public static float NextFloatRange(this Random rnd)
        {
            return (rnd.NextFloat() - 0.5f) * 2f;
        }

        public static T NextValue<T>(this Random rnd, params T[] values)
        {
            return values[rnd.Next(values.Length)];
        }

        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}