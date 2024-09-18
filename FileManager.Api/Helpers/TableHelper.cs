
using System.Globalization;

namespace FileManager.Api.Helpers;

public static class TableHelper
{
    public static object? GetDefaultValue(TypeCode typeCode) => typeCode switch
    {
        TypeCode.Boolean => default(bool),
        TypeCode.Byte => default(byte),
        TypeCode.Char => default(char),
        TypeCode.DateTime => default(DateTime),
        TypeCode.Decimal => default(decimal),
        TypeCode.Double => default(double),
        TypeCode.Int16 => default(short),
        TypeCode.Int32 => default(int),
        TypeCode.Int64 => default(long),
        TypeCode.SByte => default(sbyte),
        TypeCode.Single => default(float),
        TypeCode.String => " ",
        TypeCode.UInt16 => default(ushort),
        TypeCode.UInt32 => default(uint),
        TypeCode.UInt64 => default(ulong),
        _ => null
    };

    public static object GetValue(Type type, string field) => Type.GetTypeCode(type) switch
    {
        TypeCode.String => field,
        TypeCode.Int32 => int.Parse(field, CultureInfo.InvariantCulture),
        TypeCode.Decimal => decimal.Parse(field, CultureInfo.InvariantCulture),
        TypeCode.Boolean => bool.Parse(field),
        TypeCode.DateTime => DateTime.Parse(field, CultureInfo.InvariantCulture),
        _ => "Tipo não suportado"
    };
}