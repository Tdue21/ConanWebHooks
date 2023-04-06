using Microsoft.Extensions.Primitives;

namespace ConanWebHooks.Models;

public static class StringExtensions
{
    public static short ToInt16(this string text) => (short) (short.TryParse(text, out var value) ? value : 0);
    public static short ToInt16(this StringValues text) => (short)(short.TryParse(text, out var value) ? value : 0);
    
    public static int ToInt32(this string text) => int.TryParse(text, out var value) ? value : 0;
    public static int ToInt32(this StringValues text) => int.TryParse(text, out var value) ? value : 0;
    
    public static long ToInt64(this string text) => long.TryParse(text, out var value) ? value : 0L;
    public static long ToInt64(this StringValues text) => long.TryParse(text, out var value) ? value : 0L;

    public static DateTime ToDateTime(this string text) => DateTime.TryParse(text, out var value) ? value : DateTime.Now;
    public static DateTime ToDateTime(this StringValues text) => DateTime.TryParse(text, out var value) ? value : DateTime.Now;
}