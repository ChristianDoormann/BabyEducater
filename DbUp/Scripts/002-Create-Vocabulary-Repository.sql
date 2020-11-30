CREATE TABLE [Vocabulary] (
    [BabyId] uniqueidentifier NOT NULL,
    [Word] nvarchar(256) NOT NULL,
    CONSTRAINT [PK_Vocabulary.BabyWord] PRIMARY KEY CLUSTERED (BabyId, Word) WITH (IGNORE_DUP_KEY = ON)
)