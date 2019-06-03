USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Музеи]    Script Date: 21.12.2018 22:55:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Музеи](
	[Ид] [int] NOT NULL,
	[Название] [nvarchar](max) NULL,
	[Час_открытия] [time](7) NULL,
	[Час_закрытия] [time](7) NULL,
	[Рабочие_дни] [nvarchar](max) NULL,
	[Ид_адреса] [int] NOT NULL,
 CONSTRAINT [PK_Музеи] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Музеи]  WITH CHECK ADD  CONSTRAINT [FK_Адреса_Музеи] FOREIGN KEY([Ид_адреса])
REFERENCES [dbo].[Адреса] ([Ид])
GO

ALTER TABLE [dbo].[Музеи] CHECK CONSTRAINT [FK_Адреса_Музеи]
GO


