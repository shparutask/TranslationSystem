USE [���_���������������������]
GO

/****** Object:  Table [dbo].[�������]    Script Date: 21.12.2018 22:56:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[�������](
	[��] [int] NOT NULL,
	[��������] [nvarchar](max) NULL,
	[��������] [nvarchar](max) NULL,
	[��_������] [int] NOT NULL,
 CONSTRAINT [PK_�������] PRIMARY KEY CLUSTERED 
(
	[��] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[�������]  WITH CHECK ADD  CONSTRAINT [FK_������_�������] FOREIGN KEY([��_������])
REFERENCES [dbo].[������] ([��])
GO

ALTER TABLE [dbo].[�������] CHECK CONSTRAINT [FK_������_�������]
GO


