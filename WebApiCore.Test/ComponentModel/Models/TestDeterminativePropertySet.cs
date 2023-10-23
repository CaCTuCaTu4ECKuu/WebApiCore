using System;
using System.Text.Json;
using Newtonsoft.Json;

namespace WebApiCore.ComponentModel.Test.Models
{
    public class TestDeterminativePropertySet : DeterminativePropertySet<TestDeterminativePropertySet>
    {
        public string PropertyA
        {
            get => Get<string>(e => e.PropertyA);
            set => Set(value, e => e.PropertyA);
        }

        public int? PropertyB0
        {
            get => Get<int?>(e => e.PropertyB0);
            set => Set(value, e => e.PropertyB0);
        }

        [JsonProperty("prop_b")]
        public int PropertyB
        {
            get => Get<int>(e => e.PropertyB);
            set => Set(value, e => e.PropertyB);
        }

        public DateTime PropertyC
        {
            get => Get<DateTime>(e => e.PropertyC);
            set => Set(value, e => e.PropertyC);
        }
    }
}
