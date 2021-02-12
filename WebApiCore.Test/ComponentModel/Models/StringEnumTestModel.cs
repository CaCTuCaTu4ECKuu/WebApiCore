using System;

namespace WebApiCore.ComponentModel.Test.Models
{
    public sealed class StringEnumTestModel : StringEnum<StringEnumTestModel>, IEquatable<StringEnumTestModel>
    {
        public const int ENUMS_COUNT = 5;

        public static StringEnumTestModel A = RegisterPossibleValue("a");

        public static StringEnumTestModel B = RegisterPossibleValue("b");

        public static StringEnumTestModel C = RegisterPossibleValue("c");

        public static StringEnumTestModel X = RegisterPossibleValue("X");

        public static StringEnumTestModel OwO = RegisterPossibleValue("OwO");

        private StringEnumTestModel(string name) : base(name)
        { }

        private StringEnumTestModel(string[] names) : base(names)
        { }
    }
}
