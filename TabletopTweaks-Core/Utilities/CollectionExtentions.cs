using System;
using System.Collections.Generic;
using System.Linq;

namespace TabletopTweaks.Core.Utilities {
    /// <summary>
    /// Collection of extentions for interacting with normal C# collections.
    /// </summary>
    public static class CollectionExtentions {
        /// <summary>
        /// Sets the value of the specified key in the dictonary if no key currently exists. 
        /// </summary>
        /// <typeparam name="K">
        /// Type of Key in IDictionary
        /// </typeparam>
        /// <typeparam name="V">
        /// Type of Value in IDictionary
        /// </typeparam>
        /// <param name="self"></param>
        /// <param name="key">
        /// Key to add value at if missing.
        /// </param>
        /// <param name="value">
        /// Value to insert at key if the key is not present in the dictonary.
        /// </param>
        /// <returns>
        /// If the key already existed returns value of the key, otherwise returns the newly added value.
        /// </returns>
        public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, V value) where V : class {
            V oldValue;
            if (!self.TryGetValue(key, out oldValue)) {
                self.Add(key, value);
                return value;
            }
            return oldValue;
        }
        /// <summary>
        /// Sets the value of the specified key in the dictonary if no key currently exists. 
        /// </summary>
        /// <typeparam name="K">
        /// Type of Key in IDictionary
        /// </typeparam>
        /// <typeparam name="V">
        /// Type of Value in IDictionary
        /// </typeparam>
        /// <param name="self"></param>
        /// <param name="key">
        /// Key to add value at if missing.
        /// </param>
        /// <param name="ifAbsent">
        /// Function that returns the value to insert at key if they key is not present in the dictonary.
        /// </param>
        /// <returns>
        /// If the key already existed returns value of the key, otherwise returns the newly added value.
        /// </returns>
        public static V PutIfAbsent<K, V>(this IDictionary<K, V> self, K key, Func<V> ifAbsent) where V : class {
            V value;
            if (!self.TryGetValue(key, out value)) {
                self.Add(key, value = ifAbsent());
                return value;
            }
            return value;
        }
        /// <summary>
        /// Creates a new array equal to the old array with a new value appended to the end of it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">
        /// Value to add to the array.
        /// </param>
        /// <returns>
        /// New array with the additional value appended to the end of it.
        /// </returns>
        public static T[] AppendToArray<T>(this T[] array, T value) {
            var len = array?.Length ?? 0;
            var result = new T[len + 1];
            if (len > 0) {
                Array.Copy(array, result, len);
            }
            result[len] = value;
            return result;
        }
        /// <summary>
        /// Creates a new array equal to the old array with the new values appended to the end of it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="values">
        /// Values to add to the array.
        /// </param>
        /// <returns>
        /// New array with the additional values appended to the end of it.
        /// </returns>
        public static T[] AppendToArray<T>(this T[] array, params T[] values) {
            var len = array.Length;
            var valueLen = values.Length;
            var result = new T[len + valueLen];
            Array.Copy(array, result, len);
            Array.Copy(values, 0, result, len, valueLen);
            return result;
        }
        /// <summary>
        /// Creates a new array equal to the old array with the new values appended to the end of it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="values">
        /// Values to add to the array.
        /// </param>
        /// <returns>
        /// New array with the additional values appended to the end of it.
        /// </returns>
        public static T[] AppendToArray<T>(this T[] array, IEnumerable<T> values) => AppendToArray(array, values.ToArray());
        /// <summary>
        /// Creates a new array equal to the old array with values of type V removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// Type of the original array.
        /// <typeparam name="V">
        /// Type to remove from the original array.
        /// </typeparam>
        /// <param name="array"></param>
        /// <returns>
        /// New array with all values of the specified type removed.
        /// </returns>
        public static T[] RemoveFromArrayByType<T, V>(this T[] array) {
            List<T> list = new List<T>();

            foreach (var c in array) {
                if (!(c is V)) {
                    list.Add(c);
                }
            }

            return list.ToArray();
        }
        /// <summary>
        /// Creates a new array equal to the old array with the new value inserted before the first occurance of the specified element. 
        /// Throws a ConstraintException if the target element does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">
        /// Value to be inserted into the array.
        /// </param>
        /// <param name="element">
        /// Element for the new value to be inserted before.
        /// </param>
        /// <returns>
        /// New array with the specified value inserted before the target element.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the target element does not exist.
        /// </exception>
        public static T[] InsertBeforeElement<T>(this T[] array, T value, T element) {
            var len = array.Length;
            var result = new T[len + 1];
            int x = 0;
            bool added = false;
            for (int i = 0; i < len; i++) {
                if (array[i].Equals(element) && !added) {
                    result[x++] = value;
                    added = true;
                }
                result[x++] = array[i];
            }
            if (added == false) {
                throw new ArgumentException("Element does not exist in array.");
            }
            return result;
        }
        /// <summary>
        /// Creates a new array equal to the old array with the new value inserted after the first occurance of the specified element. 
        /// Throws a ConstraintException if the target element does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">
        /// Value to be inserted into the array.
        /// </param>
        /// <param name="element">
        /// Element for the new value to be inserted after.
        /// </param>
        /// <returns>
        /// New array with the specified value inserted after the target element.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the target element does not exist.
        /// </exception>
        public static T[] InsertAfterElement<T>(this T[] array, T value, T element) {
            var len = array.Length;
            var result = new T[len + 1];
            int x = 0;
            bool added = false;
            for (int i = 0; i < len; i++) {
                if (array[i].Equals(element) && !added) {
                    result[x++] = array[i];
                    result[x++] = value;
                    added = true;
                } else {
                    result[x++] = array[i];
                }

            }
            if (added == false) {
                throw new ArgumentException("Element does not exist in array.");
            }
            return result;
        }
        /// <summary>
        /// Creates a new array equal to the old array with the target value removed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value">
        /// Value to remove from the array.
        /// </param>
        /// <returns>
        /// New array with the target value removed.
        /// </returns>
        public static T[] RemoveFromArray<T>(this T[] array, T value) {
            var list = array.ToList();
            return list.Remove(value) ? list.ToArray() : array;
        }
    }
}
