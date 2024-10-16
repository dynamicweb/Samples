using Dynamicweb.Caching;
using Dynamicweb.Data;
using System.Collections.Concurrent;
using System.Data;

namespace ExpressDelivery.Api;

internal static class ExpressDeliveryPresetService
{
    private const string CacheKeyFormat = "ExpressDeliveryPreset_{0}";
    private static readonly ConcurrentDictionary<string, long> OrderAndPresetMap = new();

    public static ExpressDeliveryPreset? GetExpressDeliveryById(long id)
    {
        var key = string.Format(CacheKeyFormat, id);
        if (Cache.Current.TryGet<ExpressDeliveryPreset>(key, out var preset))
            return preset;

        preset = GetEntity(CommandBuilder.Create("SELECT * FROM [ExpressDeliveryPreset] WHERE [ExpressDeliveryPresetId] = {0}", id));

        if (preset is not null)
            Cache.Current.Set(key, preset);

        return preset;
    }


    public static ExpressDeliveryPreset? GetExpressDeliveryPresetByOrderId(string orderId)
    {
        if (OrderAndPresetMap.TryGetValue(orderId, out var presetId))
            return GetExpressDeliveryById(presetId);

        var sql = CommandBuilder.Create("""
            SELECT [ExpressDeliveryPresetRelationExpressDeliveryPresetId] FROM [ExpressDeliveryPresetRelation]
            WHERE [ExpressDeliveryPresetRelationOrderId] = {0}
            """, orderId);
        presetId = Database.ExecuteScalar(sql) is long id ? id : 0;

        if (presetId > 0)
            OrderAndPresetMap.TryAdd(orderId, presetId);

        return GetExpressDeliveryById(presetId);
    }

    public static IEnumerable<ExpressDeliveryPreset> GetExpressDeliveries() =>
        GetEntities(CommandBuilder.Create("SELECT * FROM [ExpressDeliveryPreset]"));

    public static bool Save(ExpressDeliveryPreset preset)
    {
        var sql = new CommandBuilder();
        sql.Add("MERGE ExpressDeliveryPreset WITH (SERIALIZABLE) AS T");
        sql.Add("USING (VALUES ");
        sql.Add("   ({0},{1},{2},{3},{4},{5})", preset.Id, preset.Name, preset.Hours, preset.OverHalfWayText, preset.UnderHalfWayText, preset.TooLateText);
        sql.Add(") AS S (ExpressDeliveryPresetId, ExpressDeliveryPresetName, ExpressDeliveryPresetHours, ExpressDeliveryPresetUnderHalfWayText, ExpressDeliveryPresetOverHalfWayText, ExpressDeliveryPresetTooLateText)");
        sql.Add("   ON S.ExpressDeliveryPresetId = T.ExpressDeliveryPresetId");
        sql.Add("WHEN MATCHED THEN");
        sql.Add("   UPDATE SET T.ExpressDeliveryPresetName = S.ExpressDeliveryPresetName, T.ExpressDeliveryPresetHours = S.ExpressDeliveryPresetHours, T.ExpressDeliveryPresetUnderHalfWayText = S.ExpressDeliveryPresetUnderHalfWayText, T.ExpressDeliveryPresetOverHalfWayText = S.ExpressDeliveryPresetOverHalfWayText, T.ExpressDeliveryPresetTooLateText = S.ExpressDeliveryPresetTooLateText");
        sql.Add("WHEN NOT MATCHED THEN");
        sql.Add("   INSERT (ExpressDeliveryPresetName, ExpressDeliveryPresetHours, ExpressDeliveryPresetUnderHalfWayText, ExpressDeliveryPresetOverHalfWayText, ExpressDeliveryPresetTooLateText) VALUES (S.ExpressDeliveryPresetName, S.ExpressDeliveryPresetHours, S.ExpressDeliveryPresetUnderHalfWayText, S.ExpressDeliveryPresetOverHalfWayText, S.ExpressDeliveryPresetTooLateText)");
        sql.Add("OUTPUT INSERTED.ExpressDeliveryPresetId;");

        long identity = 0;
        try
        {
            identity = Database.ExecuteScalar(sql) is long id ? id : 0;
            if (identity != preset.Id)
            {
                if (preset.Id > 0)
                    ClearCache(preset.Id);
                preset.Id = identity;
            }
        }
        catch { }

        return identity > 0;
    }

