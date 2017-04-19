USE [Orders]
GO

DROP TABLE [dbo].[Payment];
DROP TABLE [dbo].[Invoice];
DROP TABLE [dbo].[Shipment];
DROP TABLE [dbo].[OrderLine];
DROP TABLE [dbo].[Order];
DROP TABLE [dbo].[OrderState];
DROP TABLE [dbo].[Customer];
DROP TABLE [dbo].[Claim];
DROP TABLE [dbo].[User];
DROP TABLE [dbo].[Delivery];
DROP TABLE [dbo].[BackOrder];
DROP TABLE [dbo].[Product];
GO

CREATE TABLE [dbo].[Product](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](255) NOT NULL,
	[Price] [decimal](10,2) NOT NULL,
	[Stock] [int] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[Claim](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](255) NOT NULL,
	[Value] [nvarchar](255) NULL,
	[UserFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Claim] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Claim_User] FOREIGN KEY ([UserFk]) REFERENCES [User](Id)
)
GO

CREATE TABLE [dbo].[Customer](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](255) NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Customer] PRIMARY KEY ([Id])
)
GO

CREATE TABLE [dbo].[OrderState](
	[Name] [nvarchar](20) NOT NULL,
	CONSTRAINT [PK_OrderState] PRIMARY KEY ([Name])
)
GO

INSERT INTO [OrderState] ([Name]) VALUES ('Created');
INSERT INTO [OrderState] ([Name]) VALUES ('Confirmed');
INSERT INTO [OrderState] ([Name]) VALUES ('Paid');
INSERT INTO [OrderState] ([Name]) VALUES ('Shipped');

CREATE TABLE [dbo].[Order](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Description] [nvarchar](max) NULL,
	[Date] [datetimeoffset](7) NOT NULL,
	[Amount] [decimal](10,2) NOT NULL,
	[State] [nvarchar](20) NOT NULL,
	[CustomerFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Order] PRIMARY KEY ([Id]),
	CONSTRAINT [PK_Order_State] FOREIGN KEY ([State]) REFERENCES [OrderState]([Name]),
	CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerFk]) REFERENCES [Customer](Id)
)
GO

CREATE TABLE [dbo].[BackOrder](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Date] [datetimeoffset](7) NOT NULL,
	[State] [int] NOT NULL,
	[ProductFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_BackOrder] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_BackOrder_Product] FOREIGN KEY ([ProductFk]) REFERENCES [Product](Id)
)
GO

CREATE TABLE [dbo].[Delivery](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Date] [datetimeoffset](7) NOT NULL,
	[BackOrderFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Delivery] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Delivery_BackOrder] FOREIGN KEY ([BackOrderFk]) REFERENCES [BackOrder](Id)
)
GO

CREATE TABLE [dbo].[InvoiceState](
	[Name] [nvarchar](20) NOT NULL,
	CONSTRAINT [PK_InvoiceState] PRIMARY KEY ([Name])
)
GO

INSERT INTO [InvoiceState] ([Name]) VALUES ('Created');
INSERT INTO [InvoiceState] ([Name]) VALUES ('Sent');
INSERT INTO [InvoiceState] ([Name]) VALUES ('Paid');

CREATE TABLE [dbo].[Invoice](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Date] [datetimeoffset](7) NOT NULL,
	[State] [nvarchar](20) NOT NULL,
	[OrderFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Invoice] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Invoice_Order] FOREIGN KEY ([OrderFk]) REFERENCES [Order](Id),
	CONSTRAINT [FK_Invoice_InvoiceState] FOREIGN KEY ([State]) REFERENCES [InvoiceState](Name)
)
GO

CREATE TABLE [dbo].[Payment](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Date] [datetimeoffset](7) NOT NULL,
	[Amount] [decimal](10,2) NOT NULL,
	[InvoiceFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Payment] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Payment_Invoice] FOREIGN KEY ([InvoiceFk]) REFERENCES [Invoice](Id)
)
GO

CREATE TABLE [dbo].[Shipment](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Date] [datetimeoffset](7) NOT NULL,
	[OrderFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_Shipment] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Shipment_Order] FOREIGN KEY ([OrderFk]) REFERENCES [Order](Id)
)
GO

CREATE TABLE [dbo].[OrderLine](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Amount] [decimal](10,2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderFk] [uniqueidentifier] NOT NULL,
	[ProductFk] [uniqueidentifier] NOT NULL,
	[CreatedBy][nvarchar](255) NOT NULL,
	[CreatedOn][datetimeoffset](7) NOT NULL,
	[ModifiedBy][nvarchar](255) NOT NULL,
	[ModifiedOn][datetimeoffset](7) NOT NULL,
	CONSTRAINT [PK_OrderLine] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_OrderLine_Order] FOREIGN KEY ([OrderFk]) REFERENCES [Order](Id),
	CONSTRAINT [FK_OrderLine_Product] FOREIGN KEY ([ProductFk]) REFERENCES [Product](Id)
)
GO

INSERT INTO [dbo].[Product] ([Id], [Name], [Price], [Stock], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Sheep', 19.99, 5, 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Product] ([Id], [Name], [Price], [Stock], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Cat', 1.99, 3, 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Product] ([Id], [Name], [Price], [Stock], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Dog', 79.49, 1, 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Product] ([Id], [Name], [Price], [Stock], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Fish', 0.49, 20, 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());

INSERT INTO [dbo].[Customer] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Jan', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Customer] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Piet', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Customer] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Joris', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[Customer] ([Id], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Corneel', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());

INSERT INTO [dbo].[User] ([Id], [Name], [Email], [Password], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'John', 'john@bringme.com', 'john', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
INSERT INTO [dbo].[User] ([Id], [Name], [Email], [Password], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn]) VALUES (NEWID(),'Mia', 'mia@bringme.com', 'mia', 'SYSTEM', SYSDATETIMEOFFSET(), 'SYSTEM', SYSDATETIMEOFFSET());
GO

