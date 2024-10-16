using Dynamicweb.Updates;

namespace ExpressDelivery.Updates;

public sealed class ExpressDeliveryUpdateProvider : UpdateProvider
{
    public override IEnumerable<Update> GetUpdates() => new List<Update>
    {
        new SqlUpdate("1", this, @"
            CREATE TABLE [dbo].[ExpressDeliveryPreset](
                [ExpressDeliveryPresetId] [bigint] IDENTITY(1,1) NOT NULL,
                [ExpressDeliveryPresetName] [nvarchar](255) NULL,
                [ExpressDeliveryPresetHours] [int] NULL,
                [ExpressDeliveryPresetOverHalfWayText] [nvarchar](255) NULL,
                [ExpressDeliveryPresetUnderHalfWayText] [nvarchar](255) NULL,
                [ExpressDeliveryPresetTooLateText] [nvarchar](255) NULL,
            CONSTRAINT [DW_PK_ExpressDeliveryPreset] PRIMARY KEY CLUSTERED
            (
                [ExpressDeliveryPresetId] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
        "),
        new SqlUpdate("2", this, @"
            CREATE TABLE [dbo].[ExpressDeliveryPresetRelation](
                [ExpressDeliveryPresetRelationOrderId] [nvarchar](255) NOT NULL,
                [ExpressDeliveryPresetRelationExpressDeliveryPresetId] [bigint] NOT NULL,
            CONSTRAINT [DW_PK_ExpressDeliveryPresetRelation] PRIMARY KEY CLUSTERED
            (
                [ExpressDeliveryPresetRelationOrderId] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
        "),
        new SqlUpdate("3", this, @"
            ALTER TABLE [dbo].[ExpressDeliveryPresetRelation]  WITH CHECK ADD  CONSTRAINT [FK_ExpressDeliveryPresetRelation_ExpressDeliveryPreset] FOREIGN KEY([ExpressDeliveryPresetRelationExpressDeliveryPresetId])
            REFERENCES [dbo].[ExpressDeliveryPreset] ([ExpressDeliveryPresetId])
            ON UPDATE CASCADE
            ON DELETE CASCADE
        ")
    };
}
