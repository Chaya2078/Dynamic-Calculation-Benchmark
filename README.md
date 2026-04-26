# Dynamic-Calculation-Benchmark
Dynamic calculation benchmark
# Performance Report: Dynamic Calculation Engine for 1,000,000 Records

### Project Overview

I developed this project to evaluate the performance of dynamic formula calculations across three different environments: SQL Server, C# (.NET 8), and Python. 

I conducted the benchmark on a dataset of one million records to determine the most efficient method for mass data processing in payroll and billing systems.

---

### Visual Report (Angular)

The final results and the designed report can be viewed at the following link:

**[Link to Interactive Angular Report](https://stackblitzstartersg9uwverh-xekv--4200--4c73681d.local-credentialless.webcontainer.io/)**

---

### Performance Benchmark Results

| Calculation Method | Average Time (Seconds) | Performance Status |
| :--- | :--- | :--- |
| SQL Native | 1.906 | Fastest |
| C# (.NET 8) | 3.029 | Stable and Fast |
| Python (Eval) | 22.78 | Significantly Slower |

---

### Technical Analysis of Implementations

#### 1. SQL Native Implementation

I performed the calculation directly within the database engine. I found this to be the most efficient method because it eliminates the need to transfer data between the database server and the application server. The processing occurs very close to the data files and leverages the built-in processing power of the server.

#### 2. C# (.NET 8) Implementation

I used the DataTable.Compute method, which allows converting a text string into a mathematical formula during runtime. This method showed very good performance, but it required additional time to fetch one million records from the database into the program's RAM.

#### 3. Python Implementation

I implemented this version using the eval function within an iteration loop. I found this to be the most flexible method for writing variable formulas, but it proved to be the slowest due to the nature of Python's processing on exceptionally large datasets.

---

### Results Validation and Accuracy

I performed a full validation of the calculation results between all three methods I wrote. I used a dedicated SQL script (located in the SQL folder of this project) which confirmed an absolute match in results for every one of the million records in the table.

---

### Final Recommendation

For systems processing large data volumes, I recommend using the SQL Native method to achieve maximum performance and system stability.
