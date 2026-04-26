USE PaymentSystem;
GO

UPDATE t_data SET val_a = 10; 
GO
UPDATE t_data SET val_a = (val_a + val_b) * val_c;
GO

SELECT 
    COUNT(*) AS [Total_Records_Checked],
    SUM(CASE 
        WHEN ABS(val_a - ((10 + val_b) * val_c)) > 0.001 THEN 1 
        ELSE 0 
    END) AS [Mismatches_Found]
FROM t_data;
GO