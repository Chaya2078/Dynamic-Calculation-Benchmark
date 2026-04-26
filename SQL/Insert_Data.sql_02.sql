USE PaymentSystem;
GO

-- מילוי מיליון רשומות
INSERT INTO t_data (a, b, c, d)
SELECT TOP 1000000 
    RAND(CHECKSUM(NEWID())) * 100, 
    RAND(CHECKSUM(NEWID())) * 100, 
    RAND(CHECKSUM(NEWID())) * 100,
    RAND(CHECKSUM(NEWID())) * 100
FROM sys.all_objects a CROSS JOIN sys.all_objects b;


INSERT INTO t_targil (targil_id, targil, tnai, targil_false) VALUES 
(1, 'a + b', NULL, NULL),           -- נוסחה פשוטה
(2, 'c * 2', NULL, NULL),           -- כפל בקבוע
(3, 'b * 2', 'a > 5', 'b / 2');     -- נוסחה עם תנאי (IF)
GO