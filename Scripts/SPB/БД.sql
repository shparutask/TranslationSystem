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


