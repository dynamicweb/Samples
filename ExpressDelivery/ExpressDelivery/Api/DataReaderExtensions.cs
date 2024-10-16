using System.Data;

namespace ExpressDelivery.Api;

internal static class DataReaderExtensions
{

    internal static T? GetValue<T>(this IDataReader reader, string columnName)
    {
        ArgumentNullException.ThrowIfNull(reader);

        var ordinal = reader.GetOrdinal(columnName);
        var result = default(Is<T>) switch
        {
            Is<long> => reader.GetInt64(ordinal),
            Is<int> => reader.GetInt32(ordinal),
            Is<string> => reader.GetString(ordinal),
            Is<bool> => reader.GetBoolean(ordinal),
            Is<DateTime> => reader.GetDateTime(ordinal),
            _ => reader.GetValue(ordinal)
        };

        return result is null ? default : (T)result;
    }

    private struct Is<T>;
}
