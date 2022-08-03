namespace WutRTuples;

internal static class Worker
{
    /// <summary>
    /// Gets a tuple with a person's name and age.
    /// </summary>
    /// <returns><see cref="ValueTuple(string, int)"/></returns>
    public static ValueTuple<string, int> GetTuple()
    {
        (string name, int age) t = ("Kelly", 39);

        var x = t.name;
        var y = t.age;

        return t;
    }
}
