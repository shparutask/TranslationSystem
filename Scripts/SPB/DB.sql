USE [master]
GO

/****** Object:  Database [СПб_Достопримечательности]    Script Date: 21.12.2018 22:54:32 ******/
CREATE DATABASE [СПб_Достопримечательности]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'СПб_Достопримечательности', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\СПб_Достопримечательности.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'СПб_Достопримечательности_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\СПб_Достопримечательности_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [СПб_Достопримечательности] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [СПб_Достопримечательности].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [СПб_Достопримечательности] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET ARITHABORT OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [СПб_Достопримечательности] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [СПб_Достопримечательности] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET  DISABLE_BROKER 
GO

ALTER DATABASE [СПб_Достопримечательности] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [СПб_Достопримечательности] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [СПб_Достопримечательности] SET  MULTI_USER 
GO

ALTER DATABASE [СПб_Достопримечательности] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [СПб_Достопримечательности] SET DB_CHAINING OFF 
GO

ALTER DATABASE [СПб_Достопримечательности] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [СПб_Достопримечательности] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [СПб_Достопримечательности] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [СПб_Достопримечательности] SET  READ_WRITE 
GO


USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[Адреса]    Script Date: 21.12.2018 22:55:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
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

USE [СПб_Достопримечательности]
GO

/****** Object:  Table [dbo].[ћузеи]    Script Date: 21.12.2018 22:55:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Музеи](
	[Ид] [int] NOT NULL,
	[Название] [nvarchar](max) NULL,
	[Час_открыти¤] [time](7) NULL,
	[Час_закрыти¤] [time](7) NULL,
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
	[Час_открытия] [time](7) NULL,
	[Час_закрытия] [time](7) NULL,
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


insert into [dbo].[Районы] values (1, 'Василеостровский')
insert into [dbo].[Районы] values (2, 'Адмиралтейский')
insert into [dbo].[Районы] values (3, 'Невский')
insert into [dbo].[Районы] values (4, 'Петродворцовый')
insert into [dbo].[Районы] values (5, 'Кировский')
insert into [dbo].[Районы] values (6, 'Калининский')
insert into [dbo].[Районы] values (7, 'Красногвардейский')
insert into [dbo].[Районы] values (8, 'Приморский')
insert into [dbo].[Районы] values (9, 'Московский')
insert into [dbo].[Районы] values (10, 'Фрунзенский')
insert into [dbo].[Районы] values (11, 'Выборгский')
insert into [dbo].[Районы] values (12, 'Красносельский')
insert into [dbo].[Районы] values (13, 'Центральный')

insert into [dbo].[Адреса] values (1, 'Сенатская пл', null, 2)
insert into [dbo].[Адреса] values (2, 'Дворцовая пл', null, 2)
insert into [dbo].[Адреса] values (3, 'Ал. Невского пл', 1, 13)
insert into [dbo].[Адреса] values (4, 'Разводная ул', 2, 4)
insert into [dbo].[Адреса] values (5, 'ул Демьяна Бедного', null, 5)
insert into [dbo].[Адреса] values (6, 'Приморский пр', 74, 8)
insert into [dbo].[Адреса] values (7, 'Дворцовая пл', 2, 2)
insert into [dbo].[Адреса] values (8, 'Университетская наб', 3, 1)
insert into [dbo].[Адреса] values (9, 'Канала Грибоедова наб', 2, 13)
insert into [dbo].[Адреса] values (10, 'Реки Фонтанки наб', 118, 13)

insert into [dbo].[Музеи] values (1, 'Эрмитаж', '10:30',  '18:00', 'Кроме пн', 7)
insert into [dbo].[Музеи] values (2, 'Кунсткамера', '11:00','18:00', 'Кроме пн', 8)
insert into [dbo].[Музеи] values (3, 'Спас-На-Крови',' 10:30', '18:00', 'Кроме ср', 9)

insert into [dbo].[Памятники] values (1, 'Медный всадник', null, 1)
insert into [dbo].[Памятники] values (2, 'Александровская колоннна', null, 2)
insert into [dbo].[Памятники] values (3, 'Александру Невскому', null, 3)

insert into [dbo].[Парки] values (1, 'Верхний парк', null, '09:00',  '20:00', 'ежедневно', 4)
insert into [dbo].[Парки] values (2, 'Нижний парк', null,  '09:00',  '20:00', 'ежедневно', 4)
insert into [dbo].[Парки] values (3, 'Александрийский парк', null, '09:00', '20:00', 'ежедневно', 5)

insert into [dbo].[Усадьбы] values (1, 'Державина', null, 10)

