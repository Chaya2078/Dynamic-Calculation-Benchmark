# Dynamic-Calculation-Benchmark
**Performance Evaluation of Dynamic Formula Engines**

## Performance Report: Dynamic Calculation Engine for 1,000,000 Records

### Project Overview
I developed this project to evaluate the performance of dynamic formula calculations across three different environments: **SQL Server, C# (.NET 8), and Python.** The benchmark was conducted on a dataset of **one million records** to determine the most efficient method for mass data processing in complex systems like payroll and billing.

---

###  Interactive Visual Report (Angular)
The final results and the designed report can be viewed at the following link:

 **[View Interactive Angular Report](https://stackblitz.com/edit/stackblitz-starters-ctz5i9qm?file=package.json)**

---

### Performance Benchmark Results

| Calculation Method | Average Time (Seconds) | Performance Status |
| :--- | :--- | :--- |
| **SQL Native** | **1.906** |  **Fastest** |
| **C# (.NET 8)** | 3.029 | Stable and Fast |
| **Python (Eval)** | 22.78 | Significantly Slower |

---

### Technical Analysis of Implementations

#### 1. SQL Native Implementation
The calculation was performed directly within the database engine. This proved to be the most efficient method as it eliminates **Data Transfer Latency** between the DB and the Application server. Processing occurs "close to the metal" and leverages the engine's query optimizer.

#### 2. C# (.NET 8) Implementation
I utilized the `DataTable.Compute` method to parse text strings into mathematical formulas at runtime. While the execution is fast, the overall time includes the overhead of fetching 1,000,000 records into the application's RAM.

#### 3. Python Implementation
Implemented using the `eval()` function within a loop. While this offers the highest flexibility for dynamic formulas, it is the least performant for massive datasets due to the overhead of interpreted execution in large iterations.

---

### Results Validation and Accuracy
To ensure **100% data integrity**, I performed a full validation between all three methods. A dedicated SQL verification script confirmed an absolute match for every single one of the million records.

### Final Recommendation
For high-scale systems processing millions of records, I recommend the **SQL Native** approach to ensure maximum throughput and system stability.
