USE [LibraryDB]

SET IDENTITY_INSERT [LoaiSach] ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM [LoaiSach])
BEGIN
INSERT INTO [LoaiSach] ([Id],[TenLoai]) 
VALUES (1, N'Tiểu thuyết'),
(2, N'Tư duy - Kỹ năng sống'),
(3, N'Truyện dài'),
(4, N'Nghệ thuật sống đẹp'),
(5, N'Sách văn học'),
(6, N'Sách khởi nghiệp'),
(7, N'Sách kinh tế')
END
GO
SET IDENTITY_INSERT [Loaisach] OFF
GO

IF NOT EXISTS (SELECT TOP 1 * FROM [Sach])
BEGIN
INSERT INTO [Sach] ([Id], [TenSach],[TacGia] ,[NamXuatBan],[NhaXuatBan], [TomTat],[TongSoTrang],[LoaiSach],[TrangThai]) 
VALUES ('9786045389393', N'Kẻ trộm sách', N'Markus Zuskas', 2017, N'Nhà xuất bản Hội Nhà Văn', N'Có rất nhiều câu chuyện mà tôi cho phép chúng ta làm tôi xao nhãng trong khi làm việc...',571, 1, 1),
('9552147738847', N'Đừng Lựa Chọn An Nhàn Khi Còn Trẻ', N'Cảnh Thiên', 2019, N'	Nhà Xuất Bản Thế Giới', N'Trong độ xuân xanh phơi phới ngày ấy, bạn không dám mạo hiểm ...',316, 2, 1),
('3357672088223', N'Mắt Biếc (Tái Bản 2019)', N'Nguyễn Nhật Ánh', 2019, N'Nhà Xuất Bản Trẻ', N'Tôi gửi tình yêu cho mùa hè, nhưng mùa hè không giữ nổi. Mùa hè chỉ biết ra hoa,...',300, 3, 1),
('8935236402088', N'Điểm dối lừa', N'Dan Brown', 2014, N'Nhà Xuất Văn Hóa - Thông Tin', N'Điểm dối lừa đủ độ ly kỳ và bất ngờ để khiến độc giả dày dạn nhất thấy hồi hộp...',719, 1, 1),
('2496196333059', N'Đàn Ông Sao Hỏa Đàn Bà Sao Kim', N'John Gray', 2019, N'Nhà Xuất Bản Hồng Đức', N'Ngày xửa ngày xưa, những người sao Hỏa và sao Kim đã gặp gỡ, yêu nhau và sống hạnh phúc bởi họ tôn trọng và chấp nhận mọi điều khác biệt. Rồi họ đến Trái Đất và chứng lãng quên đã xảy ra: Họ quên rằng họ đến từ những hành tinh khác',488, 2, 1),
('9780512479372', N'Lối Sống Tối Giản Của Người Nhật (Tái Bản)', N'Sasaki Fumio',2018, N'', N'Lối sống tối giản là cách sống cắt giảm vật dụng xuống còn mức tối thiểu. Và cùng với cuộc sống ít đồ đạc, ta có thể để tâm nhiều hơn tới hạnh phúc, đó chính là chủ đề của cuốn sách này.',294, 4, 1),
('3053629188366', N'Cân Bằng Cảm Xúc, Cả Lúc Bão Giông', N'Richard Nicholls', 2019,N'Nhà Xuất Bản Thế Giới', N'Một ngày, chúng ta có khoảng 16 tiếng tiếp xúc với con người, công việc, các nguồn thông tin từ mạng xã hội, loa đài báo giấy… Việc này mang đến cho bạn vô vàn cảm xúc, cả tiêu cực lẫn tích cực.',336, 2, 1),
('2518407786529', N'Nhà Giả Kim', N'Paulo Coelho', 2013,N'Nhà Xuất Bản Văn Học', N'Tất cả những trải nghiệm trong chuyến phiêu du theo đuổi vận mệnh của mình đã giúp Santiago thấu hiểu được ý nghĩa sâu xa nhất của hạnh phúc, hòa hợp với vũ trụ và con người.',228, 5, 1),
('9786045985007', N'Để Làm Nên Sự Nghiệp', N'H.N Casson', 2017,N'Nhà Xuất Bản Lao Động', N'Đây là một cuốn sách có nội dung thiết thực, hữu ích. Nó cung cấp cho bạn đọc những tri thức, kỹ năng sống, bài học bổ ích trong lĩnh vực kinh doanh nói riêng và cuộc sống nói chung. Cuốn sách là một cẩm nang không thể bỏ qua đối với những ai đang có ý định lập nghiệp bằng con đường kinh doanh, cũng như những ai đã và đang thành công trên thương trường.',219, 6, 1),
('9786045827949', N'Khám phá ngôn ngữ tư duy', N'Philip Miller', 2017,N'Nhà Xuất Bản Tổng Hợp Thành Phố Hồ Chí Minh', N'Đằng sau thái độ, hành vi của mỗi chúng ta là cả một “bản đồ thế giới” (map of the world) – chứa đựng những thói quen, niềm tin, giá trị, ký ức,… – định hình nên suy nghĩ, hành động, cách ta nhìn nhận về bản thân, về mọi người và về thế giới xung quanh. Liệu pháp NLP (Neuro Linguistic Programming – Lập trình Ngôn Ngữ Tư duy) giúp thay đổi tận gốc hành vi, tức là thay đổi kiểu suy nghĩ dẫn đến hành vi của mỗi người. Không giống như các phương pháp truyền thống khác, chỉ đơn thuần bảo ta cần phải làm gì, NLP hướng dẫn ta cách làm để đạt được mục tiêu đề ra, để trở thành mẫu người mà mình mong muốn.',175, 7, 1),
('9786049608261', N'Tuổi trẻ đáng giá bao nhiêu?', N'Rosie Nguyễn', 2018,N'Nhà Xuất Bản Hội Văn Học', N'"Bạn hối tiếc vì không nắm bắt lấy một cơ hội nào đó, chẳng có ai phải mất ngủ.

Bạn trải qua những ngày tháng nhạt nhẽo với công việc bạn căm ghét, người ta chẳng hề bận lòng.

Bạn có chết mòn nơi xó tường với những ước mơ dang dở, đó không phải là việc của họ.

Suy cho cùng, quyết định là ở bạn. Muốn có điều gì hay không là tùy bạn.

Nên hãy làm những điều bạn thích. Hãy đi theo tiếng nói trái tim. Hãy sống theo cách bạn cho là mình nên sống.

Vì sau tất cả, chẳng ai quan tâm."',285, 2, 1)
END
GO


INSERT INTO [KhachHang] ([TenKH],[GioiTinh],[NgaySinh],[CMND],[DiaChi],[SDT],[NgayDangKy]) 
VALUES (N'Hồ Minh Tuấn', N'Nam', '1997-1-26', '261541432', N'Quận Tân Bình','0833475600', '2019-10-16'),
(N'Nguyễn Như Sang', N'Nam', '1997-10-2', '647381962', N'Quận 3','0987165433', '2019-10-10'),
(N'Hồ Thị Mận', N'Nữ', '1997-1-20', '878436251', N'Quận 5','0986386291', '2019-12-10')
GO
