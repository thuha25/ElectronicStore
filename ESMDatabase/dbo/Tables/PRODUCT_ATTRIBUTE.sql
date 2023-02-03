﻿CREATE TABLE [dbo].[PRODUCT_ATTRIBUTE]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(-2147483648, 1), 
    [ProductID] INT NOT NULL, 
    [AttributeID] INT NOT NULL, 
    [Value] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_PRODUCT_ATTRIBUTE_PRODUCT] FOREIGN KEY ([ProductID]) REFERENCES [PRODUCT]([Id]), 
    CONSTRAINT [FK_PRODUCT_ATTRIBUTE_ATTRIBUTE] FOREIGN KEY ([AttributeID]) REFERENCES [ATTRIBUTE]([Id])
)
