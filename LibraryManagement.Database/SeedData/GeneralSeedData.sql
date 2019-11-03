USE [LibraryDB]
GO
--Insert table Loai Sach
SET IDENTITY_INSERT [LoaiSach] ON
GO

IF NOT EXISTS (SELECT TOP 1 * FROM [LoaiSach])
BEGIN
INSERT INTO [LoaiSach] ([Id],[TenLoai]) 
VALUES (1, N'Phê bình - Lý luận văn học'),
(2, N'Tư duy - Kỹ năng sống'),
(3, N'Truyện dài'),
(4, N'Nghệ thuật sống đẹp'),
(5, N'Sách văn học'),
(6, N'Sách khởi nghiệp'),
(7, N'Sách kinh tế'),
(8, N'Sách tham khảo'),
(9, N'Sách Tôn giáo - Tâm linh'),
(10, N'Từ Điển'),
(11, N'Sách Khoa Học - Kỹ Thuật')
END
GO

SET IDENTITY_INSERT [Loaisach] OFF
GO

--Insert table Sach
IF NOT EXISTS (SELECT TOP 1 * FROM [Sach])
BEGIN
INSERT INTO [Sach] ([Id], [TenSach],[TacGia] ,[NamXuatBan],[NhaXuatBan], [TomTat],[TongSoTrang],[LoaiSach],[SoLuong],[HinhAnh],[TrangThai]) 
VALUES ('9786045389393', N'Kẻ trộm sách', N'Markus Zuskas', 2017, N'Nhà xuất bản Hội Nhà Văn', N'Có rất nhiều câu chuyện mà tôi cho phép chúng ta làm tôi xao nhãng trong khi làm việc...', 571, 1,10,'\uploads\images\9786045389393.jpg', 1),
('9552147738847', N'Đừng Lựa Chọn An Nhàn Khi Còn Trẻ', N'Cảnh Thiên', 2019, N'	Nhà Xuất Bản Thế Giới', N'Trong độ xuân xanh phơi phới ngày ấy, bạn không dám mạo hiểm ...',316, 2,10,'\uploads\images\9552147738847.jpg', 1),
('3357672088223', N'Mắt Biếc (Tái Bản 2019)', N'Nguyễn Nhật Ánh', 2019, N'Nhà Xuất Bản Trẻ', N'Tôi gửi tình yêu cho mùa hè, nhưng mùa hè không giữ nổi. Mùa hè chỉ biết ra hoa,...',300, 3,5,'\uploads\images\3357672088223.jpg', 1),
('8935236402088', N'Điểm dối lừa', N'Dan Brown', 2014, N'Nhà Xuất Văn Hóa - Thông Tin', N'Điểm dối lừa đủ độ ly kỳ và bất ngờ để khiến độc giả dày dạn nhất thấy hồi hộp...',719, 1,5,'\uploads\images\8935236402088.jpg', 1),
('2496196333059', N'Đàn Ông Sao Hỏa Đàn Bà Sao Kim', N'John Gray', 2019, N'Nhà Xuất Bản Hồng Đức', N'Ngày xửa ngày xưa, những người sao Hỏa và sao Kim đã gặp gỡ, yêu nhau và sống hạnh phúc bởi họ tôn trọng và chấp nhận mọi điều khác biệt. Rồi họ đến Trái Đất và chứng lãng quên đã xảy ra: Họ quên rằng họ đến từ những hành tinh khác',488, 2,3,'\uploads\images\2496196333059.jpg', 1),
('9780512479372', N'Lối Sống Tối Giản Của Người Nhật (Tái Bản)', N'Sasaki Fumio',2018, N'Nhà Xuất Bản Thế Giới', N'Lối sống tối giản là cách sống cắt giảm vật dụng xuống còn mức tối thiểu. Và cùng với cuộc sống ít đồ đạc, ta có thể để tâm nhiều hơn tới hạnh phúc, đó chính là chủ đề của cuốn sách này.',294, 4,1,'\uploads\images\9780512479372.jpg', 1),
('3053629188366', N'Cân Bằng Cảm Xúc, Cả Lúc Bão Giông', N'Richard Nicholls', 2019,N'Nhà Xuất Bản Thế Giới', N'Một ngày, chúng ta có khoảng 16 tiếng tiếp xúc với con người, công việc, các nguồn thông tin từ mạng xã hội, loa đài báo giấy… Việc này mang đến cho bạn vô vàn cảm xúc, cả tiêu cực lẫn tích cực.',336, 2,9,'\uploads\images\3053629188366.jpg', 1),
('2518407786529', N'Nhà Giả Kim', N'Paulo Coelho', 2013,N'Nhà Xuất Bản Văn Học', N'Tất cả những trải nghiệm trong chuyến phiêu du theo đuổi vận mệnh của mình đã giúp Santiago thấu hiểu được ý nghĩa sâu xa nhất của hạnh phúc, hòa hợp với vũ trụ và con người.',228, 5,2,'\uploads\images\2518407786529.jpg', 1),
('9786045985007', N'Để Làm Nên Sự Nghiệp', N'H.N Casson', 2017,N'Nhà Xuất Bản Lao Động', N'Đây là một cuốn sách có nội dung thiết thực, hữu ích. Nó cung cấp cho bạn đọc những tri thức, kỹ năng sống, bài học bổ ích trong lĩnh vực kinh doanh nói riêng và cuộc sống nói chung. Cuốn sách là một cẩm nang không thể bỏ qua đối với những ai đang có ý định lập nghiệp bằng con đường kinh doanh, cũng như những ai đã và đang thành công trên thương trường.',219, 6, 1,'\uploads\images\9786045985007.jpg',1),
('9786045827949', N'Khám phá ngôn ngữ tư duy', N'Philip Miller', 2017,N'Nhà Xuất Bản Tổng Hợp Thành Phố Hồ Chí Minh', N'Đằng sau thái độ, hành vi của mỗi chúng ta là cả một “bản đồ thế giới” (map of the world) – chứa đựng những thói quen, niềm tin, giá trị, ký ức,… – định hình nên suy nghĩ, hành động, cách ta nhìn nhận về bản thân, về mọi người và về thế giới xung quanh. Liệu pháp NLP (Neuro Linguistic Programming – Lập trình Ngôn Ngữ Tư duy) giúp thay đổi tận gốc hành vi, tức là thay đổi kiểu suy nghĩ dẫn đến hành vi của mỗi người. Không giống như các phương pháp truyền thống khác, chỉ đơn thuần bảo ta cần phải làm gì, NLP hướng dẫn ta cách làm để đạt được mục tiêu đề ra, để trở thành mẫu người mà mình mong muốn.',175, 7, 2,'\uploads\images\9786045827949.jpg', 1),
('9786049608261', N'Tuổi trẻ đáng giá bao nhiêu?', N'Rosie Nguyễn', 2018,N'Nhà Xuất Bản Hội Văn Học', N'Bạn hối tiếc vì không nắm bắt lấy một cơ hội nào đó, chẳng có ai phải mất ngủ. Bạn trải qua những ngày tháng nhạt nhẽo với công việc bạn căm ghét, người ta chẳng hề bận lòng. Bạn có chết mòn nơi xó tường với những ước mơ dang dở, đó không phải là việc của họ. Suy cho cùng, quyết định là ở bạn. Muốn có điều gì hay không là tùy bạn. Nên hãy làm những điều bạn thích. Hãy đi theo tiếng nói trái tim. Hãy sống theo cách bạn cho là mình nên sống. Vì sau tất cả, chẳng ai quan tâm.',285, 2, 5,'\uploads\images\9786049608261.jpg', 1),
('5816014371082', N'Công Phá Bài Tập Hóa lớp 10-11-12', N'Trần Phương Duy', 2018,N'Nhà Xuất Bản Đại Học Quốc Gia Hà Nội', N'Thử thách bản thân với hàng loạt bài tập phân bổ đều cả 3 lớp 10-11-12, được chọn lọc kĩ càng. Cả cuốn sách được chia thành 20 chương theo từng dạng bài cụ thể và 10 chương theo từng phương pháp giải bài tập điển hình. Các bài tập trong cuốn sách đều là những bài tập điển hình và quen thuộc nhất trong các đề thi thử THPT quốc gia và chính các bài kiểm tra trên lớp của các em học sinh. Ngoài các ví dụ giúp các bạn định hình dạng toán, cuốn sách còn bao hàm rất nhiều bài tập tự luyện có đáp án, giúp các bạn giải quen dạng và quen tay, nhằm tối ưu hóa thời gian khi làm các đề thi.',622, 8, 3,'\uploads\images\5816014371082.jpg', 1),
('6792841728659', N'Công Phá Bài Tập Sinh', N'Trần Phương Duy', 2018,N'Nhà Xuất Bản Đại Học Quốc Gia Hà Nội', N'Với những sự khác biệt từ nội dung tới hình thức, tới cách thức hỗ trợ sau khi mua sách, chúng tôi tin tưởng chắc chắn rằng CÔNG PHÁ BÀI TẬP SINH 10-11-12 và CÔNG PHÁ LÍ THUYẾT SINH 10-11-12 sẽ giúp các em tự tin với môn Sinh hơn trong mọi kì thi. Hãy đọc và cảm nhận tâm huyết trên từng trang sách của chúng tôi ngay từ bây giờ quý độc giả nhé!',478, 8, 1,'\uploads\images\6792841728659.jpg', 1),
('2393362046440', N'Đột Phá Mindmap - Tư Duy Đọc Hiểu Môn Ngữ Văn Bằng Hình Ảnh Lớp 12', N'Trịnh Văn Quỳnh', 2016,N'Nhà Xuất Bản Đại Học Quốc Gia Hà Nội', N'Đột Phá Mindmap - Tư Duy Đọc Hiểu Môn Ngữ Văn Bằng Hình Ảnh Lớp 12 sẽ mang đến một phương pháp học văn hoàn toàn mới mẻ - Học văn bằng tư duy Mindmap.',307, 8, 3,'\uploads\images\2393362046440.jpg', 1),
('9786046324416', N'Những Điều Trường Lớp Không Thể Dạy', N'Nguyễn Văn Yên, S.J', 2017,N'Nhà Xuất Bản Phương Đông', N'"Đừng ngần ngại nữa, hãy xách balo lên và đi!" Đó là lời động viên, mang tính cách tuyên ngôn, đã lưu lại nhiều ảnh hưởng mạnh mẽ trên các bạn trẻ trong thời gian qua. Nhiều người thích thú chia sẻ với nhau câu hỏi: "thế giới này là một quyển sách. Ai chưa từng xách balo lên và đi thì mới chỉ đọc duy nhất một trang của cuốn sách ấy"',155, 9, 1,'\uploads\images\9786046324416.jpg', 1),
('9786046131045', N'Đường Về Thượng Trí', N'Nguyễn Tầm Thường', 2011,N'Nhà Xuất Bản Tôn Giáo', N'Những chuyện ngắn của cha Nguyễn Tầm Thường vừa lôi cuốn vừa nhẹ nhàng dẫn người đọc vào đời sống tâm linh.',242, 9, 2,'\uploads\images\9786046131045.jpg', 1),
('8215687358258', N'Từ Điển Hán - Việt', N'Phan Văn Các',2011,N'Nhà Xuất Bản Dân Trí', N'"Từ điển Hán - Việt" là tài liệu tham khảo cần thiết đối với những người học và sử dụng tiếng Trung Quốc. Cuốn từ cung cấp một khối lượng từ phong phú, đa dạng. Trong đó đa phần là các từ hiện đại, được sử dụng phổ biến trong cuộc sống hàng ngày và trên các phương tiện thông tin đại chúng.',2024, 10, 1,'\uploads\images\8215687358258.jpg', 1),
('5013546003942', N'Từ Điển Oxford Anh Việt 350.000 Từ', N'The Windy',2018,N'Nhà Xuất Bản Đại Học Quốc Gia Hà Nội', N'Đây là cuốn từ điển được biên dịch dựa theo cuốn từ điển Oxford, Cambridge và một số cuốn từ điển uy tín trên thế giới, là một công trình liên tục được đổi mới và công bố bởi một nhà xuất bản uy tín trên thế giới với nhiều ấn phẩm khác nhau đã có mặt tại Việt Nam, trợ giúp cho các nhà nghiên cứu, các giảng viên, đặc biệt là sinh viên Việt Nam nhiều thập kỷ qua.',1664, 10, 20,'\uploads\images\5013546003942.jpg', 1),
('1929085920289', N'Từ Điển Nhật - Việt', N'Nguyễn Văn Khang',2018,N'Nhà Xuất Bản Đại Học Quốc Gia Hà Nội', N'Từ Điển Nhật - Việt là cuốn sách thu thập và giải thích những chữ Hán thông dụng trong tiếng Nhật gồm: 1945 Joyo Kanji, 18.000 tổ hợp, 2.000 thuật ngữ. Đồng thời cuốn sách này sẽ chỉ dẫn cho các bạn cách viết chữ Hán, cũng như chỉ cho các bạn cách đọc ON/KUN phiên âm La Tinh và cách đọc Hán Việt của Tiếng Việt.',894, 10, 9,'\uploads\images\1929085920289.jpg', 1),
('2448120945562', N'Code Dạo Kí Sự - Lập Trình Viên Đâu Phải Chỉ Biết Code',N'Phạm Huy Hoàng (Developer)',2017,N'Nhà Xuất Bản Dân Trí', N' Code Dạo Kí Sự - Lập Trình Viên Đâu Phải Chỉ Biết Code',266, 11, 2,'\uploads\images\2448120945562.jpg', 1)
END
GO

