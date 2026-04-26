USE PaymentSystem;
GO

DECLARE @targil_id INT, @formula NVARCHAR(MAX), @tnai NVARCHAR(MAX), @formula_false NVARCHAR(MAX);
DECLARE @sql NVARCHAR(MAX);
DECLARE @StartTime DATETIME, @EndTime DATETIME;

-- לולאה שעוברת על כל הנוסחאות בטבלה 
DECLARE cur CURSOR FOR SELECT targil_id, targil, tnai, targil_false FROM t_targil;
OPEN cur;
FETCH NEXT FROM cur INTO @targil_id, @formula, @tnai, @formula_false;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @StartTime = GETDATE();

    -- בניית שאילתת ה-SQL בצורה דינמית 
    -- אם יש תנאי, משתמשים ב-CASE WHEN 
    IF @tnai IS NOT NULL
        SET @sql = 'INSERT INTO results (data_id, targil_id, method, result) 
                    SELECT data_id, ' + CAST(@targil_id AS NVARCHAR) + ', ''SQL_Dynamic'', 
                    CASE WHEN ' + @tnai + ' THEN ' + @formula + ' ELSE ' + @formula_false + ' END FROM t_data';
    ELSE
        SET @sql = 'INSERT INTO results (data_id, targil_id, method, result) 
                    SELECT data_id, ' + CAST(@targil_id AS NVARCHAR) + ', ''SQL_Dynamic'', ' + @formula + ' FROM t_data';

    EXEC sp_executesql @sql;

    SET @EndTime = GETDATE();

    -- שמירת זמן הריצה בלוג 
    INSERT INTO t_log (targil_id, method, run_time)
    VALUES (@targil_id, 'SQL_Dynamic', DATEDIFF(ms, @StartTime, @EndTime) / 1000.0);

    FETCH NEXT FROM cur INTO @targil_id, @formula, @tnai, @formula_false;
END

CLOSE cur; DEALLOCATE cur;
GO