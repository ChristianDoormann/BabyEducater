CREATE TABLE [FamilyMembers] (
    [BabyId] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_FamilyMembers] PRIMARY KEY CLUSTERED (BabyId, Name)
)