CREATE DATABASE WebClientDB;

USE [WebClientDB]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 11/5/2015 8:34:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserName] [nvarchar](128) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[GmailUser] [nvarchar](128) NULL,
	[GmailPassword] [nvarchar](128) NULL,
 CONSTRAINT [PK_WCUsers] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


