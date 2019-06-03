USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Усадьбы]    Script Date: 21.12.2018 22:56:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Усадьбы](
	[Ид] [int] NOT NULL,
	[Название] [nvarchar](max) NULL,
	[Описание] [nvarchar](max) NULL,
	[Ид_адреса] [int] NOT NULL,
 CONSTRAINT [PK_Усадьбы] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Усадьбы]  WITH CHECK ADD  CONSTRAINT [FK_Адреса_Усадьбы] FOREIGN KEY([Ид_адреса])
REFERENCES [dbo].[Адреса] ([Ид])
GO

ALTER TABLE [dbo].[Усадьбы] CHECK CONSTRAINT [FK_Адреса_Усадьбы]
GO


