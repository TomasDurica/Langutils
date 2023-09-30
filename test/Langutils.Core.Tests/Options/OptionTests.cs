using Langutils.Core.Options;
using Langutils.Core.Tests.Asserts;

namespace Langutils.Core.Tests.Options;

public class OptionTests
{
    private static readonly object Value = new();
    private static readonly Option<object> Some = Option.Some(Value);
    private static readonly Option<object> None = Option.None<object>();

    [Fact]
    public void IsSome_OnSome_ReturnsTrue()
        => Assert.True(Some.IsSome);

    [Fact]
    public void IsSome_OnNone_ReturnsFalse()
        => Assert.False(None.IsSome);

    [Fact]
    public void IsNone_OnSome_ReturnsFalse()
        => Assert.False(Some.IsNone);

    [Fact]
    public void IsNone_OnNone_ReturnsTrue()
        => Assert.True(None.IsNone);

    [Fact]
    public void ImplicitCast_NotNull_ReturnsSome()
    {
        var value = new object();
        Option<object> option = value;

        AssertOption.Some(value, option);
    }

    [Fact]
    public void ImplicitCast_Null_ReturnsNone()
    {
        Option<object> option = null;

        AssertOption.None(option);
    }

    [Fact]
    public void ImplicitCast_None_ReturnsNone()
    {
        Option<object> option = Core.Options.None.Instance;

        AssertOption.None(option);
    }
}