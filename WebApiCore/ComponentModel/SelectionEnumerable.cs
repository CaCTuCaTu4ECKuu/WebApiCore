using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebApiCore.ComponentModel
{
    [DebuggerDisplay("[{Items.Length}] <- {TotalCount}")]
    public class SelectionEnumerable<T> : IEnumerable<T>
    {
        private T[] _items;

        /// <summary>
        /// Total count of items
        /// </summary>
        [JsonPropertyName("total")]
        [JsonProperty("total")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Items of selection
        /// </summary>
        [JsonPropertyName("items")]
        [JsonProperty("items")]
        public T[] Items
        {
            get => _items ?? (_items = Array.Empty<T>());
            set => _items = value;
        }

        public SelectionEnumerable(IEnumerable<T> items, int totalCount)
        {
            TotalCount = totalCount;
            Items = items.ToArray();
        }

        public SelectionEnumerable(IEnumerable<T> items)
        {
            TotalCount = items.Count();
            Items = items.ToArray();
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
