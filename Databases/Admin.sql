CREATE TABLE [ProductVariant] (
  [Id] int PRIMARY KEY,
  [Product_Id] int,
  [Name] NVARCHAR(100),
  [Slug] NVARCHAR(100),
  [Price] float
)
GO

CREATE TABLE [ProductAttributeValue] (
  [Id] int PRIMARY KEY,
  [Product_Id] int,
  [Product_Variant_Id] int,
  [Attribute_Value_Id] int
)
GO

CREATE TABLE [Product] (
  [Id] int PRIMARY KEY,
  [Category_Id] int,
  [Name] NVARCHAR(100),
  [Slug] NVARCHAR(100),
  [Price_1] float,
  [Price_2] float,
  [Description] NVARCHAR(100),
  [Image_Default] NVARCHAR(100),
  [Status] int
)
GO

CREATE TABLE [Attribute] (
  [Id] int PRIMARY KEY,
  [Name] NVARCHAR(100),
  [Code] NVARCHAR(100)
)
GO

CREATE TABLE [AttributeValue] (
  [Id] int PRIMARY KEY,
  [Attribute_Id] int,
  [Name] NVARCHAR(100)
)
GO

CREATE TABLE [Category] (
  [Id] int PRIMARY KEY,
  [Name] NVARCHAR(100)
)
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([Category_Id]) REFERENCES [Category] ([Id])
GO

ALTER TABLE [ProductVariant] ADD FOREIGN KEY ([Product_Id]) REFERENCES [Product] ([Id])
GO

ALTER TABLE [ProductAttributeValue] ADD FOREIGN KEY ([Product_Variant_Id]) REFERENCES [ProductVariant] ([Id])
GO

ALTER TABLE [ProductAttributeValue] ADD FOREIGN KEY ([Attribute_Value_Id]) REFERENCES [AttributeValue] ([Id])
GO

ALTER TABLE [AttributeValue] ADD FOREIGN KEY ([Attribute_Id]) REFERENCES [Attribute] ([Id])
GO