    public static bool Delete(long presetId)
    {
        try
        {
            var didDelete = Database.ExecuteNonQuery(CommandBuilder.Create("DELETE FROM ExpressDeliveryPreset WHERE ExpressDeliveryPresetId = {0}", presetId)) > 0;
            
            if (didDelete)
                ClearCache(presetId);

            return didDelete;
        }
        catch
        {
            return false;
        }
    }

    public static bool AttachOrUpdatePreset(string orderId, long presetId)
    {
        CommandBuilder sql = new();

        sql.Add("MERGE ExpressDeliveryPresetRelation WITH (SERIALIZABLE) AS T");
        sql.Add("USING (VALUES ");
        sql.Add("   ({0},{1})", orderId, presetId);
        sql.Add(") AS S (ExpressDeliveryPresetRelationOrderId, ExpressDeliveryPresetRelationExpressDeliveryPresetId)");
        sql.Add("   ON S.ExpressDeliveryPresetRelationOrderId = T.ExpressDeliveryPresetRelationOrderId");
        sql.Add("WHEN MATCHED THEN");
        sql.Add("   UPDATE SET T.ExpressDeliveryPresetRelationExpressDeliveryPresetId = S.ExpressDeliveryPresetRelationExpressDeliveryPresetId");
        sql.Add("WHEN NOT MATCHED THEN");
        sql.Add("   INSERT (ExpressDeliveryPresetRelationOrderId, ExpressDeliveryPresetRelationExpressDeliveryPresetId) VALUES (S.ExpressDeliveryPresetRelationOrderId, S.ExpressDeliveryPresetRelationExpressDeliveryPresetId);");

        int affectedRows = 0;
        try
        {
            affectedRows = Database.ExecuteNonQuery(sql);
            if (affectedRows > 0)
                OrderAndPresetMap.AddOrUpdate(orderId, presetId, (_, _) => presetId);
        }
        catch { }

        return affectedRows > 0;
    }

    public static bool DetachPreset(string orderId)
    {
        try
        {
            OrderAndPresetMap.TryRemove(orderId, out _);
            return Database.ExecuteNonQuery(CommandBuilder.Create("DELETE FROM ExpressDeliveryPresetRelation WHERE ExpressDeliveryPresetRelationOrderId = {0}", orderId)) > 0;
        }
        catch
        {
            return false;
        }
    }

    private static ExpressDeliveryPreset? GetEntity(CommandBuilder cb)
    {
        var reader = Database.CreateDataReader(cb);
        if (reader.Read())
            return MapData(reader);
        return null;
    }

    private static IEnumerable<ExpressDeliveryPreset> GetEntities(CommandBuilder cb)
    {
        var reader = Database.CreateDataReader(cb);
        while (reader.Read())
            yield return MapData(reader);
    }

    private static ExpressDeliveryPreset MapData(IDataReader reader) => new()
    {
        Id = reader.GetValue<long>("ExpressDeliveryPresetId"),
        Name = reader.GetValue<string>("ExpressDeliveryPresetName"),
        Hours = reader.GetValue<int>("ExpressDeliveryPresetHours"),
        UnderHalfWayText = reader.GetValue<string>("ExpressDeliveryPresetUnderHalfWayText"),
        OverHalfWayText = reader.GetValue<string>("ExpressDeliveryPresetOverHalfWayText"),
        TooLateText = reader.GetValue<string>("ExpressDeliveryPresetTooLateText"),
    };

    private static void ClearCache(long id)
    {
        var key = string.Format(CacheKeyFormat, id);
        Cache.Current.Remove(key);

        // TODO: Find a better way to clear the cache
        foreach (var (orderId, presetId) in OrderAndPresetMap)
        {
            if (presetId == id)
            {
                OrderAndPresetMap.TryRemove(orderId, out _);
                break;
            }
        }
    }
}
