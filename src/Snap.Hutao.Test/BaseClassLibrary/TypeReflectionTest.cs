using System;

namespace Snap.Hutao.Test.BaseClassLibrary;

[TestClass]
public sealed class TypeReflectionTest
{
    [TestMethod]
    public void TypeCodeOfEnumIsUserlyingTypeTypeCode()
    {
        Assert.AreEqual(TypeCode.Int32, Type.GetTypeCode(typeof(TestEnum)));
    }

    private enum TestEnum
    {
        A,
        B,
    }
}