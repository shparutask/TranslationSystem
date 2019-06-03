USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Адреса]    Script Date: 21.12.2018 22:55:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Адреса](
	[Ид] [int] NOT NULL,
	[Улица] [nvarchar](max) NULL,
	[Дом] [int] NULL,
	[Ид_района] [int] NOT NULL,
 CONSTRAINT [PK_Адреса] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Адреса]  WITH CHECK ADD  CONSTRAINT [FK_Адреса_Районы] FOREIGN KEY([Ид_района])
REFERENCES [dbo].[Районы] ([Ид])
GO

ALTER TABLE [dbo].[Адреса] CHECK CONSTRAINT [FK_Адреса_Районы]
GO


