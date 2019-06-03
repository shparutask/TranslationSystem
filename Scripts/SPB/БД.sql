USE [master]
GO

/****** Object:  Database [���_���������������������]    Script Date: 21.12.2018 22:54:32 ******/
CREATE DATABASE [���_���������������������]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'���_���������������������', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\���_���������������������.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'���_���������������������_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\���_���������������������_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [���_���������������������] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [���_���������������������].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [���_���������������������] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [���_���������������������] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [���_���������������������] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [���_���������������������] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [���_���������������������] SET ARITHABORT OFF 
GO

ALTER DATABASE [���_���������������������] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [���_���������������������] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [���_���������������������] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [���_���������������������] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [���_���������������������] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [���_���������������������] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [���_���������������������] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [���_���������������������] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [���_���������������������] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [���_���������������������] SET  DISABLE_BROKER 
GO

ALTER DATABASE [���_���������������������] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [���_���������������������] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [���_���������������������] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [���_���������������������] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [���_���������������������] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [���_���������������������] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [���_���������������������] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [���_���������������������] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [���_���������������������] SET  MULTI_USER 
GO

ALTER DATABASE [���_���������������������] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [���_���������������������] SET DB_CHAINING OFF 
GO

ALTER DATABASE [���_���������������������] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [���_���������������������] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [���_���������������������] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [���_���������������������] SET  READ_WRITE 
GO


