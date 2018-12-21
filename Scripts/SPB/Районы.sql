USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Районы]    Script Date: 21.12.2018 22:56:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Районы](
	[Ид] [int] NOT NULL,
	[Наименование] [nvarchar](max) NULL,
 CONSTRAINT [PK_Районы] PRIMARY KEY CLUSTERED 
(
	[Ид] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


