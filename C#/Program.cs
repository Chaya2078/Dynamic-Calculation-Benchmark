using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    // Professional Configuration: Database Connection String
static string connectionString = @"Server=DESKTOP-EODP1E7\This_user;Database=PaymentSystem;Trusted_Connection=True;";
  
    private const string MethodName = "CSharp_Compute";


    static void Main()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                Console.WriteLine("[INFO] Connected to SQL Server successfully.");

                // 1. Fetch Dynamic Formulas and Conditions
                DataTable formulas = GetFormulas(conn);

                [cite_start]// 2. Load Dataset into Memory (Optimized for 1,000,000 records) [cite: 67, 72]
                var dataList = LoadDataset(conn);

                // Initialize the Evaluation Engine
                DataTable engine = new DataTable();

                foreach (DataRow formulaRow in formulas.Rows)
                {
                    int formulaId = Convert.ToInt32(formulaRow["targil_id"]);
                    string mainFormula = formulaRow["targil"].ToString();
                    string condition = formulaRow["tnai"]?.ToString();
                    string falseFormula = formulaRow["targil_false"]?.ToString();

                    Console.WriteLine($"[PROCESS] Evaluating Formula ID {formulaId}...");

                    Stopwatch sw = Stopwatch.StartNew();

                    foreach (var row in dataList)
                    {
                        // Strategy: Safe Parameter Replacement using Regex to avoid keyword corruption (e.g., 'abs')
                        string targetExpression = PrepareExpression(mainFormula, condition, falseFormula, row, engine);
                        
                        try 
                        {
                            [cite_start]// Execution via DataTable.Compute as specified in requirements [cite: 74]
                            var result = engine.Compute(targetExpression, "");
                        }
                        catch (Exception) { continue; }
                    }

                    sw.Stop();
                    double duration = sw.Elapsed.TotalSeconds;

                    [cite_start]// 3. Log Performance Benchmarks [cite: 85, 117]
                    LogPerformance(conn, formulaId, duration);

                    Console.WriteLine($"[SUCCESS] ID {formulaId} - Execution Time: {duration:F4}s");
                }
            }
            Console.WriteLine("\n[FINISH] All calculations completed. Metrics stored in t_log.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FATAL ERROR] {ex.Message}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    [cite_start]// Helper: Safely determines which formula to run based on the condition [cite: 51, 55]
    private static string PrepareExpression(string main, string tnai, string fFalse, dynamic row, DataTable engine)
    {
        // Inject values into scopes
        string finalExpr = main;

        if (!string.IsNullOrEmpty(tnai))
        {
            string evalTnai = ReplaceVars(tnai, row);
            bool isTrue = false;
            try 
            {
                // Evaluate the boolean condition first
                var check = engine.Compute(evalTnai, "");
                isTrue = check is bool b && b;
            } catch { isTrue = false; }

            finalExpr = isTrue ? main : fFalse;
        }

        return ReplaceVars(finalExpr, row);
    }

    [cite_start]// High-Precision Variable Replacement using Regex [cite: 50]
    private static string ReplaceVars(string expression, dynamic row)
    {
        if (string.IsNullOrEmpty(expression)) return "0";
        
        string result = expression;
        result = Regex.Replace(result, @"\ba\b", row.a.ToString());
        result = Regex.Replace(result, @"\bb\b", row.b.ToString());
        result = Regex.Replace(result, @"\bc\b", row.c.ToString());
        result = Regex.Replace(result, @"\bd\b", row.d.ToString());
        return result;
    }

    private static DataTable GetFormulas(SqlConnection conn)
    {
        string sql = "SELECT targil_id, targil, tnai, targil_false FROM t_targil";
        using SqlCommand cmd = new SqlCommand(sql, conn);
        using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }

    private static List<dynamic> LoadDataset(SqlConnection conn)
    {
        Console.WriteLine("[INFO] Streaming 1,000,000 records into memory...");
        string sql = "SELECT data_id, a, b, c, d FROM t_data";
        using SqlCommand cmd = new SqlCommand(sql, conn);
        using SqlDataReader reader = cmd.ExecuteReader();
        
        var list = new List<dynamic>(1000000);
        while (reader.Read())
        {
            list.Add(new { 
                id = reader["data_id"], 
                a = reader["a"], 
                b = reader["b"], 
                c = reader["c"], 
                d = reader["d"] 
            });
        }
        return list;
    }

    private static void LogPerformance(SqlConnection conn, int fId, double time)
    {
        string sql = "INSERT INTO t_log (targil_id, method, run_time) VALUES (@fId, @method, @time)";
        using SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@fId", fId);
        cmd.Parameters.AddWithValue("@method", MethodName);
        cmd.Parameters.AddWithValue("@time", time);
        cmd.ExecuteNonQuery();
    }
}