USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Парки]    Script Date: 21.12.2018 22:56:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Парки](
	[Ид] [int] NOT NULL,
	[Название] [nvarchar](max) NULL,
	[Описание] [nvarchar](max) NULL,
	[Час_открытия] [nvarchar](max) NULL,
	[Час_закрытия] [nvarchar](max) NULL,
	[Рабочие_дни] [nvarchar](max) NULL,
	[Ид_адреса] [int] NOT NULL,
 CONSTRAINT [PK_Парки] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Парки]  WITH CHECK ADD  CONSTRAINT [FK_Парки_Адреса] FOREIGN KEY([Ид_адреса])
REFERENCES [dbo].[Адреса] ([Ид])
GO

ALTER TABLE [dbo].[Парки] CHECK CONSTRAINT [FK_Парки_Адреса]
GO


