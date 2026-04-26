USE PaymentSystem;
GO

-- מחיקת טבלאות קיימות ליצירה מחדש
DROP TABLE IF EXISTS t_log;
DROP TABLE IF EXISTS results;
DROP TABLE IF EXISTS t_targil;
DROP TABLE IF EXISTS t_data;
GO

-- 1. טבלת נתונים בדיוק לפי המפרט 
CREATE TABLE t_data (
    data_id INT IDENTITY(1,1) PRIMARY KEY,
    a FLOAT NOT NULL,
    b FLOAT NOT NULL,
    c FLOAT NOT NULL,
    d FLOAT NOT NULL
);

-- 2. טבלת נוסחאות 
CREATE TABLE t_targil (
    targil_id INT PRIMARY KEY,
    targil NVARCHAR(MAX) NOT NULL, -- הנוסחה
    tnai NVARCHAR(MAX) NULL,        -- התנאי (רשות)
    targil_false NVARCHAR(MAX) NULL -- נוסחה אם התנאי לא מתקיים
);

-- 3. טבלת תוצאות 
CREATE TABLE results (
    resultsl_id INT IDENTITY(1,1) PRIMARY KEY,
    data_id INT NOT NULL FOREIGN KEY REFERENCES t_data(data_id),
    targil_id INT NOT NULL FOREIGN KEY REFERENCES t_targil(targil_id),
    method NVARCHAR(50) NOT NULL,
    result FLOAT
);

-- 4. טבלת לוג 
CREATE TABLE t_log (
    log_id INT IDENTITY(1,1) PRIMARY KEY,
    targil_id INT NOT NULL FOREIGN KEY REFERENCES t_targil(targil_id),
    method NVARCHAR(50) NOT NULL,
    run_time FLOAT 
);
GO