--Insert table Doc Gia
IF NOT EXISTS (SELECT TOP 1 * FROM [DocGia])
BEGIN
INSERT INTO [DocGia] ([TenDG],[GioiTinh],[NgaySinh],[CMND],[DiaChi],[SDT],[NgayDangKy],[Username], [Password], [TrangThai]) 
VALUES (N'Hồ Minh Tuấn', N'Nam', '1997-1-26', '261541432', N'Quận Tân Bình','0833475600', '2019-10-16', 'tuanho@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1),
(N'Nguyễn Như Sang', N'Nam', '1997-10-2', '647381962', N'Quận 3','0987165433', '2019-10-10', 'sangnguyen@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1),
(N'Hồ Thị Mận', N'Nữ', '1997-1-20', '878436251', N'Quận 5','0986386291', '2019-12-10', 'manho@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1)
END
GO

--Insert table Nhan Vien
IF NOT EXISTS (SELECT TOP 1 * FROM [NhanVien])
BEGIN
INSERT INTO [NhanVien] ([TenNV],[GioiTinh],[NgaySinh],[CMND],[DiaChi],[SDT],[ViTri],[Username],[Password],[TrangThai]) 
VALUES (N'Admin', N'Nam', '1997-1-26', '261541432', N'Quận Tân Bình','0833475600',N'Admin', 'admin@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1),
(N'Nguyễn Văn Bình', N'Nam', '1997-10-2', '647381962', N'Quận 3','0987165433',N'Thủ thư', 'thuthu1@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1),
(N'Trần Vĩnh Phúc', N'Nam', '1997-10-2', '123545345', N'Quận 5','0154564565',N'Thủ thư', 'vinhphuc@gmail.com', '8BFDDF77E9A5F1BBA409873BA255538D2357CC88', 1)
END
GO

--Insert table Phieu Muon
IF NOT EXISTS (SELECT TOP 1 * FROM [PhieuMuon])
BEGIN
INSERT INTO [PhieuMuon] ([MaDG],[MaNV],[NgayMuon],[TongSachMuon],[HanTra],[DaTra],[TrangThai]) 
VALUES (1, 1, '2019-1-26', 2, '2019-2-5', 1, 1),
(2, 2, '2019-1-10', 3, '2019-1-27', 1, 1)
END
GO

--Insert table Chi Tiet Phieu Muon
IF NOT EXISTS (SELECT TOP 1 * FROM [CTPhieuMuon])
BEGIN
INSERT INTO [CTPhieuMuon] ([PhieuMuon],[Book],[SoLuong],[NgayMuon]) 
VALUES (1, '9552147738847',1, '2019-1-26'),
(1, '9780512479372',1, '2019-1-26'),
(2, '8935236402088',1, '2019-1-10'),
(2, '2393362046440',1, '2019-1-10'),
(2, '9786049608261',1, '2019-1-10')
END
GO

--Insert table Phieu Nhap
IF NOT EXISTS (SELECT TOP 1 * FROM [PhieuNhap])
BEGIN
INSERT INTO [PhieuNhap] ([NgayNhap],[SoLuong],[NhaCungCap],[NhanVienNhap],[TrangThai]) 
VALUES ('2018-12-24', 3, N'Nhà Sách Nhân Văn',2, 1),
('2019-10-30', 5, N'Nhà Sách Phương Nam',2, 1)
END
GO

--Insert table Chi Tiet Phieu Muon
IF NOT EXISTS (SELECT TOP 1 * FROM [CTPhieuNhap])
BEGIN
INSERT INTO [CTPhieuNhap] ([PhieuNhap],[Book],[SoLuong],[TinhTrangSach]) 
VALUES (1, '8215687358258',3,N'Tốt'),
(2, '8215687358258',2,N'Tốt'),
(2, '2448120945562',3,N'Tốt')
END
GO