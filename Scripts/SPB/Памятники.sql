USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Памятники]    Script Date: 21.12.2018 22:55:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Памятники](
	[Ид] [int] NOT NULL,
	[Название] [nvarchar](max) NULL,
	[Описание] [nvarchar](max) NULL,
	[Ид_адреса] [int] NOT NULL,
 CONSTRAINT [PK_Памятники] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Памятники]  WITH CHECK ADD  CONSTRAINT [FK_Музеи_Памятники] FOREIGN KEY([Ид_адреса])
REFERENCES [dbo].[Адреса] ([Ид])
GO

ALTER TABLE [dbo].[Памятники] CHECK CONSTRAINT [FK_Музеи_Памятники]
GO


