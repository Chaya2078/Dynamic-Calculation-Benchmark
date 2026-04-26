import pyodbc
import time

# Connection Configuration
conn_str = (
    r'DRIVER={SQL Server};'
    r'SERVER=DESKTOP-EODP1E7\This_user;'
    r'DATABASE=PaymentSystem;'
    r'Trusted_Connection=yes;'
)

try:
    conn = pyodbc.connect(conn_str)
    cursor = conn.cursor()

    # 1. Fetch all formulas first
    cursor.execute("SELECT targil_id, targil, tnai, targil_false FROM t_targil")
    formulas = cursor.fetchall()

    # 2. Fetch the 1,000,000 records ONCE to optimize performance
    print("Loading 1,000,000 records into memory...")
    cursor.execute("SELECT data_id, a, b, c, d FROM t_data")
    data_rows = cursor.fetchall() # Loaded once

    for f_id, formula, tnai, f_false in formulas:
        print(f"Processing Formula {f_id}...")
        start_time = time.perf_counter() # More precise than time.time()
        
        # Clean potential syntax differences (e.g., ^ to **)
        formula = formula.replace('^', '**')
        if f_false: f_false = f_false.replace('^', '**')
        if tnai: tnai = tnai.replace('=', '==').replace('!==', '!=')

        for row in data_rows:
            # Context dictionary for evaluation scope
            ctx = {'a': row.a, 'b': row.b, 'c': row.c, 'd': row.d}
            
            try:
                if tnai:
                    # Logic for conditional formulas (Bonus point requirement)
                    condition_met = eval(tnai, {"__builtins__": None}, ctx)
                    res = eval(formula, {"__builtins__": None}, ctx) if condition_met else eval(f_false, {"__builtins__": None}, ctx)
                else:
                    res = eval(formula, {"__builtins__": None}, ctx)
            except:
                continue

        # 3. Log Performance Benchmarks
        end_time = time.perf_counter()
        run_time = end_time - start_time
        
        cursor.execute("INSERT INTO t_log (targil_id, method, run_time) VALUES (?, ?, ?)", 
                       (f_id, 'Python_Eval', run_time))
        conn.commit()
        print(f"Formula {f_id} finished in {run_time:.2f} seconds")

    conn.close()
    print("All tasks completed successfully.")
except Exception as e:
    print(f"Error: {e}")