CREATE DATABASE QuanLyQuanCafe
GO

USE QuanLyQuanCafe
GO

-- Món

-- Danh Mục
-- Nhân Viên
-- Hóa Đơn
-- Thông tin hóa đơn


CREATE TABLE NhanVien
(
	MaNhanVien NVARCHAR(20) NOT NULL PRIMARY KEY,
	TenNhanVien NVARCHAR(100) NOT NULL,
	NgaySinh DATETIME  NOT NULL DEFAULT 18,
	ViTri NVARCHAR(100) NOT NULL  
)
GO
CREATE TABLE DanhMucHang
(
	MaDanhMuc  VARCHAR(20) NOT NULL PRIMARY KEY,
	TenDanhMuc NVARCHAR(100) NOT NULL 
)
GO


CREATE TABLE Mon
(
	MaMon  VARCHAR(20) NOT NULL PRIMARY KEY,
	TenMon NVARCHAR(100) NOT NULL ,
	MaDanhMuc varchar(20) NULL,
	DonGia INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DanhMucHang(MaDanhMuc)
)
GO

CREATE TABLE HoaDon
(
	MaHoaDon  VARCHAR(20) NOT NULL PRIMARY KEY,
	GioVao DATE NOT NULL DEFAULT GETDATE(),  -- tính từ ngày hôm nay
)
GO

CREATE TABLE ThongTinHoaDon
(
	MaTTHD  INT NOT NULL PRIMARY KEY,
	MaHoaDon varchar(20) NULL,
	MaMon varchar(20) NULL ,
	SoLuong INT NOT NULL DEFAULT 0,
	ThanhTien FLOAT NOT NULL
	
	FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon(MaHoaDon),
	FOREIGN KEY (MaMon) REFERENCES dbo.Mon(MaMon)
)
GO


-- thêm du lieu vào bảng nhân viên


INSERT INTO dbo.NhanVien  (MaNhanVien,TenNhanVien ,NgaySinh, ViTri )
VALUES  ( N'01', N'Lionel Messi' ,  CAST(0x000087B300000000 AS DateTime) ,  N'Quản Lý' )
INSERT INTO dbo.NhanVien(MaNhanVien,  TenNhanVien ,NgaySinh, ViTri )
VALUES  ( N'02', N'Linh' , CAST(0x00009EEF0148FD99 AS DateTime), N'Nhân Viên')

 -- Thêm dữ liệu vào Danh Mục
 INSERT INTO dbo.DanhMucHang(MaDanhMuc,TenDanhMuc)VALUES(N'01',N'Đồ Uống')
  INSERT INTO dbo.DanhMucHang(MaDanhMuc,TenDanhMuc)VALUES(N'02',N'Thức Ăn Nhanh')

   -- Thêm dữ liệu vào bảng Món -- MaDanhMuc DoUong -01 ,ThucAnNhan -02

 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU01',  N'CaFe Đá' ,		N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU02',  N'CaFe Sữa đá' ,		N'01' ,N'25000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU03',  N'CaFe Sữa Nóng' ,	 N'01' ,N'22000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU04',  N'Milk Tea' ,	N'01',N'28000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU05',  N'Trà Chanh' ,	 N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU06',  N'Trà Đào' ,		N'01' ,N'25000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU07',  N'Sting' ,	N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU08',  N'7UP' ,		N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU09',  N'CoCaCola' ,	N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU10',  N'PepSi' ,	N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'DU11',  N'Soda' ,	 N'01' ,N'20000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'TAN01',  N'Khoai Tây Chiên' ,N'02' , N'25000' )
 INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'TAN02',  N'Bánh Mì' , N'02', N'20000' )
INSERT INTO dbo.Mon(MaMon,TenMon,MaDanhMuc,DonGia) VALUES  ( N'TAN03',  N'Phô Mai Que' , N'02', N'19000' )


