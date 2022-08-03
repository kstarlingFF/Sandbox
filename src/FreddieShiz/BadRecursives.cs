using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FreddieShiz;
public class BadRecursives
{
    public static T ReadRecursively<T>(T data)
    {
        if (data is null)
            return data;

        foreach (PropertyInfo property in data.GetType().GetProperties())
        {
            var val = property.GetValue(data);
            if (val is null)
                continue;
            
            var propName = property.Name;
            if (propName.Contains("IncomeAsOfDate"))
            {
                val = MungeData(val);
                property.SetValue(data, val);
                Console.WriteLine($"{property.Name} :: {val}");

            }
            if (val is IEnumerable && val is not string)
            {
                IList collection = (IList)val;
                foreach (var v in collection)
                {
                    ReadRecursively(v);
                    continue;
                }
            }
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                ReadRecursively(val);
                continue;
            }
        }
        return data;
    }

    public static T MungeData<T>(T data)
    {
        if (IsDateTime(data))
           return ReturnType<T>(AddTime(data));
        return data;
    }

    // Using T here to preserve type.
    // DateTime or DateTime?
    private static T AddTime<T>(T data)
    {
        if (DateTime.TryParse(data.ToString(), out DateTime dt))
        {
            dt = dt.AddMonths(5);
            return ReturnType<T>(dt.ToString());
        }
        return data;
    }

    public static bool IsDateTime(object data)
    {
        if(data is DateTime)
        {
            return true;
        }
        var dataString = data.ToString();
        var reg = "^\\d{4}-\\d{2}-\\d{2}$";
        if (Regex.IsMatch(dataString, reg))
        {
            if (DateTime.TryParse(data.ToString(), out DateTime parsed))
            {
                if (parsed is DateTime? || parsed is DateTime)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public static T ReturnType<T>(object data)
    {
        try
        {
            return (T)Convert.ChangeType(data, typeof(T));
        }
        catch(Exception e)
        {
            throw new Exception("You done fucked up!", e);
        }
    }
}

public class SomeData
{
    public string StringData { get; set; }

    public NestedData  NestedData {get; set;}


}

public class NestedData
{
    public int IntData { get; set; }
}


