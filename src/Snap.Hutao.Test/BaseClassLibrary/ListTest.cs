using System.Collections.Generic;

namespace Snap.Hutao.Test.BaseClassLibrary;

[TestClass]
public sealed class ListTest
{
    [TestMethod]
    public void IndexOfNullIsNegativeOne()
    {
        List<object> list = [new()];
        Assert.AreEqual(-1, list.IndexOf(default!));
    }

    [TestMethod]
    public void StructElementMultipleIndexOfTest()
    {
        List<int> list = [1, 1, 1, 1];
        Assert.AreEqual(0, list.IndexOf(1));
        Assert.IsTrue(Equals(list[0], list[1]));

        // 这里就是要验证值类型装箱后 ReferenceEquals 恒为 false，不能换成 Equals
#pragma warning disable CA2013
        Assert.IsFalse(ReferenceEquals(list[0], list[0]));
        Assert.IsFalse(ReferenceEquals(list[0], list[1]));
#pragma warning restore CA2013
    }
}